using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HumanResources.DataServices.Abstract;
using HumanResources.Manager.DTOs;
using HumanResources.Manager.Validation.Attendance;
using HumanResources.Manager.Validation.Employee;

namespace HumanResources.Manager.Managers
{
    public class AttendanceManager
    {
        //this is better approach for getting thw date rather then new date() in this if the system date doesnt update it will be a prob
        private readonly DateOnly currentDate = DateOnly.FromDateTime(DateTime.Now);
        private readonly DateTime dateTime = DateTime.Now;

        private readonly IHRDataServices _hrDataServices;
        private readonly IStatusDataServices _statusDataServices;
        private readonly IEmployeeDataServices _employeeDataServices;
        private readonly IAttendanceDataServices _attendanceDataServices;


        public AttendanceManager(
            IEmployeeDataServices employeeDataServices,
            IHRDataServices hrDataServices,
            IStatusDataServices statusDataServices,
            IAttendanceDataServices attendanceDataServices
            )
        {
            _employeeDataServices = employeeDataServices;
            _hrDataServices = hrDataServices;
            _statusDataServices = statusDataServices;
            _attendanceDataServices = attendanceDataServices;
        }

        public async Task<List<AttendanceDTO>> GetAllAttendance()
        {
            var result = await _attendanceDataServices.GetAllAttendance();

            return result.Select(x => AttendanceDTO.MapToDTO(x)).ToList();
        }

        public async Task<AttendanceDTO> GetAttendanceById(int attendanceID)
        {
            var validation = new Dictionary<string, string>();

            if (attendanceID <= 0)
                validation.Add("attendanceID", "Invalid attendanceID");

            if (validation.Count == 0)
            {
                var result = await _attendanceDataServices.GetAttendanceById(attendanceID);
                if (result is null)
                {
                    throw new AttendanceValidationException($"Attendance with ID {attendanceID} does not exist.");
                }
                return AttendanceDTO.MapToDTO(result);

            }
            else
            {
                throw new AttendanceValidationException("Validation failed: " + string.Join(", ", validation.Select(kv => $"{kv.Key}: {kv.Value}")));
            }
        }

        public async Task<String> UpsertAttendance(AttendanceDTO attendanceDTO)
        {
            var validation = new Dictionary<string, string>();
            if (attendanceDTO is null)
            {
                validation.Add("Null", "attendance Cant Be Null");
            }
            else if (attendanceDTO.StatusId <= 0 || await _statusDataServices.GetStatusById(attendanceDTO.StatusId) == null)
            {
                validation.Add("WrongStatusId", "StatusId is Invalid");
            }
            else if (attendanceDTO.EmployeeId <= 0 || await _employeeDataServices.GetEmployeeById(attendanceDTO.EmployeeId) == null)
            {
                validation.Add("WrongEmployeeId", "EmployeeId is Invalid");
            }
            else if (attendanceDTO.ChangedBy >= 1 && await _hrDataServices.GetHrById((int)attendanceDTO.ChangedBy) == null)
            {
                validation.Add("WrongChangedBY", "Changed By ID is Invalid");
            }


            if (validation.Count == 0)
            {
                var attendanceEntity = AttendanceDTO.MapToEntity(attendanceDTO);

                var checkingData = await _attendanceDataServices.GetAttendanceByUnique(attendanceEntity);
                if (checkingData is not null)
                {

                    if (attendanceEntity.ChangedBy <= 0)
                    {
                        validation.Add("ChangedByCantbeEmpty", "Changed by is not Present");
                    }

                    if (validation.Count == 0)
                    {
                        checkingData.StatusId = attendanceDTO.StatusId;
                        checkingData.CheckInTime = attendanceDTO.CheckInTime;
                        checkingData.CheckOutTime = attendanceDTO.CheckOutTime;
                        checkingData.ChangedBy = attendanceDTO.ChangedBy;
                        checkingData.ChangedOn = currentDate;

                        var updatedData = await _attendanceDataServices.UpdateAttendance(attendanceEntity);
                        return "Updated";
                    }
                    else
                    {
                        throw new AttendanceValidationException("Validation failed: " + string.Join(", ", validation.Select(kv => $"{kv.Key}: {kv.Value}")));
                    }
                    
                }
                else
                {
                    Console.WriteLine("jiiii");
                    attendanceEntity.Date = currentDate;
                    attendanceEntity.CheckInTime = dateTime;
                    attendanceEntity.CheckOutTime = null;
                    attendanceEntity.ChangedBy = null;
                    var addData = await _attendanceDataServices.AddAttendance(attendanceEntity);

                    return "Added";                
                }
            }
            else
            {
                throw new AttendanceValidationException("Validation failed: " + string.Join(", ", validation.Select(kv => $"{kv.Key}: {kv.Value}")));

            }
        }

        public async Task<bool> DeleteAttendance(int attendanceID, int deletedBy)
        {
            var validation = new Dictionary<string, string>();

            if (attendanceID <= 0 || deletedBy <= 0)
            {
                validation.Add("Enter Valid Details", " attendanceID Or DeletedBy Is Invalid");
            }
            else if (await _hrDataServices.GetHrById(deletedBy) == null)
            {
                validation.Add("Hr Invalid", $" HR with {deletedBy} is Invalid");
            }
            else if (await _attendanceDataServices.GetAttendanceById(attendanceID) == null)
            {
                validation.Add("Attendance Invalid", $" Attendance with {attendanceID} is Invalid");
            }
            if (validation.Count == 0)
            {
                var attendanceData = await _attendanceDataServices.GetAttendanceById(attendanceID);

                var result = await _attendanceDataServices.DeleteAttendance(attendanceData);

                return true;
            }
            else
            {
                throw new AttendanceValidationException("Validation failed: " + string.Join(", ", validation.Select(kv => $"{kv.Key}: {kv.Value}")));

            }
        }
    }
}
