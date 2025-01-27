using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HumanResourcesAPI.EntityData.EntityModels;

namespace HumanResources.DataServices.Abstract
{
    public interface IRoleDataServices
    {
        Task<List<Role>> GetAllRole();

        Task<Role> GetRoleById(int RoleId);

        Task<bool> DeleteRole(int RoleId);

        Task<Role> GetRoleByUnique(Role role);

        Task<Role> UpdateRole(Role role);

        Task<Role> AddRole(Role role);
    }
}
