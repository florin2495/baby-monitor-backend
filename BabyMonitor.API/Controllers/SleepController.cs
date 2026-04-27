using BabyMonitor.API.DTOs;
using BabyMonitor.Core.Entities;
using BabyMonitor.Infrastructure.Persistence;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BabyMonitor.API.Controllers;

[ApiController]
[Route("api/babies/{babyId:guid}/sleep")]
public class SleepController(AppDbContext db) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<IEnumerable<SleepResponseDto>>> GetAll(Guid babyId)
    {
        if (!await db.Babies.AnyAsync(b => b.Id == babyId))
            return NotFound("Baby not found.");

        var items = await db.SleepSessions
            .Where(s => s.BabyId == babyId)
            .OrderByDescending(s => s.StartedAt)
            .Select(s => MapToDto(s))
            .ToListAsync();

        return Ok(items);
    }

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<SleepResponseDto>> GetById(Guid babyId, Guid id)
    {
        var entry = await db.SleepSessions.FirstOrDefaultAsync(
            s => s.Id == id && s.BabyId == babyId);

        if (entry is null) return NotFound();
        return Ok(MapToDto(entry));
    }

    [HttpPost]
    public async Task<ActionResult<SleepResponseDto>> Create(Guid babyId, CreateSleepDto dto)
    {
        if (!await db.Babies.AnyAsync(b => b.Id == babyId))
            return NotFound("Baby not found.");

        var entry = new SleepEntry
        {
            Id        = Guid.NewGuid(),
            ClientId  = Guid.NewGuid(),
            BabyId    = babyId,
            StartedAt = dto.StartedAt,
            EndedAt   = dto.EndedAt,
            Location  = dto.Location,
            Notes     = dto.Notes,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        db.SleepSessions.Add(entry);
        await db.SaveChangesAsync();

        return CreatedAtAction(nameof(GetById),
            new { babyId, id = entry.Id }, MapToDto(entry));
    }

    [HttpPut("{id:guid}")]
    public async Task<ActionResult<SleepResponseDto>> Update(
        Guid babyId, Guid id, UpdateSleepDto dto)
    {
        var entry = await db.SleepSessions.FirstOrDefaultAsync(
            s => s.Id == id && s.BabyId == babyId);

        if (entry is null) return NotFound();

        entry.StartedAt = dto.StartedAt;
        entry.EndedAt   = dto.EndedAt;
        entry.Location  = dto.Location;
        entry.Notes     = dto.Notes;
        entry.UpdatedAt = DateTime.UtcNow;

        await db.SaveChangesAsync();
        return Ok(MapToDto(entry));
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid babyId, Guid id)
    {
        var entry = await db.SleepSessions.FirstOrDefaultAsync(
            s => s.Id == id && s.BabyId == babyId);

        if (entry is null) return NotFound();

        db.SleepSessions.Remove(entry);
        await db.SaveChangesAsync();
        return NoContent();
    }

    private static SleepResponseDto MapToDto(SleepEntry s) => new(
        s.Id, s.ClientId, s.BabyId, s.StartedAt, s.EndedAt,
        s.Location, s.Notes, s.CreatedAt, s.UpdatedAt);
}
