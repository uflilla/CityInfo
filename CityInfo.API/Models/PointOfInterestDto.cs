using System.ComponentModel.DataAnnotations;

namespace CityInfo.API.Models;

public class PointOfInterestDto
{
  public int Id { get; set; }
  public string Name { get; set; } = string.Empty;
  public string? Description { get; set; }

}

public class PointOfInterestForCreationDto
{
  //https://fluentvalidation.net for more advance validation.

  [Required(ErrorMessage = "Name should be provided."),MaxLength(50)]
  public string Name { get; set; } = string.Empty;
  [MaxLength(200)]
  public string? Description { get; set; }
}

public class PointOfInterestForUpdateDto
{
  //https://fluentvalidation.net for more advance validation.

  [Required(ErrorMessage = "Name should be provided."),MaxLength(50)]
  public string Name { get; set; } = string.Empty;
  [MaxLength(200)]
  public string? Description { get; set; }
}