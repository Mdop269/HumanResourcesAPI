using HumanResources.Manager.DTOs;
using HumanResources.Manager.Managers;
using HumanResources.Manager.Managers.Review;
using HumanResources.Manager.Validation.Bonus;
using HumanResources.Manager.Validation.Employee;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HumanResourcesAPI.Controllers.Bonus
{
    [Route("api/[controller]")]
    [ApiController]
    public class BonusController : ControllerBase
    {
        private readonly BonusManager _bonusManager;

        public BonusController(BonusManager bonusManager)
        {
            _bonusManager = bonusManager;
        }

        [HttpGet]
        public async Task<ActionResult<List<BonusDTO>>> GetAllBonus()
        {
            try
            {
                var result = await _bonusManager.GetAllBonus();

                return result;
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occured while processing the request {ex.Message}");
            }
        }

        [HttpGet("{bonusID}")]
        public async Task<ActionResult<BonusDTO>> GetEmployeeById(int bonusID)
        {
            try
            {
                var result = await _bonusManager.GetBonusById(bonusID);
                return Ok(result);
            }
            catch (BonusValidationException ex)
            {
                return BadRequest(new { Error = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occured while processing the request {ex.Message}");
            }
        }

        [HttpPost("upsert")]
        public async Task<ObjectResult> UpsertBonus(BonusDTO bonusDTO)
        {
            try
            {
                var result = await _bonusManager.UpsertBonus(bonusDTO);
                return Ok(new { Success = $"Database Has Been {result} ." });
            }
            catch (BonusValidationException ex)
            {
                return BadRequest(new { Error = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occured while processing the request {ex.Message}");
            }
        }

        [HttpDelete("{bonusID}/{deletedBy}")]
        public async Task<ActionResult> DeleteBonusWithSoftDelete(int bonusID, int deletedBy)
        {
            try
            {
                var result = await _bonusManager.DeleteBonusWithSoftDelete(bonusID, deletedBy);
                return Ok(new { Success = result });
            }
            catch (BonusValidationException ex)
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
