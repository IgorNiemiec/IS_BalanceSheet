using Microsoft.EntityFrameworkCore;
using EnergyBalancesApi.Models;    // aby zobaczyć Country, EnergyBalanceRecord

namespace EnergyBalancesApi.Data
{
    public class EnergyDbContext : DbContext
    {
        public DbSet<Country> Countries { get; set; }
        public DbSet<EnergyBalanceRecord> EnergyBalanceRecords { get; set; }
        public DbSet<SourceMetadata> SourceMetadata { get; set; }

        public DbSet<User> Users { get; set; }


        public EnergyDbContext(DbContextOptions<EnergyDbContext> options)
            : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {


            modelBuilder.Entity<Country>(entity =>
            {
                entity.HasKey(e => e.Id);                                  // PK :contentReference[oaicite:8]{index=8}
                entity.Property(e => e.Name)
                      .IsRequired()                                         // Required :contentReference[oaicite:9]{index=9}
                      .HasMaxLength(100);
                entity.HasMany(e => e.EnergyBalanceRecords)
                      .WithOne(r => r.Country)
                      .HasForeignKey(r => r.CountryId);                     // relacja 1‑many :contentReference[oaicite:10]{index=10}
            });


            modelBuilder.Entity<EnergyBalanceRecord>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Year).IsRequired();
                entity.Property(e => e.FossilFuelKtoe).IsRequired();
                entity.Property(e => e.RenewableKtoe).IsRequired();
            });


            modelBuilder.Entity<SourceMetadata>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.SourceName).IsRequired();
            });

        }
    }
}
