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
    public class LeaveDataServices : ILeaveDataServices
    {
        private readonly HumanResourcesContext _context;

        public LeaveDataServices(HumanResourcesContext context)
        {
            _context = context;
        }

        public async Task<List<Leave>> GetAllLeave()
        {
            var result = await _context.Leaves.Where(a => a.DeletedBy == null).ToListAsync();

            return result;
        }

        public async Task<Leave> GetLeaveById(int leaveID)
        {
            var leave = await _context.Leaves.FirstOrDefaultAsync(a => a.LeaveId == leaveID && a.DeletedBy == null);

            return leave;
        }

        public async Task<Leave> AddLeave(Leave leave)
        {
            await _context.Leaves.AddAsync(leave);

            await _context.SaveChangesAsync();


            return leave;
        }

        public async Task<Leave> UpdateLeave(Leave leave)
        {
            await _context.SaveChangesAsync();

            return leave;
        }


        public async Task<bool> DeleteLeave(Leave leave)
        {
            _context.Leaves.Remove(leave);
            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<Leave> GetLeaveByUnique(Leave leave)
        {
            var existingLeave = await _context.Leaves.FirstOrDefaultAsync(a => a.EmployeeId == leave.EmployeeId && a.StartDate == leave.StartDate);

            return existingLeave;
        }

        public async Task<Leave> GetDeletedLeave(int leaveID)
        {
            var deletedLeave = await _context.Leaves.FirstOrDefaultAsync(a => a.LeaveId == leaveID && a.DeletedBy != null);

            return deletedLeave;

        }
    }
}
