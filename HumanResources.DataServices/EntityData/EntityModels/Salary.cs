using System;
using System.Collections.Generic;

namespace HumanResourcesAPI.EntityData.EntityModels;

public partial class Salary
{
    public int SalaryId { get; set; }

    public int EmployeeId { get; set; }

    public decimal BaseSalary { get; set; }

    public decimal Deductions { get; set; }

    public decimal TotalSalary { get; set; }

    public DateOnly PayDate { get; set; }

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
