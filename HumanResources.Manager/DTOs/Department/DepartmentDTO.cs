using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HumanResourcesAPI.EntityData.EntityModels;

namespace HumanResources.Manager.DTOs
{
    public class DepartmentDTO
    {
        [Required, Length(5, 49)]
        public string DepartmentName { get; set; }

        [Required, Length(10, 500)]
        public string Description { get; set; }

        //MapToEntity: Converts a DepartmentDTO object to an Department entity.
        public static Department MapToEntity(DepartmentDTO departmentDTO)
        {
            return new Department
            {
                DepartmentName = departmentDTO.DepartmentName,
                Description = departmentDTO.Description,
            };
        }

        //MapToDTO: Converts an Department entity to an DepartmentDTO object.
        public static DepartmentDTO MapToDTO(Department department) => new DepartmentDTO
        {

            DepartmentName = department.DepartmentName,
            Description = department.Description,
        };
    }
}
