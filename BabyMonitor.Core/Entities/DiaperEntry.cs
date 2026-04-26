using BabyMonitor.Core.Enums;

namespace BabyMonitor.Core.Entities;

/// <summary>
/// A diaper change event.
/// </summary>
public class DiaperEntry : BaseEntity
{
    public Guid BabyId { get; set; }

    public Baby? Baby { get; set; }

    public DateTime OccurredAt { get; set; }

    public DiaperType Type { get; set; } = DiaperType.Unknown;

    public string? Notes { get; set; }
}
