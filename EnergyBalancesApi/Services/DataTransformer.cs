using EnergyBalancesApi.Helpers;
using EnergyBalancesApi.Models.Dto;
using EnergyBalancesApi.Services;

public class DataTransformer : IDataTransformer
{
    public List<EnergyValueDto> Transform(Dictionary<string, double> values, string countryCode)
    {
        return values.Select(pair =>
        {
            return new EnergyValueDto
            {
                Code = pair.Key,
                Amount = pair.Value,
                Country = countryCode,
                Description = IdDescriptions.ProductMap.TryGetValue(pair.Key, out var desc)
                    ? desc
                    : $"Unknown code: {pair.Key}"
            };
        })
        .Where(x => x != null)
        .ToList();
    }
}
