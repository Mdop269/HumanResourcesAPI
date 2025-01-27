using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HumanResourcesAPI.EntityData.EntityModels;

namespace HumanResources.Manager.DTOs.Permissions
{
    public class PermissionsDTO
    {
        [Required, Length(5, 49)]
        public string PermissionName { get; set; } = null!;

        [Required, Length(10, 500)]
        public string PermissionDescription { get; set; } = null!;

        public static Permission MapToEntity(PermissionsDTO permissionsDTO)
        {
            return new Permission
            {
                PermissionName = permissionsDTO.PermissionName,
                PermissionDescription = permissionsDTO.PermissionDescription,
            };
        }

        public static PermissionsDTO MapToDTO(Permission permission)
        {
            return new PermissionsDTO
            {
                PermissionName = permission.PermissionName,
                PermissionDescription = permission.PermissionDescription,
            };
        }
    }
}
