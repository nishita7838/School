using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Students.Models
{
    public static class JwtTokenHelper
    {
        
        public static string GenerateToken(string username)
        {
            var key=new SymmetricSecurityKey(Encoding.UTF8.GetBytes("kjhfruivutwfueuoifeopiyugyt"));
            var creds=new SigningCredentials(key,SecurityAlgorithms.HmacSha256);
            var token = new JwtSecurityToken(
                issuer: "https://localhost:7202",
                audience: "https://localhost:7202",
                claims: new[]
                {
                    new Claim(ClaimTypes.Name,username)
                },
                expires: DateTime.Now.AddHours(1),
                signingCredentials: creds);
            return new JwtSecurityTokenHandler().WriteToken(token);
            {
                
            };
        }
    }
}
