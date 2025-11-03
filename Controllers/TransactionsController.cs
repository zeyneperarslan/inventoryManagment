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

    /// <summary>Stoğa giriş (increase)</summary>
    [HttpPost("in")]
    [ProducesResponseType(typeof(object), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(object), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(object), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> StockIn([FromBody] MoveDto dto, CancellationToken ct = default)
    {
        if (dto.Quantity <= 0) return BadRequest(new { error = "Quantity must be > 0" });

        var (ok, err, code) = await _svc.StockInAsync(
            new TransactionService.StockRequest(dto.ShelfId, dto.MaterialId, dto.Quantity, dto.UserId, dto.Notes), ct);

        return code switch
        {
            TransactionService.ErrorCode.NotFound   => NotFound(new { error = err }),
            TransactionService.ErrorCode.Validation => BadRequest(new { error = err }),
            _                                       => ok ? Ok(new { message = "Stock increased" })
                                                         : BadRequest(new { error = err ?? "Unable to increase stock" })
        };
    }

    /// <summary>Stoktan çıkış (decrease)</summary>
    [HttpPost("out")]
    [ProducesResponseType(typeof(object), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(object), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(object), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> StockOut([FromBody] MoveDto dto, CancellationToken ct = default)
    {
        if (dto.Quantity <= 0) return BadRequest(new { error = "Quantity must be > 0" });

        var (ok, err, code) = await _svc.StockOutAsync(
            new TransactionService.StockRequest(dto.ShelfId, dto.MaterialId, dto.Quantity, dto.UserId, dto.Notes), ct);

        return code switch
        {
            TransactionService.ErrorCode.NotFound   => NotFound(new { error = err }),
            TransactionService.ErrorCode.Validation => BadRequest(new { error = err }),
            _                                       => ok ? Ok(new { message = "Stock decreased" })
                                                         : BadRequest(new { error = err ?? "Unable to decrease stock" })
        };
    }
}
