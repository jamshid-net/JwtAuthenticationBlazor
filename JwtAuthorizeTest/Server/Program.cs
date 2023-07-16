using JwtAuthorizeTest.Server.Data;
using JwtAuthorizeTest.Server.Interfaces;
using JwtAuthorizeTest.Server.JWT;
using JwtAuthorizeTest.Server.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.ResponseCompression;

namespace JwtAuthorizeTest;
public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

       
        AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
        builder.Services.AddControllersWithViews();
        builder.Services.AddRazorPages();
        builder.Services.AddAuthorization();
        builder.Services.AddDataConfiguration(builder.Configuration);
        builder.Services.AddScoped<IHashStringService,HashStringService>();
        builder.Services.AddScoped<IUserRefreshTokenService,UserRefreshTokenService>();
        builder.Services.AddTransient<IJwtTokenService, JwtTokenService>();
        builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtSettings(builder.Configuration);
        var app = builder.Build();

       
        if (app.Environment.IsDevelopment())
        {
            app.UseWebAssemblyDebugging();
        }
        else
        {
            app.UseExceptionHandler("/Error");
            app.UseHsts();
        }

        app.UseHttpsRedirection();

        app.UseBlazorFrameworkFiles();
        app.UseStaticFiles();
       

        app.UseRouting();
        app.UseAuthentication();
        app.UseAuthorization();

        app.MapRazorPages();
        app.MapControllers();
        app.MapFallbackToFile("index.html");

        app.Run();
    }
}
