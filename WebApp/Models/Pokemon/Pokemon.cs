using System.Text.Json.Serialization;

namespace Models.Pokemon
{
    public class Ability
    {
        [JsonPropertyName("ability")]
        public Details? Details { get; set; } = new();

        [JsonPropertyName("is_hidden")]
        public bool? IsHidden { get; set; }

        [JsonPropertyName("slot")]
        public int? Slot { get; set; }
    }

    public class Details
    {
        [JsonPropertyName("name")]
        public string? Name { get; set; }

        [JsonPropertyName("url")]
        public string? Url { get; set; }
    }

    public class Animated
    {
        [JsonPropertyName("back_default")]
        public string? BackDefault { get; set; }

        [JsonPropertyName("back_female")]
        public string? BackFemale { get; set; }

        [JsonPropertyName("back_shiny")]
        public string? BackShiny { get; set; }

        [JsonPropertyName("back_shiny_female")]
        public string? BackShinyFemale { get; set; }

        [JsonPropertyName("front_default")]
        public string? FrontDefault { get; set; }

        [JsonPropertyName("front_female")]
        public string? FrontFemale { get; set; }

        [JsonPropertyName("front_shiny")]
        public string? FrontShiny { get; set; }

        [JsonPropertyName("front_shiny_female")]
        public string? FrontShinyFemale { get; set; }
    }

    public class BlackWhite
    {
        [JsonPropertyName("animated")]
        public Animated? Animated { get; set; }

        [JsonPropertyName("back_default")]
        public string? BackDefault { get; set; }

        [JsonPropertyName("back_female")]
        public string? BackFemale { get; set; }

        [JsonPropertyName("back_shiny")]
        public string? BackShiny { get; set; }

        [JsonPropertyName("back_shiny_female")]
        public string? BackShinyFemale { get; set; }

        [JsonPropertyName("front_default")]
        public string? FrontDefault { get; set; }

        [JsonPropertyName("front_female")]
        public string? FrontFemale { get; set; }

        [JsonPropertyName("front_shiny")]
        public string? FrontShiny { get; set; }

        [JsonPropertyName("front_shiny_female")]
        public string? FrontShinyFemale { get; set; }
    }

    public class Crystal
    {
        [JsonPropertyName("back_default")]
        public string? BackDefault { get; set; }

        [JsonPropertyName("back_shiny")]
        public string? BackShiny { get; set; }

        [JsonPropertyName("back_shiny_transparent")]
        public string? BackShinyTransparent { get; set; }

        [JsonPropertyName("back_transparent")]
        public string? BackTransparent { get; set; }

        [JsonPropertyName("front_default")]
        public string? FrontDefault { get; set; }

        [JsonPropertyName("front_shiny")]
        public string? FrontShiny { get; set; }

        [JsonPropertyName("front_shiny_transparent")]
        public string? FrontShinyTransparent { get; set; }

        [JsonPropertyName("front_transparent")]
        public string? FrontTransparent { get; set; }
    }

    public class DiamondPearl
    {
        [JsonPropertyName("back_default")]
        public string? BackDefault { get; set; }

        [JsonPropertyName("back_female")]
        public string? BackFemale { get; set; }

        [JsonPropertyName("back_shiny")]
        public string? BackShiny { get; set; }

        [JsonPropertyName("back_shiny_female")]
        public string? BackShinyFemale { get; set; }

        [JsonPropertyName("front_default")]
        public string? FrontDefault { get; set; }

        [JsonPropertyName("front_female")]
        public string? FrontFemale { get; set; }

        [JsonPropertyName("front_shiny")]
        public string? FrontShiny { get; set; }

        [JsonPropertyName("front_shiny_female")]
        public string? FrontShinyFemale { get; set; }
    }

    public class DreamWorld
    {
        [JsonPropertyName("front_default")]
        public string? FrontDefault { get; set; }

        [JsonPropertyName("front_female")]
        public string? FrontFemale { get; set; }
    }

    public class Emerald
    {
        [JsonPropertyName("front_default")]
        public string? FrontDefault { get; set; }

        [JsonPropertyName("front_shiny")]
        public string? FrontShiny { get; set; }
    }

    public class FireredLeafgreen
    {
        [JsonPropertyName("back_default")]
        public string? BackDefault { get; set; }

        [JsonPropertyName("back_shiny")]
        public string? BackShiny { get; set; }

        [JsonPropertyName("front_default")]
        public string? FrontDefault { get; set; }

