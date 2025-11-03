using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

using Inventory.Api.Data;
using Inventory.Api.Models;

namespace Inventory.Api.Controllers;


[ApiController]
[Route("api/[controller]")]
public class ShelvesController : ControllerBase
{
    private readonly AppDbContext _db;
    public ShelvesController(AppDbContext db) => _db = db;

    // GET: api/shelves
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var shelves = await _db.Shelves
            .Include(s => s.Warehouse)
            .Select(s => new {
                s.Id,
                s.Name,
                s.WarehouseId,
                Warehouse = s.Warehouse!.Name
            })
            .ToListAsync();

        return Ok(shelves);
    }

    // GET: api/shelves/5 (raf + içindeki malzemeler)
    [HttpGet("{id:int}")]
    public async Task<IActionResult> Get(int id)
    {
        var shelf = await _db.Shelves
            .Include(s => s.Warehouse)
            .Include(s => s.ShelfMaterials)
                .ThenInclude(sm => sm.Material)
            .FirstOrDefaultAsync(s => s.Id == id);

        if (shelf is null) return NotFound();

        var result = new {
            shelf.Id,
            shelf.Name,
            shelf.WarehouseId,
            Warehouse = shelf.Warehouse!.Name,
            Materials = shelf.ShelfMaterials!.Select(sm => new {
                sm.MaterialId,
                MaterialName = sm.Material!.Name,
                sm.Quantity
            })
        };

        return Ok(result);
    }

    // POST: api/shelves
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateShelfDto dto)
    {
        if (!await _db.Warehouses.AnyAsync(w => w.Id == dto.WarehouseId))
            return BadRequest($"Geçersiz WarehouseId: {dto.WarehouseId}");

        var entity = new Shelf { Name = dto.Name, WarehouseId = dto.WarehouseId };
        _db.Shelves.Add(entity);
        await _db.SaveChangesAsync();
        return CreatedAtAction(nameof(Get), new { id = entity.Id }, entity);
    }

    // PUT: api/shelves/5
    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, [FromBody] UpdateShelfDto dto)
    {
        var shelf = await _db.Shelves.FindAsync(id);
        if (shelf is null) return NotFound();

        if (dto.WarehouseId.HasValue &&
            !await _db.Warehouses.AnyAsync(w => w.Id == dto.WarehouseId.Value))
            return BadRequest($"Geçersiz WarehouseId: {dto.WarehouseId}");

        shelf.Name = dto.Name ?? shelf.Name;
        if (dto.WarehouseId.HasValue) shelf.WarehouseId = dto.WarehouseId.Value;

        await _db.SaveChangesAsync();
        return NoContent();
    }

    // DELETE: api/shelves/5
    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        var shelf = await _db.Shelves.FindAsync(id);
        if (shelf is null) return NotFound();

        _db.Shelves.Remove(shelf);
        await _db.SaveChangesAsync();
        return NoContent();
    }

    // POST: api/shelves/5/materials  → rafa malzeme ekle/güncelle
    [HttpPost("{shelfId:int}/materials")]
    public async Task<IActionResult> UpsertMaterial(int shelfId, [FromBody] UpsertShelfMaterialDto dto)
    {
        if (!await _db.Shelves.AnyAsync(s => s.Id == shelfId))
            return NotFound($"Shelf {shelfId} bulunamadı.");

        if (!await _db.Materials.AnyAsync(m => m.Id == dto.MaterialId))
            return BadRequest($"Geçersiz MaterialId: {dto.MaterialId}");

        var link = await _db.ShelfMaterials
            .FirstOrDefaultAsync(sm => sm.ShelfId == shelfId && sm.MaterialId == dto.MaterialId);

        if (link is null)
        {
            link = new ShelfMaterial { ShelfId = shelfId, MaterialId = dto.MaterialId, Quantity = dto.Quantity };
            _db.ShelfMaterials.Add(link);
        }
        else
        {
            link.Quantity = dto.Quantity;
        }

        await _db.SaveChangesAsync();
        return Ok(new { shelfId, dto.MaterialId, dto.Quantity });
    }
}

// --- DTO'lar ---
public record CreateShelfDto(string Name, int WarehouseId);
public record UpdateShelfDto(string? Name, int? WarehouseId);
public record UpsertShelfMaterialDto(int MaterialId, int Quantity);
