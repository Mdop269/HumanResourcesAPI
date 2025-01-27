using System;
using System.Collections.Generic;

namespace HumanResourcesAPI.EntityData.EntityModels;

public partial class Leave
{
    public int LeaveId { get; set; }

    public int EmployeeId { get; set; }

    public string LeaveType { get; set; } = null!;

    public DateOnly StartDate { get; set; }

    public DateOnly EndDate { get; set; }

    public string Reason { get; set; } = null!;

    public int StatusId { get; set; }

    public int CreatedBy { get; set; }

    public DateOnly CreatedOn { get; set; }

    public int? ChangedBy { get; set; }

    public DateOnly? ChangedOn { get; set; }

    public int? DeletedBy { get; set; }

    public DateOnly? DeletedOn { get; set; }

    public virtual Hr? ChangedByNavigation { get; set; }

    public virtual Employee CreatedByNavigation { get; set; } = null!;

    public virtual Hr? DeletedByNavigation { get; set; }

    public virtual Employee Employee { get; set; } = null!;

    public virtual Status Status { get; set; } = null!;
}
