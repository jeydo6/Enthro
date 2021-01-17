using Enthro.Domain.Dto;
using Enthro.Domain.Enumerations;
using Enthro.Domain.Services;
using Enthro.Domain.Storages;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text.Json;
using System.Threading.Tasks;
using System.Web;

namespace Enthro.Infrastructure.Services
{
    public class IndicatorsService : IIndicatorsService
    {
        private readonly HttpClient _httpClient;
        private readonly ILocalStorage _localStorage;
        private readonly JsonSerializerOptions _jsonSerializerOptions;

        public IndicatorsService(
            HttpClient httpClient,
            ILocalStorage localStorage,
            JsonSerializerOptions jsonSerializerOptions
        )
        {
            _httpClient = httpClient;
            _localStorage = localStorage;
            _jsonSerializerOptions = jsonSerializerOptions;
        }

        public async Task<IEnumerable<IndicatorDto>> GetAsync(Int32? month = null, Gender? gender = null, IndicatorType? type = null)
        {
            String token = await _localStorage.GetItemAsync("accessToken");
            if (token != null)
            {
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            }

            String requestUri = "indicators";

            NameValueCollection parameters = HttpUtility.ParseQueryString(String.Empty);
            if (month.HasValue)
            {
                parameters["month"] = $"{month.Value}";
            }
            if (gender.HasValue)
            {
                parameters["gender"] = $"{(Int32)gender.Value}";
            }
            if (type.HasValue)
            {
                parameters["type"] = $"{(Int32)type.Value}";
            }
            if (parameters.Count > 0)
            {
                requestUri += $"?{parameters}";
            }

            var response = await _httpClient.GetAsync(requestUri);

            if (response.IsSuccessStatusCode)
            {
                var responseStream = await response.Content.ReadAsStreamAsync();

                return await JsonSerializer.DeserializeAsync<IEnumerable<IndicatorDto>>(responseStream, _jsonSerializerOptions);
            }
            else if (response.StatusCode == HttpStatusCode.Unauthorized)
            {
                throw new UnauthorizedAccessException(response.ReasonPhrase);
            }

            return null;
        }
    }
}
