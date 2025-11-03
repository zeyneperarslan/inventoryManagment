using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

using Inventory.Api.Data;
using Inventory.Api.Models;

namespace Inventory.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UsersController : ControllerBase
{
    private readonly AppDbContext _db;
    public UsersController(AppDbContext db) => _db = db;

    // GET: api/users
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var users = await _db.Users
            .Include(u => u.Role)
            .Select(u => new {
                u.Id,
                FullName = u.Name,  
                u.Email,
                u.IsActive,
                Role = new { u.RoleId, Name = u.Role!.Name }
            })
            .ToListAsync();

        return Ok(users);
    }

    // GET: api/users/5
    [HttpGet("{id:int}")]
    public async Task<IActionResult> Get(int id)
    {
        var user = await _db.Users
            .Include(u => u.Role)
            .FirstOrDefaultAsync(u => u.Id == id);

        if (user is null) return NotFound();

        return Ok(new {
            user.Id,
            FullName = user.Name, 
            user.Email,
            user.IsActive,
            Role = user.Role is null ? null : new { user.RoleId, Name = user.Role.Name }
        });
    }

    // POST: api/users
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateUserDto dto)
    {
        if (!await _db.Roles.AnyAsync(r => r.Id == dto.RoleId))
            return BadRequest($"Geçersiz RoleId: {dto.RoleId}");

        var entity = new User {
            Name     = dto.FullName,  
            Email    = dto.Email,
            RoleId   = dto.RoleId,
            IsActive = dto.IsActive
        };

        _db.Users.Add(entity);
        await _db.SaveChangesAsync();

        return CreatedAtAction(nameof(Get), new { id = entity.Id }, new {
            entity.Id,
            FullName = entity.Name, // <—
            entity.Email,
            entity.IsActive,
            entity.RoleId
        });
    }

    // PUT: api/users/5
    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, [FromBody] UpdateUserDto dto)
    {
        var user = await _db.Users.FindAsync(id);
        if (user is null) return NotFound();

        if (dto.RoleId.HasValue &&
            !await _db.Roles.AnyAsync(r => r.Id == dto.RoleId.Value))
            return BadRequest($"Geçersiz RoleId: {dto.RoleId}");

        if (!string.IsNullOrWhiteSpace(dto.FullName)) user.Name = dto.FullName!; // <—
        if (!string.IsNullOrWhiteSpace(dto.Email))    user.Email = dto.Email!;
        if (dto.RoleId.HasValue)                      user.RoleId = dto.RoleId.Value;
        if (dto.IsActive.HasValue)                    user.IsActive = dto.IsActive.Value;

        await _db.SaveChangesAsync();
        return NoContent();
    }

    // DELETE: api/users/5
    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        var user = await _db.Users.FindAsync(id);
        if (user is null) return NotFound();

        _db.Users.Remove(user);
        await _db.SaveChangesAsync();
        return NoContent();
    }
}


public record CreateUserDto(
    string FullName,
    string Email,
    int RoleId,
    bool IsActive = true
);

public record UpdateUserDto(
    string? FullName,
    string? Email,
    int? RoleId,
    bool? IsActive
);
