using CityInfo.API.DAL;
using CityInfo.API.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;

namespace CityInfo.API.Controllers
{
  [Route("api/cities/{cityId:int}/pointsofinterest")]
  [ApiController]
  public class PointsOfInterestController : ControllerBase
  {
    private readonly ILogger<PointsOfInterestController> _logger;

    public PointsOfInterestController(ILogger<PointsOfInterestController> logger)
    {
      _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }
    [HttpGet]
    public ActionResult<IEnumerable<PointOfInterestDto>> GetPointsOfInterest(int cityId)
    {
      try
      {
        throw new Exception("Test Exception");
        var city = CityInfoInMemoryDatabase.Current.Cities.FirstOrDefault(c => c.Id == cityId);
        if (city is null)
        {
          _logger.LogInformation($"city with ID:{cityId} not found");
          return NotFound();
        }
        return Ok(city.PointsOfInterests);
      }
      catch (Exception e)
      {
        _logger.LogCritical($"Exception occured while getting POIs for city with ID: {cityId}",e);
        return StatusCode(500, "Error occurred while handling the request.");
      }
      
    }

    [HttpGet("{pointOfInterestId:int}", Name = "GetPointOfInterestOfCity")]
    public ActionResult<PointOfInterestDto> GetPointOfInterest(int cityId, int pointOfInterestId)
    {

      var city = CityInfoInMemoryDatabase.Current.Cities.FirstOrDefault(c => c.Id == cityId);
      if (city is null)
      {
        _logger.LogInformation($"city with ID:{cityId} not found");
        return NotFound();
      }
      var poi = city.PointsOfInterests.FirstOrDefault(p => p.Id == pointOfInterestId);
      if (poi is null) return NotFound();
      return Ok(poi);
    }

    [HttpPost]
    public ActionResult<PointOfInterestDto> PostPointOfInterest(int cityId,
      PointOfInterestForCreationDto pointOfInterestForCreationDto)
    {
      var city = CityInfoInMemoryDatabase.Current.Cities.FirstOrDefault(c => c.Id == cityId);
      if (city is null) return NotFound();

      var maxId = city.PointsOfInterests.Max(p => p.Id);

      var newPointOfInterest = new PointOfInterestDto()
      {
        Id = ++maxId, Name = pointOfInterestForCreationDto.Name, Description = pointOfInterestForCreationDto.Description
      };
      city.PointsOfInterests.Add(newPointOfInterest);

      return CreatedAtRoute("GetPointOfInterestOfCity", new
      {
        cityId = city.Id,
        pointOfInterestId = newPointOfInterest.Id
      }, newPointOfInterest);
    }

    [HttpPut("{pointOfInterestId}")]
    public ActionResult PutPointOfInterest(int cityId,int pointOfInterestId, PointOfInterestForUpdateDto pointOfInterest)
    {
      var city = CityInfoInMemoryDatabase.Current.Cities.FirstOrDefault(c => c.Id == cityId);
      if (city is null) return NotFound();
      var poi = city.PointsOfInterests.FirstOrDefault(p => p.Id == pointOfInterestId);
      if (poi is null) return NotFound();

      poi.Name = pointOfInterest.Name;
      poi.Description = pointOfInterest.Description;
      //SaveChanges() for db
      return NoContent(); 
    }

    [HttpPatch("{pointOfInterestId}")]
    public ActionResult PatchPointOfInterest(int cityId, int pointOfInterestId,
      JsonPatchDocument<PointOfInterestForUpdateDto> patchDoc)
    {
      var city = CityInfoInMemoryDatabase.Current.Cities.FirstOrDefault(c => c.Id == cityId);
      if (city is null) return NotFound();
      var poi = city.PointsOfInterests.FirstOrDefault(p => p.Id == pointOfInterestId);
      if (poi is null) return NotFound();

      var poiToUpdate = new PointOfInterestForUpdateDto() { Name = poi.Name, Description = poi.Description };
      patchDoc.ApplyTo(poiToUpdate,ModelState);
      if (!ModelState.IsValid) return BadRequest(ModelState); 
      if (!TryValidateModel(poiToUpdate)) return BadRequest(ModelState);

      poi.Name = poiToUpdate.Name;
      poi.Description = poiToUpdate.Description;

      return NoContent();
    }

    [HttpDelete("{poiId}")]
    public ActionResult DeletePointOfInterest(int cityId, int poiId)
    {
      var city = CityInfoInMemoryDatabase.Current.Cities.FirstOrDefault(c => c.Id == cityId);
      if (city is null) return NotFound();
      var poi = city.PointsOfInterests.FirstOrDefault(p => p.Id == poiId);
      if (poi is null) return NotFound();

      city.PointsOfInterests.Remove(poi);
      return NoContent();
    }
}
}
