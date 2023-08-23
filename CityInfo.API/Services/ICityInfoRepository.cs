using CityInfo.API.Entities;
using CityInfo.API.Models.Helpers;

namespace CityInfo.API.Services
{
  public interface ICityInfoRepository
  {
    Task<(IEnumerable<City>,PaginationMetadata)> GetCitiesAsync(string? name, string? searchQuery, int pageNumber, int pageSize);

    Task<bool> DoesCityExist(int cityId);
    Task<City?> GetCityAsync(int cityId, bool includePoi);

    Task<IEnumerable<PointOfInterest>> GetPointsOfInterestOfCity(int cityId);

    Task<PointOfInterest?> GetPointOfInterestAsync(int cityId, int poiId);

    Task AddPoiForCityAsync(int cityId, PointOfInterest poi);
    void DeletePointOfInterest(PointOfInterest poi);
    Task<bool> CityNameMatchesId(string? name, int id);
    Task<bool> SaveChangesAsync();
  }
}
