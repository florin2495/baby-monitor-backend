using BabyMonitor.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BabyMonitor.Infrastructure.Persistence.Configurations;

public class MedicationEntryConfiguration : BaseEntityConfiguration<MedicationEntry>
{
    public override void Configure(EntityTypeBuilder<MedicationEntry> builder)
    {
        base.Configure(builder);

        builder.ToTable("Medications");

        builder.HasOne(m => m.Baby)
               .WithMany(b => b.Medications)
               .HasForeignKey(m => m.BabyId)
               .OnDelete(DeleteBehavior.Cascade);

        builder.Property(m => m.AdministeredAt)
               .IsRequired();

        builder.Property(m => m.Name)
               .IsRequired()
               .HasMaxLength(200);

        builder.Property(m => m.DoseAmount)
               .HasPrecision(8, 3); // e.g., 12.500 ml

        builder.Property(m => m.DoseUnit)
               .HasMaxLength(20);

        builder.Property(m => m.Notes)
               .HasMaxLength(1000);

        builder.HasIndex(m => new { m.BabyId, m.AdministeredAt });
    }
}
