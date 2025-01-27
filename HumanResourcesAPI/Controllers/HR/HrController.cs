using HumanResources.Manager.DTOs.HR;
using HumanResources.Manager.DTOs.Permissions;
using HumanResources.Manager.Managers;
using HumanResources.Manager.Managers.HR;
using HumanResources.Manager.Validation.HR;
using HumanResources.Manager.Validation.Permissions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HumanResourcesAPI.Controllers.HR
{
    [Route("api/[controller]")]
    [ApiController]
    public class HrController : ControllerBase
    {
        private readonly HrManager _hrManager;

        public HrController(HrManager hrManager)
        {
            _hrManager = hrManager;
        }

        [HttpGet]
        public async Task<ActionResult<List<HrDTO>>> GetAllHr()
        {
            try
            {
                var result = await _hrManager.GetAllHr();

                return result;
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occured while processing the request {ex.Message}");
            }
        }

        [HttpGet("{HrId}")]
        public async Task<ActionResult<HrDTO>> GetHrById(int HrId)
        {
            try
            {
                var result = await _hrManager.GetHrById(HrId);
                return Ok(result);
            }
            catch (HrValidationException ex)
            {
                return BadRequest(new { Error = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occured while processing the request {ex.Message}");
            }
        }

        [HttpPost("upsert")]
        public async Task<ObjectResult> UpsertHR(HrDTO hrDTO)
        {
            try
            {
                var result = await _hrManager.UpsertHR(hrDTO);
                return Ok(new { Success = $"Database Has Been {result} ." });
            }
            catch (HrValidationException ex)
            {
                return BadRequest(new { Error = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occured while processing the request {ex.Message}");
            }
        }

        [HttpDelete("{HrId}/{deletedBy}")]
        public async Task<ActionResult> DeleteHrWithSoftDelete(int HrId, string deletedBy)
        {
            try
            {
                var result = await _hrManager.DeleteHrWithSoftDelete(HrId, deletedBy);
                return Ok(new { Success = result });
            }
            catch (HrValidationException ex)
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
