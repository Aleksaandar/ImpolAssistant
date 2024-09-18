using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace IMPOLAssistant.API.Controllers
{
    public class UserLogin
    {
        public string Username { get; set; }
        public string Password { get; set; }
    }

    [Route("api/[controller]")]
    public class AuthController : Controller
    {
        private string HandleLogin(UserLogin login)
        {
            if (login.Username == "admin" && login.Password == "admin")
            {
                return "Admin";
            }
            else if (login.Username == "user" && login.Password == "user")
            {
                return "User";
            }

            return string.Empty;

        }

        [HttpPost("login")]
        public IActionResult Login([FromBody] UserLogin login)
        {
            var role = HandleLogin(login);

            if (!string.IsNullOrEmpty(role))
            {
                var token = GenerateJwtToken(login.Username, role);
                return Ok(new { Token = token });
            }

            return Unauthorized();
        }

        private string GenerateJwtToken(string username, string role)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("Is7NqOS2LTt8fg6fjBEhHa0LWlrq2pyL"));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken("FonAsistent",
              "FonAudience",
              new List<Claim>()
              {
                     new Claim(ClaimTypes.Name, username),
                     new Claim(ClaimTypes.Role, role)
              },
              expires: DateTime.Now.AddMinutes(180),
              signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
