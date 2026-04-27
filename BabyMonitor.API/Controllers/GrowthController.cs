using BabyMonitor.API.DTOs;
using BabyMonitor.Core.Entities;
using BabyMonitor.Infrastructure.Persistence;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BabyMonitor.API.Controllers;

[ApiController]
[Route("api/babies/{babyId:guid}/growth")]
public class GrowthController(AppDbContext db) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<IEnumerable<GrowthResponseDto>>> GetAll(Guid babyId)
    {
        if (!await db.Babies.AnyAsync(b => b.Id == babyId))
            return NotFound("Baby not found.");

        var items = await db.GrowthEntries
            .Where(g => g.BabyId == babyId)
            .OrderByDescending(g => g.MeasuredAt)
            .Select(g => MapToDto(g))
            .ToListAsync();

        return Ok(items);
    }

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<GrowthResponseDto>> GetById(Guid babyId, Guid id)
    {
        var entry = await db.GrowthEntries.FirstOrDefaultAsync(
            g => g.Id == id && g.BabyId == babyId);

        if (entry is null) return NotFound();
        return Ok(MapToDto(entry));
    }

    [HttpPost]
    public async Task<ActionResult<GrowthResponseDto>> Create(Guid babyId, CreateGrowthDto dto)
    {
        if (!await db.Babies.AnyAsync(b => b.Id == babyId))
            return NotFound("Baby not found.");

        var entry = new GrowthEntry
        {
            Id                  = Guid.NewGuid(),
            ClientId            = Guid.NewGuid(),
            BabyId              = babyId,
            MeasuredAt          = dto.MeasuredAt,
            WeightKg            = dto.WeightKg,
            HeightCm            = dto.HeightCm,
            HeadCircumferenceCm = dto.HeadCircumferenceCm,
            Notes               = dto.Notes,
            CreatedAt           = DateTime.UtcNow,
            UpdatedAt           = DateTime.UtcNow
        };

        db.GrowthEntries.Add(entry);
        await db.SaveChangesAsync();

        return CreatedAtAction(nameof(GetById),
            new { babyId, id = entry.Id }, MapToDto(entry));
    }

    [HttpPut("{id:guid}")]
    public async Task<ActionResult<GrowthResponseDto>> Update(
        Guid babyId, Guid id, UpdateGrowthDto dto)
    {
        var entry = await db.GrowthEntries.FirstOrDefaultAsync(
            g => g.Id == id && g.BabyId == babyId);

        if (entry is null) return NotFound();

        entry.MeasuredAt          = dto.MeasuredAt;
        entry.WeightKg            = dto.WeightKg;
        entry.HeightCm            = dto.HeightCm;
        entry.HeadCircumferenceCm = dto.HeadCircumferenceCm;
        entry.Notes               = dto.Notes;
        entry.UpdatedAt           = DateTime.UtcNow;

        await db.SaveChangesAsync();
        return Ok(MapToDto(entry));
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid babyId, Guid id)
    {
        var entry = await db.GrowthEntries.FirstOrDefaultAsync(
            g => g.Id == id && g.BabyId == babyId);

        if (entry is null) return NotFound();

        db.GrowthEntries.Remove(entry);
        await db.SaveChangesAsync();
        return NoContent();
    }

    private static GrowthResponseDto MapToDto(GrowthEntry g) => new(
        g.Id, g.ClientId, g.BabyId, g.MeasuredAt,
        g.WeightKg, g.HeightCm, g.HeadCircumferenceCm,
        g.Notes, g.CreatedAt, g.UpdatedAt);
}
