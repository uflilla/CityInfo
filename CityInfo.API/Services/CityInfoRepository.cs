using CityInfo.API.DbContexts;
using CityInfo.API.Entities;
using Microsoft.EntityFrameworkCore;

namespace CityInfo.API.Services
{
  public class CityInfoRepository : ICityInfoRepository
  {
    private readonly CityInfoContext _context;

    public CityInfoRepository(CityInfoContext context)
    {
      _context = context ?? throw new ArgumentNullException(nameof(context));
    }
    public async Task<IEnumerable<City>> GetCitiesAsync()
    {
      return await _context.Cities.OrderBy(c=>c.Name).ToListAsync();
    }
    public async Task<IEnumerable<City>> GetCitiesAsync(string? name, string? searchQuery)
    {
      if(string.IsNullOrWhiteSpace(name) && string.IsNullOrWhiteSpace(searchQuery)) return await GetCitiesAsync();

      var collection=_context.Cities as IQueryable<City>;
      if (!string.IsNullOrWhiteSpace(name))
      {
        collection=collection.Where(c=>c.Name==name.Trim());
      }
      if (!string.IsNullOrWhiteSpace(searchQuery))
      {
        var sq=searchQuery.Trim();
        collection.Where(c=>c.Name.Contains(sq) || (c.Description.Contains(sq)));
      }
      return await collection.ToListAsync();
    }
    public async Task<bool> DoesCityExist(int cityId)
    {
      return await _context.Cities.AnyAsync(c => c.Id == cityId);
    }

    public async Task<City?> GetCityAsync(int cityId, bool includePoi)
    {
      if (includePoi)
      {
        return await _context.Cities.Include(c => c.PointsOfInterest).FirstOrDefaultAsync(c => c.Id == cityId);
      }

      return await _context.Cities.FirstOrDefaultAsync(c => c.Id == cityId);
    }

    public async Task<IEnumerable<PointOfInterest>> GetPointsOfInterestOfCity(int cityId)
    {
      return await _context.PointsOfInterest.Where(p => p.CityId == cityId).ToListAsync();
    }

    public async Task<PointOfInterest?> GetPointOfInterestAsync(int cityId, int poiId)
    {
      return await _context.PointsOfInterest.FirstOrDefaultAsync(p => p.Id == poiId && p.CityId == cityId);
    }

    public async Task AddPoiForCityAsync(int cityId, PointOfInterest poi)
    {
      var city = await GetCityAsync(cityId, false);
      city?.PointsOfInterest.Add(poi);
    }

    public void DeletePointOfInterest(PointOfInterest poi)
    {
      _context.PointsOfInterest.Remove(poi);
    }

    public async Task<bool> SaveChangesAsync()
    {
      return (await _context.SaveChangesAsync() >= 0);
    }

    
  }
}
