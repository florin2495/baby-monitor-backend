using BabyMonitor.API.DTOs;
using BabyMonitor.Core.Entities;
using BabyMonitor.Infrastructure.Persistence;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BabyMonitor.API.Controllers;

[ApiController]
[Route("api/babies/{babyId:guid}/feedings")]
public class FeedingsController(AppDbContext db) : ControllerBase
{
    // GET api/babies/{babyId}/feedings
    [HttpGet]
    public async Task<ActionResult<IEnumerable<FeedingResponseDto>>> GetAll(Guid babyId)
    {
        if (!await db.Babies.AnyAsync(b => b.Id == babyId))
            return NotFound("Baby not found.");

        var items = await db.Feedings
            .Where(f => f.BabyId == babyId)
            .OrderByDescending(f => f.OccurredAt)
            .Select(f => MapToDto(f))
            .ToListAsync();

        return Ok(items);
    }

    // GET api/babies/{babyId}/feedings/{id}
    [HttpGet("{id:guid}")]
    public async Task<ActionResult<FeedingResponseDto>> GetById(Guid babyId, Guid id)
    {
        var entry = await db.Feedings.FirstOrDefaultAsync(
            f => f.Id == id && f.BabyId == babyId);

        if (entry is null) return NotFound();

        return Ok(MapToDto(entry));
    }

    // POST api/babies/{babyId}/feedings
    [HttpPost]
    public async Task<ActionResult<FeedingResponseDto>> Create(Guid babyId, CreateFeedingDto dto)
    {
        if (!await db.Babies.AnyAsync(b => b.Id == babyId))
            return NotFound("Baby not found.");

        var entry = new FeedingEntry
        {
            Id              = Guid.NewGuid(),
            ClientId        = Guid.NewGuid(),
            BabyId          = babyId,
            OccurredAt      = dto.OccurredAt,
            Source          = dto.Source,
            AmountMl        = dto.AmountMl,
            DurationMinutes = dto.DurationMinutes,
            Notes           = dto.Notes,
            CreatedAt       = DateTime.UtcNow,
            UpdatedAt       = DateTime.UtcNow
        };

        db.Feedings.Add(entry);
        await db.SaveChangesAsync();

        return CreatedAtAction(nameof(GetById),
            new { babyId, id = entry.Id }, MapToDto(entry));
    }

    // PUT api/babies/{babyId}/feedings/{id}
    [HttpPut("{id:guid}")]
    public async Task<ActionResult<FeedingResponseDto>> Update(
        Guid babyId, Guid id, UpdateFeedingDto dto)
    {
        var entry = await db.Feedings.FirstOrDefaultAsync(
            f => f.Id == id && f.BabyId == babyId);

        if (entry is null) return NotFound();

        entry.OccurredAt      = dto.OccurredAt;
        entry.Source          = dto.Source;
        entry.AmountMl        = dto.AmountMl;
        entry.DurationMinutes = dto.DurationMinutes;
        entry.Notes           = dto.Notes;
        entry.UpdatedAt       = DateTime.UtcNow;

        await db.SaveChangesAsync();

        return Ok(MapToDto(entry));
    }

    // DELETE api/babies/{babyId}/feedings/{id}
    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid babyId, Guid id)
    {
        var entry = await db.Feedings.FirstOrDefaultAsync(
            f => f.Id == id && f.BabyId == babyId);

        if (entry is null) return NotFound();

        db.Feedings.Remove(entry);
        await db.SaveChangesAsync();

        return NoContent();
    }

    private static FeedingResponseDto MapToDto(FeedingEntry f) => new(
        f.Id, f.ClientId, f.BabyId, f.OccurredAt,
        f.Source, f.AmountMl, f.DurationMinutes,
        f.Notes, f.CreatedAt, f.UpdatedAt);
}
