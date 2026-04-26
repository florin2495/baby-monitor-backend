using BabyMonitor.Core.Enums;

namespace BabyMonitor.Core.Entities;

/// <summary>
/// A baby being tracked in the system. Aggregate root for all log entries
/// (feedings, sleep, diapers, growth, medications).
/// </summary>
public class Baby : BaseEntity
{
    public required string Name { get; set; }

    public DateOnly DateOfBirth { get; set; }

    public Sex Sex { get; set; } = Sex.Unknown;

    public string? Notes { get; set; }

    /// <summary>Caregivers (many-to-many with <see cref="AppUser"/>).</summary>
    public ICollection<AppUser> Caregivers { get; set; } = new List<AppUser>();

    public ICollection<FeedingEntry> Feedings { get; set; } = new List<FeedingEntry>();

    public ICollection<SleepEntry> SleepSessions { get; set; } = new List<SleepEntry>();

    public ICollection<DiaperEntry> Diapers { get; set; } = new List<DiaperEntry>();

    public ICollection<GrowthEntry> GrowthEntries { get; set; } = new List<GrowthEntry>();

    public ICollection<MedicationEntry> Medications { get; set; } = new List<MedicationEntry>();
}
