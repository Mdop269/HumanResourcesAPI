using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HumanResources.DataServices.Abstract;
using HumanResources.DataServices.Services;
using HumanResources.Manager.DTOs;
using HumanResources.Manager.DTOs.HR;
using HumanResources.Manager.Validation.Employee;
using HumanResources.Manager.Validation.HR;
using HumanResources.Manager.Validation.TeamRoles;

namespace HumanResources.Manager.Managers
{
    public class TeamsManager
    {
        //this is better approach for getting thw date rather then new date() in this if the system date doesnt update it will be a prob
        private readonly DateOnly currentDate = DateOnly.FromDateTime(DateTime.Now);

        private readonly IHRDataServices _hrDataServices;
        private readonly ITeamRolesDataServices _teamRolesDataServices;
        private readonly IDepartmentDataServices _departmentDataServices;
        private readonly ITeamsDataServices _teamsDataServices;
        private readonly IEmployeeDataServices _employeeDataServices;


        public TeamsManager(
            IHRDataServices hrDataServices, 
            ITeamRolesDataServices teamRolesDataServices,
            IDepartmentDataServices departmentDataServices,
            ITeamsDataServices teamsDataServices,
            IEmployeeDataServices employeeDataServices

            )
        {
            _hrDataServices = hrDataServices;
            _teamRolesDataServices = teamRolesDataServices;
            _departmentDataServices = departmentDataServices;
            _teamsDataServices = teamsDataServices;
            _employeeDataServices = employeeDataServices;
        }

        public async Task<List<TeamsDTO>> GetAllTeam()
        {
            var result = await _teamsDataServices.GetAllTeam();

            return result.Select(x => TeamsDTO.MapToDTO(x)).ToList();
        }

        public async Task<TeamsDTO> GetTeamById(int teamID)
        {
            var validation = new Dictionary<string, string>();

            if (teamID <= 0)
                validation.Add("teamID", "Invalid teamID");

            if (validation.Count == 0)
            {

                var result = await _teamsDataServices.GetTeamById(teamID);
                if (result is null)
                {
                    throw new TeamRolesValidationException($"Teams with ID {teamID} does not exist.");
                }
                return TeamsDTO.MapToDTO(result);

            }
            else
            {
                throw new TeamRolesValidationException("Validation failed: " + string.Join(", ", validation.Select(kv => $"{kv.Key}: {kv.Value}")));
            }
        }

