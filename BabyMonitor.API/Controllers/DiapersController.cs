using BabyMonitor.API.DTOs;
using BabyMonitor.Core.Entities;
using BabyMonitor.Infrastructure.Persistence;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BabyMonitor.API.Controllers;

[ApiController]
[Route("api/babies/{babyId:guid}/diapers")]
public class DiapersController(AppDbContext db) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<IEnumerable<DiaperResponseDto>>> GetAll(Guid babyId)
    {
        if (!await db.Babies.AnyAsync(b => b.Id == babyId))
            return NotFound("Baby not found.");

        var items = await db.Diapers
            .Where(d => d.BabyId == babyId)
            .OrderByDescending(d => d.OccurredAt)
            .Select(d => MapToDto(d))
            .ToListAsync();

        return Ok(items);
    }

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<DiaperResponseDto>> GetById(Guid babyId, Guid id)
    {
        var entry = await db.Diapers.FirstOrDefaultAsync(
            d => d.Id == id && d.BabyId == babyId);

        if (entry is null) return NotFound();
        return Ok(MapToDto(entry));
    }

    [HttpPost]
    public async Task<ActionResult<DiaperResponseDto>> Create(Guid babyId, CreateDiaperDto dto)
    {
        if (!await db.Babies.AnyAsync(b => b.Id == babyId))
            return NotFound("Baby not found.");

        var entry = new DiaperEntry
        {
            Id         = Guid.NewGuid(),
            ClientId   = Guid.NewGuid(),
            BabyId     = babyId,
            OccurredAt = dto.OccurredAt,
            Type       = dto.Type,
            Notes      = dto.Notes,
            CreatedAt  = DateTime.UtcNow,
            UpdatedAt  = DateTime.UtcNow
        };

        db.Diapers.Add(entry);
        await db.SaveChangesAsync();

        return CreatedAtAction(nameof(GetById),
            new { babyId, id = entry.Id }, MapToDto(entry));
    }

    [HttpPut("{id:guid}")]
    public async Task<ActionResult<DiaperResponseDto>> Update(
        Guid babyId, Guid id, UpdateDiaperDto dto)
    {
        var entry = await db.Diapers.FirstOrDefaultAsync(
            d => d.Id == id && d.BabyId == babyId);

        if (entry is null) return NotFound();

        entry.OccurredAt = dto.OccurredAt;
        entry.Type       = dto.Type;
        entry.Notes      = dto.Notes;
        entry.UpdatedAt  = DateTime.UtcNow;

        await db.SaveChangesAsync();
        return Ok(MapToDto(entry));
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid babyId, Guid id)
    {
        var entry = await db.Diapers.FirstOrDefaultAsync(
            d => d.Id == id && d.BabyId == babyId);

        if (entry is null) return NotFound();

        db.Diapers.Remove(entry);
        await db.SaveChangesAsync();
        return NoContent();
    }

    private static DiaperResponseDto MapToDto(DiaperEntry d) => new(
        d.Id, d.ClientId, d.BabyId, d.OccurredAt,
        d.Type, d.Notes, d.CreatedAt, d.UpdatedAt);
}
