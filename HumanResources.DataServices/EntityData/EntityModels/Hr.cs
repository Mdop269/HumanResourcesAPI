using System;
using System.Collections.Generic;

namespace HumanResourcesAPI.EntityData.EntityModels;

public partial class Hr
{
    public int HrId { get; set; }

    public string HrFirstName { get; set; } = null!;

    public string HrLastName { get; set; } = null!;

    public string HrEmail { get; set; } = null!;

    public int HrRoleId { get; set; }

    public string CreatedBy { get; set; } = null!;

    public DateOnly CreatedOn { get; set; }

    public string? ChangedBy { get; set; }

    public DateOnly? ChangedOn { get; set; }

    public string? DeletedBy { get; set; }

    public DateOnly? DeletedOn { get; set; }

    public virtual ICollection<Attendance> Attendances { get; set; } = new List<Attendance>();

    public virtual ICollection<Bonu> BonuChangedByNavigations { get; set; } = new List<Bonu>();

    public virtual ICollection<Bonu> BonuCreatedByNavigations { get; set; } = new List<Bonu>();

    public virtual ICollection<Bonu> BonuDeletedByNavigations { get; set; } = new List<Bonu>();

    public virtual ICollection<Employee> EmployeeChangedByNavigations { get; set; } = new List<Employee>();

    public virtual ICollection<Employee> EmployeeCreatedByNavigations { get; set; } = new List<Employee>();

    public virtual ICollection<Employee> EmployeeDeletedByNavigations { get; set; } = new List<Employee>();

    public virtual Role HrRole { get; set; } = null!;

    public virtual ICollection<Leave> LeaveChangedByNavigations { get; set; } = new List<Leave>();

    public virtual ICollection<Leave> LeaveDeletedByNavigations { get; set; } = new List<Leave>();

    public virtual ICollection<Promotion> PromotionChangedByNavigations { get; set; } = new List<Promotion>();

    public virtual ICollection<Promotion> PromotionCreatedByNavigations { get; set; } = new List<Promotion>();

    public virtual ICollection<Promotion> PromotionDeletedByNavigations { get; set; } = new List<Promotion>();

    public virtual ICollection<Salary> SalaryChangedByNavigations { get; set; } = new List<Salary>();

    public virtual ICollection<Salary> SalaryCreatedByNavigations { get; set; } = new List<Salary>();

    public virtual ICollection<Salary> SalaryDeletedByNavigations { get; set; } = new List<Salary>();

    public virtual ICollection<Team> TeamChangedByNavigations { get; set; } = new List<Team>();

    public virtual ICollection<Team> TeamCreatedByNavigations { get; set; } = new List<Team>();

    public virtual ICollection<Team> TeamDeletedByNavigations { get; set; } = new List<Team>();
}
