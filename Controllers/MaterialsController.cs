using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Inventory.Api.Data;
using Inventory.Api.Models;

namespace Inventory.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class MaterialsController : ControllerBase
{
    private readonly AppDbContext _db;
    public MaterialsController(AppDbContext db) => _db = db;

    // GET: /api/Materials
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<Material>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAll()
        => Ok(await _db.Materials.AsNoTracking().ToListAsync());

    // GET: /api/Materials/{id}
    [HttpGet("{id:int}")]
    [ProducesResponseType(typeof(Material), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Get(int id)
        => (await _db.Materials.FindAsync(id)) is { } m ? Ok(m) : NotFound();

    // POST: /api/Materials
    [HttpPost]
    [ProducesResponseType(typeof(Material), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Create([FromBody] MaterialDto dto)
    {
        if (string.IsNullOrWhiteSpace(dto.Name))
            return BadRequest(new { error = "Name is required." });

        var m = new Material
        {
            Name        = dto.Name.Trim(),
            Description = dto.Description?.Trim() ?? string.Empty  // <- uyarıyı önler
        };

        _db.Materials.Add(m);
        await _db.SaveChangesAsync();
        return CreatedAtAction(nameof(Get), new { id = m.Id }, m);
    }

    // PUT: /api/Materials/{id}
    [HttpPut("{id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Update(int id, [FromBody] MaterialDto dto)
    {
        if (string.IsNullOrWhiteSpace(dto.Name))
            return BadRequest(new { error = "Name is required." });

        var m = await _db.Materials.FindAsync(id);
        if (m is null) return NotFound();

        m.Name        = dto.Name.Trim();
        m.Description = dto.Description?.Trim() ?? m.Description; // null gelirse mevcut kalsın
        await _db.SaveChangesAsync();
        return NoContent();
    }

    // DELETE: /api/Materials/{id}
    [HttpDelete("{id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(int id)
    {
        var m = await _db.Materials.FindAsync(id);
        if (m is null) return NotFound();

        _db.Materials.Remove(m);
        await _db.SaveChangesAsync();
        return NoContent();
    }
}

// DTO
public record MaterialDto(string Name, string? Description);
