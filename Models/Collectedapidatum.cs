using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace _4LL_Monitoring.Models;

public class Collectedapidatum
{
    [Key]
    public int Id { get; set; }

    public int HttpStatusCode { get; set; }

    [Required]
    public DateTime Created { get; set; } = DateTime.UtcNow;

    [Required]
    [MaxLength(255)]
    public string? ApiName { get; set; }

    [Required]
    [MaxLength(20)]
    public string Type { get; set; }

    public int? Threshold { get; set; }

    public int? Value { get; set; }

    [MaxLength(255)]
    public string? Status { get; set; }

    // Computed columns
    [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
    public DateTime Date { get; private set; }

    [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
    public int Year { get; private set; }

    [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
    public int Month { get; private set; }

    [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
    public int Day { get; private set; }

    [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
    public int Hour { get; private set; }

    [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
    public int Minute { get; private set; }

    public string? Note { get; set; }

    public string? JsonResponse { get; set; }

    public string? ErrorDetails { get; set; }

    public long ElapsedMilliseconds { get; set; }
}
