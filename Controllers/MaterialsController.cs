using Inventory.Api.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Inventory.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class MaterialsController : ControllerBase
{
    private readonly AppDbContext _db;
    public MaterialsController(AppDbContext db) => _db = db;

    // GET /api/materials?search=vida&skip=0&take=20
    [HttpGet]
    public async Task<IActionResult> Get([FromQuery] string? search, [FromQuery] int skip = 0, [FromQuery] int take = 50)
    {
        var q = _db.Materials.AsQueryable();
        if (!string.IsNullOrWhiteSpace(search))
            q = q.Where(m => m.Name.Contains(search) || m.Barcode.Contains(search));

        var total = await q.CountAsync();
        var items = await q
            .OrderBy(m => m.Name)
            .Skip(skip).Take(take)
            .Select(m => new { m.Id, m.Barcode, m.Name, m.UnitOfMeasure })
            .ToListAsync();

        return Ok(new { total, items });
    }
}
