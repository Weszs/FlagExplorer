using Microsoft.AspNetCore.Mvc;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

[ApiController]
[Route("api/[controller]")]
public class CountryController : ControllerBase
{
    private readonly HttpClient _httpClient;

    public CountryController(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    [HttpGet]
    public async Task<IActionResult> Get(double lat, double lng)
    {
        try
        {
            var geoUrl = $"https://nominatim.openstreetmap.org/reverse?format=json&lat={lat}&lon={lng}";

            var request = new HttpRequestMessage(HttpMethod.Get, geoUrl);
            request.Headers.Add("User-Agent", "FlagExplorerApp/1.0 (+https://github.com/seuusuario/flagexplorer)");

            var geoResponse = await _httpClient.SendAsync(request);
            geoResponse.EnsureSuccessStatusCode();

            var geoJsonString = await geoResponse.Content.ReadAsStringAsync();
            using var geoJson = JsonDocument.Parse(geoJsonString);

            if (!geoJson.RootElement.TryGetProperty("address", out var address))
                return BadRequest("País não encontrado");

            if (!address.TryGetProperty("country_code", out var countryCodeProp))
                return BadRequest("País não encontrado");

            var countryCode = countryCodeProp.GetString()?.ToUpper();
            if (string.IsNullOrEmpty(countryCode))
                return BadRequest("País não encontrado");

            var countryUrl = $"https://restcountries.com/v3.1/alpha/{countryCode}";
            var countryResponse = await _httpClient.GetStringAsync(countryUrl);

            return Content(countryResponse, "application/json");
        }
        catch (HttpRequestException ex)
        {
            Console.WriteLine($"Erro na requisição HTTP: {ex.Message}");
            return BadRequest("Erro ao consultar o país (HTTP)");
        }
        catch (JsonException ex)
        {
            Console.WriteLine($"Erro ao processar JSON: {ex.Message}");
            return BadRequest("Erro ao consultar o país (JSON)");
        }
        catch (System.Exception ex)
        {
            Console.WriteLine($"Erro inesperado: {ex.Message}");
            return BadRequest("Erro ao consultar o país (desconhecido)");
        }
    }
}
