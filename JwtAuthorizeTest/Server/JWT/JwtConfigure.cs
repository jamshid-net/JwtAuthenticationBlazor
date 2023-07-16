using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Security;
using System.Text;

namespace JwtAuthorizeTest.Server.JWT;

public static class JwtConfigure
{
    public static AuthenticationBuilder AddJwtSettings(this AuthenticationBuilder builder,IConfiguration configuration)
    {
        builder.AddJwtBearer(cfg =>
        {
            cfg.SaveToken = true;
            cfg.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidAudience = configuration.GetValue<string>("JWT:Audience"),
                ValidIssuer = configuration.GetValue<string>("JWT:Issuer"),
                ClockSkew = TimeSpan.Zero,
                IssuerSigningKey = new SymmetricSecurityKey
                                        (Encoding.UTF8.GetBytes(configuration.GetValue<string>("JWT:Key")))
            };

            cfg.Events = new JwtBearerEvents()
            {
                OnAuthenticationFailed = context =>
                {
                    if(context.Exception.GetType() == typeof(SecurityException))
                    {
                        context.Response.Headers.Add("IS-TOKEN-EXPIRED", "true");
                    }
                    return Task.CompletedTask;
                }
            };

        });

        return builder;
    }
}
