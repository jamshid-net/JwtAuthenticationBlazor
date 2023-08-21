using JwtAuthorizeTest.Client;
using JwtAuthorizeTest.Server.Data;
using JwtAuthorizeTest.Server.Interfaces;
using JwtAuthorizeTest.Server.JWT;
using JwtAuthorizeTest.Server.Middlewares;
using JwtAuthorizeTest.Server.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Components.WebAssembly.Authentication;
using Microsoft.AspNetCore.ResponseCompression;

namespace JwtAuthorizeTest;
public class Program
{
    public static async Task Main(string[] args)
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

        builder.Services.AddCors(options =>
        {
            options.AddDefaultPolicy(builder =>
            {
                builder.WithOrigins("http://127.0.0.1:5500")
                       .AllowAnyHeader()
                       .AllowAnyMethod();
            });
        });

        var app = builder.Build();
        Console.WriteLine(Ulid.NewUlid());
        app.UseCors();
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
