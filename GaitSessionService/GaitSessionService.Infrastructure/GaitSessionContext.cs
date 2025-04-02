using GaitSessionService.Domain.Aggregate;
using GaitSessionService.Domain.ValueObjects;
using Microsoft.EntityFrameworkCore;

namespace GaitSessionService.Infrastructure
{
    public class GaitSessionContext : DbContext
    {
        public DbSet<GaitSession> GaitSessions { get; set; }
        public DbSet<GaitCycle> GaitCycles { get; set; }
        public DbSet<GaitAnalysis> GaitAnalyses { get; set; }

        public GaitSessionContext(DbContextOptions<GaitSessionContext> options) : base(options)
        {
            Database.EnsureCreated();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<GaitSession>(builder =>
            {
                builder.HasKey(gs => gs.Id);

                // Owned types
                builder.OwnsOne(gs => gs.Biometrics, b =>
                {
                    b.Property(p => p.Height);
                    b.Property(p => p.Weight);
                    b.Property(p => p.LLegLength);
                    b.Property(p => p.RLegLength);
                });

                builder.OwnsOne(gs => gs.SystemInfo, s =>
                {
                    s.Property(p => p.Software);
                    s.Property(p => p.Version);
                    s.Property(p => p.MarkerSetup);
                });

                // Primitive collections
                builder.OwnsMany<string>("_angleLabels", a =>
                {
                    a.WithOwner().HasForeignKey("GaitSessionId");
                    a.Property<string>("Value");
                    a.ToTable("AngleLabels");
                    a.HasKey("GaitSessionId", "Value");
                });

                builder.OwnsMany<string>("_forceLabels", a =>
                {
                    a.WithOwner().HasForeignKey("GaitSessionId");
                    a.Property<string>("Value");
                    a.ToTable("ForceLabels");
                    a.HasKey("GaitSessionId", "Value");
                });

                builder.OwnsMany<string>("_modeledLabels", a =>
                {
                    a.WithOwner().HasForeignKey("GaitSessionId");
                    a.Property<string>("Value");
                    a.ToTable("ModeledLabels");
                    a.HasKey("GaitSessionId", "Value");
                });

                builder.OwnsMany<string>("_momentLabels", a =>
                {
                    a.WithOwner().HasForeignKey("GaitSessionId");
                    a.Property<string>("Value");
                    a.ToTable("MomentLabels");
                    a.HasKey("GaitSessionId", "Value");
                });

                builder.OwnsMany<string>("_powerLabels", a =>
                {
                    a.WithOwner().HasForeignKey("GaitSessionId");
                    a.Property<string>("Value");
                    a.ToTable("PowerLabels");
                    a.HasKey("GaitSessionId", "Value");
                });

                builder.OwnsMany<string>("_pointLabels", a =>
                {
                    a.WithOwner().HasForeignKey("GaitSessionId");
                    a.Property<string>("Value");
                    a.ToTable("PointLabels");
                    a.HasKey("GaitSessionId", "Value");
                });

                // Collections af komplekse typer
                builder.OwnsMany(gs => gs.LGaitCycles, a =>
                {
                    a.WithOwner().HasForeignKey("GaitSessionId");
                    a.ToTable("LGaitCycles");
                    a.HasKey("GaitSessionId", "Number"); // Antag 'Number' er unik pr. session
                });

                builder.OwnsMany(gs => gs.RGaitCycles, a =>
                {
                    a.WithOwner().HasForeignKey("GaitSessionId");
                    a.ToTable("RGaitCycles");
                    a.HasKey("GaitSessionId", "Number");
                });

                builder.OwnsMany(gs => gs.GaitAnalyses, a =>
                {
                    a.WithOwner().HasForeignKey("GaitSessionId");
                    a.ToTable("GaitAnalyses");
                    a.HasKey("GaitSessionId", "Name"); // Antag 'Name' er unik pr. session
                });
            });
        }
    }
}
