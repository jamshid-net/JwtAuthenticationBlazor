namespace JwtAuthorizeTest.Server.Interfaces;

public interface IHashStringService
{
    Task<string> GetHashStringAsync(string hashString);
}
