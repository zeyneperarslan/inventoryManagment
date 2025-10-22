using Inventory.Api.Data;
using Inventory.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace Inventory.Api.Services;

public class TransactionService
{
    private readonly AppDbContext _db;
    public TransactionService(AppDbContext db) => _db = db;

    public record StockRequest(int ShelfId, int MaterialId, int Quantity, int UserId, string? Notes);

    public async Task<(bool ok, string? error)> StockInAsync(StockRequest req, CancellationToken ct = default)
        => await MoveAsync(req, "IN", ct);

    public async Task<(bool ok, string? error)> StockOutAsync(StockRequest req, CancellationToken ct = default)
        => await MoveAsync(req, "OUT", ct);

    private async Task<(bool ok, string? error)> MoveAsync(StockRequest req, string type, CancellationToken ct)
    {
        if (req.Quantity <= 0) return (false, "Quantity must be > 0");

        await using var tx = await _db.Database.BeginTransactionAsync(ct);
        try
        {
            var shelf = await _db.Shelves.FirstOrDefaultAsync(s => s.Id == req.ShelfId, ct);
            var mat   = await _db.Materials.FirstOrDefaultAsync(m => m.Id == req.MaterialId, ct);
            var user  = await _db.Users.FirstOrDefaultAsync(u => u.Id == req.UserId, ct);
            if (shelf is null || mat is null || user is null) return (false, "Shelf/Material/User not found");

            var sm = await _db.ShelfMaterials
                .FirstOrDefaultAsync(x => x.ShelfId == req.ShelfId && x.MaterialId == req.MaterialId, ct);

            if (sm is null)
            {
                sm = new ShelfMaterial
                {
                    ShelfId = req.ShelfId,
                    MaterialId = req.MaterialId,
                    Quantity = 0,
                    LastUpdated = DateTime.UtcNow
                };
                _db.ShelfMaterials.Add(sm);
            }

            if (type == "OUT")
            {
                if (sm.Quantity < req.Quantity)
                    return (false, $"Insufficient stock. Available: {sm.Quantity}");
                sm.Quantity -= req.Quantity;
            }
            else
            {
                sm.Quantity += req.Quantity;
            }

            sm.LastUpdated = DateTime.UtcNow;

            _db.Transactions.Add(new Transaction
            {
                TransactionType = type,
                ShelfId = req.ShelfId,
                MaterialId = req.MaterialId,
                UserId = req.UserId,
                Quantity = req.Quantity,
                TransactionDate = DateTime.UtcNow,
                Notes = req.Notes
            });

            await _db.SaveChangesAsync(ct);
            await tx.CommitAsync(ct);
            return (true, null);
        }
        catch (Exception ex)
        {
            await tx.RollbackAsync(ct);
            return (false, ex.Message);
        }
    }
}
