using HumanResources.Manager.DTOs;
using HumanResources.Manager.Managers;
using HumanResources.Manager.Validation.Department;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HumanResourcesAPI.Controllers.Department
{
    [Route("api/[controller]")]
    [ApiController]
    public class DepartmentController : ControllerBase
    {
        private readonly DepartmentManager _manager;

        public DepartmentController(DepartmentManager manager)
        {
            _manager = manager;
        }

        [HttpGet]
        public async Task<ActionResult<List<DepartmentDTO>>> GetAllDepartments()
        {
            try
            {
                var result = await _manager.GetAllDepartments();

                return result;
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occured while processing the request {ex.Message}");
            }

        }

        [HttpGet("{DepId}")]
        public async Task<ActionResult<DepartmentDTO>> GetDepartmentById(int DepId)
        {
            try
            {
                var result = await _manager.GetDepartmentById(DepId);
                return Ok(result);
            }
            catch (DepartmentValidationException ex)
            {
                return BadRequest(new { Error = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occured while processing the request {ex.Message}");
            }
        }


        [HttpPost("upsert")]
        public async Task<ObjectResult> UpsertDepartment(DepartmentDTO departmentDTO)
        {
            try
            {
                var result = await _manager.UpsertDepartment(departmentDTO);
                return Ok(new { Result = $"Data has been {result} in the Database" });
            }
            catch (DepartmentValidationException ex)
            {
                return BadRequest(new { Error = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occured while processing the request {ex.Message}");
            }

        }

        [HttpDelete]

        public async Task<ActionResult> DeleteDepartment(int DepID)
        {
            try
            {
                var result = await _manager.DeleteDepartment(DepID);

                return Ok(new { Success = result });
            }
            catch (DepartmentValidationException ex)
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
