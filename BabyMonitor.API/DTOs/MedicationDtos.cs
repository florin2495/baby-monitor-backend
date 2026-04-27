namespace BabyMonitor.API.DTOs;

public record CreateMedicationDto(
    DateTime AdministeredAt,
    string Name,
    decimal? DoseAmount = null,
    string? DoseUnit = null,
    string? Notes = null);

public record UpdateMedicationDto(
    DateTime AdministeredAt,
    string Name,
    decimal? DoseAmount = null,
    string? DoseUnit = null,
    string? Notes = null);

public record MedicationResponseDto(
    Guid Id,
    Guid ClientId,
    Guid BabyId,
    DateTime AdministeredAt,
    string Name,
    decimal? DoseAmount,
    string? DoseUnit,
    string? Notes,
    DateTime CreatedAt,
    DateTime UpdatedAt);
