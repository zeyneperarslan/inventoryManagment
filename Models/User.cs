using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema; 

namespace Inventory.Api.Models;

public class User
{
    public int Id { get; set; }

    [Required, MaxLength(150)]
    [Column("FullName")]               
    public string Name { get; set; } = string.Empty;

    [Required, EmailAddress, MaxLength(200)]
    public string Email { get; set; } = string.Empty;

    public bool IsActive { get; set; } = true;

    public int RoleId { get; set; }
    public Role? Role { get; set; }

    public ICollection<Transaction> Transactions { get; set; } = new List<Transaction>();
}
