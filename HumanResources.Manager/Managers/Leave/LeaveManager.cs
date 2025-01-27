using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HumanResources.DataServices.Abstract;
using HumanResources.Manager.DTOs;
using HumanResources.Manager.Validation.Employee;
using HumanResources.Manager.Validation.Leave;

namespace HumanResources.Manager.Managers
{
    public class LeaveManager
    {
        //this is better approach for getting thw date rather then new date() in this if the system date doesnt update it will be a prob
        private readonly DateOnly currentDate = DateOnly.FromDateTime(DateTime.Now);

        private readonly IHRDataServices _hrDataServices;
        private readonly IStatusDataServices _statusDataServices;
        private readonly IEmployeeDataServices _employeeDataServices;
        private readonly ILeaveDataServices _leaveDataServices;


        public LeaveManager(
            IEmployeeDataServices employeeDataServices,
            IHRDataServices hrDataServices,
            IStatusDataServices statusDataServices,
            ILeaveDataServices leaveDataServices
            )
        {
            _employeeDataServices = employeeDataServices;
            _hrDataServices = hrDataServices;
            _statusDataServices = statusDataServices;
            _leaveDataServices = leaveDataServices;
        }

        public async Task<List<LeaveDTO>> GetAllLeave()
        {
            var result = await _leaveDataServices.GetAllLeave();

            return result.Select(x => LeaveDTO.MapToDTO(x)).ToList();
        }

        public async Task<LeaveDTO> GetLeaveById(int leaveID)
        {
            var validation = new Dictionary<string, string>();

            if (leaveID <= 0)
                validation.Add("leaveID", "Invalid leaveID");

            if (validation.Count == 0)
            {

                var result = await _leaveDataServices.GetLeaveById(leaveID);
                if (result is null)
                {
                    throw new LeaveValidationException($"Leave with ID {leaveID} does not exist.");
                }
                return LeaveDTO.MapToDTO(result);

            }
            else
            {
                throw new LeaveValidationException("Validation failed: " + string.Join(", ", validation.Select(kv => $"{kv.Key}: {kv.Value}")));
            }
        }

        public async Task<String> UpsertLeave(LeaveDTO leaveDTO)
        {
            var validation = new Dictionary<string, string>();
            if (leaveDTO is null)
            {
                validation.Add("Null", "Employee Cant Be Null");
            }
            else if (string.IsNullOrWhiteSpace(leaveDTO.LeaveType))
            {
                validation.Add("EmptyLeaveType", "LeaveType Cant Be Null or WhiteSpace");

            }
            else if (string.IsNullOrWhiteSpace(leaveDTO.Reason))
            {
                validation.Add("EmptyReason", "Reason Cant Be Null or WhiteSpace");
            }
            else if (leaveDTO.EmployeeId <= 0 || await _employeeDataServices.GetEmployeeById(leaveDTO.EmployeeId) == null)
            {
                validation.Add("WrongEmployeeId", "EmployeeId is Invalid");
            }
            else if (leaveDTO.ChangedBy >= 1 && await _hrDataServices.GetHrById((int)leaveDTO.ChangedBy) == null)
            {
                validation.Add("WrongChangedBY", "Changed By ID is Invalid");
            }


            if (validation.Count == 0)
            {
                var leaveEntity = LeaveDTO.MapToEntity(leaveDTO);

                var checkingData = await _leaveDataServices.GetLeaveByUnique(leaveEntity);
                if (checkingData is not null)
                {
                    if (checkingData.DeletedBy is not null || checkingData.DeletedOn is not null)
                    {
                        checkingData.LeaveType = leaveDTO.LeaveType;
                        checkingData.EndDate = leaveDTO.EndDate;
                        checkingData.Reason = leaveDTO.Reason;
                        checkingData.CreatedBy = leaveDTO.EmployeeId;
                        checkingData.CreatedOn = currentDate;
                        checkingData.ChangedBy = null;
                        checkingData.ChangedOn = null;
                        checkingData.DeletedBy = null;
                        checkingData.DeletedOn = null;

                        var updatedDeletedData = await _leaveDataServices.UpdateLeave(leaveEntity);
                        return "UpdatedDeletedData";

                    }
                    else
                    {
                        if (leaveEntity.ChangedBy <= 0)
                        {
                            validation.Add("ChangedByCantbeEmpty", "Changed by is not Present");
                        }

                        if (validation.Count == 0)
                        {

                            checkingData.LeaveType = leaveDTO.LeaveType;
                            checkingData.EndDate = leaveDTO.EndDate;
                            checkingData.Reason = leaveDTO.Reason;
                            checkingData.ChangedBy = leaveDTO.ChangedBy;
                            checkingData.ChangedOn = currentDate;

                            var updatedData = await _leaveDataServices.UpdateLeave(leaveEntity);
                            return "Updated";
                        }
                        else
                        {
                            throw new LeaveValidationException("Validation failed: " + string.Join(", ", validation.Select(kv => $"{kv.Key}: {kv.Value}")));
                        }
                    }
                }
                else
                {
                    //if (leaveEntity.CreatedBy <= 0)
                    //{
                    //    validation.Add("CreatedOnCantByEmpty", "Created By Cant Be null");
                    //}

                    
                    if (leaveEntity.ChangedBy <= 0)
                    {
                        leaveEntity.ChangedBy = null;
                    }
                    leaveEntity.CreatedBy = leaveEntity.EmployeeId;
                    leaveEntity.CreatedOn = currentDate;
                    leaveEntity.ChangedBy = null;
                    var addData = await _leaveDataServices.AddLeave(leaveEntity);

                    return "Added";
                   

                }
            }
            else
            {
                throw new LeaveValidationException("Validation failed: " + string.Join(", ", validation.Select(kv => $"{kv.Key}: {kv.Value}")));

            }
        }

        public async Task<bool> DeleteLeaveWithSoftDelete(int leaveID, int deletedBy)
        {
            var validation = new Dictionary<string, string>();

            if (leaveID <= 0 || deletedBy <= 0)
            {
                validation.Add("Enter Valid Details", " leaveID Or DeletedBy Is Invalid");
            }
            else if (await _hrDataServices.GetHrById(deletedBy) == null)
            {
                validation.Add("Hr Invalid", $" HR with {deletedBy} is Invalid");
            }
            else if (await _leaveDataServices.GetLeaveById(leaveID) == null)
            {
                validation.Add("Leave Invalid", $" Leave with {leaveID} is Invalid");
            }
            if (validation.Count == 0)
            {
                var leaveData = await _leaveDataServices.GetLeaveById(leaveID);

                leaveData.DeletedBy = deletedBy;
                leaveData.DeletedOn = currentDate;

                var result = await _leaveDataServices.UpdateLeave(leaveData);

                return true;
            }
            else
            {
                throw new LeaveValidationException("Validation failed: " + string.Join(", ", validation.Select(kv => $"{kv.Key}: {kv.Value}")));
            }
        }
    }
}
