using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HumanResources.Manager.DTOs.HR;
using HumanResourcesAPI.EntityData.EntityModels;

namespace HumanResources.Manager.DTOs
{
    public class TeamsDTO
    {
        public string TeamName { get; set; } = null!;

        public string Description { get; set; } = null!;

        public int DepartmentId { get; set; }

        public int TeamLeadEmployeeId { get; set; }

        public int TeamRoleId { get; set; }

        public int? CreatedBy { get; set; }

        public int? ChangedBy { get; set; }

        public static Team MapToEntity(TeamsDTO teamsDTO)
        {
            return new Team
            {
                TeamName = teamsDTO.TeamName,
                Description = teamsDTO.Description,
                DepartmentId = teamsDTO.DepartmentId,
                TeamLeadEmployeeId = teamsDTO.TeamLeadEmployeeId,
                TeamRoleId = teamsDTO.TeamRoleId,
                CreatedBy = teamsDTO.CreatedBy,
                ChangedBy = teamsDTO.ChangedBy,
            };
        }

        public static TeamsDTO MapToDTO(Team team) => new TeamsDTO
        {

            TeamName = team.TeamName,
            Description = team.Description,
            DepartmentId = team.DepartmentId,
            TeamLeadEmployeeId = team.TeamLeadEmployeeId,
            TeamRoleId = team.TeamRoleId,
            CreatedBy = team.CreatedBy,
            ChangedBy = team.ChangedBy,
        };

    }
}
