using EnergyBalancesApi.Data;
using EnergyBalancesApi.Models.EnergyModels;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Collections.Generic;



namespace EnergyBalancesApi.Services.FrontService
{
    public class JsonExportService
    {
        private readonly EnergyDbContext _context;

        public JsonExportService(EnergyDbContext context)
        {
            _context = context;
        }

          public async Task<string?> ExportEnergyDataToJsonStringAsync(
            string? countryCode, int? year, string? flowCode, string? productCode)
        {
            IQueryable<EnergyValue> query = _context.EnergyValues
                .Include(ev => ev.Country)
                .Include(ev => ev.Product)
                .Include(ev => ev.FlowType);

            if (!string.IsNullOrEmpty(countryCode))
            {
                query = query.Where(ev => ev.Country.Code == countryCode);
            }

            if (year.HasValue)
            {
                query = query.Where(ev => ev.Year == year.Value);
            }

            if (!string.IsNullOrEmpty(flowCode))
            {
                query = query.Where(ev => ev.FlowType.Code == flowCode);
            }

            if (!string.IsNullOrEmpty(productCode))
            {
                query = query.Where(ev => ev.Product.Code == productCode);
            }

            var dataToExport = await query.ToListAsync();

            if (!dataToExport.Any())
            {
                return null; 
            }

            var jsonOptions = new JsonSerializerOptions
            {
                WriteIndented = true,
                ReferenceHandler = ReferenceHandler.Preserve,
                DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
            };

            return JsonSerializer.Serialize(dataToExport, jsonOptions);
        }
    }
}
