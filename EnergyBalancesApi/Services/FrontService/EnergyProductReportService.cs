using EnergyBalancesApi.Data;
using EnergyBalancesApi.Models.FrontModels;
using Microsoft.EntityFrameworkCore;

namespace EnergyBalancesApi.Services.FrontService
{
    public class EnergyProductReportService
    {

        private readonly EnergyDbContext _context;


        public EnergyProductReportService(EnergyDbContext context)
        {
            _context = context;
        }

        public async Task<List<EnergyProductReportDto>> GetTotalByProductAsync(
        string? country,
        int? year,
        string? flow,
        string? unit)
        {
            var query = _context.EnergyValues
                .Include(e => e.Country)
                .Include(e => e.Product)
                .Include(e => e.FlowType)
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(country))
                query = query.Where(e => e.Country.Code == country);

            if (year.HasValue)
                query = query.Where(e => e.Year == year.Value);

            if (!string.IsNullOrWhiteSpace(flow))
                query = query.Where(e => e.FlowType.Code == flow);

            if (!string.IsNullOrWhiteSpace(unit))
                query = query.Where(e => e.Unit == unit);

            var grouped = await query
                .GroupBy(e => new { e.Product.Code, e.Product.Description })
                .Select(g => new EnergyProductReportDto
                {
                    ProductCode = g.Key.Code,
                    ProductDescription = g.Key.Description,
                    TotalAmount = g.Sum(e => e.Amount)
                })
                .OrderByDescending(r => r.TotalAmount)
                .ToListAsync();

            return grouped;
        }






    }
}
