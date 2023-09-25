using NRedisStack;
using NRedisStack.RedisStackCommands;
using StackExchange.Redis;
using Clients.Pokemon;
using Microsoft.AspNetCore.Mvc;

// redis
ConnectionMultiplexer redis = ConnectionMultiplexer.Connect("localhost");
IDatabase db = redis.GetDatabase();

db.StringSet("foo", "bar");
Console.WriteLine(db.StringGet("foo"));
// redis end

var builder = WebApplication.CreateBuilder(args);
var corsPolicyName = "pokemonCorsPolicy";

var httpClientName = "PokemonClient";

builder.Services.AddCors(options => options.AddPolicy(corsPolicyName, (policy) =>
{
    policy.WithOrigins("http://localhost:4000");
}));

builder.Services.AddControllers();

builder.Services.AddHttpClient(httpClientName);

builder.Services.AddScoped<PokemonClient>();


var app = builder.Build();

app.UseCors(corsPolicyName);
app.MapControllers();

app.Run();


namespace Controllers.Pokemon
{
    using Clients.Pokemon;
    using Models.Pokemon;
    using System.Net;

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
            var (result, statusCode) = await PokemonClient.GetPokemonAsync(name);

            if (result == null)
            {
                if (statusCode == HttpStatusCode.NotFound)
                {
                    return NotFound(new { message = "Pokemon not found" });
                }
                else
                {
                    return StatusCode(500, new { message = "An error occurred." });
                }
            }

            return result;
        }
    }
}

namespace Clients.Pokemon
{
    using System.Net;
    using Models.Pokemon;
    public class PokemonClient
    {
        public HttpClient Client { get; }
        public PokemonClient(IHttpClientFactory httpClientFactory)
        {
            Client = httpClientFactory.CreateClient("PokemonClient");
        }

        public async Task<(Pokemon? result, HttpStatusCode statusCode)> GetPokemonAsync(string name)
        {
            return await GetDataAsync<Pokemon>($"https://pokeapi.co/api/v2/pokemon/{name}");
        }

        public async Task<(Pokemon? result, HttpStatusCode statusCode)> GetAllPokemonNamesAsync()
        {
            return await GetDataAsync<Pokemon>($"https://pokeapi.co/api/v2/pokemon");
        }

        public async Task<(T? results, HttpStatusCode statusCode)> GetDataAsync<T>(string url)
        {
            HttpStatusCode statusCode = HttpStatusCode.InternalServerError;

            try
            {
                var response = await Client.GetAsync(url);
                statusCode = response.StatusCode;
                response.EnsureSuccessStatusCode();
                var results = await response.Content.ReadFromJsonAsync<T>();
                return (results, statusCode);
            }
            catch (HttpRequestException e)
            {
                return (default, statusCode);
            }
            catch (Exception e)
            {
                return (default, HttpStatusCode.InternalServerError);
            }
        }
    }
}