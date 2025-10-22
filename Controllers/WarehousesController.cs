using Inventory.Api.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Inventory.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class WarehousesController : ControllerBase
{
    private readonly AppDbContext _db;
    public WarehousesController(AppDbContext db) => _db = db;

    // GET /api/warehouses
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var list = await _db.Warehouses
            .Select(w => new { w.Id, w.Name })
            .ToListAsync();
        return Ok(list);
    }

    // GET /api/warehouses/{id}/shelves
    [HttpGet("{id:int}/shelves")]
    public async Task<IActionResult> GetShelves(int id)
    {
        var shelves = await _db.Shelves
            .Where(s => s.WarehouseId == id)
            .Select(s => new { s.Id, s.Name, s.ShelfLevel })
            .ToListAsync();
        return Ok(shelves);
    }
}
