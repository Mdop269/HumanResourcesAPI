using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HumanResourcesAPI.EntityData.EntityModels;

namespace HumanResources.Manager.DTOs
{ 
    public class SalaryDTO
    {
        [Required] 
        public int EmployeeId { get; set; }

        [Required]
        public decimal BaseSalary { get; set; }

        [Required]
        public decimal Deductions { get; set; }

        public decimal TotalSalary { get; set; }

        [Required, DataType(DataType.Date)]
        public DateOnly PayDate { get; set; }

        public int CreatedBy { get; set; }

        public int? ChangedBy { get; set; }

        public static Salary MapToEntity(SalaryDTO salaryDTO)
        {
            return new Salary
            {
                EmployeeId = salaryDTO.EmployeeId,
                BaseSalary = salaryDTO.BaseSalary,
                Deductions = salaryDTO.Deductions,
                PayDate = salaryDTO.PayDate,
                CreatedBy = salaryDTO.CreatedBy,
                ChangedBy = salaryDTO.ChangedBy,
            };
        }

        public static SalaryDTO MapToDTO(Salary salary) => new SalaryDTO
        {

            EmployeeId = salary.EmployeeId,
            BaseSalary = salary.BaseSalary,
            Deductions = salary.Deductions,
            TotalSalary = salary.TotalSalary,
            PayDate = salary.PayDate,
            CreatedBy = salary.CreatedBy,
            ChangedBy = salary.ChangedBy,
        };
    }
}
