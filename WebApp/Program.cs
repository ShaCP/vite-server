using StackExchange.Redis;
using Microsoft.AspNetCore.Mvc;
using Pokemon.Middleware;
using Pokemon.Clients;
using System.Net;
using Pokemon.Filters;
using Pokemon.Services.PokemonDataService;
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

builder.Services.AddScoped<PokemonDataService>();
builder.Services.AddScoped<PokemonClient>();

var app = builder.Build();
app.UseMiddleware<ExceptionMiddleware>();
app.UseCors(corsPolicyName);

app.MapControllers();

app.Run();


namespace Pokemon.Controllers.PokemonController
{
    using Clients;
    using global::Models.Pokemon;
    using Pokemon.Models.PokemonData;
    using Pokemon.Services.PokemonDataService;

    [ApiController]
    [Route("api/[controller]")]
    public class PokemonController : ControllerBase
    {
        public PokemonClient PokemonClient { get; }
        public PokemonDataService PokemonDataService { get; }
        public PokemonController(PokemonClient pokemonClient, PokemonDataService pokemonDataService)
        {
            PokemonDataService = pokemonDataService;
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
                HttpContext.Items["IsSuccessful"] = false;
                return ex.StatusCode switch
                {
                    HttpStatusCode.NotFound => NotFound(new { message = "Pokemon not found." }),
                    _ => throw ex,
                };
            }
        }


        [HttpGet("matches/{name}")]
        public async Task<ActionResult<PokemonMatches>> GetPokemonMatchesAsync(string name, int page = 1)
        {
            var results = await PokemonDataService.GetPokemonMatches(name, page);
            return Ok(results);
        }
    }
}

namespace Pokemon.Clients
{
    using global::Models.Pokemon;
    using global::Models.Pokemon.ResultsData;

    public class PokemonClient
    {
        private readonly HttpClient client;

        public PokemonClient(IHttpClientFactory httpClientFactory)
        {
            client = httpClientFactory.CreateClient("PokemonClient");
        }

        public async Task<Pokemon?> GetPokemonAsync(string name)
        {
            var pokemon = await GetDataAsync<Pokemon>($"https://pokeapi.co/api/v2/pokemon/{name}");
            return pokemon;
        }

        public async Task<ResultsData?> GetAllPokemonAsync()
        {
            var pokemons = await GetDataAsync<ResultsData>("https://pokeapi.co/api/v2/pokemon?limit=10000");
            return pokemons;
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

namespace Pokemon.Services.PokemonDataService
{
    using global::Models.Pokemon.ResultsData;
    using Pokemon.Models.PokemonData;
    public class PokemonDataService
    {
        private Lazy<Task<ResultsData?>> AllPokemon { get; }
        public PokemonDataService(PokemonClient pokemonClient)
        {
            AllPokemon = new Lazy<Task<ResultsData?>>(pokemonClient.GetAllPokemonAsync);
        }

        public async Task<PokemonMatches> GetPokemonMatches(string name, int page)
        {
            var matches = await GetPokemonMatches(name);
            var resultsPerPage = 5;
            var startIndex = resultsPerPage * (page - 1);
            var results = matches.GetRange(startIndex, Math.Min(resultsPerPage, matches.Count - startIndex));
            return new PokemonMatches
            {
                Page = page,
                TotalPages = (matches.Count + resultsPerPage - 1) / resultsPerPage,
                TotalCount = matches.Count,
                Results = results
            };
        }

        public async Task<List<string>> GetPokemonMatches(string name)
        {
            var allPokemon = await AllPokemon.Value;
            var allPokemonNames = allPokemon?.Results.SelectMany(x => x.Name == null ? new() : new List<string>() { x.Name }).ToList() ?? new();
            var normalizedPokemonNames = allPokemonNames.Select(x => x.ToLower().Replace(" ", ""));
            var matches = allPokemonNames.Where(x => x.Contains(name.ToLower().Replace(" ", ""))).ToList();
            return matches;
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
    using Microsoft.AspNetCore.Mvc.Filters;
    using System.Text.Json;

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
            key += $"{context.ActionDescriptor.RouteValues["Action"]}:" ?? "";

            foreach (var item in parameters)
            {
                key += $"{item.Key}:{item.Value}";
            }

            string? cachedData = await redis.StringGetAsync(key);

            if (cachedData is not null)
            {
                context.Result = new OkObjectResult(cachedData);
            }
            else
            {
                var executedContext = await next();

                if (executedContext.Exception is null && (bool)(context.HttpContext.Items["IsSuccessful"] ?? true))
                {
                    if (executedContext.Result is ObjectResult objectResult)
                    {
                        var cacheData = JsonSerializer.Serialize(objectResult.Value, new JsonSerializerOptions
                        {
                            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
                        });
                        await redis.StringSetAsync(key, cacheData, TimeSpan.FromSeconds(3600));
                    }
                }
            }
        }
    }
}

namespace Pokemon.Models.PokemonData
{
    public class PokemonMatches
    {
        public int TotalCount { get; set; } = 0;
        public int TotalPages { get; set; } = 0;
        public int Page { get; set; } = 0;
        public List<string> Results { set; get; } = new();
    }
}