﻿using CityInfo.API.DAL;
using CityInfo.API.Models;
using Microsoft.AspNetCore.Mvc;

namespace CityInfo.API.Controllers
{
  [Route("/api/Cities")]
  [ApiController]
  public class CitiesController : ControllerBase
  {
    [HttpGet]
    public ActionResult<IEnumerable<CityDto>> GetCities()
    {
      return Ok(CityInfoInMemoryDatabase.Current.Cities);
    }

    [HttpGet("{id}")]
    public ActionResult<CityDto> GetCity(int id)
    {
      var city=CityInfoInMemoryDatabase.Current.Cities.FirstOrDefault(c => c.Id == id);
      if (city is null) return NotFound();
      return Ok(city);
    }
  }
}
