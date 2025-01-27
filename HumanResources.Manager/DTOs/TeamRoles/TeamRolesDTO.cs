using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HumanResourcesAPI.EntityData.EntityModels;

namespace HumanResources.Manager.DTOs
{
    public class TeamRolesDTO
    {
        [Required, Length(5, 49)]
        public string RoleName { get; set; } = null!;

        [Required, Length(10, 500)]
        public string RoleDescription { get; set; } = null!;

        public static TeamRole MapToEntity(TeamRolesDTO teamRoleDTO)
        {
            return new TeamRole
            {
                RoleName = teamRoleDTO.RoleName,
                RoleDescription = teamRoleDTO.RoleDescription,
            };
        }

        public static TeamRolesDTO MapToDTO(TeamRole teamRole)
        {
            return new TeamRolesDTO
            {
                RoleName = teamRole.RoleName,
                RoleDescription = teamRole.RoleDescription,
            };
        }
    }
}
