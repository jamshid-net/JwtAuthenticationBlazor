using System.Security.Claims;

namespace JwtAuthorizeTest.Server.Interfaces;

public interface IJwtTokenService
{
    ValueTask<string> CreateTokenAsync(string userName);
    ValueTask<ClaimsPrincipal> GetPrincipalFromExpiredToken(string token);
    ValueTask<string> GenerateRefreshTokenAsync(string userName);
}
