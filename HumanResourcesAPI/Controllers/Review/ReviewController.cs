using HumanResources.Manager.DTOs;
using HumanResources.Manager.Managers;
using HumanResources.Manager.Managers.Review;
using HumanResources.Manager.Validation.Employee;
using HumanResources.Manager.Validation.Review;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HumanResourcesAPI.Controllers.Review
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReviewController : ControllerBase
    {
        private readonly ReviewManager _reviewManager;

        public ReviewController(ReviewManager reviewManager)
        {
            _reviewManager = reviewManager;
        }

        [HttpGet]
        public async Task<ActionResult<List<ReviewDTO>>> GetAllReview()
        {
            try
            {
                var result = await _reviewManager.GetAllReview();

                return result;
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occured while processing the request {ex.Message}");
            }
        }

        [HttpGet("{reviewID}")]
        public async Task<ActionResult<ReviewDTO>> GetReviewById(int reviewID)
        {
            try
            {
                var result = await _reviewManager.GetReviewById(reviewID);
                return Ok(result);
            }
            catch (ReviewValidationException ex)
            {
                return BadRequest(new { Error = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occured while processing the request {ex.Message}");
            }
        }

        [HttpPost("upsert")]
        public async Task<ObjectResult> UpsertReview(ReviewDTO reviewDTO)
        {
            try
            {
                var result = await _reviewManager.UpsertReview(reviewDTO);
                return Ok(new { Success = $"Database Has Been {result} ." });
            }
            catch (ReviewValidationException ex)
            {
                return BadRequest(new { Error = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occured while processing the request {ex.Message}");
            }
        }

        [HttpDelete("{reviewID}")]
        public async Task<ActionResult> DeleteReview(int reviewID)
        {
            try
            {
                var result = await _reviewManager.DeleteReview(reviewID);
                return Ok(new { Success = result });
            }
            catch (ReviewValidationException ex)
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
