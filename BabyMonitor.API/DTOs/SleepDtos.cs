namespace BabyMonitor.API.DTOs;

public record CreateSleepDto(
    DateTime StartedAt,
    DateTime? EndedAt = null,
    string? Location = null,
    string? Notes = null);

public record UpdateSleepDto(
    DateTime StartedAt,
    DateTime? EndedAt = null,
    string? Location = null,
    string? Notes = null);

public record SleepResponseDto(
    Guid Id,
    Guid ClientId,
    Guid BabyId,
    DateTime StartedAt,
    DateTime? EndedAt,
    string? Location,
    string? Notes,
    DateTime CreatedAt,
    DateTime UpdatedAt);
