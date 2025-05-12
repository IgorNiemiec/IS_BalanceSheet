using EnergyBalancesApi.Models.Dto;

namespace EnergyBalancesApi.Services
{
    public interface IDataTransformer
    {
        List<EnergyValueDto> Transform(Dictionary<string, double> values, string countryCode);
    }
}
