using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HumanResourcesAPI.EntityData.EntityModels;
using HumanResourcesAPI.EntityData;
using Microsoft.EntityFrameworkCore;
using HumanResources.DataServices.Abstract;

namespace HumanResources.DataServices.Services
{
    public class StatusDataServices : IStatusDataServices
    {
        private readonly HumanResourcesContext _context;

        public StatusDataServices(HumanResourcesContext context)
        {
            _context = context;
        }

        public async Task<List<Status>> GetAllStatus()
        {
            var result = await _context.Statuses.ToListAsync();

            return result;
        }

        public async Task<Status> GetStatusById(int StatusId)
        {
            var status = await _context.Statuses.FindAsync(StatusId);

            return status;
        }

        public async Task<Status> AddStatus(Status status)
        {
            await _context.Statuses.AddAsync(status);

            await _context.SaveChangesAsync();

            return status;
        }

        public async Task<Status> UpdateStatus(Status status)
        {
            var existingStatus = await _context.Statuses.FirstOrDefaultAsync(a => a.StatusName == status.StatusName);
            existingStatus.StatusName = status.StatusName;
            await _context.SaveChangesAsync();
            return existingStatus;
        }


        public async Task<bool> DeleteStatus(int StatusId)
        {
            var existingStatus = await _context.Statuses.FindAsync(StatusId);
            _context.Statuses.Remove(existingStatus);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<Status> GetStatusByUnique(Status status)
        {
            var existingStatus = await _context.Statuses.FirstOrDefaultAsync(a => a.StatusName == status.StatusName);

            return existingStatus;
        }

        public async Task<Status> AddStatusSP(Status status)
        {

            await _context.Database.ExecuteSqlInterpolatedAsync($"Exec InsertInStatus {status.StatusName}");

            //await _context.SaveChangesAsync();

            return status;
        }
    }
}
