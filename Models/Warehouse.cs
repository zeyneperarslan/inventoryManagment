using System.Collections.Generic;

namespace Inventory.Api.Models;

public class Warehouse
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public ICollection<Shelf> Shelves { get; set; } = new List<Shelf>();
}
