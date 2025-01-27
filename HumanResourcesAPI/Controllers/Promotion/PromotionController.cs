using HumanResources.Manager.DTOs;
using HumanResources.Manager.Managers;
using HumanResources.Manager.Validation.Bonus;
using HumanResources.Manager.Validation.Employee;
using HumanResources.Manager.Validation.Permissions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HumanResourcesAPI.Controllers.Promotion
{
    [Route("api/[controller]")]
    [ApiController]
    public class PromotionController : ControllerBase
    {
        private readonly PromotionManager _promotionManager;

        public PromotionController(PromotionManager promotionManager)
        {
            _promotionManager = promotionManager;
        }

        [HttpGet]
        public async Task<ActionResult<List<PromotionDTO>>> GetAllEmployee()
        {
            try
            {
                var result = await _promotionManager.GetAllPromotion();

                return result;
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occured while processing the request {ex.Message}");
            }
        }

        [HttpGet("{promotionID}")]
        public async Task<ActionResult<PromotionDTO>> GetPromotionById(int promotionID)
        {
            try
            {
                var result = await _promotionManager.GetPromotionById(promotionID);
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
        public async Task<ObjectResult> UpsertPromotion(PromotionDTO promotionDTO)
        {
            try
            {
                var result = await _promotionManager.UpsertPromotion(promotionDTO);
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

        [HttpDelete("{promotionID}/{deletedBy}")]
        public async Task<ActionResult> DeletePromotionWithSoftDelete(int promotionID, int deletedBy)
        {
            try
            {
                var result = await _promotionManager.DeletePromotionWithSoftDelete(promotionID, deletedBy);
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
