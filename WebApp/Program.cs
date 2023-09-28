using StackExchange.Redis;
using Microsoft.AspNetCore.Mvc;
using Pokemon.Middleware;
using Pokemon.Clients;
using System.Net;
using Pokemon.Filters;

var builder = WebApplication.CreateBuilder(args);
var corsPolicyName = "pokemonCorsPolicy";

var httpClientName = "PokemonClient";

builder.Services.AddCors(options => options.AddPolicy(corsPolicyName, (policy) =>
{
    policy.WithOrigins("http://localhost:4000");
}));

builder.Services.AddControllers(options => options.Filters.Add(typeof(RedisCacheFilter)));

builder.Services.AddSingleton<IConnectionMultiplexer>(ConnectionMultiplexer.Connect("localhost"));
builder.Services.AddHttpClient(httpClientName);

builder.Services.AddScoped<PokemonClient>();


var app = builder.Build();
app.UseMiddleware<ExceptionMiddleware>();
app.UseCors(corsPolicyName);

app.MapControllers();

app.Run();


namespace Pokemon.Controllers.PokemonController
{
    using Clients;
    using Models.Pokemon;

    [ApiController]
    [Route("api/[controller]")]
    public class PokemonController : ControllerBase
    {
        public PokemonClient PokemonClient { get; }
        public PokemonController(PokemonClient pokemonClient)
        {
            PokemonClient = pokemonClient;

        }

        [HttpGet("{name}")]
        public async Task<ActionResult<Pokemon?>> GetPokemonAsync(string name)
        {
            try
            {
                var result = await PokemonClient.GetPokemonAsync(name);
                return result;
            }
            catch (HttpRequestException ex)
            {
                return ex.StatusCode switch
                {
                    HttpStatusCode.NotFound => NotFound(new { message = "Pokemon not found." }),
                    _ => throw ex,
                };
            }
        }
    }
}

namespace Pokemon.Clients
{
    using Models.Pokemon;
    public class PokemonClient
    {
        private readonly HttpClient client;

        public PokemonClient(IHttpClientFactory httpClientFactory, IConnectionMultiplexer muxer)
        {
            client = httpClientFactory.CreateClient("PokemonClient");
        }

        public async Task<Pokemon?> GetPokemonAsync(string name)
        {
            var pokemon = await GetDataAsync<Pokemon>($"https://pokeapi.co/api/v2/pokemon/{name}");
            return pokemon;
        }

        public async Task<T?> GetDataAsync<T>(string url)
        {
            var response = await client.GetAsync(url);
            response.EnsureSuccessStatusCode();
            var results = await response.Content.ReadFromJsonAsync<T>();
            return results;
        }
    }
}


namespace Pokemon.Middleware
{

    using System.Net;
    using System.Text.Json;

    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionMiddleware> _logger;

        public ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task Invoke(HttpContext httpContext)
        {
            try
            {
                await _next(httpContext);
            }
            catch (HttpRequestException ex)
            {
                _logger.LogError(ex, "An unhandled http request exception has occured while executing the request.");
                await HandleExceptionAsync(httpContext, ex, ex.StatusCode);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An unhandled exception has occured while executing the request.");
                await HandleExceptionAsync(httpContext, ex);
            }
        }

        private static Task HandleExceptionAsync(HttpContext httpContext, Exception ex, HttpStatusCode? statusCode = HttpStatusCode.InternalServerError)
        {
            httpContext.Response.ContentType = "application/json";
            httpContext.Response.StatusCode = (int)(statusCode ?? HttpStatusCode.InternalServerError);

            return httpContext.Response.WriteAsync(new ErrorDetails
            {
                StatusCode = httpContext.Response.StatusCode,
                Message = statusCode is null ? "Internal Server Error" : ex.Message
            }.ToString());
        }
    }

    internal class ErrorDetails
    {
        public int StatusCode { get; set; }
        public string Message { get; set; } = "Error";

        public override string ToString()
        {
            return JsonSerializer.Serialize(this);
        }
    }
}

namespace Pokemon.Filters
{
    using System.Text.Json;
    using Microsoft.AspNetCore.Mvc.Filters;
    public class RedisCacheFilter : IAsyncActionFilter
    {
        private readonly IDatabase redis;
        public RedisCacheFilter(IConnectionMultiplexer muxer)
        {
            redis = muxer.GetDatabase();
        }
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var parameters = context.ActionArguments;
            string key = $"{context.ActionDescriptor.RouteValues["Controller"]}:" ?? "";

            foreach (var item in parameters)
            {
                key += $"{item.Key}:{item.Value}";
            }

            string? cachedData = await redis.StringGetAsync(key);

            if (cachedData is not null)
            {
                var data = JsonSerializer.Deserialize<object>(cachedData);
                context.Result = new JsonResult(data);
            }
            else
            {
                var executedContext = await next();

                if (executedContext.Exception is null)
                {
                    if (executedContext.Result is ObjectResult objectResult)
                    {
                        var cacheData = JsonSerializer.Serialize(objectResult.Value);
                        await redis.StringSetAsync(key, cacheData, TimeSpan.FromSeconds(3600));
                    }
                }
            }
        }
    }
}