using BabyMonitor.Core.Enums;

namespace BabyMonitor.API.DTOs;

// ── Request DTOs ────────────────────────────────────────────────────────────

public record CreateBabyDto(
    string Name,
    DateOnly DateOfBirth,
    Sex Sex = Sex.Unknown,
    string? Notes = null);

public record UpdateBabyDto(
    string Name,
    DateOnly DateOfBirth,
    Sex Sex = Sex.Unknown,
    string? Notes = null);

// ── Response DTO ────────────────────────────────────────────────────────────

public record BabyResponseDto(
    Guid Id,
    Guid ClientId,
    string Name,
    DateOnly DateOfBirth,
    Sex Sex,
    string? Notes,
    DateTime CreatedAt,
    DateTime UpdatedAt);
