using System;
using System.Collections.Generic;

namespace _4LL_Monitoring.Models;

public partial class Collectedapidatum
{
    public int Id { get; set; }

    public int HttpStatusCode { get; set; }

    public DateTime Created { get; set; }

    public string ApiName { get; set; } = null!;

    public string Type { get; set; } = null!;

    public int? Threshold { get; set; }

    public int? Value { get; set; }

    public string? Status { get; set; }

    public DateOnly? Date { get; set; }

    public int? Year { get; set; }

    public int? Month { get; set; }

    public int? Day { get; set; }

    public int? Hour { get; set; }

    public int? Minute { get; set; }

    public string? Note { get; set; }

    public string? JsonResponse { get; set; }

    public string? ErrorDetails { get; set; }
}
