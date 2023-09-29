namespace Models.Pokemon.ResultsData
{
    public class ResultsData
    {
        public int? Count { get; set; }
        public string? Next { get; set; }
        public string? Previous { get; set; }
        public List<Details> Results { get; set; } = new();
    }
}