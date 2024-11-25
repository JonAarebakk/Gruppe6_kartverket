using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Gruppe6_Kartverket.Mvc.Models.Services;

namespace Gruppe6_Kartverket.Mvc.Services
{
    public class KartverketApiService : IKartverketApiService
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<KartverketApiService> _logger;
        private const string BaseUrl = "https://api.kartverket.no/kommuneinfo/v1/punkt";


        /// <summary>
        /// Initializes a new instance of the KartverketApiService.
        /// </summary>
        /// <param name="httpClient">HTTP client for making API requests.</param>
        /// <param name="logger">Logger for service operations.</param>
        public KartverketApiService(HttpClient httpClient, ILogger<KartverketApiService> logger)
        {
            _httpClient = httpClient;
            _logger = logger;
        }

        /// <summary>
        /// Retrieves municipality number for given geographical coordinates.
        /// </summary>
        /// <param name="longitude">Longitude in EUREF89 format.</param>
        /// <param name="latitude">Latitude in EUREF89 format.</param>
        /// <returns>Municipality number as string.</returns>
        /// <exception cref="JsonException">Thrown when municipality number is not found in response.</exception>
        public async Task<KartverkApiInfo> GetMunicipalityAndCountyNameAsync(double longitude, double latitude)
        {
            var jsonDocument = await GetKartverketDataAsync(longitude, latitude);

            var kartverkApiInfo = new KartverkApiInfo();

            if (jsonDocument.RootElement.TryGetProperty("kommunenavn", out var kommunenavnElement) &&
                kommunenavnElement.ValueKind == JsonValueKind.String)
            {
                kartverkApiInfo.Kommunenavn = kommunenavnElement.GetString();
            }
            else
            {
                _logger.LogError("Property 'kommunenavn' not found or is not a string in the response.");
                throw new JsonException("Property 'kommunenavn' not found or is not a string in the response.");
            }

            if (jsonDocument.RootElement.TryGetProperty("fylkesnavn", out var fylkesnavnElement) &&
                fylkesnavnElement.ValueKind == JsonValueKind.String)
            {
                kartverkApiInfo.Fylkesnavn = fylkesnavnElement.GetString();
            }
            else
            {
                _logger.LogError("Property 'fylkesnavn' not found or is not a string in the response.");
                throw new JsonException("Property 'fylkesnavn' not found or is not a string in the response.");
            }

            return kartverkApiInfo;

        }

        /// <summary>
        /// Makes HTTP request to Kartverket API and returns parsed JSON response.
        /// </summary>
        /// <param name="longitude">Longitude in EUREF89 format.</param>
        /// <param name="latitude">Latitude in EUREF89 format.</param>
        /// <returns>JsonDocument containing API response.</returns>
        /// <exception cref="HttpRequestException">Thrown when API request fails or returns non-success status code.</exception>
        private async Task<JsonDocument> GetKartverketDataAsync(double longitude, double latitude)
        {
            var formattedLongitude = longitude.ToString(System.Globalization.CultureInfo.InvariantCulture);
            var formattedLatitude = latitude.ToString(System.Globalization.CultureInfo.InvariantCulture);

            var url = $"{BaseUrl}?nord={formattedLatitude}&ost={formattedLongitude}&koordsys=4258";
            _logger.LogInformation("Requesting URL: {Url}", url);

            var response = await _httpClient.GetAsync(url);

            if (response == null)
            {
                _logger.LogError("Response is null for URL: {Url}", url);
                throw new HttpRequestException("Response is null");
            }

            if (!response.IsSuccessStatusCode)
            {
                var errorContent = await response.Content.ReadAsStringAsync();
                _logger.LogError("API request failed with status code {StatusCode}. Error: {ErrorContent}",
                    response.StatusCode, errorContent);
                throw new HttpRequestException(
                    $"API request failed with status code {response.StatusCode}. Error: {errorContent}");
            }

            var content = await response.Content.ReadAsStringAsync();
            return JsonDocument.Parse(content);
        }
    }
}
