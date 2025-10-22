using Inventory.Api.Services;
using Microsoft.AspNetCore.Mvc;

namespace Inventory.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TransactionsController : ControllerBase
{
    private readonly TransactionService _svc;
    public TransactionsController(TransactionService svc) => _svc = svc;

    public record MoveDto(int ShelfId, int MaterialId, int Quantity, int UserId, string? Notes);

    // POST /api/transactions/in
    [HttpPost("in")]
    public async Task<IActionResult> StockIn([FromBody] MoveDto dto, CancellationToken ct)
    {
        var (ok, err) = await _svc.StockInAsync(
            new TransactionService.StockRequest(dto.ShelfId, dto.MaterialId, dto.Quantity, dto.UserId, dto.Notes), ct);
        return ok ? Ok(new { message = "Stock increased" }) : BadRequest(new { error = err });
    }

    // POST /api/transactions/out
    [HttpPost("out")]
    public async Task<IActionResult> StockOut([FromBody] MoveDto dto, CancellationToken ct)
    {
        var (ok, err) = await _svc.StockOutAsync(
            new TransactionService.StockRequest(dto.ShelfId, dto.MaterialId, dto.Quantity, dto.UserId, dto.Notes), ct);
        return ok ? Ok(new { message = "Stock decreased" }) : BadRequest(new { error = err });
    }
}
