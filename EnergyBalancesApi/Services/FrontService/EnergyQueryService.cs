using EnergyBalancesApi.Data;
using EnergyBalancesApi.Models.Dto;
using EnergyBalancesApi.Models.FrontModels;
using Microsoft.EntityFrameworkCore;

namespace EnergyBalancesApi.Services.FrontService
{
    public class EnergyQueryService
    {

        private readonly EnergyDbContext _context;

        public EnergyQueryService(EnergyDbContext context)
        {
            _context = context;
        }


        public async Task<List<FossilFuelsReportDto>> GetEnergyValuesByProductIdsAsync(string? country, int? year, int[] allowedProductIds)
        {
            var query = _context.EnergyValues
            .Include(ev => ev.Product)
            .Include(ev => ev.Country)
             .Where(ev => allowedProductIds.Contains(ev.ProductId));

                 if (!string.IsNullOrEmpty(country))
                query = query.Where(ev => ev.Country.Code == country);


                 if (year.HasValue)
                query = query.Where(ev => ev.Year == year.Value);

                 var grouped = await query
                .GroupBy(ev => new { ev.Product.Code, ev.Product.Description })
                .Select(g => new FossilFuelsReportDto
                {
                    ProductCode = g.Key.Code,
                    ProductDescription = g.Key.Description,
                    Amount = g.Sum(ev => ev.Amount)
                })
                .ToListAsync();

            return grouped;
        }



        public async Task<List<RenewableProductReportDto>> GetRenewableEnergyValuesByProductIdsAsync(string? country, int? year, int[] allowedProductIds)
        {
            var query = _context.EnergyValues
            .Include(ev => ev.Product)
            .Include(ev => ev.Country)
             .Where(ev => allowedProductIds.Contains(ev.ProductId));

            if (!string.IsNullOrEmpty(country))
                query = query.Where(ev => ev.Country.Code == country);


            if (year.HasValue)
                query = query.Where(ev => ev.Year == year.Value);

            var grouped = await query
           .GroupBy(ev => new { ev.Product.Code, ev.Product.Description })
           .Select(g => new RenewableProductReportDto
           {
               ProductCode = g.Key.Code,
               ProductDescription = g.Key.Description,
               Amount = g.Sum(ev => ev.Amount)
           })
           .ToListAsync();

            return grouped;
        }



        public async Task<List<EnergyValuesDto>> GetEnergyValuesAsync(string? country, int? year, string? flow, string? unit, string? product)
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

            if (!string.IsNullOrWhiteSpace(product))
                query = query.Where(e => e.Product.Code == product);

            var result = await query
                .Select(e => new EnergyValuesDto
                {
                    CountryCode = e.Country.Code,
                    CountryName = e.Country.Name,
                    Year = e.Year,
                    FlowCode = e.FlowType.Code,
                    FlowDescription = e.FlowType.Description,
                    ProductCode = e.Product.Code,
                    ProductDescription = e.Product.Description,
                    Unit = e.Unit,
                    Amount = e.Amount
                })
                .ToListAsync();

            return result;
        }

    }
}
