using JwtAuthorizeTest.Server.Interfaces;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace JwtAuthorizeTest.Server.Services;

public class JwtTokenService: IJwtTokenService
{
    public async ValueTask<string> CreateTokenAsync(string userName)
    {
        var claims = new List<Claim>() { new Claim(ClaimTypes.Name,userName)};

        var jwt = new JwtSecurityToken(
            issuer: "https://localhost:7183/",
            audience: "https://localhost:7183/",
            claims: claims,
            expires: DateTime.Now.AddMinutes(10),
            signingCredentials: new SigningCredentials(
                                    new SymmetricSecurityKey
                                        (Encoding.UTF8.GetBytes("mymegasuperultrasecretkeyaaaaaaaaaaaaaa")),
                                        SecurityAlgorithms.HmacSha256));


        return await ValueTask.FromResult(new JwtSecurityTokenHandler().WriteToken(jwt));
    }
    
}
