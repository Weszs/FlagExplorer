namespace FlagExplorer.API.Models
{
    public class CountryDto
    {
        public string? Name { get; set; }
        public string? Capital { get; set; }
        public string? FlagUrl { get; set; }
        public long Population { get; set; }
        public string? Region { get; set; }
        public required List<string> Timezones { get; set; }
        public required List<string> Languages { get; set; }
    }
}
