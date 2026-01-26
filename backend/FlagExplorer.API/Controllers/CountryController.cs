using Microsoft.AspNetCore.Mvc;
using FlagExplorer.API.Services;

namespace FlagExplorer.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CountryController : ControllerBase
    {
        private readonly CountryService _service;

        public CountryController(CountryService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> Get(double lat, double lng)
        {
            var result = await _service.GetCountryByCoordinates(lat, lng);

            if (result == null)
                return NotFound("País não encontrado");

            return Ok(result);
        }
    }
}
