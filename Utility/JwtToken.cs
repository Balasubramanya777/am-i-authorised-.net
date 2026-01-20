using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Text.Json;
using AmIAuthorised.DataAccessLayer.DTO;
using Microsoft.IdentityModel.Tokens;

namespace AmIAuthorised.Utility
{
    public class JwtToken
    {
        private readonly IConfiguration _configuration;

        public JwtToken(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public string GenerateJWT(UserDTO userDto)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            //var key = Encoding.ASCII.GetBytes(_configuration["JwtSettings:Key"]!);
            var key = Encoding.ASCII.GetBytes("balasubramanyaauthorisemebalasubramanya");
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(GetClaims(userDto)),
                //Issuer = _configuration["JwtSettings:Issuer"],
                //Audience = _configuration["JwtSettings:Audience"],
                Issuer = "balasubramanya",
                Audience = "wholeworldglobe",
                Expires = DateTime.UtcNow.AddSeconds(3600),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

        private static List<Claim> GetClaims(UserDTO userDto)
        {
            List<Claim> claims = new()
            {
                new(JwtRegisteredClaimNames.Sub, userDto.UserId.ToString()),
                new(JwtRegisteredClaimNames.Email, userDto.Email)
            };

            foreach (var permission in userDto.Permissions)
            {
                claims.Add(new Claim("permissions", permission));
            }

            return claims;
        }
    }
}
