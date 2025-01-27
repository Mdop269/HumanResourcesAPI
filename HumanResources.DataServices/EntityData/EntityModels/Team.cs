using System;
using System.Collections.Generic;

namespace HumanResourcesAPI.EntityData.EntityModels;

public partial class Team
{
    public int TeamId { get; set; }

    public string TeamName { get; set; } = null!;

    public string Description { get; set; } = null!;

    public int DepartmentId { get; set; }

    public int TeamLeadEmployeeId { get; set; }

    public int TeamRoleId { get; set; }

    public int? CreatedBy { get; set; }

    public DateOnly CreatedOn { get; set; }

    public int? ChangedBy { get; set; }

    public DateOnly? ChangedOn { get; set; }

    public int? DeletedBy { get; set; }

    public DateOnly? DeletedOn { get; set; }

    public virtual Hr? ChangedByNavigation { get; set; }

    public virtual Hr? CreatedByNavigation { get; set; }

    public virtual Hr? DeletedByNavigation { get; set; }

    public virtual Department Department { get; set; } = null!;

    public virtual ICollection<Employee> Employees { get; set; } = new List<Employee>();

    public virtual ICollection<Review> Reviews { get; set; } = new List<Review>();

    public virtual Employee TeamLeadEmployee { get; set; } = null!;

    public virtual TeamRole TeamRole { get; set; } = null!;
}
