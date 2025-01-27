using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HumanResourcesAPI.EntityData.EntityModels;

namespace HumanResources.DataServices.Abstract
{
    public interface IAttendanceDataServices
    {
        Task<List<Attendance>> GetAllAttendance();

        Task<Attendance> GetAttendanceById(int attendanceID);

        Task<Attendance> AddAttendance(Attendance attendance);

        Task<Attendance> UpdateAttendance(Attendance attendance);

        Task<bool> DeleteAttendance(Attendance attendance);

        Task<Attendance> GetAttendanceByUnique(Attendance attendance);

        Task<Attendance> GetDeletedAttendance(int attendanceID);
    }
}
