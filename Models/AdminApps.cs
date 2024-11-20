using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace _4LL_Monitoring.Models;

[Table("Admin-Apps", Schema = "dbo")]
public class AdminApp
{
    [Key]
    [StringLength(30)]
    [Column(TypeName = "varchar")]
    public string AppID { get; set; } = default!;
    [StringLength(30)]
    [Column(TypeName = "varchar")]
    public string? FunctionID { get; set; }
    [Required]
    [Column(TypeName = "bit")]
    public bool ReportApp { get; set; } = true; // Set the default value to 1 (true)

    // Navigation property to AdminFunction
    [ForeignKey("FunctionID")]
    public AdminFunction? Function { get; set; }
}
