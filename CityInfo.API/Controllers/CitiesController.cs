using System.Security.Authentication;
using System.Text.Json;
using AutoMapper;
using CityInfo.API.DAL;
using CityInfo.API.Models;
using CityInfo.API.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CityInfo.API.Controllers
{
  [Route("/api/Cities")]
  [Authorize]
  [ApiController]
  public class CitiesController : ControllerBase
  {
    private readonly ICityInfoRepository _cityInfoRepository;
    private readonly IMapper _mapper;
    private const int maxPageSize = 10;

    public CitiesController(ICityInfoRepository cityInfoRepository, IMapper mapper)
    {
      _cityInfoRepository = cityInfoRepository ?? throw new ArgumentNullException(nameof(cityInfoRepository));
      _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
    }
    [HttpGet]
    public async Task<ActionResult<IEnumerable<CityDtoWithoutPoi>>> GetCities(string? name, string? searchQuery,int pageNumber=1, int pageSize=10)
    {
      pageSize = pageSize > maxPageSize ? maxPageSize : pageSize;
      var (cityEntities,paginationMetaData) = await _cityInfoRepository.GetCitiesAsync(name, searchQuery, pageNumber, pageSize);
      var result = _mapper.Map<IEnumerable<CityDtoWithoutPoi>>(cityEntities);
      Response.Headers.Add("X-Pagination",
        JsonSerializer.Serialize(paginationMetaData));
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
