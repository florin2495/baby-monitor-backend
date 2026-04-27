using BabyMonitor.API.DTOs;
using BabyMonitor.Core.Entities;
using BabyMonitor.Infrastructure.Persistence;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BabyMonitor.API.Controllers;

[ApiController]
[Route("api/babies/{babyId:guid}/medications")]
public class MedicationsController(AppDbContext db) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<IEnumerable<MedicationResponseDto>>> GetAll(Guid babyId)
    {
        if (!await db.Babies.AnyAsync(b => b.Id == babyId))
            return NotFound("Baby not found.");

        var items = await db.Medications
            .Where(m => m.BabyId == babyId)
            .OrderByDescending(m => m.AdministeredAt)
            .Select(m => MapToDto(m))
            .ToListAsync();

        return Ok(items);
    }

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<MedicationResponseDto>> GetById(Guid babyId, Guid id)
    {
        var entry = await db.Medications.FirstOrDefaultAsync(
            m => m.Id == id && m.BabyId == babyId);

        if (entry is null) return NotFound();
        return Ok(MapToDto(entry));
    }

    [HttpPost]
    public async Task<ActionResult<MedicationResponseDto>> Create(
        Guid babyId, CreateMedicationDto dto)
    {
        if (!await db.Babies.AnyAsync(b => b.Id == babyId))
            return NotFound("Baby not found.");

        var entry = new MedicationEntry
        {
            Id             = Guid.NewGuid(),
            ClientId       = Guid.NewGuid(),
            BabyId         = babyId,
            AdministeredAt = dto.AdministeredAt,
            Name           = dto.Name,
            DoseAmount     = dto.DoseAmount,
            DoseUnit       = dto.DoseUnit,
            Notes          = dto.Notes,
            CreatedAt      = DateTime.UtcNow,
            UpdatedAt      = DateTime.UtcNow
        };

        db.Medications.Add(entry);
        await db.SaveChangesAsync();

        return CreatedAtAction(nameof(GetById),
            new { babyId, id = entry.Id }, MapToDto(entry));
    }

    [HttpPut("{id:guid}")]
    public async Task<ActionResult<MedicationResponseDto>> Update(
        Guid babyId, Guid id, UpdateMedicationDto dto)
    {
        var entry = await db.Medications.FirstOrDefaultAsync(
            m => m.Id == id && m.BabyId == babyId);

        if (entry is null) return NotFound();

        entry.AdministeredAt = dto.AdministeredAt;
        entry.Name           = dto.Name;
        entry.DoseAmount     = dto.DoseAmount;
        entry.DoseUnit       = dto.DoseUnit;
        entry.Notes          = dto.Notes;
        entry.UpdatedAt      = DateTime.UtcNow;

        await db.SaveChangesAsync();
        return Ok(MapToDto(entry));
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid babyId, Guid id)
    {
        var entry = await db.Medications.FirstOrDefaultAsync(
            m => m.Id == id && m.BabyId == babyId);

        if (entry is null) return NotFound();

        db.Medications.Remove(entry);
        await db.SaveChangesAsync();
        return NoContent();
    }

    private static MedicationResponseDto MapToDto(MedicationEntry m) => new(
        m.Id, m.ClientId, m.BabyId, m.AdministeredAt,
        m.Name, m.DoseAmount, m.DoseUnit,
        m.Notes, m.CreatedAt, m.UpdatedAt);
}
