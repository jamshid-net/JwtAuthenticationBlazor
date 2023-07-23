using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.Authorization;
using System.Linq;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text.Json;

namespace JwtAuthorizeTest.Client;

public class CustomAuthStateProvider : AuthenticationStateProvider
{
    private readonly HttpClient _httpClient;
    private readonly ILocalStorageService _localStorage;
    public CustomAuthStateProvider(HttpClient httpClient, ILocalStorageService localStorage)
    {
        _httpClient = httpClient;
        _localStorage = localStorage;
    }

    public override async Task<AuthenticationState> GetAuthenticationStateAsync()
    {
        string token = await _localStorage.GetItemAsStringAsync("token");
        string refreshtoken = await _localStorage.GetItemAsStringAsync("refreshtoken");
        var claimIdentity = new ClaimsIdentity();
        _httpClient.DefaultRequestHeaders.Authorization = null;

        if (!string.IsNullOrEmpty(token))
        {
            var claims = ParseClaimsFromJwt(token);
            
            claimIdentity = new ClaimsIdentity(claims, "jwt");
          var res=  _httpClient.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", token.Replace("\"", ""));


        }
        var user = new ClaimsPrincipal(claimIdentity);
        var userPrincipal = await TransformAsync(user);
        var state = new AuthenticationState(userPrincipal);
        NotifyAuthenticationStateChanged(Task.FromResult(state));
        return state;
    }
    public Task<ClaimsPrincipal> TransformAsync(ClaimsPrincipal principal)
    {
        var identity = (ClaimsIdentity)principal.Identity;
        if (identity.IsAuthenticated)
        {
            if (identity.HasClaim(c => c.Type == "http://schemas.microsoft.com/ws/2008/06/identity/claims/role"))
            {
                var roles = identity.FindFirst("http://schemas.microsoft.com/ws/2008/06/identity/claims/role").Value.Split(',');
                foreach (var role in roles)
                {
                    identity.AddClaim(new Claim(ClaimTypes.Role, role.Trim()));
                }
            }
        }
        return Task.FromResult(principal);
    }
    public static IEnumerable<Claim> ParseClaimsFromJwt(string jwt)
    {
        var payload = jwt.Split('.')[1];
        var jsonBytes = ParseBase64WithoutPadding(payload);
        var keyValuePairs = JsonSerializer.Deserialize<Dictionary<string, object>>(jsonBytes);
        return keyValuePairs.Select(kvp => new Claim(kvp.Key, kvp.Value.ToString()));
    }
    private static byte[] ParseBase64WithoutPadding(string base64)
    {
        switch (base64.Length % 4)
        {
            case 2: base64 += "=="; break;
            case 3: base64 += "="; break;
        }
        return Convert.FromBase64String(base64);
    }
}
