using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HumanResources.DataServices.Abstract;
using HumanResources.Manager.DTOs;
using HumanResources.Manager.Validation.TeamRoles;

namespace HumanResources.Manager.Managers
{
    public class TeamRolesManager
    {
        private readonly ITeamRolesDataServices _TeamRoleservices;

        public TeamRolesManager(ITeamRolesDataServices teamRoleservices)
        {
            _TeamRoleservices = teamRoleservices;
        }

        public async Task<List<TeamRolesDTO>> GetAllTeamRole()
        {
            var result = await _TeamRoleservices.GetAllTeamRole();

            return result.Select(x => TeamRolesDTO.MapToDTO(x)).ToList();
        }

        public async Task<TeamRolesDTO> GetTeamRoleById(int teamRoleID)
        {
            var validation = new Dictionary<string, string>();

            if (teamRoleID <= 0)
                validation.Add("teamRoleID", "Invalid teamRoleID");

            if (validation.Count == 0)
            {

                var result = await _TeamRoleservices.GetTeamRoleById(teamRoleID);
                if (result is null)
                {
                    throw new TeamRolesValidationException($"TeamRole with ID {teamRoleID} does not exist.");
                }
                return TeamRolesDTO.MapToDTO(result);

            }
            else
            {
                throw new TeamRolesValidationException("Validation failed: " + string.Join(", ", validation.Select(kv => $"{kv.Key}: {kv.Value}")));
            }
        }

        public async Task<String> UpsertTeamRole(TeamRolesDTO TeamRolesDTO)
        {
            var validation = new Dictionary<string, string>();
            if (TeamRolesDTO is null)
                validation.Add("Null", "TeamRole Cant Be Null");

            if (string.IsNullOrWhiteSpace(TeamRolesDTO.RoleName))
            {
                validation.Add("EmptyTeamRoleName", "TeamRole Name Cant Be Null or WhiteSpace");

            }
            else if (string.IsNullOrWhiteSpace(TeamRolesDTO.RoleDescription))
            {
                validation.Add("EmptyTeamRoleDescription", "TeamRole Description Cant Be Null or WhiteSpace");
            }

            if (validation.Count == 0)
            {
                var teamRoleEntity = TeamRolesDTO.MapToEntity(TeamRolesDTO);

                var checkingData = await _TeamRoleservices.GetTeamRoleByUnique(teamRoleEntity);
                if (checkingData is not null)
                {
                    var updatedData = await _TeamRoleservices.UpdateTeamRole(teamRoleEntity);
                    return "Updated";
                }
                else
                {
                    var addData = await _TeamRoleservices.AddTeamRole(teamRoleEntity);
                    return "Added";
                }
            }
            else
            {
                throw new TeamRolesValidationException("Validation failed: " + string.Join(", ", validation.Select(kv => $"{kv.Key}: {kv.Value}")));

            }
        }

        public async Task<bool> DeleteTeamRole(int teamRoleID)
        {
            var validation = new Dictionary<string, string>();

            if (teamRoleID <= 0)
                validation.Add("teamRoleID", "Invalid teamRoleID");

            if (validation.Count == 0)
            {
                var checkingData = await _TeamRoleservices.GetTeamRoleById(teamRoleID);

                if (checkingData is null)
                {
                    throw new TeamRolesValidationException($"TeamRole with  ID {teamRoleID} does not exist.");
                }
                var result = await _TeamRoleservices.DeleteTeamRole(teamRoleID);
                return result;

            }
            else
            {
                throw new TeamRolesValidationException("Validation failed: " + string.Join(", ", validation.Select(kv => $"{kv.Key}: {kv.Value}")));
            }
        }
    }
}
