using GaitSessionService.Domain.Aggregate;
using GaitSessionService.Domain.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GaitSessionService.Infrastructure
{
    public class GaitSessionConfiguration : IEntityTypeConfiguration<GaitSession>
    {
        public void Configure(EntityTypeBuilder<GaitSession> builder)
        {
            builder.ToTable("GaitSessions");

            builder.HasKey(x => x.Id);

            // Value Object: Biometrics (Owned)
            builder.OwnsOne(g => g.Biometrics, b =>
            {
                b.Property(p => p.Height).HasColumnName("Height");
                b.Property(p => p.Weight).HasColumnName("Weight");
                b.Property(p => p.LLegLength).HasColumnName("LLegLength");
                b.Property(p => p.RLegLength).HasColumnName("RLegLength");
            });

            // Value Object: SystemInfo (Owned)
            builder.OwnsOne(g => g.SystemInfo, s =>
            {
                s.Property(p => p.Software).HasColumnName("Software");
                s.Property(p => p.Version).HasColumnName("Version");
                s.Property(p => p.MarkerSetup).HasColumnName("MarkerSetup");
            });

            // Backing fields for labels
            builder.Metadata
                .FindNavigation(nameof(GaitSession.AngleLabels))!
                .SetPropertyAccessMode(PropertyAccessMode.Field);

            builder.Metadata
                .FindNavigation(nameof(GaitSession.ForceLabels))!
                .SetPropertyAccessMode(PropertyAccessMode.Field);

            builder.Metadata
                .FindNavigation(nameof(GaitSession.ModeledLabels))!
                .SetPropertyAccessMode(PropertyAccessMode.Field);

            builder.Metadata
                .FindNavigation(nameof(GaitSession.MomentLabels))!
                .SetPropertyAccessMode(PropertyAccessMode.Field);

            builder.Metadata
                .FindNavigation(nameof(GaitSession.PowerLabels))!
                .SetPropertyAccessMode(PropertyAccessMode.Field);

            builder.Metadata
                .FindNavigation(nameof(GaitSession.PointLabels))!
                .SetPropertyAccessMode(PropertyAccessMode.Field);

            // GaitCycles (as owned collection or separate table)
            builder.OwnsMany(typeof(GaitCycle), "_lGaitCycles", l =>
            {
                l.WithOwner().HasForeignKey("GaitSessionId");
                l.Property<int>("Id"); // Shadow key
                l.HasKey("Id");
                l.ToTable("LeftGaitCycles");
            });

            builder.OwnsMany(typeof(GaitCycle), "_rGaitCycles", r =>
            {
                r.WithOwner().HasForeignKey("GaitSessionId");
                r.Property<int>("Id");
                r.HasKey("Id");
                r.ToTable("RightGaitCycles");
            });

            // GaitAnalyses (as owned collection)
            builder.OwnsMany(typeof(GaitAnalysis), "_gaitAnalyses", ga =>
            {
                ga.WithOwner().HasForeignKey("GaitSessionId");
                ga.Property<int>("Id");
                ga.HasKey("Id");
                ga.ToTable("GaitAnalyses");
            });
        }
    }
}
