using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HumanResourcesAPI.EntityData.EntityModels;

namespace HumanResources.DataServices.Abstract
{
    public interface IPermissionsDataServices
    {
        Task<List<Permission>> GetAllPermission();

        Task<Permission> GetPermissionById(int PermID);

        Task<Permission> AddPermission(Permission permission);

        Task<Permission> UpdatePermission(Permission permission);

        Task<bool> DeletePermission(int PermID);

        Task<Permission> GetPermissionByUnique(Permission permission);

    }
}
