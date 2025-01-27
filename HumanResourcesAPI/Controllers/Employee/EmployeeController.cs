using HumanResources.Manager.DTOs;
using HumanResources.Manager.Managers;
using HumanResources.Manager.Validation.Employee;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HumanResourcesAPI.Controllers.Employee
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeeController : ControllerBase
    {
        private readonly EmployeeManager _employeeManager;

        public EmployeeController(EmployeeManager employeeManager)
        {
            _employeeManager = employeeManager;
        }

        [HttpGet]
        public async Task<ActionResult<List<EmployeeDTO>>> GetAllEmployee()
        {
            try
            {
                var result = await _employeeManager.GetAllEmployee();

                return result;
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occured while processing the request {ex.Message}");
            }
        }

        [HttpGet("{employeeID}")]
        public async Task<ActionResult<EmployeeDTO>> GetEmployeeById(int employeeID)
        {
            try
            {
                var result = await _employeeManager.GetEmployeeById(employeeID);
                return Ok(result);
            }
            catch (EmployeeValidationException ex)
            {
                return BadRequest(new { Error = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occured while processing the request {ex.Message}");
            }
        }

        [HttpPost("upsert")]
        public async Task<ObjectResult> UpsertEmployee(EmployeeDTO employeeDTO)
        {
            try
            {
                var result = await _employeeManager.UpsertEmployee(employeeDTO);
                return Ok(new { Success = $"Database Has Been {result} ." });
            }
            catch (EmployeeValidationException ex)
            {
                return BadRequest(new { Error = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occured while processing the request {ex.Message}");
            }
        }

        [HttpDelete("{employeeID}/{deletedBy}")]
        public async Task<ActionResult> DeleteEmployeeWithSoftDelete(int employeeID, int deletedBy)
        {
            try
            {
                var result = await _employeeManager.DeleteEmployeeWithSoftDelete(employeeID, deletedBy);
                return Ok(new { Success = result });
            }
            catch (EmployeeValidationException ex)
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
