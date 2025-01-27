using System;
using System.Collections.Generic;

namespace HumanResourcesAPI.EntityData.EntityModels;

public partial class Bonu
{
    public int BonusId { get; set; }

    public int EmployeeId { get; set; }

    public decimal? BonusAmount { get; set; }

    public string Reason { get; set; } = null!;

    public DateOnly DateAwarded { get; set; }

    public int CreatedBy { get; set; }

    public DateOnly CreatedOn { get; set; }

    public int? ChangedBy { get; set; }

    public DateOnly? ChangedOn { get; set; }

    public int? DeletedBy { get; set; }

    public DateOnly? DeletedOn { get; set; }

    public virtual Hr? ChangedByNavigation { get; set; }

    public virtual Hr CreatedByNavigation { get; set; } = null!;

    public virtual Hr? DeletedByNavigation { get; set; }

    public virtual Employee Employee { get; set; } = null!;
}
