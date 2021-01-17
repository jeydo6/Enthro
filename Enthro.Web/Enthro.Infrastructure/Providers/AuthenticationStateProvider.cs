using Enthro.Domain.Dto;
using Enthro.Domain.Storages;
using Microsoft.AspNetCore.Components.Authorization;
using System;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text.Json;
using System.Threading.Tasks;

namespace Enthro.Infrastructure.Providers
{
    public class AuthenticationStateProvider : Microsoft.AspNetCore.Components.Authorization.AuthenticationStateProvider
    {
        private readonly HttpClient _httpClient;
        private readonly ILocalStorage _localStorage;
        private readonly JsonSerializerOptions _jsonSerializerOptions;

        public AuthenticationStateProvider(
            HttpClient httpClient,
            ILocalStorage localStorage,
            JsonSerializerOptions jsonSerializerOptions
        )
        {
            _httpClient = httpClient;
            _localStorage = localStorage;
            _jsonSerializerOptions = jsonSerializerOptions;
        }

        public void NotifyAuthenticationStateChanged()
        {
            NotifyAuthenticationStateChanged(GetAuthenticationStateAsync());
        }

        public override async Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            try
            {
                var userInfo = await GetUserInfoAsync();

                var user = GetClaimsPrincipal(userInfo);

                return new AuthenticationState(user);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Request failed:" + ex.ToString());

                throw;
            }
        }

        private async Task<UserInfoDto> GetUserInfoAsync()
        {
            String token = await _localStorage.GetItemAsync("accessToken");

            if (!String.IsNullOrEmpty(token))
            {
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                var response = await _httpClient.GetAsync("/authentication/userInfo");

                if (response.IsSuccessStatusCode)
                {
                    var responseStream = await response.Content.ReadAsStreamAsync();

                    return await JsonSerializer.DeserializeAsync<UserInfoDto>(responseStream, _jsonSerializerOptions);
                }
            }

            return null;
        }

        private static ClaimsPrincipal GetClaimsPrincipal(UserInfoDto userInfo)
        {
            if (userInfo != null && userInfo.IsAuthenticated)
            {
                var claims = userInfo.Claims
                    .Select(c => new Claim(c.Type, c.Value, c.ValueType, c.Issuer, c.OriginalIssuer))
                    .ToArray();
                var identity = new ClaimsIdentity(claims, "Enthro");

                return new ClaimsPrincipal(identity);
            }

            return new ClaimsPrincipal();
        }
    }
}
