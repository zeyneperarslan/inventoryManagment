namespace Inventory.Api.Models;

public class ShelfMaterial
{
    public int Id { get; set; }

    public int ShelfId { get; set; }
    public Shelf Shelf { get; set; } = null!;

    public int MaterialId { get; set; }
    public Material Material { get; set; } = null!;

    public int Quantity { get; set; }
    public DateTime LastUpdated { get; set; }
}
