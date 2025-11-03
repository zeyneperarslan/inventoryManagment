using System.Collections.Generic;

namespace Inventory.Api.Models;

public class Material
{
    public int Id { get; set; }
    public string Barcode { get; set; } = null!;
    public string Name { get; set; } = null!;
    public string DoorType { get; set; } = null!;        
    public string UnitOfMeasure { get; set; } = null!;   
    public string Description { get; set; } = string.Empty;


    public ICollection<ShelfMaterial> ShelfMaterials { get; set; } = new List<ShelfMaterial>();
    public ICollection<Transaction> Transactions { get; set; } = new List<Transaction>();
}