        public async Task<String> UpsertTeams(TeamsDTO teamsDTO)
        {
            var validation = new Dictionary<string, string>();

            if (teamsDTO is null)
                validation.Add("Null", "Teams Cant Be Null");

            if (string.IsNullOrWhiteSpace(teamsDTO.TeamName))
            {
                validation.Add("EmptyTeamName", "Team Name Cant Be Null or WhiteSpace");

            }
            else if (string.IsNullOrWhiteSpace(teamsDTO.Description) || teamsDTO.Description.Length <= 15)
            {
                validation.Add("Description", "Description Cant Be Null or Less then 15 Length");
            }
            else if (teamsDTO.DepartmentId <= 0 || await _departmentDataServices.GetDepartmentById(teamsDTO.DepartmentId) == null)
            {
                validation.Add("WrongDepartmentId", "DepartmentId is Invalid");
            }
            else if (teamsDTO.TeamRoleId <= 0 || await _teamRolesDataServices.GetTeamRoleById(teamsDTO.TeamRoleId) == null)
            {
                validation.Add("WrongTeamRoleId", "TeamRoleId is Invalid");
            }
            else if (teamsDTO.TeamLeadEmployeeId <= 0 || await _employeeDataServices.GetEmployeeById(teamsDTO.TeamLeadEmployeeId) == null)
            {
                validation.Add("WrongTeamLeadEmployeeId", "Team Lead Employee Id is Invalid");
            }
            else if (teamsDTO.CreatedBy >= 1 && await _hrDataServices.GetHrById((int)teamsDTO.CreatedBy) == null)
            {
                validation.Add("WrongCreatedBY", "Created By ID is Invalid");
            }
            else if (teamsDTO.ChangedBy >= 1 && await _hrDataServices.GetHrById((int)teamsDTO.ChangedBy) == null)
            {
                validation.Add("WrongChangedBY", "Changed By ID is Invalid");
            }


            if (validation.Count == 0)
            {
                var teamsEntity = TeamsDTO.MapToEntity(teamsDTO);

                var checkingData = await _teamsDataServices.GetTeamByUnique(teamsEntity);
                if (checkingData is not null)
                {
                    if (checkingData.DeletedBy is not null || checkingData.DeletedOn is not null)
                    {
                        checkingData.Description = teamsDTO.Description;
                        checkingData.DepartmentId = teamsDTO.DepartmentId;
                        checkingData.TeamLeadEmployeeId = teamsDTO.TeamLeadEmployeeId;
                        checkingData.TeamRoleId = teamsDTO.TeamRoleId;
                        checkingData.CreatedBy = teamsDTO.CreatedBy;
                        checkingData.CreatedOn = currentDate;
                        checkingData.ChangedBy = null;
                        checkingData.ChangedOn = null;
                        checkingData.DeletedBy = null;
                        checkingData.DeletedOn = null;

                        var updatedDeletedData = await _teamsDataServices.UpdateTeam(teamsEntity);
                        return "UpdatedDeletedData";

                    }
                    else
                    {
                        if (teamsEntity.ChangedBy <= 0)
                        {
                            validation.Add("ChangedByCantbeEmpty", "Changed by is not Present");
                        }

                        if (validation.Count == 0)
                        {
                            checkingData.Description = teamsDTO.Description;
                            checkingData.DepartmentId = teamsDTO.DepartmentId;
                            checkingData.TeamLeadEmployeeId = teamsDTO.TeamLeadEmployeeId;
                            checkingData.TeamRoleId = teamsDTO.TeamRoleId;
                            checkingData.ChangedBy = teamsDTO.ChangedBy;
                            checkingData.ChangedOn = currentDate;

                            var updatedData = await _teamsDataServices.UpdateTeam(teamsEntity);
                            return "Updated";
                        }
                        else
                        {
                            throw new TeamRolesValidationException("Validation failed: " + string.Join(", ", validation.Select(kv => $"{kv.Key}: {kv.Value}")));
                        }
                    }
                }
                else
                {
                    if (teamsEntity.CreatedBy <= 0)
                    {
                        validation.Add("CreatedOnCantByEmpty", "Created By Cant Be null");
                    }

                    if (validation.Count == 0)
                    {
                        teamsEntity.CreatedOn = currentDate;
                        teamsEntity.ChangedBy = null;

                        var addData = await _teamsDataServices.AddTeam(teamsEntity);

                        return "Added";
                    }
                    else
                    {
                        throw new TeamRolesValidationException("Validation failed: " + string.Join(", ", validation.Select(kv => $"{kv.Key}: {kv.Value}")));
                    }

                }
            }
            else
            {
                throw new TeamRolesValidationException("Validation failed: " + string.Join(", ", validation.Select(kv => $"{kv.Key}: {kv.Value}")));

            }
        }

        public async Task<bool> DeleteTeamWithSoftDelete(int teamID, int deletedBy)
        {
            var validation = new Dictionary<string, string>();

            if (teamID <= 0 || deletedBy <= 0)
            {
                validation.Add("Enter Valid Details", " teamID Or DeletedBy Is Invalid");
            }
            else if (await _hrDataServices.GetHrById(deletedBy) == null)
            {
                validation.Add("Hr Invalid", $" HR with {deletedBy} is Invalid");
            }
            else if (await _teamsDataServices.GetTeamById(teamID) == null)
            {
                validation.Add("Team Invalid", $" Team with {teamID} is Invalid");
            }
            if (validation.Count == 0)
            {
                var TeamData = await _teamsDataServices.GetTeamById(teamID);

                TeamData.DeletedBy = deletedBy;
                TeamData.DeletedOn = currentDate;

                var result = await _teamsDataServices.UpdateTeam(TeamData);

                return true;
            }
            else
            {
                throw new TeamRolesValidationException("Validation failed: " + string.Join(", ", validation.Select(kv => $"{kv.Key}: {kv.Value}")));

            }
        }
    }
}
