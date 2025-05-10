using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using EnergyBalancesApi.Data;

public class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<EnergyDbContext>
{
    public EnergyDbContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<EnergyDbContext>();
        optionsBuilder.UseMySql(
            "Server=localhost;Port=3306;Database=EnergyDb;User=root;Password='';",
            ServerVersion.AutoDetect("Server=localhost;Port=3306;Database=EnergyDb;User=root;Password='';")
        );
        return new EnergyDbContext(optionsBuilder.Options);
    }
}
