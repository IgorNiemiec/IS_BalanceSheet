using EnergyBalancesApi.Models;

namespace EnergyBalancesApi.Services
{
    public interface IEurostatDataService
    {
        Task<EurostatApiResponse?> GetEurostatDataAsync(string url);
    }
}
