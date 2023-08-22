using System.Security.Authentication;
using AutoMapper;
using CityInfo.API.DAL;
using CityInfo.API.Models;
using CityInfo.API.Services;
using Microsoft.AspNetCore.Mvc;

namespace CityInfo.API.Controllers
{
  [Route("/api/Cities")]
  [ApiController]
  public class CitiesController : ControllerBase
  {
    private readonly ICityInfoRepository _cityInfoRepository;
    private readonly IMapper _mapper;

    public CitiesController(ICityInfoRepository cityInfoRepository, IMapper mapper)
    {
      _cityInfoRepository = cityInfoRepository ?? throw new ArgumentNullException(nameof(cityInfoRepository));
      _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
    }
    [HttpGet]
    public async Task<ActionResult<IEnumerable<CityDtoWithoutPoi>>> GetCities()
    {
      var cityEntities = await _cityInfoRepository.GetCitiesAsync();
      var result = _mapper.Map<IEnumerable<CityDtoWithoutPoi>>(cityEntities);
      return Ok(result);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetCity(int id, bool includePoi=false)
    {
      var city = await _cityInfoRepository.GetCityAsync(id, includePoi);
      if (city is null) return NotFound();
      if (includePoi)
      {
        return Ok(_mapper.Map<CityDto>(city));
      }

      return Ok(_mapper.Map<CityDtoWithoutPoi>(city));
      /*var city=_cityInfoRepository.Cities.FirstOrDefault(c => c.Id == id);
      if (city is null) return NotFound();*/
    }
  }
}
