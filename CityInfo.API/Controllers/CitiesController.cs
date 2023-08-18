using System.Security.Authentication;
using CityInfo.API.DAL;
using CityInfo.API.Models;
using Microsoft.AspNetCore.Mvc;

namespace CityInfo.API.Controllers
{
  [Route("/api/Cities")]
  [ApiController]
  public class CitiesController : ControllerBase
  {
    private readonly CityInfoInMemoryDatabase _cityInfoDatabase;

    public CitiesController(CityInfoInMemoryDatabase cityInfoDatabase)
    {
      _cityInfoDatabase = cityInfoDatabase ?? throw new ArgumentNullException(nameof(cityInfoDatabase));
    }
    [HttpGet]
    public ActionResult<IEnumerable<CityDto>> GetCities()
    {
      return Ok(_cityInfoDatabase.Cities);
    }

    [HttpGet("{id}")]
    public ActionResult<CityDto> GetCity(int id)
    {
      var city=_cityInfoDatabase.Cities.FirstOrDefault(c => c.Id == id);
      if (city is null) return NotFound();
      return Ok(city);
    }
  }
}
