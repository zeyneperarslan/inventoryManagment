using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Inventory.Api.Data;
using Inventory.Api.Models;

namespace Inventory.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class WarehousesController : ControllerBase
{
    private readonly AppDbContext _db;
    public WarehousesController(AppDbContext db) => _db = db;

    // Depoları ve içindeki raf sayısını listele
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var list = await _db.Warehouses
            .Include(w => w.Shelves)
            .Select(w => new {
                w.Id, w.Name,
                ShelfCount = w.Shelves!.Count
            })
            .ToListAsync();

        return Ok(list);
    }

    // Depo + rafları detay
    [HttpGet("{id:int}")]
    public async Task<IActionResult> Get(int id)
    {
        var w = await _db.Warehouses
            .Include(w => w.Shelves)
            .FirstOrDefaultAsync(w => w.Id == id);

        if (w is null) return NotFound();

        return Ok(new {
            w.Id, w.Name,
            Shelves = w.Shelves!.Select(s => new { s.Id, s.Name })
        });
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] WarehouseDto dto)
    {
        var w = new Warehouse { Name = dto.Name };
        _db.Warehouses.Add(w);
        await _db.SaveChangesAsync();
        return CreatedAtAction(nameof(Get), new { id = w.Id }, w);
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, [FromBody] WarehouseDto dto)
    {
        var w = await _db.Warehouses.FindAsync(id);
        if (w is null) return NotFound();
        w.Name = dto.Name;
        await _db.SaveChangesAsync();
        return NoContent();
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        var w = await _db.Warehouses.FindAsync(id);
        if (w is null) return NotFound();
        _db.Warehouses.Remove(w);
        await _db.SaveChangesAsync();
        return NoContent();
    }
}

public record WarehouseDto(string Name);
