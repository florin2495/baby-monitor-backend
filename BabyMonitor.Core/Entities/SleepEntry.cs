namespace BabyMonitor.Core.Entities;

/// <summary>
/// A sleep session. <see cref="EndedAt"/> is null while the baby is still
/// sleeping (open session).
/// </summary>
public class SleepEntry : BaseEntity
{
    public Guid BabyId { get; set; }

    public Baby? Baby { get; set; }

    public DateTime StartedAt { get; set; }

    public DateTime? EndedAt { get; set; }

    /// <summary>Free-text location (e.g., "crib", "stroller", "car seat").</summary>
    public string? Location { get; set; }

    public string? Notes { get; set; }
}
