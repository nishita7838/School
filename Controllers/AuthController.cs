using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using Students.Models;

namespace Students.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        [HttpPost("login")]
        public IActionResult Login([FromBody] LoginRequest request)
        {
            if (request.Username == "admin" && request.Password == "password")
            {
                var token = JwtTokenHelper.GenerateToken(request.Username);
                return Ok(new { Token = token });
            }
            else
            {
                return Unauthorized("Invalid Credentials");
            }
        }
            public class LoginRequest
        {
            public string Username { get; set; }
            public string Password { get; set; }
        }
    }
}
