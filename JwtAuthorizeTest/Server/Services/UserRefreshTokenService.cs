using JwtAuthorizeTest.Server.Data;
using JwtAuthorizeTest.Server.Interfaces;
using JwtAuthorizeTest.Shared.DTOs;
using JwtAuthorizeTest.Shared.Models;
using Microsoft.EntityFrameworkCore;

namespace JwtAuthorizeTest.Server.Services;

public class UserRefreshTokenService : IUserRefreshTokenService
{
    private readonly ApplicationDbContext _context;

    private readonly IHashStringService _hashStringService;
    public UserRefreshTokenService(ApplicationDbContext context, IHashStringService hashStringService)
    {
        _context = context;
        _hashStringService = hashStringService;
    }

    public async Task<UserRefreshToken> AddOrUpdateRefreshToken(UserRefreshToken refreshToken, CancellationToken cancellationToken = default)
    {
        var foundRefreshtoken = await _context.RefreshTokens.AsNoTracking()
            .FirstOrDefaultAsync(x=> x.UserName == refreshToken.UserName,cancellationToken);
        if (foundRefreshtoken is null)
        {
            await _context.RefreshTokens.AddAsync(refreshToken,cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);
            return refreshToken;
        }
        else
        {
            foundRefreshtoken.RefreshToken = refreshToken.RefreshToken;
            foundRefreshtoken.ExpiresTime = refreshToken.ExpiresTime;
            _context.RefreshTokens.Update(foundRefreshtoken);
            await _context.SaveChangesAsync();
            return refreshToken;
        }
    }

    public async Task<bool> AuthenAsync(LoginDto user)
    {
        string hashedPassword = await _hashStringService.GetHashStringAsync(user.Password);
        var foundUser = await _context.Users
            .SingleOrDefaultAsync(x => x.UserName == user.UserName && x.Password == hashedPassword);
        if (user is not null)
            return true;
        else return false;
    }

    public async Task<bool> DeleteUserRefreshTokens(string refreshToken, CancellationToken cancellationToken = default)
    {
        var foundRefreshtoken = await _context.RefreshTokens.AsNoTracking()
            .FirstOrDefaultAsync(x => x.RefreshToken == refreshToken);
        _context.RefreshTokens.Remove(foundRefreshtoken);

        return (await _context.SaveChangesAsync(cancellationToken)) > 0;
    }

    public async Task<UserRefreshToken> GetSavedRefreshTokens(string username, string refreshToken)
    {
        var foundRefreshtoken = await _context.RefreshTokens.AsNoTracking()
             .FirstOrDefaultAsync(x => x.UserName == username && x.RefreshToken == refreshToken);

        return foundRefreshtoken;
    }

}
