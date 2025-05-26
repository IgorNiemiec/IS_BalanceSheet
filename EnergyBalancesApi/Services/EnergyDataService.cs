using EnergyBalancesApi.Data;
using EnergyBalancesApi.Models;
using EnergyBalancesApi.Models.Dto;
using EnergyBalancesApi.Models.EnergyModels;
using Microsoft.EntityFrameworkCore;

namespace EnergyBalancesApi.Services
{
    public class EnergyDataService
    {
        private readonly EnergyDbContext _context;

        public EnergyDataService(EnergyDbContext context)
        {
            _context = context;
        }

        public async Task SaveDataAsync(List<EnergyValueDto> data)
        {
            var entities = data.Select(dto => new EnergyData
            {
                Code = dto.Code,
                Amount = dto.Amount,
                Description = dto.Description,
                Country = dto.Country
            }).ToList();

            _context.EnergyData.AddRange(entities);

            await _context.SaveChangesAsync();
            Console.WriteLine("Dane zapisane do bazy danych.");
        }


        public async Task SaveDataAsync(List<EnergyValueDto> data, string flowCode, string unit, int year)
        {
            var flowType = await _context.FlowTypes.FirstOrDefaultAsync(f => f.Code == flowCode);
            if (flowType == null)
            {
                flowType = new EnergyFlowType { Code = flowCode, Description = flowCode };
                _context.FlowTypes.Add(flowType);
                await _context.SaveChangesAsync();
            }

            foreach (var item in data)
            {
                var country = await _context.Countries.FirstOrDefaultAsync(c => c.Code == item.Country);
                if (country == null)
                {
                    country = new Country { Code = item.Country, Name = item.Country };
                    _context.Countries.Add(country);
                    await _context.SaveChangesAsync();
                }

                var product = await _context.Products.FirstOrDefaultAsync(p => p.Code == item.Code);
                if (product == null)
                {
                    product = new EnergyProduct { Code = item.Code, Description = item.Description };
                    _context.Products.Add(product);
                    await _context.SaveChangesAsync();
                }

                var energyValue = new EnergyValue
                {
                    CountryId = country.Id,
                    ProductId = product.Id,
                    FlowTypeId = flowType.Id,
                    Year = year,
                    Unit = unit,
                    Amount = item.Amount
                };

                _context.EnergyValues.Add(energyValue);
            }

            using var transaction = await _context.Database.BeginTransactionAsync();

            try
            {
                await transaction.CommitAsync();

                await _context.SaveChangesAsync();

            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();

                throw;

            }

        }




        }
}
