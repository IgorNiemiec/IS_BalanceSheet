using EnergyBalancesApi.Data;
using EnergyBalancesApi.Models.Dto;
using Microsoft.EntityFrameworkCore;
using EnergyBalancesApi.Models.FrontModels;


namespace EnergyBalancesApi.Services.FrontService
{
    public class EnergyReportService
    {

        private readonly EnergyDbContext _context;

        public EnergyReportService(EnergyDbContext context)
        {
            _context = context;
        }


        public async Task<List<EnergyReportDto>> GetReportAsync(string? countryCode, int? year, string? flowCode)
        {
            var query = _context.EnergyValues
                .Include(ev => ev.Product)
                .Include(ev => ev.Country)
                .Include(ev => ev.FlowType)
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(countryCode))
                query = query.Where(ev => ev.Country.Code == countryCode);

            if (year.HasValue)
                query = query.Where(ev => ev.Year == year.Value);

            if (!string.IsNullOrWhiteSpace(flowCode))
                query = query.Where(ev => ev.FlowType.Code == flowCode);

            var grouped = await query
                .GroupBy(ev => new { ev.Product.Code, ev.Product.Description })
                .Select(g => new EnergyReportDto
                {
                    ProductCode = g.Key.Code,
                    ProductDescription = g.Key.Description,
                    TotalAmount = g.Sum(ev => ev.Amount)
                })
                .OrderByDescending(r => r.TotalAmount)
                .ToListAsync();

            return grouped;
        }



        public async Task<List<EnergyCountryReportDto>> GetTotalByCountryAsync(
            string? product,
            int? year,
            string? flow,
            string? unit)
        {
            var query = _context.EnergyValues
            .Include(e => e.Country)
            .Include(e => e.Product)
            .Include(e => e.FlowType)
            .AsQueryable();
            if (!string.IsNullOrWhiteSpace(product))
                query = query.Where(e => e.Product.Code == product);

            if (year.HasValue)
                query = query.Where(e => e.Year == year.Value);

            if (!string.IsNullOrWhiteSpace(flow))
                query = query.Where(e => e.FlowType.Code == flow);

            if (!string.IsNullOrWhiteSpace(unit))
                query = query.Where(e => e.Unit == unit);

            var grouped = await query
                .GroupBy(e => new { e.Country.Code, e.Country.Name })
                .Select(g => new EnergyCountryReportDto
                {
                    CountryCode = g.Key.Code,
                    CountryName = g.Key.Name,
                    TotalAmount = g.Sum(e => e.Amount)
                })
                .OrderByDescending(r => r.TotalAmount)
                .ToListAsync();

            return grouped;
        }









        }
}
