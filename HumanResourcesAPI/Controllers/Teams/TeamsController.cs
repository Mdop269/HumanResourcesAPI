using HumanResources.Manager.DTOs;
using HumanResources.Manager.Managers;
using HumanResources.Manager.Validation.Employee;
using HumanResources.Manager.Validation.Teams;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HumanResourcesAPI.Controllers.Teams
{
    [Route("api/[controller]")]
    [ApiController]
    public class TeamsController : ControllerBase
    {
        private readonly TeamsManager _teamsManager;

        public TeamsController(TeamsManager teamsManager)
        {
            _teamsManager = teamsManager;
        }

        [HttpGet]
        public async Task<ActionResult<List<TeamsDTO>>> GetAllTeam()
        {
            try
            {
                var result = await _teamsManager.GetAllTeam();

                return result;
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occured while processing the request {ex.Message}");
            }
        }

        [HttpGet("{teamID}")]
        public async Task<ActionResult<EmployeeDTO>> GetTeamById(int teamID)
        {
            try
            {
                var result = await _teamsManager.GetTeamById(teamID);
                return Ok(result);
            }
            catch (TeamsValidationException ex)
            {
                return BadRequest(new { Error = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occured while processing the request {ex.Message}");
            }
        }

        [HttpPost("upsert")]
        public async Task<ActionResult<string>> UpsertTeams(TeamsDTO teamsDTO)
        {
            try
            {
                var result = await _teamsManager.UpsertTeams(teamsDTO);
                return Ok(new { Success = $"Database Has Been {result} ." });
            }
            catch (TeamsValidationException ex)
            {
                return BadRequest(new { Error = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occured while processing the request {ex.Message}");
            }
        }

        [HttpDelete("{teamID}/{deletedBy}")]
        public async Task<ActionResult> DeleteTeamWithSoftDelete(int teamID, int deletedBy)
        {
            try
            {
                var result = await _teamsManager.DeleteTeamWithSoftDelete(teamID, deletedBy);
                return Ok(new { Success = result });
            }
            catch (TeamsValidationException ex)
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