        [JsonPropertyName("front_shiny")]
        public string? FrontShiny { get; set; }
    }

    public class GameIndex
    {
        [JsonPropertyName("game_index")]
        public int? Index { get; set; }

        [JsonPropertyName("version")]
        public Details? Version { get; set; }
    }

    public class GenerationI
    {
        [JsonPropertyName("red-blue")]
        public RedBlue? RedBlue { get; set; }

        [JsonPropertyName("yellow")]
        public Yellow? Yellow { get; set; }
    }

    public class GenerationII
    {
        [JsonPropertyName("crystal")]
        public Crystal? Crystal { get; set; }

        [JsonPropertyName("gold")]
        public Gold? Gold { get; set; }

        [JsonPropertyName("silver")]
        public Silver? Silver { get; set; }
    }

    public class GenerationIII
    {
        [JsonPropertyName("emerald")]
        public Emerald? Emerald { get; set; }

        [JsonPropertyName("firered-leafgreen")]
        public FireredLeafgreen? FireredLeafgreen { get; set; }

        [JsonPropertyName("ruby-sapphire")]
        public RubySapphire? RubySapphire { get; set; }
    }

    public class GenerationIV
    {
        [JsonPropertyName("diamond-pearl")]
        public DiamondPearl? DiamondPearl { get; set; }

        [JsonPropertyName("heartgold-soulsilver")]
        public HeartgoldSoulsilver? HeartgoldSoulsilver { get; set; }

        [JsonPropertyName("platinum")]
        public Platinum? Platinum { get; set; }
    }

    public class GenerationV
    {
        [JsonPropertyName("black-white")]
        public BlackWhite? BlackWhite { get; set; }
    }

    public class GenerationVI
    {
        [JsonPropertyName("omegaruby-alphasapphire")]
        public OmegarubyAlphasapphire? OmegarubyAlphasapphire { get; set; }

        [JsonPropertyName("x-y")]
        public XY? XY { get; set; }
    }

    public class GenerationVII
    {
        [JsonPropertyName("icons")]
        public Icons? Icons { get; set; }

        [JsonPropertyName("ultra-sun-ultra-moon")]
        public UltraSunUltraMoon? UltraSunUltraMoon { get; set; }
    }

    public class GenerationVIII
    {
        [JsonPropertyName("icons")]
        public Icons? Icons { get; set; }
    }

    public class Gold
    {
        [JsonPropertyName("back_default")]
        public string? BackDefault { get; set; }

        [JsonPropertyName("back_shiny")]
        public string? BackShiny { get; set; }

        [JsonPropertyName("front_default")]
        public string? FrontDefault { get; set; }

        [JsonPropertyName("front_shiny")]
        public string? FrontShiny { get; set; }

        [JsonPropertyName("front_transparent")]
        public string? FrontTransparent { get; set; }
    }

    public class HeartgoldSoulsilver
    {
        [JsonPropertyName("back_default")]
        public string? BackDefault { get; set; }

        [JsonPropertyName("back_female")]
        public string? BackFemale { get; set; }

        [JsonPropertyName("back_shiny")]
        public string? BackShiny { get; set; }

        [JsonPropertyName("back_shiny_female")]
        public string? BackShinyFemale { get; set; }

        [JsonPropertyName("front_default")]
        public string? FrontDefault { get; set; }

        [JsonPropertyName("front_female")]
        public string? FrontFemale { get; set; }

        [JsonPropertyName("front_shiny")]
        public string? FrontShiny { get; set; }

        [JsonPropertyName("front_shiny_female")]
        public string? FrontShinyFemale { get; set; }
    }

    public class HeldItem
    {
        [JsonPropertyName("item")]
        public Details? Item { get; set; }

        [JsonPropertyName("version_details")]
        public List<VersionDetails>? VersionDetails { get; set; } = new();
    }

    public class Home
    {
        [JsonPropertyName("front_default")]
        public string? FrontDefault { get; set; }

        [JsonPropertyName("front_female")]
        public string? FrontFemale { get; set; }

        [JsonPropertyName("front_shiny")]
        public string? FrontShiny { get; set; }

        [JsonPropertyName("front_shiny_female")]
        public string? FrontShinyFemale { get; set; }
    }

    public class Icons
    {
        [JsonPropertyName("front_default")]
        public string? FrontDefault { get; set; }

        [JsonPropertyName("front_female")]
        public string? FrontFemale { get; set; }
    }

    public class Move
    {
        [JsonPropertyName("move")]
        public Details? Details { get; set; }

