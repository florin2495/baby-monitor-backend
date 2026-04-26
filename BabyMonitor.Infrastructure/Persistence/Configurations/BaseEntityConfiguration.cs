using BabyMonitor.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BabyMonitor.Infrastructure.Persistence.Configurations;

/// <summary>
/// Shared configuration for every entity that derives from <see cref="BaseEntity"/>.
/// Each concrete configuration should override <see cref="Configure"/>, call
/// <c>base.Configure(builder)</c>, and then add entity-specific rules.
/// </summary>
public abstract class BaseEntityConfiguration<TEntity> : IEntityTypeConfiguration<TEntity>
    where TEntity : BaseEntity
{
    public virtual void Configure(EntityTypeBuilder<TEntity> builder)
    {
        builder.HasKey(e => e.Id);

        // Server assigns Id explicitly (Guid generated in app code), not by the DB.
        builder.Property(e => e.Id)
               .ValueGeneratedNever();

        builder.Property(e => e.ClientId)
               .IsRequired();

        // ClientId is generated on the device once per record and is used as a
        // dedup key during sync — it must be globally unique.
        builder.HasIndex(e => e.ClientId)
               .IsUnique();

        builder.Property(e => e.CreatedAt)
               .IsRequired();

        builder.Property(e => e.UpdatedAt)
               .IsRequired();

        // The sync endpoint queries by UpdatedAt to fetch records changed since
        // a given watermark; index this column.
        builder.HasIndex(e => e.UpdatedAt);
    }
}
