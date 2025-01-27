using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HumanResources.DataServices.Abstract;
using HumanResources.DataServices.Services;
using HumanResources.Manager.DTOs;
using HumanResources.Manager.DTOs.HR;
using HumanResources.Manager.Validation.Employee;
using HumanResources.Manager.Validation.HR;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Model;

namespace HumanResources.Manager.Managers
{
    public class EmployeeManager
    {
        //this is better approach for getting thw date rather then new date() in this if the system date doesnt update it will be a prob
        private readonly DateOnly currentDate = DateOnly.FromDateTime(DateTime.Now);

        private readonly IHRDataServices _hrDataServices;
        private readonly IRoleDataServices _roleDataServices;
        private readonly IDepartmentDataServices _departmentDataServices;
        private readonly IStatusDataServices _statusDataServices;
        private readonly IEmployeeDataServices _employeeDataServices;
        private readonly IPermissionsDataServices _permissionsDataServices;
        private readonly ITeamsDataServices _teamsDataServices;


        public EmployeeManager(
            IEmployeeDataServices employeeDataServices,
            IHRDataServices hrDataServices,
            IRoleDataServices roleDataServices,
            IDepartmentDataServices departmentDataServices,
            IStatusDataServices statusDataServices,
            IPermissionsDataServices permissionsDataServices,
            ITeamsDataServices teamsDataServices
            )
        {
            _employeeDataServices = employeeDataServices;
            _hrDataServices = hrDataServices;
            _roleDataServices = roleDataServices;
            _departmentDataServices = departmentDataServices;
            _statusDataServices = statusDataServices;
            _permissionsDataServices = permissionsDataServices;
            _teamsDataServices = teamsDataServices;
        }

        public async Task<List<EmployeeDTO>> GetAllEmployee()
        {
            var result = await _employeeDataServices.GetAllEmployee();

            return result.Select(x => EmployeeDTO.MapToDTO(x)).ToList();
        }

        public async Task<EmployeeDTO> GetEmployeeById(int employeeID)
        {
            var validation = new Dictionary<string, string>();

            if (employeeID <= 0)
                validation.Add("employeeID", "Invalid employeeID");

            if (validation.Count == 0)
            {

                var result = await _employeeDataServices.GetEmployeeById(employeeID);
                if (result is null)
                {
                    throw new EmployeeValidationException($"Employee with ID {employeeID} does not exist.");
                }
                return EmployeeDTO.MapToDTO(result);

            }
            else
            {
                throw new EmployeeValidationException("Validation failed: " + string.Join(", ", validation.Select(kv => $"{kv.Key}: {kv.Value}")));
            }
        }

