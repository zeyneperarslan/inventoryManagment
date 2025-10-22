using System.Collections.Generic;

namespace Inventory.Api.Models;

public class Shelf
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public int ShelfLevel { get; set; }

    public int WarehouseId { get; set; }
    public Warehouse Warehouse { get; set; } = null!;

    public ICollection<ShelfMaterial> ShelfMaterials { get; set; } = new List<ShelfMaterial>();
}
