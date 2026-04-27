using BabyMonitor.Core.Enums;

namespace BabyMonitor.API.DTOs;

public record CreateFeedingDto(
    DateTime OccurredAt,
    FeedingSource Source = FeedingSource.Unknown,
    int? AmountMl = null,
    int? DurationMinutes = null,
    string? Notes = null);

public record UpdateFeedingDto(
    DateTime OccurredAt,
    FeedingSource Source = FeedingSource.Unknown,
    int? AmountMl = null,
    int? DurationMinutes = null,
    string? Notes = null);

public record FeedingResponseDto(
    Guid Id,
    Guid ClientId,
    Guid BabyId,
    DateTime OccurredAt,
    FeedingSource Source,
    int? AmountMl,
    int? DurationMinutes,
    string? Notes,
    DateTime CreatedAt,
    DateTime UpdatedAt);