        public async Task<String> UpsertEmployee(EmployeeDTO employeeDTO)
        {
            var validation = new Dictionary<string, string>();
            if (employeeDTO is null)
            {
                validation.Add("Null", "Employee Cant Be Null");
            }
            else if (string.IsNullOrWhiteSpace(employeeDTO.FirstName))
            {
                validation.Add("EmptyFirstName", "First Name Cant Be Null or WhiteSpace");

            }
            else if (string.IsNullOrWhiteSpace(employeeDTO.LastName))
            {
                validation.Add("EmptyLastName", "LastName Cant Be Null or WhiteSpace");
            }
            else if (string.IsNullOrWhiteSpace(employeeDTO.Email) )
            {
                validation.Add("EmptyEmail", "Email Cant Be Null or WhiteSpace");
            }
            else if (string.IsNullOrWhiteSpace(employeeDTO.Phone))
            {
                validation.Add("EmptyEmail", "Email Cant Be Null or WhiteSpace");
            }
            else if (employeeDTO.StatusId <= 0 || await _statusDataServices.GetStatusById(employeeDTO.StatusId) == null)
            {
                validation.Add("WrongStatusId", "StatusId is Invalid");
            }
            else if (employeeDTO.DepartmentId <= 0 || await _departmentDataServices.GetDepartmentById(employeeDTO.DepartmentId) == null)
            {
                validation.Add("WrongDepartmentId", "DepartmentId is Invalid");
            }
            else if (employeeDTO.RoleId <= 0 || await _roleDataServices.GetRoleById(employeeDTO.RoleId) == null)
            {
                validation.Add("WrongRoleId", "RoleId is Invalid");
            }
            else if (employeeDTO.PermissionsId <= 0 || await _permissionsDataServices.GetPermissionById(employeeDTO.PermissionsId) == null)
            {
                validation.Add("WrongPermissionsId", "PermissionsId is Invalid");
            } 
            // we use int becuse its optional thats why
            else if (employeeDTO.TeamId >= 1 && await _teamsDataServices.GetTeamById((int)employeeDTO.TeamId) == null)
            {
                validation.Add("WrongTeamId", "TeamId is Invalid");
            }
            else if (employeeDTO.TeamId <= 0 )
            {
                validation.Add("WrongTeamId", "TeamId cant be below 0 if you dont need add NULL");
            }
            else if (employeeDTO.CreatedBy >= 1 && await _hrDataServices.GetHrById((int)employeeDTO.CreatedBy) == null)
            {
                validation.Add("WrongCreatedBY", "Created By ID is Invalid");
            }
            else if (employeeDTO.ChangedBy >= 1 && await _hrDataServices.GetHrById((int)employeeDTO.ChangedBy) == null )
            {
                validation.Add("WrongChangedBY", "Changed By ID is Invalid");
            }


            if (validation.Count == 0)
            {

                var employeeEntity = EmployeeDTO.MapToEntity(employeeDTO);

                var checkingData = await _employeeDataServices.GetEmployeeByUnique(employeeEntity);
                if (checkingData is not null)
                {
                    if(checkingData.DeletedBy is not null || checkingData.DeletedOn is not null )
                    {
                        if (await _employeeDataServices.GetEmailOfEmployee(employeeDTO.Email) != null)
                        {
                            throw new EmployeeValidationException("Email: Email Already Exist");
                        }
                        var checkPhone = await _employeeDataServices.GetPhoneNoOfEmployee(employeeDTO.Phone);
                        if (checkPhone != null)
                        {
                            if (checkPhone.EmployeeId == checkingData.EmployeeId && checkPhone.Phone == employeeDTO.Phone)
                            {
                                checkingData.Phone = employeeDTO.Phone;
                            }
                            else if (checkPhone.EmployeeId != checkingData.EmployeeId && checkPhone.Phone == employeeDTO.Phone)
                            {
                                throw new EmployeeValidationException("Phone No : Phone No Already Exist");
                            }
                        }
                        else
                        {
                            checkingData.Phone = employeeDTO.Phone;
                        }
                        checkingData.DateJoined = currentDate;
                        checkingData.StatusId = employeeDTO.StatusId;
                        checkingData.DepartmentId = employeeDTO.DepartmentId;   
                        checkingData.RoleId = employeeDTO.RoleId;
                        checkingData.PermissionsId = employeeDTO.PermissionsId;
                        checkingData.TeamId = employeeDTO.TeamId;
                        checkingData.CreatedBy = employeeDTO.CreatedBy;
                        checkingData.CreatedOn = currentDate;
                        checkingData.ChangedBy = null;
                        checkingData.ChangedOn = null;
                        checkingData.DeletedBy = null;
                        checkingData.DeletedOn = null;

                        var updatedDeletedData = await _employeeDataServices.UpdateEmployee(employeeEntity);
                        return "UpdatedDeletedData";

                    }
                    else
                    {
                        if (employeeEntity.ChangedBy <= 0)
                        {
                            validation.Add("ChangedByCantbeEmpty", "Changed by is not Present");
                        }                       

                        if (validation.Count == 0)
                        {
                            var checkPhone = await _employeeDataServices.GetPhoneNoOfEmployee(employeeDTO.Phone);
                            if(checkPhone != null)
                            {
                                if (checkPhone.EmployeeId == checkingData.EmployeeId && checkPhone.Phone == employeeDTO.Phone)
                                {
                                    checkingData.Phone = employeeDTO.Phone;
                                }
                                else if (checkPhone.EmployeeId != checkingData.EmployeeId && checkPhone.Phone == employeeDTO.Phone)
                                {
                                    throw new EmployeeValidationException("Phone No : Phone No Already Exist");
                                }
                            }
                            else
                            {
                                checkingData.Phone = employeeDTO.Phone;
                            }
                            checkingData.DateJoined = currentDate;
                            checkingData.StatusId = employeeDTO.StatusId;
                            checkingData.DepartmentId = employeeDTO.DepartmentId;
                            checkingData.RoleId = employeeDTO.RoleId;
                            checkingData.PermissionsId = employeeDTO.PermissionsId;
                            checkingData.TeamId = employeeDTO.TeamId;
                            checkingData.ChangedBy = employeeDTO.ChangedBy;
                            checkingData.ChangedOn = currentDate;

                            var updatedData = await _employeeDataServices.UpdateEmployee(employeeEntity);
                            return "Updated";
                        }
                        else
                        {
                            throw new EmployeeValidationException("Validation failed: " + string.Join(", ", validation.Select(kv => $"{kv.Key}: {kv.Value}")));
                        }
                    }
                }
                else
                {
                    if (employeeEntity.CreatedBy <= 0)
                    {
                        validation.Add("CreatedOnCantByEmpty", "Created By Cant Be null");
                    }

                    if (validation.Count == 0)
                    {
                        if (await _employeeDataServices.GetEmailOfEmployee(employeeDTO.Email) != null)
                        {
                            throw new EmployeeValidationException("Email: Email Already Exist");
                        }
                        else if (await _employeeDataServices.GetPhoneNoOfEmployee(employeeDTO.Phone) != null)
                        {
                            throw new EmployeeValidationException("Phone No : Phone No Already Exist");
                        }
                        if(employeeEntity.TeamId <= 0)
                        {
                            employeeEntity.TeamId = null;
                        }
                        employeeEntity.DateJoined = currentDate;
                        employeeEntity.CreatedOn = currentDate;
                        employeeEntity.ChangedBy = null;
                        var addData = await _employeeDataServices.AddEmployee(employeeEntity);

                        return "Added";
                    }
                    else
                    {
                        throw new EmployeeValidationException("Validation failed: " + string.Join(", ", validation.Select(kv => $"{kv.Key}: {kv.Value}")));
                    }

                }
            }
            else
            {
                throw new EmployeeValidationException("Validation failed: " + string.Join(", ", validation.Select(kv => $"{kv.Key}: {kv.Value}")));

            }
        }

        public async Task<bool> DeleteEmployeeWithSoftDelete(int employeeID , int  deletedBy)
        {
            var validation = new Dictionary<string, string>();

            if (employeeID <= 0 || deletedBy <= 0)
            {
                validation.Add("Enter Valid Details", " EmployeeId Or DeletedBy Is Invalid");
            }
            else if (await _hrDataServices.GetHrById(deletedBy) == null)
            {
                validation.Add("Hr Invalid", $" HR with {deletedBy} is Invalid");
            }
            else if (await _employeeDataServices.GetEmployeeById(employeeID) == null)
            {
                validation.Add("Employee Invalid", $" Employee with {employeeID} is Invalid");
            }
            if (validation.Count == 0)
            {
                var EmployeeData = await _employeeDataServices.GetEmployeeById(employeeID);

                EmployeeData.DeletedBy = deletedBy;
                EmployeeData.DeletedOn = currentDate;

                var result = await _employeeDataServices.UpdateEmployee(EmployeeData);

                return true;
            }
            else
            {
                throw new EmployeeValidationException("Validation failed: " + string.Join(", ", validation.Select(kv => $"{kv.Key}: {kv.Value}")));
            }
        }
    }
}
