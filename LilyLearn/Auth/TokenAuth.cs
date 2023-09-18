using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using TaskManager.DomainLayer.Models;

namespace TaskManagerApi.Auth
{
    public class TokenAuth : ITokenAuth
    {
        private readonly IConfiguration configuration;
        public TokenAuth(IConfiguration configuration)
        {
            this.configuration = configuration;
        }


        public string GenerateToken(User user, string role)
        {
            var issuer = configuration["Jwt:Issuer"];
            var audience = configuration["Jwt:Audience"];
            var key = Encoding.UTF8.GetBytes(configuration["Jwt:Key"]);
            var signingCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha512Signature);

            var subject = new ClaimsIdentity(new[]
            {

                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Name, user.LastName),
                new Claim(ClaimTypes.GivenName, user.FirstName),
                new Claim(ClaimTypes.Role, role),
                //new Claim(ClaimTypes.HomePhone, user.PhoneNumber),
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString())

            });


            var expires = DateTime.UtcNow.AddDays(1);

            var tokenDecriptor = new SecurityTokenDescriptor
            {
                Subject = subject,
                Issuer = issuer,
                Audience = audience,
                Expires = expires,
                SigningCredentials = signingCredentials

            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDecriptor);
            

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

    }


}
