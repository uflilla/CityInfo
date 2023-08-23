using AutoMapper;
using CityInfo.API.DAL;
using CityInfo.API.Entities;
using CityInfo.API.Models;
using CityInfo.API.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;

namespace CityInfo.API.Controllers
{
  [Route("api/cities/{cityId:int}/pointsofinterest")]
  [Authorize]
  [ApiController]
  public class PointsOfInterestController : ControllerBase
  {
    private readonly ILogger<PointsOfInterestController> _logger;
    private readonly IMailService _mailService;
    private readonly ICityInfoRepository _cityInfoRepository;
    private readonly IMapper _mapper;

    public PointsOfInterestController(ILogger<PointsOfInterestController> logger,
      IMailService mailService, ICityInfoRepository cityInfoRepository, IMapper mapper)
    {
      _logger = logger ?? throw new ArgumentNullException(nameof(logger));
      _mailService = mailService ?? throw new ArgumentNullException(nameof(mailService));
      _cityInfoRepository = cityInfoRepository ?? throw new ArgumentNullException(nameof(cityInfoRepository));
      _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
    }
    [HttpGet]
    public async Task<ActionResult<IEnumerable<PointOfInterestDto>>> GetPointsOfInterest(int cityId)
    {
      var city = User.Claims.FirstOrDefault(c => c.Type == "city")?.Value;
      if (!await _cityInfoRepository.CityNameMatchesId(city, cityId)) return Forbid();
      if (!await _cityInfoRepository.DoesCityExist(cityId)) return NotFound();
      var pois = await _cityInfoRepository.GetPointsOfInterestOfCity(cityId);
      return Ok(_mapper.Map<IEnumerable<PointOfInterestDto>>(pois));

    }

    [HttpGet("{pointOfInterestId:int}", Name = "GetPointOfInterestOfCity")]
    public async Task<ActionResult<PointOfInterestDto>> GetPointOfInterest(int cityId, int pointOfInterestId)
    {
      if (!await _cityInfoRepository.DoesCityExist(cityId))
      {
        _logger.LogInformation($"city with ID:{cityId} not found");
        return NotFound();
      }

      var poi = await _cityInfoRepository.GetPointOfInterestAsync(cityId, pointOfInterestId);
      if (poi is null) return NotFound();
      return Ok(_mapper.Map<PointOfInterestDto>(poi));
    }

    [HttpPost]
    public async Task<ActionResult<PointOfInterestDto>> PostPointOfInterest(int cityId,
      PointOfInterestForCreationDto pointOfInterestForCreationDto)
    {
      if (!await _cityInfoRepository.DoesCityExist(cityId)) return NotFound();

      var finalPoi = _mapper.Map<PointOfInterest>(pointOfInterestForCreationDto);
      await _cityInfoRepository.AddPoiForCityAsync(cityId, finalPoi);
      await _cityInfoRepository.SaveChangesAsync();
      var createdPoi = _mapper.Map<PointOfInterestDto>(finalPoi);
      return CreatedAtRoute("GetPointOfInterestOfCity", new
      {
        cityId = cityId,
        pointOfInterestId = createdPoi.Id
      }, createdPoi);
    }

    [HttpPut("{pointOfInterestId}")]
    public async Task<ActionResult> PutPointOfInterest(int cityId,int pointOfInterestId, PointOfInterestForUpdateDto pointOfInterest)
    {
      if (!await _cityInfoRepository.DoesCityExist(cityId)) return NotFound();
      var poi = await _cityInfoRepository.GetPointOfInterestAsync(cityId, pointOfInterestId);
      if (poi is null) return NotFound();
      _mapper.Map(pointOfInterest, poi);
      await _cityInfoRepository.SaveChangesAsync();
      return NoContent(); 
    }
    
    [HttpPatch("{pointOfInterestId}")]
    public async Task<ActionResult> PatchPointOfInterest(int cityId, int pointOfInterestId,
      JsonPatchDocument<PointOfInterestForUpdateDto> patchDoc)
    {
      if (!await _cityInfoRepository.DoesCityExist(cityId)) return NotFound();
      var poi = await _cityInfoRepository.GetPointOfInterestAsync(cityId, pointOfInterestId);
      if (poi is null) return NotFound();

      var poiToUpdate = _mapper.Map<PointOfInterestForUpdateDto>(poi);
      patchDoc.ApplyTo(poiToUpdate,ModelState);
      if (!ModelState.IsValid) return BadRequest(ModelState); 
      if (!TryValidateModel(poiToUpdate)) return BadRequest(ModelState);

      _mapper.Map(poiToUpdate, poi);
      await _cityInfoRepository.SaveChangesAsync();
    
      return NoContent();
    }

    [HttpDelete("{poiId}")]
    public async Task<ActionResult> DeletePointOfInterest(int cityId, int poiId)
    {
      if (!await _cityInfoRepository.DoesCityExist(cityId)) return NotFound();
      var poi = await _cityInfoRepository.GetPointOfInterestAsync(cityId, poiId);
      if (poi is null) return NotFound();
      _cityInfoRepository.DeletePointOfInterest(poi);
      await _cityInfoRepository.SaveChangesAsync();
      _mailService.Send("A point of interest deleted", $"A POI with ID: {poiId} from city with ID: {cityId} has been deleted");
      return NoContent();
    }
  }
}
