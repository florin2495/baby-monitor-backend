using BabyMonitor.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BabyMonitor.Infrastructure.Persistence.Configurations;

public class GrowthEntryConfiguration : BaseEntityConfiguration<GrowthEntry>
{
    public override void Configure(EntityTypeBuilder<GrowthEntry> builder)
    {
        base.Configure(builder);

        builder.ToTable("GrowthEntries");

        builder.HasOne(g => g.Baby)
               .WithMany(b => b.GrowthEntries)
               .HasForeignKey(g => g.BabyId)
               .OnDelete(DeleteBehavior.Cascade);

        builder.Property(g => g.MeasuredAt)
               .IsRequired();

        // Pediatric measurements: 1 decimal place is plenty (e.g., 3.4 kg, 52.5 cm).
        builder.Property(g => g.WeightKg)
               .HasPrecision(5, 2);

        builder.Property(g => g.HeightCm)
               .HasPrecision(5, 2);

        builder.Property(g => g.HeadCircumferenceCm)
               .HasPrecision(5, 2);

        builder.Property(g => g.Notes)
               .HasMaxLength(1000);

        builder.HasIndex(g => new { g.BabyId, g.MeasuredAt });
    }
}
