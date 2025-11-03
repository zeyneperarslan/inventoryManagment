using Inventory.Api.Data;
using Inventory.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace Inventory.Api.Services;

public class TransactionService
{
    private readonly AppDbContext _db;
    public TransactionService(AppDbContext db) => _db = db;

    public enum ErrorCode { None, Validation, NotFound, Conflict }

    public record StockRequest(int ShelfId, int MaterialId, int Quantity, int UserId, string? Notes);

    public Task<(bool ok, string? err, ErrorCode code)> StockInAsync(StockRequest req, CancellationToken ct = default)
        => MoveStockAsync(req, isIn: true, ct);

    public Task<(bool ok, string? err, ErrorCode code)> StockOutAsync(StockRequest req, CancellationToken ct = default)
        => MoveStockAsync(req, isIn: false, ct);

    private async Task<(bool ok, string? err, ErrorCode code)> MoveStockAsync(
        StockRequest req, bool isIn, CancellationToken ct)
    {
        if (req.Quantity <= 0)
            return (false, "Quantity must be > 0", ErrorCode.Validation);

        var shelf = await _db.Shelves.AsNoTracking().FirstOrDefaultAsync(s => s.Id == req.ShelfId, ct);
        if (shelf is null)
            return (false, $"Shelf {req.ShelfId} not found.", ErrorCode.NotFound);

        var material = await _db.Materials.AsNoTracking().FirstOrDefaultAsync(m => m.Id == req.MaterialId, ct);
        if (material is null)
            return (false, $"Material {req.MaterialId} not found.", ErrorCode.NotFound);

        var userExists = await _db.Users.AsNoTracking().AnyAsync(u => u.Id == req.UserId, ct);
        if (!userExists)
            return (false, $"User {req.UserId} not found.", ErrorCode.NotFound);

        await using var tx = await _db.Database.BeginTransactionAsync(ct);
        try
        {
            // ShelfMaterial upsert
            var link = await _db.ShelfMaterials
                .FirstOrDefaultAsync(x => x.ShelfId == req.ShelfId && x.MaterialId == req.MaterialId, ct);

            if (link is null)
            {
                link = new ShelfMaterial
                {
                    ShelfId = req.ShelfId,
                    MaterialId = req.MaterialId,
                    Quantity = 0
                };
                _db.ShelfMaterials.Add(link);
            }

            var delta = isIn ? req.Quantity : -req.Quantity;
            var newQty = link.Quantity + delta;
            if (newQty < 0)
                return (false, "Stock cannot be negative.", ErrorCode.Validation);

            link.Quantity = newQty;

            // Transaction kaydı (Direction/CreatedAt olmadan)
            var t = new Transaction
            {
                ShelfId    = req.ShelfId,
                MaterialId = req.MaterialId,
                Quantity   = delta,          // In: +Q, Out: -Q
                UserId     = req.UserId,
                Notes      = req.Notes
                // Direction / CreatedAt alanları yoksa yazmayız
            };
            _db.Transactions.Add(t);

            await _db.SaveChangesAsync(ct);
            await tx.CommitAsync(ct);
            return (true, null, ErrorCode.None);
        }
        catch (Exception ex)
        {
            await tx.RollbackAsync(ct);
            return (false, ex.Message, ErrorCode.Conflict);
        }
    }
}
