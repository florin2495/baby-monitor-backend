namespace BabyMonitor.API.DTOs;

public record CreateGrowthDto(
    DateTime MeasuredAt,
    decimal? WeightKg = null,
    decimal? HeightCm = null,
    decimal? HeadCircumferenceCm = null,
    string? Notes = null);

public record UpdateGrowthDto(
    DateTime MeasuredAt,
    decimal? WeightKg = null,
    decimal? HeightCm = null,
    decimal? HeadCircumferenceCm = null,
    string? Notes = null);

public record GrowthResponseDto(
    Guid Id,
    Guid ClientId,
    Guid BabyId,
    DateTime MeasuredAt,
    decimal? WeightKg,
    decimal? HeightCm,
    decimal? HeadCircumferenceCm,
    string? Notes,
    DateTime CreatedAt,
    DateTime UpdatedAt);
