using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace _4LL_Monitoring.Models;

[Table("Admin-UserAppState", Schema = "dbo")]
public class AdminUserAppState
{
    [Key]
    [StringLength(30)]
    [Column(TypeName = "varchar")]
    public string UserID { get; set; } = default!;

    [Key]
    [StringLength(30)]
    [Column(TypeName = "varchar")]
    public string AppID { get; set; } = default!;

    [StringLength(300)]
    [Column(TypeName = "varchar")]
    public string? Settings { get; set; }

    // Navigation properties
    [ForeignKey("UserID")]
    public AdminUser? User { get; set; }

    [ForeignKey("AppID")]
    public AdminApp? App { get; set; }
}
