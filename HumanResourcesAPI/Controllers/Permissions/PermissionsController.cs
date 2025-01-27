using HumanResources.Manager.DTOs.Permissions;
using HumanResources.Manager.Managers;
using HumanResources.Manager.Validation.Permissions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HumanResourcesAPI.Controllers.Permissions
{
    [Route("api/[controller]")]
    [ApiController]
    public class PermissionsController : ControllerBase
    {
        private readonly PermissionsManager _permissionsManager;

        public PermissionsController(PermissionsManager permissionsManager)
        {
            _permissionsManager = permissionsManager;
        }

        [HttpGet]
        public async Task<ActionResult<List<PermissionsDTO>>> GetAllPermission()
        {
            try
            {
                var result = await _permissionsManager.GetAllPermission();

                return result;
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occured while processing the request {ex.Message}");
            }
        }

        [HttpGet("{PermID}")]
        public async Task<ActionResult<PermissionsDTO>> GetPermissionById(int PermID)
        {
            try
            {
                var result = await _permissionsManager.GetPermissionById(PermID);
                return Ok(result);
            }
            catch (PermissionsValidationException ex)
            {
                return BadRequest(new { Error = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occured while processing the request {ex.Message}");
            }
        }

        [HttpPost("upsert")]
        public async Task<ObjectResult> UpsertPermission(PermissionsDTO permissionsDTO)
        {
            try
            {
                var result = await _permissionsManager.UpsertPermission(permissionsDTO);
                return Ok(new { Success = $"Database Has Been {result} ." });
            }
            catch (PermissionsValidationException ex)
            {
                return BadRequest(new { Error = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occured while processing the request {ex.Message}");
            }

        }

        [HttpDelete]
        public async Task<ActionResult> DeletePermission(int PermID)
        {
            try
            {
                var result = await _permissionsManager.DeletePermission(PermID);
                return Ok(new { Success = result });
            }
            catch (PermissionsValidationException ex)
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
