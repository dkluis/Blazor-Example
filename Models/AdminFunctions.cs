using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace _4LL_Monitoring.Models;

[Table("Admin-Functions", Schema = "dbo")]
public class AdminFunction
{
    [Key]
    [Required]
    [MaxLength(30)]
    public required string FunctionID { get; set; }
}
