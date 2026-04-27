using BabyMonitor.Core.Enums;

namespace BabyMonitor.API.DTOs;

public record CreateDiaperDto(
    DateTime OccurredAt,
    DiaperType Type = DiaperType.Unknown,
    string? Notes = null);

public record UpdateDiaperDto(
    DateTime OccurredAt,
    DiaperType Type = DiaperType.Unknown,
    string? Notes = null);

public record DiaperResponseDto(
    Guid Id,
    Guid ClientId,
    Guid BabyId,
    DateTime OccurredAt,
    DiaperType Type,
    string? Notes,
    DateTime CreatedAt,
    DateTime UpdatedAt);