        [JsonPropertyName("version_group_details")]
        public List<VersionGroupDetails>? VersionGroupDetails { get; set; } = new();
    }

    public class OfficialArtwork
    {
        [JsonPropertyName("front_default")]
        public string? FrontDefault { get; set; }

        [JsonPropertyName("front_shiny")]
        public string? FrontShiny { get; set; }
    }

    public class OmegarubyAlphasapphire
    {
        [JsonPropertyName("front_default")]
        public string? FrontDefault { get; set; }

        [JsonPropertyName("front_female")]
        public string? FrontFemale { get; set; }

        [JsonPropertyName("front_shiny")]
        public string? FrontShiny { get; set; }

        [JsonPropertyName("front_shiny_female")]
        public string? FrontShinyFemale { get; set; }
    }

    public class Other
    {
        [JsonPropertyName("dream_world")]
        public DreamWorld? DreamWorld { get; set; }

        [JsonPropertyName("home")]
        public Home? Home { get; set; }

        [JsonPropertyName("official-artwork")]
        public OfficialArtwork? OfficialArtwork { get; set; }
    }

    public class Platinum
    {
        [JsonPropertyName("back_default")]
        public string? BackDefault { get; set; }

        [JsonPropertyName("back_female")]
        public string? BackFemale { get; set; }

        [JsonPropertyName("back_shiny")]
        public string? BackShiny { get; set; }

        [JsonPropertyName("back_shiny_female")]
        public string? BackShinyFemale { get; set; }

        [JsonPropertyName("front_default")]
        public string? FrontDefault { get; set; }

        [JsonPropertyName("front_female")]
        public string? FrontFemale { get; set; }

        [JsonPropertyName("front_shiny")]
        public string? FrontShiny { get; set; }

        [JsonPropertyName("front_shiny_female")]
        public string? FrontShinyFemale { get; set; }
    }

    public class RedBlue
    {
        [JsonPropertyName("back_default")]
        public string? BackDefault { get; set; }

        [JsonPropertyName("back_gray")]
        public string? BackGray { get; set; }

        [JsonPropertyName("back_transparent")]
        public string? BackTransparent { get; set; }

        [JsonPropertyName("front_default")]
        public string? FrontDefault { get; set; }

        [JsonPropertyName("front_gray")]
        public string? FrontGray { get; set; }

        [JsonPropertyName("front_transparent")]
        public string? FrontTransparent { get; set; }
    }

    public class Pokemon
    {
        [JsonPropertyName("abilities")]
        public List<Ability>? Abilities { get; set; } = new();

        [JsonPropertyName("base_experience")]
        public int? BaseExperience { get; set; }

        [JsonPropertyName("forms")]
        public List<Details>? Forms { get; set; } = new();

        [JsonPropertyName("game_indices")]
        public List<GameIndex>? GameIndices { get; set; } = new();

        [JsonPropertyName("height")]
        public int? Height { get; set; }

        [JsonPropertyName("held_items")]
        public List<HeldItem>? HeldItems { get; set; } = new();

        [JsonPropertyName("id")]
        public int? Id { get; set; }

        [JsonPropertyName("is_default")]
        public bool? IsDefault { get; set; }

        [JsonPropertyName("location_area_encounters")]
        public string? LocationAreaEncounters { get; set; }

        [JsonPropertyName("moves")]
        public List<Move> Moves { get; set; } = new();

        [JsonPropertyName("name")]
        public string? Name { get; set; }

        [JsonPropertyName("order")]
        public int? Order { get; set; }

        [JsonPropertyName("past_types")]
        public List<object> PastTypes { get; set; } = new();

        [JsonPropertyName("species")]
        public Details? Species { get; set; }

        [JsonPropertyName("sprites")]
        public Sprites? Sprites { get; set; }

        [JsonPropertyName("stats")]
        public List<Stat>? Stats { get; set; } = new();

        [JsonPropertyName("types")]
        public List<Type>? Types { get; set; } = new();

        [JsonPropertyName("weight")]
        public int? Weight { get; set; }
    }

    public class RubySapphire
    {
        [JsonPropertyName("back_default")]
        public string? BackDefault { get; set; }

        [JsonPropertyName("back_shiny")]
        public string? BackShiny { get; set; }

        [JsonPropertyName("front_default")]
        public string? FrontDefault { get; set; }

        [JsonPropertyName("front_shiny")]
        public string? FrontShiny { get; set; }
    }

    public class Silver
    {
        [JsonPropertyName("back_default")]
        public string? BackDefault { get; set; }

