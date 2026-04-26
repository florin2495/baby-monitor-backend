namespace BabyMonitor.Core.Entities;

/// <summary>
/// A growth measurement (weight, height, head circumference). All metric
/// values are optional so a single measurement can record any subset.
/// </summary>
public class GrowthEntry : BaseEntity
{
    public Guid BabyId { get; set; }

    public Baby? Baby { get; set; }

    public DateTime MeasuredAt { get; set; }

    public decimal? WeightKg { get; set; }

    public decimal? HeightCm { get; set; }

    public decimal? HeadCircumferenceCm { get; set; }

    public string? Notes { get; set; }
}
