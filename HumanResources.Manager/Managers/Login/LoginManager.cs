using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HumanResources.DataServices.Abstract;
using HumanResources.DataServices.Services;
using HumanResources.Manager.DTOs;
using HumanResources.Manager.Validation.Employee;
using HumanResources.Manager.Validation.Login;

namespace HumanResources.Manager.Managers
{
    public class LoginManager
    {
        //this is better approach for getting thw date rather then new date() in this if the system date doesnt update it will be a prob
        private readonly DateTime currentDatetime = DateTime.UtcNow;

        private readonly IStatusDataServices _statusDataServices;
        private readonly IEmployeeDataServices _employeeDataServices;
        private readonly ILoginDataServices _loginDataServices;

        public LoginManager(
            IEmployeeDataServices employeeDataServices,
            IStatusDataServices statusDataServices,
            ILoginDataServices loginDataServices
            )
        {
            _employeeDataServices = employeeDataServices;
            _statusDataServices = statusDataServices;
            _loginDataServices = loginDataServices;
        }

        public async Task<List<LoginDTO>> GetAllLogin()
        {
            var result = await _loginDataServices.GetAllLogin();

            return result.Select(x => LoginDTO.MapToDTO(x)).ToList();
        }

        public async Task<LoginDTO> GetLoginById(int loginID)
        {
            var validation = new Dictionary<string, string>();

            if (loginID <= 0)
                validation.Add("loginID", "Invalid loginID");

            if (validation.Count == 0)
            {

                var result = await _loginDataServices.GetLoginById(loginID);
                if (result is null)
                {
                    throw new LoginValidationException($"Login with ID {loginID} does not exist.");
                }
                return LoginDTO.MapToDTO(result);

            }
            else
            {
                throw new LoginValidationException("Validation failed: " + string.Join(", ", validation.Select(kv => $"{kv.Key}: {kv.Value}")));
            }
        }

        public async Task<LoginDTO> GetLoginByUsernamePass(string username, string password)
        {
            var validation = new Dictionary<string, string>();

            if (string.IsNullOrWhiteSpace(username))
            {
                validation.Add("EmptyUserName", "User Name Cant Be Null or WhiteSpace");

            }
            else if (string.IsNullOrWhiteSpace(password))
            {
                validation.Add("EmptyPassword", "Password Cant Be Null or WhiteSpace");
            }
            if (validation.Count == 0)
            {

                var result = await _loginDataServices.GetLogin(username, password);
                if (result is null)
                {
                    throw new LoginValidationException("Username Or Password Is Invalid");
                }
                return LoginDTO.MapToDTO(result);

            }
            else
            {
                throw new LoginValidationException("Validation failed: " + string.Join(", ", validation.Select(kv => $"{kv.Key}: {kv.Value}")));
            }
        }

        public async Task<String> UpsertLogin(LoginDTO loginDTO)
        {
            var validation = new Dictionary<string, string>();
            if (loginDTO is null)
            {
                validation.Add("Null", "Employee Cant Be Null");
            }
            
            else if (string.IsNullOrWhiteSpace(loginDTO.Password))
            {
                validation.Add("EmptyPassword", "Password Cant Be Null or WhiteSpace");
            }
            else if (loginDTO.StatusId <= 0 || await _statusDataServices.GetStatusById(loginDTO.StatusId) == null)
            {
                validation.Add("WrongStatusId", "StatusId is Invalid");
            }
            else if (loginDTO.EmployeeId <= 0 || await _employeeDataServices.GetEmployeeById(loginDTO.EmployeeId) == null)
            {
                validation.Add("WrongEmployeeId", "EmployeeId is Invalid");
            }

            if (validation.Count == 0)
            {
                var loginEntity = LoginDTO.MapToEntity(loginDTO);

                var checkingData = await _loginDataServices.GetLoginByUnique(loginEntity);
                if (checkingData is not null)
                {
                    var checkUsername = await _loginDataServices.GetUsername(loginDTO.Username);
                    if (checkUsername != null)
                    {
                        if (checkUsername.EmployeeId == checkingData.EmployeeId && checkUsername.Username == checkingData.Username)
                        {
                            checkingData.Username = checkingData.Username;
                        }
                        else if (checkUsername.EmployeeId != checkingData.EmployeeId && checkUsername.Username == checkingData.Username)
                        {
                            throw new LoginValidationException("Username : UserName Already Exist");
                        }
                    }
                    else
                    {
                        checkingData.Username = loginDTO.Username;
                    }
                    checkingData.Password = loginDTO.Password;
                    checkingData.StatusId = loginDTO.StatusId;
                    checkingData.LogoutTime = loginDTO.LogoutTime;

                    var updatedData = await _loginDataServices.UpdateLogin(loginEntity);
                    return "Updated";                  
                }
                else
                {
                    if (string.IsNullOrWhiteSpace(loginDTO.Username) || await _loginDataServices.GetUsername(loginDTO.Username) != null)
                    {
                       throw new LoginValidationException("EmptyUserName : User Name Cant Be Null or it already exist");
                    }
                    loginEntity.LoginTime = currentDatetime;
                    loginEntity.LogoutTime = null;
                    var addData = await _loginDataServices.AddLogin(loginEntity);

                    return "Added";
                }
            }
            else
            {
                throw new LoginValidationException("Validation failed: " + string.Join(", ", validation.Select(kv => $"{kv.Key}: {kv.Value}")));

            }
        }

        public async Task<bool> DeleteLogin(int loginID)
        {
            var validation = new Dictionary<string, string>();

            if (loginID <= 0)
            {
                validation.Add("Enter Valid Details", " loginID Or DeletedBy Is Invalid");
            }
            else if (await _loginDataServices.GetLoginById(loginID) == null)
            {
                validation.Add("Login Invalid", $" Login with {loginID} is Invalid");
            }

            if (validation.Count == 0)
            {
                var loginData = await _loginDataServices.GetLoginById(loginID);
                var result = await _loginDataServices.DeleteLogin(loginData);

                return true;
            }
            else
            {
                throw new LoginValidationException("Validation failed: " + string.Join(", ", validation.Select(kv => $"{kv.Key}: {kv.Value}")));
            }
        }
    }
}
