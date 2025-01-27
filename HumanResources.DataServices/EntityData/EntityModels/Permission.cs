using System;
using System.Collections.Generic;

namespace HumanResourcesAPI.EntityData.EntityModels;

public partial class Permission
{
    public int PermissionsId { get; set; }

    public string PermissionName { get; set; } = null!;

    public string PermissionDescription { get; set; } = null!;

    public virtual ICollection<Employee> Employees { get; set; } = new List<Employee>();
}
