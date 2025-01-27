using HumanResources.Manager.DTOs;
using HumanResources.Manager.Managers;
using HumanResources.Manager.Validation;
using HumanResources.Manager.Validation.Employee;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HumanResourcesAPI.Controllers.Salary
{
    [Route("api/[controller]")]
    [ApiController]
    public class SalaryController : ControllerBase
    {
        private readonly SalaryManager _salaryManager;

        public SalaryController(SalaryManager salaryManager)
        {
            _salaryManager = salaryManager;
        }

        [HttpGet]
        public async Task<ActionResult<List<SalaryDTO>>> GetAllSalary()
        {
            try
            {
                var result = await _salaryManager.GetAllSalary();

                return result;
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occured while processing the request {ex.Message}");
            }
        }

        [HttpGet("{salaryID}")]
        public async Task<ActionResult<SalaryDTO>> GetSalaryById(int salaryID)
        {
            try
            {
                var result = await _salaryManager.GetSalaryById(salaryID);
                return Ok(result);
            }
            catch (SalaryValidationException ex)
            {
                return BadRequest(new { Error = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occured while processing the request {ex.Message}");
            }
        }

        [HttpPost("upsert")]
        public async Task<ObjectResult> UpsertSalary(SalaryDTO salaryDTO)
        {
            try
            {
                var result = await _salaryManager.UpsertSalary(salaryDTO);
                return Ok(new { Success = $"Database Has Been {result} ." });
            }
            catch (SalaryValidationException ex)
            {
                return BadRequest(new { Error = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occured while processing the request {ex.Message}");
            }
        }

        [HttpDelete("{salaryID}/{deletedBy}")]
        public async Task<ActionResult> DeleteSalaryWithSoftDelete(int salaryID, int deletedBy)
        {
            try
            {
                var result = await _salaryManager.DeleteSalaryWithSoftDelete(salaryID, deletedBy);
                return Ok(new { Success = result });
            }
            catch (SalaryValidationException ex)
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
