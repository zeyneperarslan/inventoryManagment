using System.Collections.Generic;

namespace Inventory.Api.Models;

public class User
{
    public int Id { get; set; }
    public string FirstName { get; set; } = null!;
    public string LastName { get; set; } = null!;
    public string PersonnelId { get; set; } = null!;
    public string CardNumber { get; set; } = null!;
    public string Email { get; set; } = null!;
    public string PasswordHash { get; set; } = null!;
    public bool IsActive { get; set; }

    public int RoleId { get; set; }
    public Role Role { get; set; } = null!;

    public ICollection<Transaction> Transactions { get; set; } = new List<Transaction>();
}
