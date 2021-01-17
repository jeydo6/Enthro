using Enthro.Domain.Dto;
using Enthro.Domain.Services;
using Enthro.Domain.Storages;
using Enthro.Infrastructure.Configs;
using Microsoft.AspNetCore.Components.Authorization;
using System;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Enthro.Infrastructure.Services
{
    public class AuthenticationService : IAuthenticationService
    {
        private readonly HttpClient _httpClient;
        private readonly EndpointConfig _endpointConfig;
        private readonly ILocalStorage _localStorage;
        private readonly AuthenticationStateProvider _stateProvider;
        private readonly JsonSerializerOptions _jsonSerializerOptions;

        public AuthenticationService(
            HttpClient httpClient,
            EndpointConfig endpointConfig,
            ILocalStorage localStorage,
            AuthenticationStateProvider stateProvider,
            JsonSerializerOptions jsonSerializerOptions
        )
        {
            _httpClient = httpClient;
            _endpointConfig = endpointConfig;
            _localStorage = localStorage;
            _stateProvider = stateProvider;
            _jsonSerializerOptions = jsonSerializerOptions;
        }

        public async Task LoginAsync(LoginDto login)
        {
            await _localStorage.SetItemAsync("accessToken", null);

            String jsonContent = JsonSerializer.Serialize(
                new
                {
                    login.UserName,
                    login.Password,
                    _endpointConfig.Audience,
                    _endpointConfig.Secret
                },
                _jsonSerializerOptions
            );

            var response = await _httpClient.PostAsync("/authentication/token", new StringContent(jsonContent, Encoding.UTF8, "application/json"));

            if (!response.IsSuccessStatusCode)
            {
                throw new Exception(response.ReasonPhrase);
            }

            var stringContent = await response.Content.ReadAsStringAsync();

            await _localStorage.SetItemAsync("accessToken", stringContent);

            var stateProvider = (Providers.AuthenticationStateProvider)_stateProvider;
            stateProvider.NotifyAuthenticationStateChanged();
        }

        public async Task LogoutAsync()
        {
            await _localStorage.SetItemAsync("accessToken", null);

            var stateProvider = (Providers.AuthenticationStateProvider)_stateProvider;
            stateProvider.NotifyAuthenticationStateChanged();
        }
    }
}
