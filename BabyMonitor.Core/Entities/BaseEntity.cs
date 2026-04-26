namespace BabyMonitor.Core.Entities;

/// <summary>
/// Base class for all persisted entities. Carries identifiers and timestamps
/// shared across the domain. <see cref="ClientId"/> is generated client-side
/// and used by the offline-first sync layer to deduplicate records.
/// </summary>
public abstract class BaseEntity
{
    /// <summary>Server-side primary key.</summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Client-generated identifier (created on the device while offline).
    /// Used by the sync endpoint to detect already-synced records.
    /// </summary>
    public Guid ClientId { get; set; }

    /// <summary>UTC timestamp of first persistence.</summary>
    public DateTime CreatedAt { get; set; }

    /// <summary>
    /// UTC timestamp of the last update. Drives last-write-wins conflict
    /// resolution during sync.
    /// </summary>
    public DateTime UpdatedAt { get; set; }
}
