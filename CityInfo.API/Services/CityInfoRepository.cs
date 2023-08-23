using CityInfo.API.DbContexts;
using CityInfo.API.Entities;
using CityInfo.API.Models.Helpers;
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
    public async Task<(IEnumerable<City>,PaginationMetadata)> GetCitiesAsync(string? name, string? searchQuery, int pageNumber, int pageSize)
    {
      
      var collection=_context.Cities as IQueryable<City>;
      if (!string.IsNullOrWhiteSpace(name))
      {
        collection=collection.Where(c=>c.Name==name.Trim());
      }
      if (!string.IsNullOrWhiteSpace(searchQuery))
      {
        var sq=searchQuery.Trim();
        collection=collection.Where(c=>c.Name.Contains(sq) || (c.Description.Contains(sq)));
      }

      var count = await collection.CountAsync();
      var paginationData = new PaginationMetadata(count, pageNumber, pageSize);
      var citiesToReturn= await collection.OrderBy(c=>c.Name).Skip((pageNumber-1)*pageSize).Take(pageSize).ToListAsync();
      return (citiesToReturn, paginationData);
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

    public Task<bool> CityNameMatchesId(string? name, int id)
    {
      return _context.Cities.AnyAsync(c => c.Name == name && c.Id == id);
    }

    public async Task<bool> SaveChangesAsync()
    {
      return (await _context.SaveChangesAsync() >= 0);
    }

    
  }
}
