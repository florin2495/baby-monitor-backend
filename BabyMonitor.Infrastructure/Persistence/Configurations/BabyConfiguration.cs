using BabyMonitor.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BabyMonitor.Infrastructure.Persistence.Configurations;

public class BabyConfiguration : BaseEntityConfiguration<Baby>
{
    public override void Configure(EntityTypeBuilder<Baby> builder)
    {
        base.Configure(builder);

        builder.ToTable("Babies");

        builder.Property(b => b.Name)
               .IsRequired()
               .HasMaxLength(120);

        builder.Property(b => b.Sex)
               .HasConversion<int>()   // store enum as int
               .IsRequired();

        builder.Property(b => b.Notes)
               .HasMaxLength(2000);

        // Many-to-many: Baby <-> AppUser via "BabyCaregivers" join table.
        builder.HasMany(b => b.Caregivers)
               .WithMany(u => u.Babies)
               .UsingEntity(j => j.ToTable("BabyCaregivers"));
    }
}
