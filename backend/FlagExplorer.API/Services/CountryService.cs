using System.Text.Json;
using FlagExplorer.API.Models;

namespace FlagExplorer.API.Services
{
    public class CountryService
    {
        private readonly HttpClient _httpClient;

        public CountryService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<CountryDto?> GetCountryByCoordinates(double lat, double lng)
        {
            // 1️⃣ Reverse geocoding
            var geoRequest = new HttpRequestMessage(
                HttpMethod.Get,
                $"https://nominatim.openstreetmap.org/reverse?format=json&lat={lat}&lon={lng}"
            );

            geoRequest.Headers.Add("User-Agent", "FlagExplorer/1.0");

            var geoResponse = await _httpClient.SendAsync(geoRequest);
            geoResponse.EnsureSuccessStatusCode();

            var geoJson = JsonDocument.Parse(await geoResponse.Content.ReadAsStringAsync());

            if (!geoJson.RootElement.TryGetProperty("address", out var address) ||
                !address.TryGetProperty("country_code", out var codeProp))
                return null;

            var countryCode = codeProp.GetString()?.ToUpper();
            if (string.IsNullOrEmpty(countryCode))
                return null;

            // 2️⃣ Country data
            var countryResponse = await _httpClient
                .GetStringAsync($"https://restcountries.com/v3.1/alpha/{countryCode}");

            var countryJson = JsonDocument.Parse(countryResponse);
            var country = countryJson.RootElement[0];

            return new CountryDto
            {
                Name = country.GetProperty("name").GetProperty("common").GetString(),
                Capital = country.GetProperty("capital")[0].GetString(),
                FlagUrl = country.GetProperty("flags").GetProperty("png").GetString(),
                Population = country.GetProperty("population").GetInt64(),
                Region = country.GetProperty("region").GetString(),
                Timezones = country.GetProperty("timezones")
                                  .EnumerateArray()
                                  .Select(t => t.GetString())
                                  .Where(t => t != null)
                                  .ToList()!,
                Languages = country.GetProperty("languages")
                                   .EnumerateObject()
                                   .Select(l => l.Value.GetString())
                                   .Where(l => l != null)
                                   .ToList()!
            };
        }
    }
}
