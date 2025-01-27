using System;
using System.Collections.Generic;
using HumanResourcesAPI.EntityData.EntityModels;
using Microsoft.EntityFrameworkCore;

namespace HumanResourcesAPI.EntityData;

public partial class HumanResourcesContext : DbContext
{
    public HumanResourcesContext()
    {
    }

    public HumanResourcesContext(DbContextOptions<HumanResourcesContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Attendance> Attendances { get; set; }

    public virtual DbSet<Bonu> Bonus { get; set; }

    public virtual DbSet<Department> Departments { get; set; }

    public virtual DbSet<Employee> Employees { get; set; }

    public virtual DbSet<Hr> Hrs { get; set; }

    public virtual DbSet<Leave> Leaves { get; set; }

    public virtual DbSet<Login> Logins { get; set; }

    public virtual DbSet<Permission> Permissions { get; set; }

    public virtual DbSet<Promotion> Promotions { get; set; }

    public virtual DbSet<Review> Reviews { get; set; }

    public virtual DbSet<Role> Roles { get; set; }

    public virtual DbSet<Salary> Salaries { get; set; }

    public virtual DbSet<Status> Statuses { get; set; }

    public virtual DbSet<Team> Teams { get; set; }

    public virtual DbSet<TeamRole> TeamRoles { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=LP027;Database=Human_Resources;Trusted_Connection=True;TrustServerCertificate=true");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Attendance>(entity =>
        {
            entity.HasKey(e => e.AttendanceId).HasName("PK__Attendan__57FA4914072A792E");

            entity.ToTable("Attendance");

            entity.Property(e => e.AttendanceId).HasColumnName("Attendance_Id");
            entity.Property(e => e.CheckInTime)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("Check_In_Time");
            entity.Property(e => e.CheckOutTime)
                .HasColumnType("datetime")
                .HasColumnName("Check_Out_Time");
            entity.Property(e => e.Date).HasDefaultValueSql("(getdate())");
            entity.Property(e => e.EmployeeId).HasColumnName("Employee_Id");
            entity.Property(e => e.StatusId).HasColumnName("Status_Id");

            entity.HasOne(d => d.ChangedByNavigation).WithMany(p => p.Attendances)
                .HasForeignKey(d => d.ChangedBy)
                .HasConstraintName("FK_Attendance_ChangedBy");

            entity.HasOne(d => d.Employee).WithMany(p => p.Attendances)
                .HasForeignKey(d => d.EmployeeId)
                .HasConstraintName("FK_Attendance_Employee_Id");

            entity.HasOne(d => d.Status).WithMany(p => p.Attendances)
                .HasForeignKey(d => d.StatusId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Attendance_Status_Id");
        });

        modelBuilder.Entity<Bonu>(entity =>
        {
            entity.HasKey(e => e.BonusId).HasName("PK__Bonus__1CDEA531FCEA632C");

            entity.Property(e => e.BonusId).HasColumnName("Bonus_Id");
            entity.Property(e => e.BonusAmount)
                .HasColumnType("decimal(18, 0)")
                .HasColumnName("Bonus_Amount");
            entity.Property(e => e.CreatedOn).HasDefaultValueSql("(getdate())");
            entity.Property(e => e.DateAwarded).HasColumnName("Date_Awarded");
            entity.Property(e => e.EmployeeId).HasColumnName("Employee_Id");
            entity.Property(e => e.Reason).IsUnicode(false);

            entity.HasOne(d => d.ChangedByNavigation).WithMany(p => p.BonuChangedByNavigations)
                .HasForeignKey(d => d.ChangedBy)
                .HasConstraintName("FK_Bonus_ChangedBy");

            entity.HasOne(d => d.CreatedByNavigation).WithMany(p => p.BonuCreatedByNavigations)
                .HasForeignKey(d => d.CreatedBy)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Bonus_CreatedBy");

            entity.HasOne(d => d.DeletedByNavigation).WithMany(p => p.BonuDeletedByNavigations)
                .HasForeignKey(d => d.DeletedBy)
                .HasConstraintName("FK_Bonus_DeletedBy");

            entity.HasOne(d => d.Employee).WithMany(p => p.Bonus)
                .HasForeignKey(d => d.EmployeeId)
                .HasConstraintName("FK_Bonus_Employee_Id");
        });

        modelBuilder.Entity<Department>(entity =>
        {
            entity.HasKey(e => e.DepartmentId).HasName("PK__Departme__151675F15E1551E4");

            entity.ToTable("Department");

            entity.HasIndex(e => e.DepartmentName, "UQ_Department_Department_Name").IsUnique();

            entity.Property(e => e.DepartmentId).HasColumnName("Department_Id");
            entity.Property(e => e.DepartmentName)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("Department_Name");
            entity.Property(e => e.Description).IsUnicode(false);
        });

        modelBuilder.Entity<Employee>(entity =>
        {
            entity.HasKey(e => e.EmployeeId).HasName("PK__Employee__781134A184C184D5");

            entity.ToTable("Employee");

            entity.HasIndex(e => new { e.FirstName, e.LastName, e.Email }, "UQ_Employee_Name").IsUnique();

            entity.HasIndex(e => e.Phone, "UQ__Employee__5C7E359E16611F62").IsUnique();

            entity.HasIndex(e => e.Email, "UQ__Employee__A9D10534A3E6B39A").IsUnique();

            entity.Property(e => e.EmployeeId).HasColumnName("Employee_Id");
            entity.Property(e => e.CreatedOn).HasDefaultValueSql("(getdate())");
            entity.Property(e => e.DateJoined).HasColumnName("Date_Joined");
            entity.Property(e => e.DepartmentId).HasColumnName("Department_Id");
            entity.Property(e => e.Email)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.FirstName)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("First_Name");
            entity.Property(e => e.LastName)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("Last_Name");
            entity.Property(e => e.PermissionsId).HasColumnName("Permissions_Id");
            entity.Property(e => e.Phone)
                .HasMaxLength(10)
                .IsUnicode(false);
            entity.Property(e => e.RoleId).HasColumnName("Role_Id");
            entity.Property(e => e.StatusId).HasColumnName("Status_Id");
            entity.Property(e => e.TeamId).HasColumnName("Team_Id");

            entity.HasOne(d => d.ChangedByNavigation).WithMany(p => p.EmployeeChangedByNavigations)
                .HasForeignKey(d => d.ChangedBy)
                .HasConstraintName("FK_Employee_ChangedBy");

            entity.HasOne(d => d.CreatedByNavigation).WithMany(p => p.EmployeeCreatedByNavigations)
                .HasForeignKey(d => d.CreatedBy)
                .HasConstraintName("FK_Employee_CreatedBy");

            entity.HasOne(d => d.DeletedByNavigation).WithMany(p => p.EmployeeDeletedByNavigations)
                .HasForeignKey(d => d.DeletedBy)
                .HasConstraintName("FK_Employee_DeletedBy");

            entity.HasOne(d => d.Department).WithMany(p => p.Employees)
                .HasForeignKey(d => d.DepartmentId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Employee_Department_Id");

            entity.HasOne(d => d.Permissions).WithMany(p => p.Employees)
                .HasForeignKey(d => d.PermissionsId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Employee_Permission_Id");

            entity.HasOne(d => d.Role).WithMany(p => p.Employees)
                .HasForeignKey(d => d.RoleId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Employee_Role_Id");

            entity.HasOne(d => d.Status).WithMany(p => p.Employees)
                .HasForeignKey(d => d.StatusId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Employee_Status_Id");

            entity.HasOne(d => d.Team).WithMany(p => p.Employees)
                .HasForeignKey(d => d.TeamId)
                .HasConstraintName("FK_Employee_Team_Id");
        });

        modelBuilder.Entity<Hr>(entity =>
        {
            entity.HasKey(e => e.HrId).HasName("PK__HR__420AEF95C39257AC");

            entity.ToTable("HR");

            entity.HasIndex(e => new { e.HrFirstName, e.HrLastName, e.HrEmail }, "UQ_Hr_Name").IsUnique();

            entity.HasIndex(e => e.HrEmail, "UQ__HR__7557D472A3670754").IsUnique();

            entity.Property(e => e.HrId).HasColumnName("Hr_Id");
            entity.Property(e => e.ChangedBy)
                .HasMaxLength(20)
                .IsUnicode(false);
            entity.Property(e => e.CreatedBy)
                .HasMaxLength(20)
                .IsUnicode(false);
            entity.Property(e => e.CreatedOn).HasDefaultValueSql("(getdate())");
            entity.Property(e => e.DeletedBy)
                .HasMaxLength(20)
                .IsUnicode(false);
            entity.Property(e => e.HrEmail)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("Hr_Email");
            entity.Property(e => e.HrFirstName)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("Hr_First_Name");
            entity.Property(e => e.HrLastName)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("Hr_Last_Name");
            entity.Property(e => e.HrRoleId).HasColumnName("Hr_Role_Id");

            entity.HasOne(d => d.HrRole).WithMany(p => p.Hrs)
                .HasForeignKey(d => d.HrRoleId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_HR_Role_ID");
        });

        modelBuilder.Entity<Leave>(entity =>
        {
            entity.HasKey(e => e.LeaveId).HasName("PK__Leave__D54C3B80E20F138D");

            entity.ToTable("Leave");

            entity.Property(e => e.LeaveId).HasColumnName("Leave_Id");
            entity.Property(e => e.CreatedOn).HasDefaultValueSql("(getdate())");
            entity.Property(e => e.EmployeeId).HasColumnName("Employee_Id");
            entity.Property(e => e.EndDate).HasColumnName("End_Date");
            entity.Property(e => e.LeaveType)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("Leave_Type");
            entity.Property(e => e.Reason).IsUnicode(false);
            entity.Property(e => e.StartDate).HasColumnName("Start_Date");
            entity.Property(e => e.StatusId)
                .HasDefaultValue(4)
                .HasColumnName("Status_Id");

            entity.HasOne(d => d.ChangedByNavigation).WithMany(p => p.LeaveChangedByNavigations)
                .HasForeignKey(d => d.ChangedBy)
                .HasConstraintName("FK_Leave_ChangedBy");

            entity.HasOne(d => d.CreatedByNavigation).WithMany(p => p.LeaveCreatedByNavigations)
                .HasForeignKey(d => d.CreatedBy)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Leave_CreatedBy");

            entity.HasOne(d => d.DeletedByNavigation).WithMany(p => p.LeaveDeletedByNavigations)
                .HasForeignKey(d => d.DeletedBy)
                .HasConstraintName("FK_Leave_DeletedBy");

            entity.HasOne(d => d.Employee).WithMany(p => p.LeaveEmployees)
                .HasForeignKey(d => d.EmployeeId)
                .HasConstraintName("FK_Leave_Employee_Id");

            entity.HasOne(d => d.Status).WithMany(p => p.Leaves)
                .HasForeignKey(d => d.StatusId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Leave_Status_Id");
        });

        modelBuilder.Entity<Login>(entity =>
        {
            entity.HasKey(e => e.LoginId).HasName("PK__Login__D7886B875B92B109");

            entity.ToTable("Login");

            entity.HasIndex(e => e.Username, "UQ__Login__536C85E42DC0856B").IsUnique();

            entity.Property(e => e.LoginId).HasColumnName("Login_Id");
            entity.Property(e => e.EmployeeId).HasColumnName("Employee_Id");
            entity.Property(e => e.LoginTime)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("Login_Time");
            entity.Property(e => e.LogoutTime)
                .HasColumnType("datetime")
                .HasColumnName("Logout_Time");
            entity.Property(e => e.Password)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.SaltKeyId).HasColumnName("SaltKey_Id");
            entity.Property(e => e.StatusId).HasColumnName("Status_Id");
            entity.Property(e => e.Username)
                .HasMaxLength(50)
                .IsUnicode(false);

            entity.HasOne(d => d.Employee).WithMany(p => p.Logins)
                .HasForeignKey(d => d.EmployeeId)
                .HasConstraintName("FK_Login_Employee_Id");

            entity.HasOne(d => d.Status).WithMany(p => p.Logins)
                .HasForeignKey(d => d.StatusId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Login_Status_Id");
        });

        modelBuilder.Entity<Permission>(entity =>
        {
            entity.HasKey(e => e.PermissionsId).HasName("PK__Permissi__AABA60C2889325CF");

            entity.HasIndex(e => e.PermissionName, "UQ_Permissions_Permission_Name").IsUnique();

            entity.Property(e => e.PermissionsId).HasColumnName("Permissions_Id");
            entity.Property(e => e.PermissionDescription)
                .IsUnicode(false)
                .HasColumnName("Permission_Description");
            entity.Property(e => e.PermissionName)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("Permission_Name");
        });

        modelBuilder.Entity<Promotion>(entity =>
        {
            entity.HasKey(e => e.PromotionId).HasName("PK__Promotio__2CB9556B0E832CF2");

            entity.ToTable("Promotion");

            entity.Property(e => e.PromotionId).HasColumnName("promotion_id");
            entity.Property(e => e.CreatedOn).HasDefaultValueSql("(getdate())");
            entity.Property(e => e.EmployeeId).HasColumnName("Employee_Id");
            entity.Property(e => e.NewRoleId).HasColumnName("New_Role_Id");
            entity.Property(e => e.OldRoleId).HasColumnName("Old_Role_Id");
            entity.Property(e => e.PromotionDate).HasColumnName("Promotion_Date");
            entity.Property(e => e.Reason).IsUnicode(false);

            entity.HasOne(d => d.ChangedByNavigation).WithMany(p => p.PromotionChangedByNavigations)
                .HasForeignKey(d => d.ChangedBy)
                .HasConstraintName("FK_Promotion_ChangedBy");

            entity.HasOne(d => d.CreatedByNavigation).WithMany(p => p.PromotionCreatedByNavigations)
                .HasForeignKey(d => d.CreatedBy)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Promotion_CreatedBy");

            entity.HasOne(d => d.DeletedByNavigation).WithMany(p => p.PromotionDeletedByNavigations)
                .HasForeignKey(d => d.DeletedBy)
                .HasConstraintName("FK_Promotion_DeletedBy");

            entity.HasOne(d => d.Employee).WithMany(p => p.Promotions)
                .HasForeignKey(d => d.EmployeeId)
                .HasConstraintName("FK_Promotion_Employee_Id");
        });

        modelBuilder.Entity<Review>(entity =>
        {
            entity.HasKey(e => e.ReviewId).HasName("PK__Review__F85DA78B98928E47");

            entity.ToTable("Review");

            entity.Property(e => e.ReviewId).HasColumnName("Review_Id");
            entity.Property(e => e.Comments).IsUnicode(false);
            entity.Property(e => e.EmployeeId).HasColumnName("Employee_Id");
            entity.Property(e => e.PerformanceRating)
                .HasColumnType("decimal(18, 0)")
                .HasColumnName("Performance_Rating");
            entity.Property(e => e.ReviewDate).HasColumnName("Review_Date");
            entity.Property(e => e.ReviewedBy).HasColumnName("Reviewed_By");
            entity.Property(e => e.TeamId).HasColumnName("Team_Id");

            entity.HasOne(d => d.Employee).WithMany(p => p.ReviewEmployees)
                .HasForeignKey(d => d.EmployeeId)
                .HasConstraintName("FK_Review_Employee_Id");

            entity.HasOne(d => d.ReviewedByNavigation).WithMany(p => p.ReviewReviewedByNavigations)
                .HasForeignKey(d => d.ReviewedBy)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Review_Reviewed_By");

            entity.HasOne(d => d.Team).WithMany(p => p.Reviews)
                .HasForeignKey(d => d.TeamId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Review_Team_Id");
        });

        modelBuilder.Entity<Role>(entity =>
        {
            entity.HasKey(e => e.RoleId).HasName("PK__Role__D80AB4BBD15576B4");

            entity.ToTable("Role");

            entity.HasIndex(e => e.RoleName, "UQ_Role_Role_Name").IsUnique();

            entity.Property(e => e.RoleId).HasColumnName("Role_Id");
            entity.Property(e => e.RoleDescription)
                .IsUnicode(false)
                .HasColumnName("Role_Description");
            entity.Property(e => e.RoleName)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("Role_Name");
        });

        modelBuilder.Entity<Salary>(entity =>
        {
            entity.HasKey(e => e.SalaryId).HasName("PK__Salary__D64E0E04E48102A2");

            entity.ToTable("Salary");

            entity.Property(e => e.SalaryId).HasColumnName("Salary_Id");
            entity.Property(e => e.BaseSalary)
                .HasColumnType("decimal(18, 0)")
                .HasColumnName("Base_Salary");
            entity.Property(e => e.CreatedOn).HasDefaultValueSql("(getdate())");
            entity.Property(e => e.Deductions).HasColumnType("decimal(18, 0)");
            entity.Property(e => e.EmployeeId).HasColumnName("Employee_Id");
            entity.Property(e => e.PayDate).HasColumnName("Pay_Date");
            entity.Property(e => e.TotalSalary)
                .HasColumnType("decimal(18, 0)")
                .HasColumnName("Total_Salary");

            entity.HasOne(d => d.ChangedByNavigation).WithMany(p => p.SalaryChangedByNavigations)
                .HasForeignKey(d => d.ChangedBy)
                .HasConstraintName("FK_Salary_ChangedBy");

            entity.HasOne(d => d.CreatedByNavigation).WithMany(p => p.SalaryCreatedByNavigations)
                .HasForeignKey(d => d.CreatedBy)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Salary_CreatedBy");

            entity.HasOne(d => d.DeletedByNavigation).WithMany(p => p.SalaryDeletedByNavigations)
                .HasForeignKey(d => d.DeletedBy)
                .HasConstraintName("FK_Salary_DeletedBy");

            entity.HasOne(d => d.Employee).WithMany(p => p.Salaries)
                .HasForeignKey(d => d.EmployeeId)
                .HasConstraintName("FK_Salary_Employee_Id");
        });

        modelBuilder.Entity<Status>(entity =>
        {
            entity.HasKey(e => e.StatusId).HasName("PK__Status__5190094C39ACCD94");

            entity.ToTable("Status");

            entity.HasIndex(e => e.StatusName, "UQ_Status_Status_Name").IsUnique();

            entity.Property(e => e.StatusId).HasColumnName("Status_Id");
            entity.Property(e => e.StatusName)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("Status_Name");
        });

        modelBuilder.Entity<Team>(entity =>
        {
            entity.HasKey(e => e.TeamId).HasName("PK__Teams__02215C6AD3E70A03");

            entity.HasIndex(e => e.TeamName, "UQ_Teams_Team_Name").IsUnique();

            entity.Property(e => e.TeamId).HasColumnName("Team_Id");
            entity.Property(e => e.CreatedOn).HasDefaultValueSql("(getdate())");
            entity.Property(e => e.DepartmentId).HasColumnName("Department_Id");
            entity.Property(e => e.Description).IsUnicode(false);
            entity.Property(e => e.TeamLeadEmployeeId).HasColumnName("team_lead_employee_id");
            entity.Property(e => e.TeamName)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("Team_Name");
            entity.Property(e => e.TeamRoleId).HasColumnName("Team_Role_Id");

            entity.HasOne(d => d.ChangedByNavigation).WithMany(p => p.TeamChangedByNavigations)
                .HasForeignKey(d => d.ChangedBy)
                .HasConstraintName("FK_Teams_ChangedBy");

            entity.HasOne(d => d.CreatedByNavigation).WithMany(p => p.TeamCreatedByNavigations)
                .HasForeignKey(d => d.CreatedBy)
                .HasConstraintName("FK_Teams_CreatedBy");

            entity.HasOne(d => d.DeletedByNavigation).WithMany(p => p.TeamDeletedByNavigations)
                .HasForeignKey(d => d.DeletedBy)
                .HasConstraintName("FK_Teams_DeletedBy");

            entity.HasOne(d => d.Department).WithMany(p => p.Teams)
                .HasForeignKey(d => d.DepartmentId)
                .HasConstraintName("FK_Teams_Department_Id");

            entity.HasOne(d => d.TeamLeadEmployee).WithMany(p => p.Teams)
                .HasForeignKey(d => d.TeamLeadEmployeeId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Teams_team_lead_employee_id");

            entity.HasOne(d => d.TeamRole).WithMany(p => p.Teams)
                .HasForeignKey(d => d.TeamRoleId)
                .HasConstraintName("FK_Teams_Team_Role_Id");
        });

        modelBuilder.Entity<TeamRole>(entity =>
        {
            entity.HasKey(e => e.TeamRoleId).HasName("PK__Team_Rol__3D0DA8339FF16DB4");

            entity.ToTable("Team_Roles");

            entity.HasIndex(e => e.RoleName, "UQ_Team_Roles_Role_Name").IsUnique();

            entity.Property(e => e.TeamRoleId).HasColumnName("Team_Role_Id");
            entity.Property(e => e.RoleDescription)
                .IsUnicode(false)
                .HasColumnName("Role_Description");
            entity.Property(e => e.RoleName)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("Role_Name");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
