using BabyMonitor.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BabyMonitor.Infrastructure.Persistence.Configurations;

public class FeedingEntryConfiguration : BaseEntityConfiguration<FeedingEntry>
{
    public override void Configure(EntityTypeBuilder<FeedingEntry> builder)
    {
        base.Configure(builder);

        builder.ToTable("Feedings");

        builder.HasOne(f => f.Baby)
               .WithMany(b => b.Feedings)
               .HasForeignKey(f => f.BabyId)
               .OnDelete(DeleteBehavior.Cascade);

        builder.Property(f => f.OccurredAt)
               .IsRequired();

        builder.Property(f => f.Source)
               .HasConversion<int>()
               .IsRequired();

        builder.Property(f => f.Notes)
               .HasMaxLength(1000);

        // Most queries filter by baby and a recent time window.
        builder.HasIndex(f => new { f.BabyId, f.OccurredAt });
    }
}
