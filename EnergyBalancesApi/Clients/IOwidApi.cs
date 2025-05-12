using Refit;
using System.Threading.Tasks;

namespace EnergyBalancesApi.Infrastructure.Clients
{
    /// <summary>
    /// Klient do pobierania danych OWID (Our World in Data).
    /// </summary>
    public interface IOwidApi
    {
        // Przykład: globalne dane energetyczne. Ścieżka zależy od dokumentacji OWID.
        [Get("/datasets/energy-countries/metadata.json")]
        Task<string> GetDatasetMetadataAsync();

        // Przykład: dane CSV (zwracane jako tekst)
        [Get("/datasets/energy-countries/data.csv")]
        Task<string> GetEnergyDataCsvAsync(
            [AliasAs("format")] string format = "csv"
        );
    }
}
