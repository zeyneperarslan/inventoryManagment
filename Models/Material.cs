using System.Collections.Generic;

namespace Inventory.Api.Models;

public class Material
{
    public int Id { get; set; }
    public string Barcode { get; set; } = null!;
    public string Name { get; set; } = null!;
    public string DoorType { get; set; } = null!;        // örn: "Regular" | "Big"
    public string UnitOfMeasure { get; set; } = null!;   // örn: "BOX", "LITRE", "PIECE"
    public string Description { get; set; } = null!;     // örn: "Hydraulic", "Pneumatic"

    public ICollection<ShelfMaterial> ShelfMaterials { get; set; } = new List<ShelfMaterial>();
    public ICollection<Transaction> Transactions { get; set; } = new List<Transaction>();
}
