using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace _4LL_Monitoring.Models;

[Table("Admin-Roles", Schema = "dbo")]
public class AdminRole
{
    [Key]
    [StringLength(30)]
    [Column(TypeName = "varchar")]
    public string RoleID { get;  set; } = default!;
    public int  RoleLevel { get; set; }
    public bool ReadOnly  { get; set; }
    [Required]
    [Column(TypeName = "bit")]
    public bool Enabled { get; set; } = true; // Set the default value to 1 (true)
}
