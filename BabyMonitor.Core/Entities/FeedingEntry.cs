using BabyMonitor.Core.Enums;

namespace BabyMonitor.Core.Entities;

/// <summary>
/// A single feeding event (breast, bottle, formula or solid).
/// </summary>
public class FeedingEntry : BaseEntity
{
    public Guid BabyId { get; set; }

    public Baby? Baby { get; set; }

    /// <summary>UTC timestamp when the feeding occurred.</summary>
    public DateTime OccurredAt { get; set; }

    public FeedingSource Source { get; set; } = FeedingSource.Unknown;

    /// <summary>Volume in millilitres (bottle/formula). Null for breast feeds.</summary>
    public int? AmountMl { get; set; }

    /// <summary>Duration of the feeding in minutes (typical for breast feeds).</summary>
    public int? DurationMinutes { get; set; }

    public string? Notes { get; set; }
}
