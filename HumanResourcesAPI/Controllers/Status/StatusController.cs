using HumanResources.Manager.DTOs;
using HumanResources.Manager.Managers;
using HumanResources.Manager.Validation.Status;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HumanResourcesAPI.Controllers.Status
{
    [Route("api/[controller]")]
    [ApiController]
    public class StatusController : ControllerBase
    {
        private readonly StatusManager _statusManager;

        public StatusController(StatusManager statusManager)
        {
            _statusManager = statusManager;
        }

        [HttpGet]
        public async Task<ActionResult<List<StatusDTO>>> GetAllStatus()
        {
            try
            {
                var result = await _statusManager.GetAllStatus();

                return result;
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occured while processing the request {ex.Message}");
            }
        }

        [HttpGet("{statusID}")]
        public async Task<ActionResult<StatusDTO>> GetStatusById(int statusID)
        {
            try
            {
                var result = await _statusManager.GetStatusById(statusID);
                return Ok(result);
            }
            catch (StatusValidationException ex)
            {
                return BadRequest(new { Error = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occured while processing the request {ex.Message}");
            }
        }

        [HttpPost("upsert")]
        public async Task<ObjectResult> UpsertStatus(StatusDTO statusDTO)
        {
            try
            {
                var result = await _statusManager.UpsertStatus(statusDTO);
                return Ok(new { Success = $"Database Has Been {result} ." });
            }
            catch (StatusValidationException ex)
            {
                return BadRequest(new { Error = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occured while processing the request {ex.Message}");
            }
        }

        [HttpPost("upsert/SP")]
        public async Task<ObjectResult> UpsertStatusSP(StatusDTO statusDTO)
        {
            try
            {
                var result = await _statusManager.UpsertStatusSP(statusDTO);
                return Ok(new { Success = $"Database Has Been {result} ." });
            }
            catch (StatusValidationException ex)
            {
                return BadRequest(new { Error = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occured while processing the request {ex.Message}");
            }
        }

        [HttpDelete("{statusID}")]
        public async Task<ActionResult> DeleteStatus(int statusID)
        {
            try
            {
                var result = await _statusManager.DeleteStatus(statusID);
                return Ok(new { Success = result });
            }
            catch (StatusValidationException ex)
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
