using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Api.Models;
using Microsoft.IdentityModel.Tokens;

namespace Api.Services;

public class JwtTokenGenerator
{
    private readonly string _secretKey;

    public JwtTokenGenerator(IConfiguration configuration)
    {
        _secretKey = configuration["AuthSettings:SecretKey"];
    }

    public string GenerateJwtToken(AppUser user, IList<string> roles)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.ASCII.GetBytes(_secretKey);

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(new []
            {
                new Claim("FirsName", user.FirstName),
                new Claim("Id", user.Id),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Role, string.Join(",", roles))
            }),

            Expires = DateTime.UtcNow.AddDays(1),

            SigningCredentials = new SigningCredentials(
                new SymmetricSecurityKey(key),
                SecurityAlgorithms.HmacSha512Signature)
        };

        var token = tokenHandler.CreateToken(tokenDescriptor);
        var result = tokenHandler.WriteToken(token);
        return result;
    }
}