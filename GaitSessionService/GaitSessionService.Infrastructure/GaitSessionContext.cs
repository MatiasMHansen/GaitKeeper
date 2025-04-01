using GaitSessionService.Domain.Aggregate;
using Microsoft.EntityFrameworkCore;

namespace GaitSessionService.Infrastructure
{
    public class GaitSessionContext : DbContext
    {
        public DbSet<GaitSession> GaitSessions { get; set; }

        public GaitSessionContext(DbContextOptions<GaitSessionContext> options) : base(options)
        {
            Database.EnsureCreated();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new GaitSessionConfiguration());
        }

        // Add-Migration InitialMigration -Context GaitSessionContext -Project GaitSessionService.DatabaseMigration
        // Update-Database -Context GaitSessionContext -Project GaitSessionService.DatabaseMigration
    }
}
