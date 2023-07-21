using JwtAuthorizeTest.Server.Interfaces;
using JwtAuthorizeTest.Shared;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security;
using System.Security.Claims;
using System.Text;

namespace JwtAuthorizeTest.Server.Services;

public class JwtTokenService: IJwtTokenService
{
    private readonly IConfiguration _configuration;
    private readonly IHashStringService _hashStringService;

    public JwtTokenService(IConfiguration configuration, IHashStringService hashStringService) 
             => (_configuration, _hashStringService) = (configuration, hashStringService);
    public async ValueTask<TokenResponse> CreateTokenAsync(string userName)
    {
        var claims = new List<Claim>()
        {
            new Claim(ClaimTypes.Name,userName),
            new Claim(ClaimTypes.Role,"Hello"),
            new Claim(ClaimTypes.Role,"AAA")
        
        };
       

        var jwt = new JwtSecurityToken(
            issuer: _configuration.GetValue<string>("JWT:Audience"),
            audience: _configuration.GetValue<string>("JWT:Issuer"),
            claims: claims,
            expires: DateTime.Now.AddMinutes(_configuration.GetValue<int>("JWT:ExpiresInMinutes", 10)),
            signingCredentials: new SigningCredentials(
                                    new SymmetricSecurityKey
                                        (Encoding.UTF8.GetBytes(_configuration.GetValue<string>("JWT:Key"))),
                                        SecurityAlgorithms.HmacSha256));
        var responseModel = new TokenResponse
        {
            AccessToken = new JwtSecurityTokenHandler().WriteToken(jwt),
            RefreshToken = await GenerateRefreshTokenAsync(userName)
        };

        return responseModel;
    }

    public async ValueTask<string> GenerateRefreshTokenAsync(string userName)
    {
        var refreshtoken = await  _hashStringService.GetHashStringAsync(userName+DateTime.Now);
        return refreshtoken;
    }

    public ValueTask<ClaimsPrincipal> GetPrincipalFromExpiredToken(string token)
    {
        var tokenValidationParameters = new TokenValidationParameters
        {
            ValidateAudience = true,
            ValidateIssuer = true,
            ValidateLifetime = false,
            ValidAudience = _configuration.GetValue<string>("JWT:Audience"),
            ValidIssuer = _configuration.GetValue<string>("JWT:Issuer"),
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration.GetValue<string>("JWT:Key")))


        };
        var tokenHandler = new JwtSecurityTokenHandler(); 
        var principal = tokenHandler.ValidateToken(token,tokenValidationParameters,out SecurityToken securityToken);
        JwtSecurityToken jwtSecurityToken = securityToken as JwtSecurityToken;
        if(jwtSecurityToken  == null || !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256,StringComparison.InvariantCultureIgnoreCase))
        {
            throw new SecurityTokenException("Invalid token");
        }
        return ValueTask.FromResult(principal);
    }
}
