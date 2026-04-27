using BabyMonitor.API.DTOs;
using BabyMonitor.Core.Entities;
using BabyMonitor.Infrastructure.Persistence;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BabyMonitor.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class BabiesController(AppDbContext db) : ControllerBase
{
    // GET api/babies
    [HttpGet]
    public async Task<ActionResult<IEnumerable<BabyResponseDto>>> GetAll()
    {
        var babies = await db.Babies
            .OrderByDescending(b => b.CreatedAt)
            .Select(b => MapToDto(b))
            .ToListAsync();

        return Ok(babies);
    }

    // GET api/babies/{id}
    [HttpGet("{id:guid}")]
    public async Task<ActionResult<BabyResponseDto>> GetById(Guid id)
    {
        var baby = await db.Babies.FindAsync(id);
        if (baby is null) return NotFound();

        return Ok(MapToDto(baby));
    }

    // POST api/babies
    [HttpPost]
    public async Task<ActionResult<BabyResponseDto>> Create(CreateBabyDto dto)
    {
        var baby = new Baby
        {
            Id          = Guid.NewGuid(),
            ClientId    = Guid.NewGuid(),
            Name        = dto.Name,
            DateOfBirth = dto.DateOfBirth,
            Sex         = dto.Sex,
            Notes       = dto.Notes,
            CreatedAt   = DateTime.UtcNow,
            UpdatedAt   = DateTime.UtcNow
        };

        db.Babies.Add(baby);
        await db.SaveChangesAsync();

        return CreatedAtAction(nameof(GetById), new { id = baby.Id }, MapToDto(baby));
    }

    // PUT api/babies/{id}
    [HttpPut("{id:guid}")]
    public async Task<ActionResult<BabyResponseDto>> Update(Guid id, UpdateBabyDto dto)
    {
        var baby = await db.Babies.FindAsync(id);
        if (baby is null) return NotFound();

        baby.Name        = dto.Name;
        baby.DateOfBirth = dto.DateOfBirth;
        baby.Sex         = dto.Sex;
        baby.Notes       = dto.Notes;
        baby.UpdatedAt   = DateTime.UtcNow;

        await db.SaveChangesAsync();

        return Ok(MapToDto(baby));
    }

    // DELETE api/babies/{id}
    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        var baby = await db.Babies.FindAsync(id);
        if (baby is null) return NotFound();

        db.Babies.Remove(baby);
        await db.SaveChangesAsync();

        return NoContent();
    }

    // ── Mapping helper ──────────────────────────────────────────────────────

    private static BabyResponseDto MapToDto(Baby b) => new(
        b.Id, b.ClientId, b.Name, b.DateOfBirth,
        b.Sex, b.Notes, b.CreatedAt, b.UpdatedAt);
}
