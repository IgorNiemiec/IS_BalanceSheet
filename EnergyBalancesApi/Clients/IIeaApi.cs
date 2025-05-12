using Refit;
using System.Threading.Tasks;

namespace EnergyBalancesApi.Infrastructure.Clients
{
    /// <summary>
    /// Klient do pobierania danych z API IEA.
    /// </summary>
    public interface IIeaApi
    {
        // Przykład: lista krajów
        [Get("/countries")]
        Task<IList<CountryDto>> GetCountriesAsync();

        // Przykład: dane bilansów energii dla danego kraju i roku
        [Get("/energy/balances/{countryCode}/{year}")]
        Task<EnergyBalanceDto> GetEnergyBalanceAsync(
            [AliasAs("countryCode")] string countryCode,
            [AliasAs("year")] int year
        );
    }

    // DTO definiowane obok lub w folderze DTO
    public class CountryDto
    {
        public string CountryCode { get; set; } = null!;
        public string Name { get; set; } = null!;
    }
    public class EnergyBalanceDto
    {
        public int Year { get; set; }
        public decimal FossilFuel { get; set; }
        public decimal Renewable { get; set; }
    }
}
