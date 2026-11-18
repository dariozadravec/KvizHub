using System.Net.Http.Json;
using System.Security.Claims;
using KvizHub.Shared.Auth;
using Microsoft.AspNetCore.Components.Authorization;

namespace KvizHub.Client.Auth
{
    // Gets the signed-in user from the API, because the HttpOnly cookie cannot be read in the browser
    public class ApiAuthenticationStateProvider(HttpClient http) : AuthenticationStateProvider
    {
        private static readonly AuthenticationState Anonymous = new(new ClaimsPrincipal(new ClaimsIdentity()));

        public override async Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            try
            {
                var response = await http.GetAsync("api/auth/me");
                if (!response.IsSuccessStatusCode)
                    return Anonymous;

                var user = await response.Content.ReadFromJsonAsync<AuthUserDto>();
                return user is null ? Anonymous : new AuthenticationState(ToPrincipal(user));
            }
            catch (HttpRequestException)
            {
                return Anonymous;
            }
        }

        // Called after login, registration and logout so the menu updates immediately
        public void NotifyUserChanged() => NotifyAuthenticationStateChanged(GetAuthenticationStateAsync());

        private static ClaimsPrincipal ToPrincipal(AuthUserDto user) => new(new ClaimsIdentity(
            [
                new Claim(ClaimTypes.Name, user.DisplayName),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Role, user.Role.ToString())
            ],
            authenticationType: "KvizHub"));
    }
}