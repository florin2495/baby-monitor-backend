using BabyMonitor.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BabyMonitor.Infrastructure.Persistence.Configurations;

public class DiaperEntryConfiguration : BaseEntityConfiguration<DiaperEntry>
{
    public override void Configure(EntityTypeBuilder<DiaperEntry> builder)
    {
        base.Configure(builder);

        builder.ToTable("Diapers");

        builder.HasOne(d => d.Baby)
               .WithMany(b => b.Diapers)
               .HasForeignKey(d => d.BabyId)
               .OnDelete(DeleteBehavior.Cascade);

        builder.Property(d => d.OccurredAt)
               .IsRequired();

        builder.Property(d => d.Type)
               .HasConversion<int>()
               .IsRequired();

        builder.Property(d => d.Notes)
               .HasMaxLength(1000);

        builder.HasIndex(d => new { d.BabyId, d.OccurredAt });
    }
}
