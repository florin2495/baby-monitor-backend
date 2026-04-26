namespace BabyMonitor.Core.Entities;

/// <summary>
/// Application user (caregiver). A user can be linked to multiple babies
/// through the <see cref="Babies"/> many-to-many relationship.
/// </summary>
public class AppUser : BaseEntity
{
    public required string Email { get; set; }

    public required string DisplayName { get; set; }

    public string PasswordHash { get; set; } = string.Empty;

    /// <summary>Babies this user is a caregiver for.</summary>
    public ICollection<Baby> Babies { get; set; } = new List<Baby>();
}
