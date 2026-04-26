using BabyMonitor.Core.Entities;
using Microsoft.EntityFrameworkCore;

namespace BabyMonitor.Infrastructure.Persistence;

/// <summary>
/// Primary EF Core DbContext for the Luna domain. Entity configuration is
/// loaded automatically from <see cref="IEntityTypeConfiguration{TEntity}"/>
/// implementations in this assembly.
/// </summary>
public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    public DbSet<AppUser> Users => Set<AppUser>();

    public DbSet<Baby> Babies => Set<Baby>();

    public DbSet<FeedingEntry> Feedings => Set<FeedingEntry>();

    public DbSet<SleepEntry> SleepSessions => Set<SleepEntry>();

    public DbSet<DiaperEntry> Diapers => Set<DiaperEntry>();

    public DbSet<GrowthEntry> GrowthEntries => Set<GrowthEntry>();

    public DbSet<MedicationEntry> Medications => Set<MedicationEntry>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);
    }
}
