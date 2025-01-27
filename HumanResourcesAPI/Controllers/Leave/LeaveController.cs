using HumanResources.Manager.DTOs;
using HumanResources.Manager.Managers;
using HumanResources.Manager.Validation.Leave;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HumanResourcesAPI.Controllers.Leave
{
    [Route("api/[controller]")]
    [ApiController]
    public class LeaveController : ControllerBase
    {
        private readonly LeaveManager _leaveManager;

        public LeaveController(LeaveManager leaveManager)
        {
            _leaveManager = leaveManager;
        }

        [HttpGet]
        public async Task<ActionResult<List<LeaveDTO>>> GetAllLeave()
        {
            try
            {
                var result = await _leaveManager.GetAllLeave();

                return result;
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occured while processing the request {ex.Message}");
            }
        }

        [HttpGet("{leaveID}")]
        public async Task<ActionResult<EmployeeDTO>> GetLeaveById(int leaveID)
        {
            try
            {
                var result = await _leaveManager.GetLeaveById(leaveID);
                return Ok(result);
            }
            catch (LeaveValidationException ex)
            {
                return BadRequest(new { Error = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occured while processing the request {ex.Message}");
            }
        }

        [HttpPost("upsert")]
        public async Task<ObjectResult> UpsertLeave(LeaveDTO leaveDTO)
        {
            try
            {
                var result = await _leaveManager.UpsertLeave(leaveDTO);
                return Ok(new { Success = $"Database Has Been {result} ." });
            }
            catch (LeaveValidationException ex)
            {
                return BadRequest(new { Error = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occured while processing the request {ex.Message}");
            }
            }

        [HttpDelete("{leaveID}/{deletedBy}")]
        public async Task<ActionResult> DeleteLeaveWithSoftDelete(int leaveID, int deletedBy)
        {
            try
            {
                var result = await _leaveManager.DeleteLeaveWithSoftDelete(leaveID, deletedBy);
                return Ok(new { Success = result });
            }
            catch (LeaveValidationException ex)
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
