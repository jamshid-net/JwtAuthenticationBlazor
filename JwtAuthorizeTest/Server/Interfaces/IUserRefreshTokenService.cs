using JwtAuthorizeTest.Shared.DTOs;
using JwtAuthorizeTest.Shared.Models;

namespace JwtAuthorizeTest.Server.Interfaces;

public interface IUserRefreshTokenService
{
    Task<UserRefreshToken> AddOrUpdateRefreshToken(UserRefreshToken refreshToken, CancellationToken cancellationToken = default);
    Task<bool> AuthenAsync(LoginDto user);
    Task<bool> DeleteUserRefreshTokens(string username, string refreshToken, CancellationToken cancellationToken = default);
    Task<UserRefreshToken> GetSavedRefreshTokens(string username, string refreshtoken); 

}
