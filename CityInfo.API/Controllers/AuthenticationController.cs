using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace CityInfo.API.Controllers
{
  [Route("api/authentication")]
  [ApiController]
  public class AuthenticationController : ControllerBase
  {
    private readonly IConfiguration _configuration;

    public AuthenticationController(IConfiguration configuration)
    {
      this._configuration = configuration?? throw new ArgumentNullException(nameof(configuration));
    }
    [HttpPost("authenticate")]
    public ActionResult<string> Authenticate(AuthenticationRequestUserInfo userAuthInfo)
    {
      //Step-1: Validate user credentials
      var user = ValidateUserCredentials(userAuthInfo);
      if (user is null) return Unauthorized();
      //Step-2: Generate securityKey from the secret
      var securityKey = new SymmetricSecurityKey(
        Encoding.ASCII.GetBytes(_configuration["Authentication:secretForKey"]));
      //Step-3: Generate signing Credentials from the securityKey
      var signingCredentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
      //Step-4: Create Claims for the token
      var claimsForToken = new List<Claim>()
      {
        new Claim("sub", user.UserId.ToString()),
        new Claim("given_name", user.FirstName),
        new Claim("family_name", user.LastName),
        new Claim("city", user.City),
      };
      //Step-5: Generate JwtSecurityToken
      var jwtSecToken = new JwtSecurityToken(
        _configuration["Authentication:Issuer"],
        _configuration["Authentication:Audience"],
        claimsForToken,DateTime.UtcNow,DateTime.UtcNow.AddHours(1),
        signingCredentials);
      //Step-6: Write (Serialize) Token using JwtSecurityTokenHandler
      var tokenToReturn = new JwtSecurityTokenHandler().WriteToken(jwtSecToken);
      return tokenToReturn;
    }

    private CityInfoUser ValidateUserCredentials(AuthenticationRequestUserInfo userAuthInfo)
    {
      // In actual user info might be stored in a database. It is checked from there.
      var user = new CityInfoUser(1, userAuthInfo.UserName??"", "Umar", "Faruq", "Antwerp");
      return user;
    }


    class CityInfoUser
    {
      public int UserId { get; set; }
      public string UserName { get; set; }
      public string FirstName { get; set; }
      public string LastName { get; set; }
      public string City { get; set; }
      public CityInfoUser(int userId, string userName, string firstName, string lastName, string city)
      {
        UserId = userId;
        UserName = userName;
        FirstName = firstName;
        LastName = lastName;
        City = city;
      }
    }
    public class AuthenticationRequestUserInfo
    {
      public string UserName { get; set; }
      public string Password { get; set; }
    }
  }
}
