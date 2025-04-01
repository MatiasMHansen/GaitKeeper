using GaitSessionService.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using System.IO;

namespace GaitSessionService.DatabaseMigration
{
    //public class GaitSessionContextFactory : IDesignTimeDbContextFactory<GaitSessionContext>
    //{
    //    public GaitSessionContext CreateDbContext(string[] args)
    //    {
    //        // Byg konfigurationen fra appsettings og env vars
    //        var configuration = new ConfigurationBuilder()
    //            .SetBasePath(Directory.GetCurrentDirectory())
    //            .AddJsonFile("appsettings.json", optional: true)
    //            .AddEnvironmentVariables()
    //            .Build();

    //        var optionsBuilder = new DbContextOptionsBuilder<GaitSessionContext>();

    //        // Henter connection string fra konfiguration
    //        var connectionString = configuration.GetConnectionString("GaitkeeperDB");
    //        optionsBuilder.UseSqlServer(connectionString,
    //            x => x.MigrationsAssembly("GaitSessionService.DatabaseMigration"));

    //        return new GaitSessionContext(optionsBuilder.Options);
    //    }
    //}
}
