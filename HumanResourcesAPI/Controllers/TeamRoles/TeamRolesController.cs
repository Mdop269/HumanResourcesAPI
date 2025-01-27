using HumanResources.Manager.DTOs;
using HumanResources.Manager.Managers;
using HumanResources.Manager.Validation.TeamRoles;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HumanResourcesAPI.Controllers.TeamRoles
{
    [Route("api/[controller]")]
    [ApiController]
    public class TeamRolesController : ControllerBase
    {
        private readonly TeamRolesManager _teamRolemanager;

        public TeamRolesController(TeamRolesManager teamRolemanager)
        {
            _teamRolemanager = teamRolemanager;
        }

        [HttpGet]
        public async Task<ActionResult<List<TeamRolesDTO>>> GetAllTeamRole()
        {
            try
            {
                var result = await _teamRolemanager.GetAllTeamRole();

                return result;
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occured while processing the request {ex.Message}");
            }
        }

        [HttpGet("{teamRoleID}")]
        public async Task<ActionResult<TeamRolesDTO>> GetTeamRoleById(int teamRoleID)
        {
            try
            {
                var result = await _teamRolemanager.GetTeamRoleById(teamRoleID);
                return Ok(result);
            }
            catch (TeamRolesValidationException ex)
            {
                return BadRequest(new { Error = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occured while processing the request {ex.Message}");
            }
        }

        [HttpPost("upsert")]
        public async Task<ObjectResult> UpsertTeamRole(TeamRolesDTO TeamRolesDTO)
        {
            try
            {
                var result = await _teamRolemanager.UpsertTeamRole(TeamRolesDTO);
                return Ok(new { Success = $"Database Has Been {result} ." });
            }
            catch (TeamRolesValidationException ex)
            {
                return BadRequest(new { Error = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occured while processing the request {ex.Message}");
            }

        }

        [HttpDelete]
        public async Task<ActionResult> DeleteTeamRole(int teamRoleID)
        {
            try
            {
                var result = await _teamRolemanager.DeleteTeamRole(teamRoleID);
                return Ok(new { Success = result });
            }
            catch (TeamRolesValidationException ex)
            {
                return BadRequest(new { Error = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occured while processing the request {ex.Message}");
            }
        }
    }
}
