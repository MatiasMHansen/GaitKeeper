using GaitSessionService.Domain.Aggregate;
using GaitSessionService.Domain.ValueObjects;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;

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
                builder.HasIndex(gs => gs.PointDataId).IsUnique(); // Unik constraint på PointDataId

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

                // Primitive collections - Gemmer som JSON
                builder.Property(gs => gs.AngleLabels)
                    .HasColumnType("nvarchar(max)")
                    .HasConversion(
                        v => JsonSerializer.Serialize(v, (JsonSerializerOptions)null),
                        v => JsonSerializer.Deserialize<List<string>>(v, (JsonSerializerOptions)null)
                    );

                builder.Property(gs => gs.ForceLabels)
                    .HasColumnType("nvarchar(max)")
                    .HasConversion(
                        v => JsonSerializer.Serialize(v, (JsonSerializerOptions)null),
                        v => JsonSerializer.Deserialize<List<string>>(v, (JsonSerializerOptions)null)
                    );

                builder.Property(gs => gs.ModeledLabels)
                    .HasColumnType("nvarchar(max)")
                    .HasConversion(
                        v => JsonSerializer.Serialize(v, (JsonSerializerOptions)null),
                        v => JsonSerializer.Deserialize<List<string>>(v, (JsonSerializerOptions)null)
                    );

                builder.Property(gs => gs.MomentLabels)
                    .HasColumnType("nvarchar(max)")
                    .HasConversion(
                        v => JsonSerializer.Serialize(v, (JsonSerializerOptions)null),
                        v => JsonSerializer.Deserialize<List<string>>(v, (JsonSerializerOptions)null)
                    );

                builder.Property(gs => gs.PowerLabels)
                    .HasColumnType("nvarchar(max)")
                    .HasConversion(
                        v => JsonSerializer.Serialize(v, (JsonSerializerOptions)null),
                        v => JsonSerializer.Deserialize<List<string>>(v, (JsonSerializerOptions)null)
                    );

                builder.Property(gs => gs.PointLabels)
                    .HasColumnType("nvarchar(max)")
                    .HasConversion(
                        v => JsonSerializer.Serialize(v, (JsonSerializerOptions)null),
                        v => JsonSerializer.Deserialize<List<string>>(v, (JsonSerializerOptions)null)
                    );

                // Collections af komplekse typer
                builder.OwnsMany(gs => gs.LGaitCycles, a =>
                {
                    a.WithOwner().HasForeignKey("GaitSessionId");
                    a.ToTable("LGaitCycles");
                    a.HasKey("GaitSessionId", "Number"); // Composite PK
                    a.Property(p => p.Number).ValueGeneratedNever(); // Undgå identity
                });

                builder.OwnsMany(gs => gs.RGaitCycles, a =>
                {
                    a.WithOwner().HasForeignKey("GaitSessionId");
                    a.ToTable("RGaitCycles");
                    a.HasKey("GaitSessionId", "Number");
                    a.Property(p => p.Number).ValueGeneratedNever();
                });

                builder.OwnsMany(gs => gs.GaitAnalyses, a =>
                {
                    a.WithOwner().HasForeignKey("GaitSessionId");
                    a.ToTable("GaitAnalyses");
                    a.HasKey("GaitSessionId","Context","Name");
                });
            });
        }
    }
}
