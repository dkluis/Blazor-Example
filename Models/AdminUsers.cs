using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace _4LL_Monitoring.Models;

[Table("Admin-Users", Schema = "dbo")]
public class AdminUser
{
    [Key]
    [StringLength(30)]
    [Column(TypeName = "varchar")]
    public string UserID { get; set; } = default!;
    [StringLength(100)]
    [Column(TypeName = "varchar")]
    public string Password { get; set; } = default!;
    [StringLength(100)]
    [Column(TypeName = "varchar")]
    public string? Salt { get; set; }
    [Required]
    [Column(TypeName = "bit")]
    public bool Enabled { get; set; } = true; // Set the default value to 1 (true)
}
