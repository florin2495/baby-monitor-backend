using BabyMonitor.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BabyMonitor.Infrastructure.Persistence.Configurations;

public class SleepEntryConfiguration : BaseEntityConfiguration<SleepEntry>
{
    public override void Configure(EntityTypeBuilder<SleepEntry> builder)
    {
        base.Configure(builder);

        builder.ToTable("SleepSessions");

        builder.HasOne(s => s.Baby)
               .WithMany(b => b.SleepSessions)
               .HasForeignKey(s => s.BabyId)
               .OnDelete(DeleteBehavior.Cascade);

        builder.Property(s => s.StartedAt)
               .IsRequired();

        builder.Property(s => s.Location)
               .HasMaxLength(120);

        builder.Property(s => s.Notes)
               .HasMaxLength(1000);

        builder.HasIndex(s => new { s.BabyId, s.StartedAt });
    }
}
