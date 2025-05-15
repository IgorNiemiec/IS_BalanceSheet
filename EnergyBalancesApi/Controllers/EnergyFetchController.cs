using EnergyBalancesApi.Models.Dto;
using EnergyBalancesApi.Services;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/fetch")]
public class EnergyFetchController : ControllerBase
{
    private readonly IEurostatDataService _dataService;
    private readonly IDataTransformer _transformer;
    private const string Unit = "KTOE";
    private static readonly string[] Countries = { "PL", "DE", "FR", "IT" };

    public EnergyFetchController(IEurostatDataService dataService, IDataTransformer transformer)
    {
        _dataService = dataService;
        _transformer = transformer;
    }

    private async Task<IEnumerable<EnergyValueDto>> Fetch(string nrgBal, string year)
    {
        var results = new List<EnergyValueDto>();
        foreach (var geo in Countries)
        {
            var url = $"https://ec.europa.eu/eurostat/api/dissemination/statistics/1.0/data/nrg_bal_c" +
                      $"?geo={geo}&nrg_bal={nrgBal}&unit={Unit}&time={year}";
            var raw = await _dataService.GetEurostatDataAsync(url);
            if (raw?.Value != null)
                results.AddRange(_transformer.Transform(raw.Value, geo));
        }
        return results;
    }

    [HttpGet("primary-production/{year}")]
    public async Task<IActionResult> PrimaryProduction(string year)
        => Ok(await Fetch("PPRD", year));

    [HttpGet("gross-consumption/{year}")]
    public async Task<IActionResult> GrossConsumption(string year)
        => Ok(await Fetch("GIC_TOT", year));

    [HttpGet("household-consumption/{year}")]
    public async Task<IActionResult> Household(string year)
        => Ok(await Fetch("FC_OTH_HH", year));

    [HttpGet("industry-consumption/{year}")]
    public async Task<IActionResult> Industry(string year)
        => Ok(await Fetch("FC_IND", year));
}