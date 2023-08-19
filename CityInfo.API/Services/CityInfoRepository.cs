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

    public async Task<PointOfInterest?> GetPointOfInterest(int cityId, int poiId)
    {
      return await _context.PointsOfInterest.FirstOrDefaultAsync(p => p.Id == poiId && p.CityId == cityId);
    }
  }
}
