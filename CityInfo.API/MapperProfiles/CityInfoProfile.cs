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
      CreateMap<City, CityDto>();
      CreateMap<PointOfInterest, PointOfInterestDto>();
      CreateMap<PointOfInterestForCreationDto, PointOfInterest>();
      CreateMap<PointOfInterestForUpdateDto, PointOfInterest>();
      CreateMap<PointOfInterest, PointOfInterestForUpdateDto>();

    }
  }
}
