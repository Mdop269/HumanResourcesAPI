using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HumanResourcesAPI.EntityData.EntityModels;

namespace HumanResources.Manager.DTOs
{
    public class AttendanceDTO
    {

        public int EmployeeId { get; set; }

        public DateOnly Date { get; set; }

        public DateTime CheckInTime { get; set; }

        public DateTime? CheckOutTime { get; set; }

        public int StatusId { get; set; }

        public int? ChangedBy { get; set; }

        public static Attendance MapToEntity(AttendanceDTO attendanceDTO)
        {
            return new Attendance
            {
                EmployeeId = attendanceDTO.EmployeeId,
                Date = attendanceDTO.Date,
                CheckInTime = attendanceDTO.CheckInTime,
                CheckOutTime = attendanceDTO.CheckOutTime,
                StatusId = attendanceDTO.StatusId,
                ChangedBy = attendanceDTO.ChangedBy,
            };
        }

        public static AttendanceDTO MapToDTO(Attendance attendance) => new AttendanceDTO
        {

            EmployeeId = attendance.EmployeeId,
            Date = attendance.Date,
            CheckInTime = attendance.CheckInTime,
            CheckOutTime = attendance.CheckOutTime,
            StatusId = attendance.StatusId,
            ChangedBy = attendance.ChangedBy,

        };
    }
}
