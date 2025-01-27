using System;
using System.Collections.Generic;

namespace HumanResourcesAPI.EntityData.EntityModels;

public partial class Review
{
    public int ReviewId { get; set; }

    public int TeamId { get; set; }

    public int EmployeeId { get; set; }

    public int ReviewedBy { get; set; }

    public DateOnly ReviewDate { get; set; }

    public string Comments { get; set; } = null!;

    public decimal PerformanceRating { get; set; }

    public virtual Employee Employee { get; set; } = null!;

    public virtual Employee ReviewedByNavigation { get; set; } = null!;

    public virtual Team Team { get; set; } = null!;
}
