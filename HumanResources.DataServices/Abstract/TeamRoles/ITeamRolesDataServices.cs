using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HumanResourcesAPI.EntityData.EntityModels;

namespace HumanResources.DataServices.Abstract
{
    public interface ITeamRolesDataServices
    {
        Task<List<TeamRole>> GetAllTeamRole();

        Task<TeamRole> GetTeamRoleById(int TeamRoleId);

        Task<TeamRole> AddTeamRole(TeamRole teamRole);

        Task<TeamRole> UpdateTeamRole(TeamRole teamRole);

        Task<bool> DeleteTeamRole(int TeamRoleId);

        Task<TeamRole> GetTeamRoleByUnique(TeamRole teamRole);
    }
}
