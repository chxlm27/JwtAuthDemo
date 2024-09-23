using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace JwtAuthDemo.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthTestController : ControllerBase
    {
        private readonly IConfiguration _configuration;

        public AuthTestController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [HttpGet("public")]
        public IActionResult PublicEndpoint()
        {
            return Ok(new { message = "This is a public endpoint, no authentication required." });
        }

        [Authorize]
        [HttpGet("protected")]
        public IActionResult ProtectedEndpoint()
        {
            return Ok(new { message = "This is a protected endpoint, you are authenticated!" });
        }

        // POST: api/AuthTest/login
        [AllowAnonymous]
        [HttpPost("login")]
        public IActionResult Login([FromBody] UserCredentials credentials)
        {
            if (credentials.Username == "test" && credentials.Password == "password")
            {
                var claims = new[]
                {
                    new Claim(JwtRegisteredClaimNames.Sub, credentials.Username),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
                };

                var key = Encoding.UTF8.GetBytes(_configuration.GetSection("Jwt").GetValue<string>("Key"));
                var creds = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256);

                var token = new JwtSecurityToken(
                    issuer: _configuration.GetSection("Jwt").GetValue<string>("Issuer"),
                    audience: _configuration.GetSection("Jwt").GetValue<string>("Audience"),
                    claims: claims,
                    expires: DateTime.Now.AddMinutes(30),
                    signingCredentials: creds);

                return Ok(new { token = new JwtSecurityTokenHandler().WriteToken(token) });
            }

            return Unauthorized();
        }
    }
}
