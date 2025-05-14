using EnergyBalancesApi.Models.Dto;
using EnergyBalancesApi.Models.EnergyModels;
using EnergyBalancesApi.Services;
using EnergyBalancesApi.Services.FrontService;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
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



    // FRONTEND


    [HttpGet("report")]
    public async Task<IActionResult> GetReport(
        [FromQuery] string? country,
        [FromQuery] int? year,
        [FromQuery] string? flow,
        [FromServices] EnergyReportService reportService)
        {
            var result = await reportService.GetReportAsync(country, year, flow);
            if (result.Count == 0)
                return NotFound("Brak danych do raportu.");

             return Ok(result);
        }


    [HttpGet("values")]
    public async Task<IActionResult> GetEnergyValues(
        [FromQuery] string? country,
        [FromQuery] int? year,
        [FromQuery] string? flow,
        [FromQuery] string? unit,
        [FromQuery] string? product,
        [FromServices] EnergyQueryService queryService)
    {
        var data = await queryService.GetEnergyValuesAsync(country, year, flow, unit, product);


        if (data.Count == 0)
            return NotFound("Brak pasujących danych.");
        return Ok(data);
    }

    // GET /api/energy/values
    //
    // GET /api/energy/values? country = PL
    //
    // GET /api/energy/values? country = PL & year = 2010 & flow = PPRD
    //
    // GET /api/energy/values? country = DE & year = 2010 & flow = GIC_TOT & product = G3000 & unit = KTOE


    [HttpGet("report/by-product")]
    public async Task<IActionResult> GetProductReport(
    [FromQuery] string? country,
    [FromQuery] int? year,
    [FromQuery] string? flow,
    [FromQuery] string? unit,
    [FromServices] EnergyProductReportService reportService)
    {
        var report = await reportService.GetTotalByProductAsync(country, year, flow, unit);
        if (report == null || !report.Any())
            return NotFound("Brak danych dla podanych parametrów.");

        return Ok(report);

    }

    //GET /api/energy/report/by-product
    //
    //GET /api/energy/report/by-product? country = PL
    //
    //GET /api/energy/report/by-product? country = PL & year = 2010
    //
    //GET /api/energy/report/by-product? country = PL & year = 2010 & flow = PPRD
    //
    //GET /api/energy/report/by-product? country = DE & year = 2010 & flow = GIC_TOT & unit = KTOE


    [HttpGet("report/by-country")]
    public async Task<IActionResult> GetCountryReport(
    [FromQuery] string? product,
    [FromQuery] int? year,
    [FromQuery] string? flow,
    [FromQuery] string? unit,
    [FromServices] EnergyReportService reportService)
     {
        var report = await reportService.GetTotalByCountryAsync(product, year, flow, unit);
        if (report == null || !report.Any())
            return NotFound("Brak danych dla podanych parametrów.");
        return Ok(report);
     }


   // GET /api/energy/report/by-country
   //
   // GET /api/energy/report/by-country? product = G3000
   //
   // GET /api/energy/report/by-country? product = G3000 & year = 2010
   //
   // GET /api/energy/report/by-country? product = RA000 & year = 2010 & flow = PPRD







    }