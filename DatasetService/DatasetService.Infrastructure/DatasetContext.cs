using DatasetService.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;

namespace DatasetService.Infrastructure
{
    public class DatasetContext : DbContext
    {
        public DbSet<Dataset> Datasets { get; set; }

        public DatasetContext(DbContextOptions<DatasetContext> options) : base(options)
        {
            Database.EnsureCreated();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Dataset>(builder =>
            {
                builder.HasKey(d => d.Id);
                builder.Property(d => d.Name);

                // ---------------- Primitive collection: AllLabels ----------------
                builder.Property(d => d.AllLabels)
                    .HasColumnType("nvarchar(max)")
                    .HasConversion(
                        v => JsonSerializer.Serialize(v, (JsonSerializerOptions)null),
                        v => JsonSerializer.Deserialize<List<string>>(v, (JsonSerializerOptions)null)
                    );

                // ---------------- Owned Subjects ----------------
                builder.OwnsMany(d => d.Subjects, sb =>
                {
                    sb.WithOwner().HasForeignKey("DatasetId");
                    sb.ToTable("Subjects");
                    sb.HasKey("DatasetId", "SubjectId"); // Composite PK

                    sb.Property(p => p.SubjectId).HasColumnName("SubjectId");
                    sb.Property(p => p.PointDataId).HasColumnName("PointDataId");
                    sb.Property(p => p.Description).HasColumnName("Description");
                    sb.Property(p => p.Sex).HasColumnName("Sex");
                    sb.Property(p => p.Age).HasColumnName("Age");
                    sb.Property(p => p.Height).HasColumnName("Height");
                    sb.Property(p => p.Weight).HasColumnName("Weight");
                    sb.Property(p => p.LLegLength).HasColumnName("LLegLength");
                    sb.Property(p => p.RLegLength).HasColumnName("RLegLength");
                });

                // ---------------- Owned ContinuousVariables ----------------
                builder.OwnsMany(d => d.ContinuousDataSummery, cb =>
                {
                    cb.WithOwner().HasForeignKey("DatasetId");
                    cb.ToTable("ContinuousVariables");
                    cb.HasKey("DatasetId", "Name"); // Composite PK

                    cb.Property(p => p.Name).HasColumnName("Name");
                    cb.Property(p => p.Min).HasColumnName("Min");
                    cb.Property(p => p.Max).HasColumnName("Max");
                    cb.Property(p => p.Mean).HasColumnName("Mean");
                    cb.Property(p => p.Median).HasColumnName("Median");
                    cb.Property(p => p.StdDev).HasColumnName("StdDev");
                });
            });
        }
    }
}