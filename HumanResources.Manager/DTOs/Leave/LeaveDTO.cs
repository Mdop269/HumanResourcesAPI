using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HumanResourcesAPI.EntityData.EntityModels;

namespace HumanResources.Manager.DTOs
{
    public class LeaveDTO
    {
        [Required]
        public int EmployeeId { get; set; }

        [Required, Length(4,49)]
        public string LeaveType { get; set; } = null!;

        [Required, DataType(DataType.Date)]
        public DateOnly StartDate { get; set; }

        [Required, DataType(DataType.Date)]
        public DateOnly EndDate { get; set; }

        [Required]
        public string Reason { get; set; } = null!;

        public int? ChangedBy { get; set; }

        public static Leave MapToEntity(LeaveDTO leaveDTO)
        {
            return new Leave
            {
                EmployeeId = leaveDTO.EmployeeId,
                LeaveType = leaveDTO.LeaveType,
                StartDate = leaveDTO.StartDate,
                EndDate = leaveDTO.EndDate,
                Reason = leaveDTO.Reason,
                ChangedBy = leaveDTO.ChangedBy,
            };
        }

        public static LeaveDTO MapToDTO(Leave leave) => new LeaveDTO
        {

            EmployeeId = leave.EmployeeId,
            LeaveType = leave.LeaveType,
            StartDate = leave.StartDate,
            EndDate = leave.EndDate,
            Reason = leave.Reason,
            ChangedBy = leave.ChangedBy,
        };
    }
}
