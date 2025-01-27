using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HumanResources.Manager.DTOs.HR;
using HumanResourcesAPI.EntityData.EntityModels;

namespace HumanResources.Manager.DTOs
{
    public class EmployeeDTO
    {
        [Required, Length(4,49)]
        public string FirstName { get; set; } = null!;

        [Required, Length(4, 49)]
        public string LastName { get; set; } = null!;

        [EmailAddress]
        public string Email { get; set; } = null!;

        [Phone]
        public string Phone { get; set; } = null!;

        [Required]
        public int StatusId { get; set; }

        [Required]
        public int DepartmentId { get; set; }

        [Required]
        public int RoleId { get; set; }

        [Required]
        public int PermissionsId { get; set; }

        public int? TeamId { get; set; }

        public int? CreatedBy { get; set; }

        public int? ChangedBy { get; set; }

        public static Employee MapToEntity(EmployeeDTO employeeDTO)
        {
            return new Employee
            {
                FirstName = employeeDTO.FirstName,
                LastName = employeeDTO.LastName,
                Email = employeeDTO.Email,
                Phone = employeeDTO.Phone,
                StatusId = employeeDTO.StatusId,
                DepartmentId = employeeDTO.DepartmentId,
                RoleId = employeeDTO.RoleId,
                PermissionsId = employeeDTO.PermissionsId,
                TeamId = employeeDTO.TeamId,
                CreatedBy = employeeDTO.CreatedBy,
                ChangedBy = employeeDTO.ChangedBy,
            };
        }

        public static EmployeeDTO MapToDTO(Employee employee) => new EmployeeDTO
        { 

            FirstName = employee.FirstName,
            LastName = employee.LastName,
            Email = employee.Email,
            Phone = employee.Phone,
            StatusId = employee.StatusId,
            DepartmentId = employee.DepartmentId,
            RoleId = employee.RoleId,
            PermissionsId = employee.PermissionsId,
            TeamId = employee.TeamId,
            CreatedBy = employee.CreatedBy,
            ChangedBy = employee.ChangedBy,
        };

    }
}
