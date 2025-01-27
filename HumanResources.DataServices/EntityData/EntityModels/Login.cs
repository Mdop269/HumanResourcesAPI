using System;
using System.Collections.Generic;

namespace HumanResourcesAPI.EntityData.EntityModels;

public partial class Login
{
    public int LoginId { get; set; }

    public string? Username { get; set; }

    public string Password { get; set; } = null!;

    public int? SaltKeyId { get; set; }

    public int StatusId { get; set; }

    public int EmployeeId { get; set; }

    public DateTime LoginTime { get; set; }

    public DateTime? LogoutTime { get; set; }

    public virtual Employee Employee { get; set; } = null!;

    public virtual Status Status { get; set; } = null!;
}
