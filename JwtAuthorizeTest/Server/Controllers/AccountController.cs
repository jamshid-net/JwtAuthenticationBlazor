using JwtAuthorizeTest.Server.Data;
using JwtAuthorizeTest.Server.Interfaces;
using JwtAuthorizeTest.Shared.DTOs;
using JwtAuthorizeTest.Shared.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace JwtAuthorizeTest.Server.Controllers;
[Route("api/[controller]")]
[ApiController]
public class AccountController : ControllerBase
{
    private readonly ApplicationDbContext _context;
    private readonly IJwtTokenService _jwtTokenService;
    private readonly IHashStringService _hashStringService;
    private readonly IConfiguration _configuration;
    private readonly IUserRefreshTokenService _userRefreshTokenService;
    public AccountController(ApplicationDbContext context, IJwtTokenService jwtTokenService, IHashStringService hashStringService, IUserRefreshTokenService userRefreshTokenService, IConfiguration configuration)
    {
        _context = context;
        _jwtTokenService = jwtTokenService;
        _hashStringService = hashStringService;
        _userRefreshTokenService = userRefreshTokenService;
        _configuration = configuration;
    }


    [HttpPost("login")]
    public async Task<IActionResult> Login(LoginDto logindto)
    {
        string hashedPassword =await _hashStringService.GetHashStringAsync(logindto.Password);
        var foundUser = await _context.Users
            .FirstOrDefaultAsync(x => x.UserName == logindto.UserName && x.Password == hashedPassword);
        if (foundUser is null)
            return BadRequest("User is not found!");
        var tokenResponse =await _jwtTokenService.CreateTokenAsync(logindto.UserName);
        var userRefreshtoken = new UserRefreshToken
        {
            Id = Guid.NewGuid(),
            UserName = logindto.UserName,
            ExpiresTime = DateTime.Now.AddMinutes(_configuration.GetValue("JWT:RefreshTokenExpiresTime", 5)),
            RefreshToken = tokenResponse.RefreshToken,

        };

       await _userRefreshTokenService.AddOrUpdateRefreshToken(userRefreshtoken);

        return Ok(tokenResponse);
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register(RegisterDto register)
    {

        if (ModelState.IsValid)
        {
            var IsExistUser =  await _context.Users.AsNoTracking()
                .AnyAsync(x=> x.UserName == register.UserName);
            if (IsExistUser)
                return BadRequest("User is already exist");

            string hashedPassword = await _hashStringService.GetHashStringAsync(register.Password);
            var user = new User { UserName = register.UserName, Password = hashedPassword };
            _context.Users.Add(user);   
             await _context.SaveChangesAsync();

            return Ok(_jwtTokenService.CreateTokenAsync(user.UserName));
        }
        return BadRequest("Error");
    }
}
