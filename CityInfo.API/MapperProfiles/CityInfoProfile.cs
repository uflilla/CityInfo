using AutoMapper;
using CityInfo.API.Entities;
using CityInfo.API.Models;

namespace CityInfo.API.MapperProfiles
{
  public class CityInfoProfile : Profile
  {
    public CityInfoProfile()
    {
      CreateMap<City, CityDtoWithoutPoi>();
    }
  }
}
