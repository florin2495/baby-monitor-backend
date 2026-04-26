namespace BabyMonitor.Core.Entities;

/// <summary>
/// A medication administration event.
/// </summary>
public class MedicationEntry : BaseEntity
{
    public Guid BabyId { get; set; }

    public Baby? Baby { get; set; }

    public DateTime AdministeredAt { get; set; }

    public required string Name { get; set; }

    public decimal? DoseAmount { get; set; }

    /// <summary>Free-text unit (e.g., "ml", "mg", "drops").</summary>
    public string? DoseUnit { get; set; }

    public string? Notes { get; set; }
}
