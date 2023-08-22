using CityInfo.API.Entities;

namespace CityInfo.API.Services
{
  public interface ICityInfoRepository
  {
    Task<IEnumerable<City>> GetCitiesAsync(string? name, string? searchQuery);

    Task<bool> DoesCityExist(int cityId);
    Task<City?> GetCityAsync(int cityId, bool includePoi);

    Task<IEnumerable<PointOfInterest>> GetPointsOfInterestOfCity(int cityId);

    Task<PointOfInterest?> GetPointOfInterestAsync(int cityId, int poiId);

    Task AddPoiForCityAsync(int cityId, PointOfInterest poi);
    void DeletePointOfInterest(PointOfInterest poi);
    Task<bool> SaveChangesAsync();
  }
}
