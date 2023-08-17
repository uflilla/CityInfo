using CityInfo.API.Models;

namespace CityInfo.API.DAL
{
  public class CityInfoInMemoryDatabase 
  {
    public List<CityDto> Cities { get; set; } = new List<CityDto>();

    public static CityInfoInMemoryDatabase Current { get; } = new();

    private CityInfoInMemoryDatabase()
    {
      GetCities();
    }

    

    private void GetCities()
    {
      Cities.Add(new CityDto()
      {
        Id=1,Name = "Islamabad",Description = "Capital of Pakistan",
        PointsOfInterests = new List<PointOfInterestDto>()
        {
          new PointOfInterestDto(){Id = 1,Name = "Faisal Mosque", Description = "Largest Mosque in Pakistan"},
          new PointOfInterestDto(){Id = 2,Name = "Pakistan Monument", Description = "A monument for making of pakistan"},
        }
      });
      Cities.Add(new CityDto()
      {
        Id=2,Name = "Lahore",Description = "Capital of Punjab",
        PointsOfInterests = new List<PointOfInterestDto>()
        {
          new PointOfInterestDto(){Id = 1,Name = "Badshahi Mosque", Description = "Largest Mosque in Punjab"},
          new PointOfInterestDto(){Id = 2,Name = "Lahore Fort", Description = "A fort build by Mughals"},
        }
      });
      Cities.Add(new CityDto()
      {
        Id=3,Name = "Karachi",Description = "Capital of Sindh",
        PointsOfInterests = new List<PointOfInterestDto>()
        {
          new PointOfInterestDto(){Id = 1,Name = "Qaid Tomb", Description = "Grave of M.A Jinnah"},
          new PointOfInterestDto(){Id = 2,Name = "Clifton", Description = "A beach in Karachi"},
        }
      });
    }
  }
}
