using BabyMonitor.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BabyMonitor.Infrastructure.Persistence.Configurations;

public class AppUserConfiguration : BaseEntityConfiguration<AppUser>
{
    public override void Configure(EntityTypeBuilder<AppUser> builder)
    {
        base.Configure(builder);

        builder.ToTable("Users");

        builder.Property(u => u.Email)
               .IsRequired()
               .HasMaxLength(256);

        builder.HasIndex(u => u.Email)
               .IsUnique();

        builder.Property(u => u.DisplayName)
               .IsRequired()
               .HasMaxLength(120);

        builder.Property(u => u.PasswordHash)
               .IsRequired()
               .HasMaxLength(512);

        // Many-to-many with Baby is configured on the Baby side
        // (see BabyConfiguration) using a shared join table.
    }
}
