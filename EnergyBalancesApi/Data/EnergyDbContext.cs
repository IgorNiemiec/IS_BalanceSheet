using Microsoft.EntityFrameworkCore;
using EnergyBalancesApi.Models;
using EnergyBalancesApi.Models.EnergyModels;

namespace EnergyBalancesApi.Data
{
    public class EnergyDbContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<EnergyData> EnergyData { get; set; }  

        public DbSet<Country> Countries { get; set; }
        public DbSet<EnergyProduct> Products { get; set; }
        public DbSet<EnergyFlowType> FlowTypes { get; set; }
        public DbSet<EnergyValue> EnergyValues { get; set; }

        public EnergyDbContext(DbContextOptions<EnergyDbContext> options)
            : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Country>()
                .HasIndex(c => c.Code)
                .IsUnique();

            modelBuilder.Entity<EnergyProduct>()
                .HasIndex(p => p.Code)
                .IsUnique();

            modelBuilder.Entity<EnergyFlowType>()
                .HasIndex(f => f.Code)
                .IsUnique();

            modelBuilder.Entity<EnergyValue>()
                .HasOne(e => e.Country)
                .WithMany(c => c.EnergyValues)
                .HasForeignKey(e => e.CountryId);

            modelBuilder.Entity<EnergyValue>()
                .HasOne(e => e.Product)
                .WithMany(p => p.EnergyValues)
                .HasForeignKey(e => e.ProductId);

            modelBuilder.Entity<EnergyValue>()
                .HasOne(e => e.FlowType)
                .WithMany(f => f.EnergyValues)
                .HasForeignKey(e => e.FlowTypeId);
        }
    }
}
