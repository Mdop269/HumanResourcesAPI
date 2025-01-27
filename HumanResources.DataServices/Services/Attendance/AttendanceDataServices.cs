using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HumanResources.DataServices.Abstract;
using HumanResourcesAPI.EntityData.EntityModels;
using HumanResourcesAPI.EntityData;
using Microsoft.EntityFrameworkCore;

namespace HumanResources.DataServices.Services
{
    public class AttendanceDataServices : IAttendanceDataServices
    {
        private readonly HumanResourcesContext _context;

        public AttendanceDataServices(HumanResourcesContext context)
        {
            _context = context;
        }

        public async Task<List<Attendance>> GetAllAttendance()
        {
            var result = await _context.Attendances.ToListAsync();

            return result;
        }

        public async Task<Attendance> GetAttendanceById(int attendanceID)
        {
            var attendance = await _context.Attendances.FirstOrDefaultAsync(a => a.AttendanceId == attendanceID);

            return attendance;
        }

        public async Task<Attendance> AddAttendance(Attendance attendance)
        {
            await _context.Attendances.AddAsync(attendance);

            await _context.SaveChangesAsync();


            return attendance;
        }

        public async Task<Attendance> UpdateAttendance(Attendance attendance)
        {
            await _context.SaveChangesAsync();

            return attendance;
        }


        public async Task<bool> DeleteAttendance(Attendance attendance)
        {
            _context.Attendances.Remove(attendance);
            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<Attendance> GetAttendanceByUnique(Attendance attendance)
        {
            var existingEmployee = await _context.Attendances.FirstOrDefaultAsync(a => a.EmployeeId == attendance.EmployeeId && a.Date == attendance.Date);

            return existingEmployee;
        }

        public async Task<Attendance> GetDeletedAttendance(int attendanceID)
        {
            var deletedEmployee = await _context.Attendances.FirstOrDefaultAsync(a => a.AttendanceId == attendanceID);

            return deletedEmployee;

        }
    }
}
