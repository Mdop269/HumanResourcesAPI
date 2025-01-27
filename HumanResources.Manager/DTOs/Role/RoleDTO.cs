using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HumanResourcesAPI.EntityData.EntityModels;

namespace HumanResources.Manager.DTOs
{ 
    public class RoleDTO
    {
        [Required, Length(5, 49)]
        public string RoleName { get; set; } = null!;

        [Required, Length(10, 500)]
        public string RoleDescription { get; set; } = null!;

        public static Role MapToEntity(RoleDTO roleDTO)
        {
            return new Role
            {
                RoleName = roleDTO.RoleName,
                RoleDescription = roleDTO.RoleDescription,
            };
        }

        public static RoleDTO MapToDTO(Role role)
        {
            return new RoleDTO
            {
                RoleName = role.RoleName,
                RoleDescription = role.RoleDescription,
            };
        }
    }
}
