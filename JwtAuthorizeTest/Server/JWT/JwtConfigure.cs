using Microsoft.AspNetCore.Authentication;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace JwtAuthorizeTest.Server.JWT;

public static class JwtConfigure
{
    public static AuthenticationBuilder AddJwtSettings(this AuthenticationBuilder builder)
    {
        builder.AddJwtBearer(cfg =>
        {
            cfg.SaveToken = true;
            cfg.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidAudience = "https://localhost:7183/",
                ValidIssuer = "https://localhost:7183/",
                ClockSkew = TimeSpan.Zero,
                IssuerSigningKey = new SymmetricSecurityKey
                                        (Encoding.UTF8.GetBytes("mymegasuperultrasecretkeyaaaaaaaaaaaaaa"))
            };
           
        });

        return builder;
    }
}
