using HumanResources.Manager.DTOs;
using HumanResources.Manager.Managers;
using HumanResources.Manager.Validation.Employee;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HumanResourcesAPI.Controllers.Attendance
{
    [Route("api/[controller]")]
    [ApiController]
    public class AttendanceController : ControllerBase
    {
        private readonly AttendanceManager _attendanceManager;

        public AttendanceController(AttendanceManager attendanceManager)
        {
            _attendanceManager = attendanceManager;
        }

        [HttpGet]
        public async Task<ActionResult<List<AttendanceDTO>>> GetAllAttendance()
        {
            try
            {
                var result = await _attendanceManager.GetAllAttendance();

                return result;
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occured while processing the request {ex.Message}");
            }
        }

        [HttpGet("{attendanceID}")]
        public async Task<ActionResult<AttendanceDTO>> GetAttendanceById(int attendanceID)
        {
            try
            {
                var result = await _attendanceManager.GetAttendanceById(attendanceID);
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
        public async Task<ObjectResult> UpsertAttendance(AttendanceDTO AttendanceDTO)
        {
            try
            {
                var result = await _attendanceManager.UpsertAttendance(AttendanceDTO);
                
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

        [HttpDelete("{attendanceID}/{deletedBy}")]
        public async Task<ActionResult> DeleteAttendance(int attendanceID, int deletedBy)
        {
            try
            {
                var result = await _attendanceManager.DeleteAttendance(attendanceID, deletedBy);
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
