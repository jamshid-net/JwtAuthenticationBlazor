namespace JwtAuthorizeTest.Server.Interfaces;

public interface IJwtTokenService
{
    ValueTask<string> CreateTokenAsync(string userName);
}
