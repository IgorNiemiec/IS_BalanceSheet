using EnergyBalancesApi.Models.Dto;
using EnergyBalancesApi.Services;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

[ApiController]
[Route("api/[controller]")]
public class EnergyController : ControllerBase
{
    private readonly IEurostatDataService _dataService;
    private readonly IDataTransformer _transformer;

    public EnergyController(IEurostatDataService dataService, IDataTransformer transformer)
    {
        _dataService = dataService;
        _transformer = transformer;
    }

  

    private async Task<IActionResult> GetDataForCountries(string nrg_bal, string year)
    {
        var countries = new[] { "PL", "DE", "FR", "IT" };
        const string unit = "KTOE";

        var results = new List<EnergyValueDto>();

        foreach (var geo in countries)
        {
            var url = $"https://ec.europa.eu/eurostat/api/dissemination/statistics/1.0/data/nrg_bal_c?geo={geo}&nrg_bal={nrg_bal}&unit={unit}&time={year}";
            var rawData = await _dataService.GetEurostatDataAsync(url);

            if (rawData != null)
            {
                var transformed = _transformer.Transform(rawData.Value, geo);
                results.AddRange(transformed);
            }
        }

        return Ok(results);
    }

    [HttpGet("primary-production")]
    public async Task<IActionResult> GetPrimaryProduction([FromServices] EnergyDataService dataService)
    {
        var countries = new[] { "PL", "DE", "FR", "IT" };
        const string nrg_bal = "PPRD";  // Bilanse produkcji pierwotnej
        const string unit = "KTOE";
        const string time = "2010";

        var results = new List<EnergyValueDto>();

        foreach (var geo in countries)
        {
            var url = $"https://ec.europa.eu/eurostat/api/dissemination/statistics/1.0/data/nrg_bal_c?geo={geo}&nrg_bal={nrg_bal}&unit={unit}&time={time}";
            var rawData = await _dataService.GetEurostatDataAsync(url);

            if (rawData == null || rawData.Value == null || !rawData.Value.Any())
            {
                Console.WriteLine($"Brak danych dla {geo} ({nrg_bal}) w {time}");
                continue;
            }

            // Tworzymy transformer bez słownika w konstruktorze
            var transformer = new DataTransformer();
            var transformed = transformer.Transform(rawData.Value, geo);
            results.AddRange(transformed);
        }

        await dataService.SaveDataAsync(results, "PPRD", "KTOE", 2010);

        return Ok(results);
    }



    [HttpGet("gross-consumption")]
    public async Task<IActionResult> GetGrossInlandConsumption()
    {
        var countries = new[] { "PL", "DE", "FR", "IT" };
        const string nrg_bal = "GIC_TOT";
        const string unit = "KTOE";
        const string time = "2010";

        var results = new List<EnergyValueDto>();

       

        foreach (var geo in countries)
        {
            var url = $"https://ec.europa.eu/eurostat/api/dissemination/statistics/1.0/data/nrg_bal_c?geo={geo}&nrg_bal={nrg_bal}&unit={unit}&time={time}";
            var rawData = await _dataService.GetEurostatDataAsync(url);

            if (rawData != null)
            {
                var transformed = _transformer.Transform(rawData.Value, geo);
                results.AddRange(transformed);
            }
        }

        return Ok(results);
    }




    [HttpGet("households-consumption")]
    public async Task<IActionResult> GetHouseholdConsumption() =>
        await GetDataForCountries("FC_OTH_HH", "2010");

    [HttpGet("industry-consumption")]
    public async Task<IActionResult> GetIndustryConsumption() =>
        await GetDataForCountries("FC_IND", "2010");






}