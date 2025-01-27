using HumanResources.Manager.DTOs;
using HumanResources.Manager.Managers;
using HumanResources.Manager.Managers.Review;
using HumanResources.Manager.Validation.Login;
using HumanResources.Manager.Validation.Review;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HumanResourcesAPI.Controllers.Login
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        private readonly LoginManager _loginManager;

        public LoginController(LoginManager loginManager)
        {
            _loginManager = loginManager;
        }

        [HttpGet]
        public async Task<ActionResult<List<LoginDTO>>> GetAllLogin()
        {
            try
            {
                var result = await _loginManager.GetAllLogin();

                return result;
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occured while processing the request {ex.Message}");
            }
        }

        [HttpGet("{loginID}")]
        public async Task<ActionResult<LoginDTO>> GetLoginById(int loginID)
        {
            try
            {
                var result = await _loginManager.GetLoginById(loginID);
                return Ok(result);
            }
            catch (LoginValidationException ex)
            {
                return BadRequest(new { Error = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occured while processing the request {ex.Message}");
            }
        }

        [HttpGet("{username}/{password}")]
        public async Task<ActionResult<LoginDTO>> GetLoginByUsernamePass(string username , string password)
        {
            try
            {
                var result = await _loginManager.GetLoginByUsernamePass(username, password);
                return Ok(result);
            }
            catch (LoginValidationException ex)
            {
                return BadRequest(new { Error = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occured while processing the request {ex.Message}");
            }
        }

        [HttpPost("upsert")]
        public async Task<ActionResult<string>> UpsertLogin(LoginDTO loginDTO)
        {
            try
            {
                var result = await _loginManager.UpsertLogin(loginDTO);
                return Ok(new { Success = $"Database Has Been {result} ." });
            }
            catch (LoginValidationException ex)
            {
                return BadRequest(new { Error = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occured while processing the request {ex.Message}");
            }
        }

        [HttpDelete("{loginID}")]
        public async Task<ActionResult> DeleteLogin(int loginID)
        {
            try
            {
                var result = await _loginManager.DeleteLogin(loginID);
                return Ok(new { Success = result });
            }
            catch (LoginValidationException ex)
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
