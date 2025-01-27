using System;
using System.Collections.Generic;

namespace HumanResourcesAPI.EntityData.EntityModels;

public partial class Employee
{
    public int EmployeeId { get; set; }

    public string FirstName { get; set; } = null!;

    public string LastName { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string Phone { get; set; } = null!;

    public DateOnly DateJoined { get; set; }

    public int StatusId { get; set; }

    public int DepartmentId { get; set; }

    public int RoleId { get; set; }

    public int PermissionsId { get; set; }

    public int? TeamId { get; set; }

    public int? CreatedBy { get; set; }

    public DateOnly CreatedOn { get; set; }

    public int? ChangedBy { get; set; }

    public DateOnly? ChangedOn { get; set; }

    public int? DeletedBy { get; set; }

    public DateOnly? DeletedOn { get; set; }

    public virtual ICollection<Attendance> Attendances { get; set; } = new List<Attendance>();

    public virtual ICollection<Bonu> Bonus { get; set; } = new List<Bonu>();

    public virtual Hr? ChangedByNavigation { get; set; }

    public virtual Hr? CreatedByNavigation { get; set; }

    public virtual Hr? DeletedByNavigation { get; set; }

    public virtual Department Department { get; set; } = null!;

    public virtual ICollection<Leave> LeaveCreatedByNavigations { get; set; } = new List<Leave>();

    public virtual ICollection<Leave> LeaveEmployees { get; set; } = new List<Leave>();

    public virtual ICollection<Login> Logins { get; set; } = new List<Login>();

    public virtual Permission Permissions { get; set; } = null!;

    public virtual ICollection<Promotion> Promotions { get; set; } = new List<Promotion>();

    public virtual ICollection<Review> ReviewEmployees { get; set; } = new List<Review>();

    public virtual ICollection<Review> ReviewReviewedByNavigations { get; set; } = new List<Review>();

    public virtual Role Role { get; set; } = null!;

    public virtual ICollection<Salary> Salaries { get; set; } = new List<Salary>();

    public virtual Status Status { get; set; } = null!;

    public virtual Team? Team { get; set; }

    public virtual ICollection<Team> Teams { get; set; } = new List<Team>();
}
