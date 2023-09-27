using StackExchange.Redis;
using Clients.Pokemon;
using Microsoft.AspNetCore.Mvc;

var builder = WebApplication.CreateBuilder(args);
var corsPolicyName = "pokemonCorsPolicy";

var httpClientName = "PokemonClient";

builder.Services.AddCors(options => options.AddPolicy(corsPolicyName, (policy) =>
{
    policy.WithOrigins("http://localhost:4000");
}));

builder.Services.AddControllers();

builder.Services.AddSingleton<IConnectionMultiplexer>(ConnectionMultiplexer.Connect("localhost"));
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
    using System.Text.Json;
    using Models.Pokemon;
    public class PokemonClient
    {
        private readonly HttpClient client;
        private readonly IDatabase redis;

        public PokemonClient(IHttpClientFactory httpClientFactory, IConnectionMultiplexer muxer)
        {
            client = httpClientFactory.CreateClient("PokemonClient");
            redis = muxer.GetDatabase();
        }

        public async Task<(Pokemon? result, HttpStatusCode statusCode)> GetPokemonAsync(string name)
        {
            var keyName = $"pokemon:{name}";
            var (pokemon, statusCode) = await GetDataAsync<Pokemon>(keyName, $"https://pokeapi.co/api/v2/pokemon/{name}");
            return (pokemon, statusCode);
        }

        // public async Task<(Pokemon? result, HttpStatusCode statusCode)> GetAllPokemonNamesAsync()
        // {
        //     return await GetDataAsync<Pokemon>($"https://pokeapi.co/api/v2/pokemon");
        // }

        public async Task<(T? results, HttpStatusCode statusCode)> GetDataAsync<T>(string keyName, string url)
        {
            string? json = await redis.StringGetAsync(keyName);
            T? results;
            HttpStatusCode statusCode = HttpStatusCode.OK;

            if (string.IsNullOrEmpty(json))
            {
                (json, var isOk, statusCode) = await MakeRequestAsync(url);
                if (isOk == false)
                {
                    return (default, statusCode);
                }
                var setTask = redis.StringSetAsync(keyName, json);
                var expireTask = redis.KeyExpireAsync(keyName, TimeSpan.FromSeconds(3600));
                await Task.WhenAll(setTask, expireTask);
            }

            results = string.IsNullOrEmpty(json) ? default : JsonSerializer.Deserialize<T>(json);

            return (results, statusCode);
        }

        public async Task<(string? jsonResults, bool isOk, HttpStatusCode statusCode)> MakeRequestAsync(string url)
        {
            HttpStatusCode statusCode = HttpStatusCode.InternalServerError;

            try
            {
                var response = await client.GetAsync(url);
                statusCode = response.StatusCode;
                response.EnsureSuccessStatusCode();
                var jsonString = await response.Content.ReadAsStringAsync();
                return (jsonString, true, statusCode);
            }
            catch (HttpRequestException e)
            {
                return (null, false, statusCode);
            }
            catch (Exception e)
            {
                return (null, false, statusCode);
            }
        }
    }
}