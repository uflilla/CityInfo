using CityInfo.API.Entities;

namespace CityInfo.API.Services
{
  public interface ICityInfoRepository
  {
    Task<IEnumerable<City>> GetCitiesAsync();

    Task<City?> GetCityAsync(int cityId, bool includePoi);

    Task<IEnumerable<PointOfInterest>> GetPointsOfInterestOfCity(int cityId);

    Task<PointOfInterest?> GetPointOfInterest(int cityId, int poiId);
  }
}
