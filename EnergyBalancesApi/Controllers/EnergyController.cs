using EnergyBalancesApi.Models.Dto;
using EnergyBalancesApi.Models.EnergyModels;
using EnergyBalancesApi.Services;
using EnergyBalancesApi.Services.FrontService;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.Text.Json.Serialization;
using System.Text.Json;

[ApiController]
[Route("api/[controller]")]
public class EnergyController : ControllerBase
{
    private readonly IEurostatDataService _dataService;
    private readonly IDataTransformer _transformer;
    private readonly JsonExportService _jsonExportService;


    public EnergyController(IEurostatDataService dataService, IDataTransformer transformer, JsonExportService jsonExportService)
    {
        _dataService = dataService;
        _transformer = transformer;
        _jsonExportService = jsonExportService;

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
        var countries = new[] { "PL", "DE", "FR", "IT", "UK", "SE", "ES", "HR"};
        const string nrg_bal = "PPRD";  
        const string unit = "KTOE";
        var time = new[] { 2010, 2011, 2012, 2013, 2014, 2015,2016, 2017,2018, 2019, 2020, 2021,2022 };

        var results = new List<EnergyValueDto>();


        foreach (var year in time)
        {
            foreach (var geo in countries)
            {
                var url = $"https://ec.europa.eu/eurostat/api/dissemination/statistics/1.0/data/nrg_bal_c?geo={geo}&nrg_bal={nrg_bal}&unit={unit}&time={year}";
                var rawData = await _dataService.GetEurostatDataAsync(url);

                if (rawData == null || rawData.Value == null || !rawData.Value.Any())
                {
                    Console.WriteLine($"Brak danych dla {geo} ({nrg_bal}) w {time}");
                    continue;
                }

                var transformer = new DataTransformer();
                var transformed = transformer.Transform(rawData.Value, geo);
                results.AddRange(transformed);

            }

            await dataService.SaveDataAsync(results, "PPRD", "KTOE", year);
        }

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


    // Paliwa Kopalne

   

    [HttpGet("report/by-product-filtered")]
    public async Task<IActionResult> GetFilteredProductReport(
    [FromQuery] string? country,
    [FromQuery] int? year,
    [FromServices] EnergyQueryService queryService)
    {
        var allowedProductIds = new[] { 1, 3, 4, 5, 6, 18 };

        var data = await queryService.GetEnergyValuesByProductIdsAsync(country,year,allowedProductIds);

        var simplified = data.Select(x => new {
            Code = x.ProductCode,
            Description = x.ProductDescription,
            Amount = x.Amount
        }).ToList();

        if (!simplified.Any())
            return NotFound("Brak danych dla podanych parametrów.");

        return Ok(simplified);
    }

    // GET /api/energy/report/by-product-filtered?country=PL&year=2010



    // Energia Odnawialna

    [HttpGet("report/by-renewableproduct-filtered")]
    public async Task<IActionResult> GetFilteredRenewableProductReport(
    [FromQuery] string? country,
    [FromQuery] int? year,
    [FromServices] EnergyQueryService queryService
      )
    {
        var allowedProductIds = new[] { 26, 28, 29 , 30};

        var data = await queryService.GetRenewableEnergyValuesByProductIdsAsync(country, year, allowedProductIds);

        var simplified = data.Select(x => new {
            Code = x.ProductCode,
            Description = x.ProductDescription,
            Amount = x.Amount
        }).ToList();

        if (!simplified.Any())
            return NotFound("Brak danych dla podanych parametrów.");

        return Ok(simplified);

    }




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


  [HttpGet("export-to-json")]
    public async Task<IActionResult> ExportEnergyDataToJson(
        [FromQuery] string? countryCode,
        [FromQuery] int? year,
        [FromQuery] string? flowCode,
        [FromQuery] string? productCode)
    {
        var jsonData = await _jsonExportService.ExportEnergyDataToJsonStringAsync(
            countryCode, year, flowCode, productCode);

        if (jsonData == null)
        {
            return NotFound("Brak danych do wyeksportowania dla podanych kryteriów.");
        }

        var fileName = "energy_data.json";
        if (!string.IsNullOrEmpty(countryCode)) fileName = $"{countryCode}_energy_data.json";
        if (year.HasValue) fileName = $"{year.Value}_{fileName}";

        return File(System.Text.Encoding.UTF8.GetBytes(jsonData), "application/json", fileName);
    }


}