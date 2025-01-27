using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HumanResourcesAPI.EntityData.EntityModels;

namespace HumanResources.DataServices.Abstract
{
    public interface ITeamsDataServices
    {
        Task<List<Team>> GetAllTeam();

        Task<Team> GetTeamById(int teamID);

        Task<Team> AddTeam(Team team);

        Task<Team> UpdateTeam(Team team);

        Task<bool> DeleteTeam(Team team);

        Task<Team> GetTeamByUnique(Team team);

        Task<Team> GetDeletedTeam(int teamID);

    }
}
