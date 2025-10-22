namespace Inventory.Api.Models;

public class Transaction
{
    public int Id { get; set; }
    public string TransactionType { get; set; } = null!; // "IN" veya "OUT"
    public int MaterialId { get; set; }
    public Material Material { get; set; } = null!;
    public int ShelfId { get; set; }
    public Shelf Shelf { get; set; } = null!;
    public int UserId { get; set; }
    public User User { get; set; } = null!;
    public int Quantity { get; set; }
    public DateTime TransactionDate { get; set; }
    public string? Notes { get; set; }
}
