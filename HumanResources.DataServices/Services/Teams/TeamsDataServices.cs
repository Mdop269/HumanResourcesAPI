using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HumanResources.DataServices.Abstract;
using HumanResourcesAPI.EntityData.EntityModels;
using HumanResourcesAPI.EntityData;
using Microsoft.EntityFrameworkCore;

namespace HumanResources.DataServices.Services
{
    public class TeamsDataServices : ITeamsDataServices
    {
        private readonly HumanResourcesContext _context;

        public TeamsDataServices(HumanResourcesContext context)
        {
            _context = context;
        }

        public async Task<List<Team>> GetAllTeam()
        {
            var result = await _context.Teams.Where(a => a.DeletedBy == null).ToListAsync();

            return result;
        }

        public async Task<Team> GetTeamById(int teamID)
        {
            var team = await _context.Teams.FirstOrDefaultAsync(a => a.TeamId == teamID && a.DeletedBy == null);

            return team;
        }

        public async Task<Team> AddTeam(Team team)
        {
            await _context.Teams.AddAsync(team);

            await _context.SaveChangesAsync();

            return team;
        }

        public async Task<Team> UpdateTeam(Team team)
        {
            _context.SaveChanges();
            return team;
        }


        public async Task<bool> DeleteTeam(Team team)
        {
            _context.Teams.Remove(team);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<Team> GetTeamByUnique(Team team)
        {
            var existingTeam = await _context.Teams.FirstOrDefaultAsync(a => a.TeamName == team.TeamName);

            return existingTeam;
        }

        public async Task<Team> GetDeletedTeam(int teamID)
        {
            var deletedTeam = await _context.Teams.FirstOrDefaultAsync(a => a.TeamId == teamID && a.DeletedBy != null);

            return deletedTeam;

        }
    }

}
