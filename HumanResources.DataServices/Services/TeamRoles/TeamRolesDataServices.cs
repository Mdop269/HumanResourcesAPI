using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HumanResourcesAPI.EntityData.EntityModels;
using HumanResourcesAPI.EntityData;
using HumanResources.DataServices.Abstract;
using Microsoft.EntityFrameworkCore;


namespace HumanResources.DataServices.Services
{
    public class TeamRolesDataServices : ITeamRolesDataServices
    {
        private readonly HumanResourcesContext _context;

        public TeamRolesDataServices(HumanResourcesContext context)
        {
            _context = context;
        }

        public async Task<List<TeamRole>> GetAllTeamRole()
        {
            var result = await _context.TeamRoles.ToListAsync();

            return result;
        }

        public async Task<TeamRole> GetTeamRoleById(int TeamRoleId)
        {
            var teamRole = await _context.TeamRoles.FindAsync(TeamRoleId);

            return teamRole;
        }

        public async Task<TeamRole> AddTeamRole(TeamRole teamRole)
        {
            await _context.TeamRoles.AddAsync(teamRole);

            await _context.SaveChangesAsync();

            return teamRole;
        }

        public async Task<TeamRole> UpdateTeamRole(TeamRole teamRole)
        {
            var existingTeamRole = await _context.TeamRoles.FirstOrDefaultAsync(a => a.RoleName == teamRole.RoleName);
            existingTeamRole.RoleName = teamRole.RoleName;
            existingTeamRole.RoleDescription = teamRole.RoleDescription;
            await _context.SaveChangesAsync();
            return teamRole;
        }


        public async Task<bool> DeleteTeamRole(int TeamRoleId)
        {
            var existingTeamRole = await _context.TeamRoles.FindAsync(TeamRoleId);
            _context.TeamRoles.Remove(existingTeamRole);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<TeamRole> GetTeamRoleByUnique(TeamRole teamRole)
        {
            var existingTeamRole = await _context.TeamRoles.FirstOrDefaultAsync(a => a.RoleName == teamRole.RoleName);

            return existingTeamRole;
        }
    }
}
