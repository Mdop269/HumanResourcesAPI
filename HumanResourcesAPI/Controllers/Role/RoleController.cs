using HumanResources.Manager.DTOs;
using HumanResources.Manager.Managers;
using HumanResources.Manager.Validation.Role;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HumanResourcesAPI.Controllers.Role
{
    [Route("api/[controller]")]
    [ApiController]
    public class RoleController : ControllerBase
    {
        private readonly RoleManager _roleManager;
        public RoleController(RoleManager roleManager)
        {
            _roleManager = roleManager;
        }

        [HttpGet]
        public async Task<ActionResult<List<RoleDTO>>> GetAllRole()
        {
            try
            {
                var result = await _roleManager.GetAllRole();

                return result;
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occured while processing the request {ex.Message}");
            }
        }

        [HttpGet("{RoleId}")]
        public async Task<ActionResult<RoleDTO>> GetRoleById(int RoleId)
        {
            try
            {
                var result = await _roleManager.GetRoleById(RoleId);
                return Ok(result);
            }
            catch (RoleValidationException ex)
            {
                return BadRequest(new { Error = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occured while processing the request {ex.Message}");
            }
        }

        [HttpPost("upsert")]
        public async Task<ObjectResult> UpsertRole(RoleDTO roleDTO)
        {
            try
            {
                var result = await _roleManager.UpsertRole(roleDTO);
                return Ok(new { Success = $"Database Has Been {result} ." });
            }
            catch (RoleValidationException ex)
            {
                return BadRequest(new { Error = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occured while processing the request {ex.Message}");
            }
        }

        [HttpDelete]
        public async Task<ActionResult> DeleteRole(int RoleId)
        {
            try
            {
                var result = await _roleManager.DeleteRole(RoleId);
                return Ok(new { Success = result });
            }
            catch (RoleValidationException ex)
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
