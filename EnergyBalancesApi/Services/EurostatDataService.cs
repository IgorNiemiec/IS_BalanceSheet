using EnergyBalancesApi.Models;
using System.Text.Json;

namespace EnergyBalancesApi.Services
{
    public class EurostatDataService : IEurostatDataService
    {

        private readonly HttpClient _httpClient;

        public EurostatDataService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<EurostatApiResponse?> GetEurostatDataAsync(string url)
        {
            var response = await _httpClient.GetAsync(url);

            if (!response.IsSuccessStatusCode)
            {
                return null;
            }

            var content = await response.Content.ReadAsStringAsync();

            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
            };

            return JsonSerializer.Deserialize<EurostatApiResponse>(content, options);
        }


    }
}