        [JsonPropertyName("back_shiny")]
        public string? BackShiny { get; set; }

        [JsonPropertyName("front_default")]
        public string? FrontDefault { get; set; }

        [JsonPropertyName("front_shiny")]
        public string? FrontShiny { get; set; }

        [JsonPropertyName("front_transparent")]
        public string? FrontTransparent { get; set; }
    }

    public class Sprites
    {
        [JsonPropertyName("back_default")]
        public string? BackDefault { get; set; }

        [JsonPropertyName("back_female")]
        public string? BackFemale { get; set; }

        [JsonPropertyName("back_shiny")]
        public string? BackShiny { get; set; }

        [JsonPropertyName("back_shiny_female")]
        public string? BackShinyFemale { get; set; }

        [JsonPropertyName("front_default")]
        public string? FrontDefault { get; set; }

        [JsonPropertyName("front_female")]
        public string? FrontFemale { get; set; }

        [JsonPropertyName("front_shiny")]
        public string? FrontShiny { get; set; }

        [JsonPropertyName("front_shiny_female")]
        public string? FrontShinyFemale { get; set; }

        [JsonPropertyName("other")]
        public Other? Other { get; set; }

        [JsonPropertyName("versions")]
        public Versions? Versions { get; set; }
    }

    public class Stat
    {
        [JsonPropertyName("base_stat")]
        public int? BaseStat { get; set; }

        [JsonPropertyName("effort")]
        public int? Effort { get; set; }

        [JsonPropertyName("stat")]
        public Details? Details { get; set; }
    }

    public class Type
    {
        [JsonPropertyName("slot")]
        public int? Slot { get; set; }

        [JsonPropertyName("type")]
        public Details? Details { get; set; }
    }

    public class UltraSunUltraMoon
    {
        [JsonPropertyName("front_default")]
        public string? FrontDefault { get; set; }

        [JsonPropertyName("front_female")]
        public string? FrontFemale { get; set; }

        [JsonPropertyName("front_shiny")]
        public string? FrontShiny { get; set; }

        [JsonPropertyName("front_shiny_female")]
        public string? FrontShinyFemale { get; set; }
    }

    public class VersionDetails
    {
        [JsonPropertyName("rarity")]
        public int? Rarity { get; set; }

        [JsonPropertyName("version")]
        public Details? Version { get; set; }
    }

    public class VersionGroupDetails
    {
        [JsonPropertyName("level_learned_at")]
        public int? LevelLearnedAt { get; set; }

        [JsonPropertyName("move_learn_method")]
        public Details? MoveLearnMethod { get; set; }

        [JsonPropertyName("version_group")]
        public Details? VersionGroup { get; set; }
    }

    public class Versions
    {
        [JsonPropertyName("generation-i")]
        public GenerationI? GenerationI { get; set; }

        [JsonPropertyName("generation-ii")]
        public GenerationII? GenerationII { get; set; }

        [JsonPropertyName("generation-iii")]
        public GenerationIII? GenerationIII { get; set; }

        [JsonPropertyName("generation-iv")]
        public GenerationIV? GenerationIV { get; set; }

        [JsonPropertyName("generation-v")]
        public GenerationV? GenerationV { get; set; }

        [JsonPropertyName("generation-vi")]
        public GenerationVI? GenerationVI { get; set; }

        [JsonPropertyName("generation-vii")]
        public GenerationVII? GenerationVII { get; set; }

        [JsonPropertyName("generation-viii")]
        public GenerationVIII? GenerationVIII { get; set; }
    }

    public class XY
    {
        [JsonPropertyName("front_default")]
        public string? FrontDefault { get; set; }

        [JsonPropertyName("front_female")]
        public string? FrontFemale { get; set; }

        [JsonPropertyName("front_shiny")]
        public string? FrontShiny { get; set; }

        [JsonPropertyName("front_shiny_female")]
        public string? FrontShinyFemale { get; set; }
    }

    public class Yellow
    {
        [JsonPropertyName("back_default")]
        public string? BackDefault { get; set; }

        [JsonPropertyName("back_gray")]
        public string? BackGray { get; set; }

        [JsonPropertyName("back_transparent")]
        public string? BackTransparent { get; set; }

        [JsonPropertyName("front_default")]
        public string? FrontDefault { get; set; }

        [JsonPropertyName("front_gray")]
        public string? FrontGray { get; set; }

        [JsonPropertyName("front_transparent")]
        public string? FrontTransparent { get; set; }
    }
}