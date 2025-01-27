using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HumanResources.DataServices.Abstract;
using HumanResourcesAPI.EntityData;
using HumanResourcesAPI.EntityData.EntityModels;
using Microsoft.EntityFrameworkCore;

namespace HumanResources.DataServices.Services
{
    public class HRDataServices : IHRDataServices
    {

        private readonly HumanResourcesContext _context;

        public HRDataServices(HumanResourcesContext context)
        {
            _context = context;
        }

        public async Task<List<Hr>> GetAllHr()
        {
            var result = await _context.Hrs.Where(a => a.DeletedBy == null).ToListAsync();

            return result;
        }

        public async Task<Hr> GetHrById(int HrId)
        {
            var result = await _context.Hrs.FirstOrDefaultAsync(a => a.HrId == HrId && a.DeletedBy == null);

            return result;
        }

        public async Task<Hr> AddHr(Hr Hr)
        {
            await _context.Hrs.AddAsync(Hr);

            await _context.SaveChangesAsync();

            return Hr;
        }

        public async Task<Hr> UpdateHr(Hr Hr)
        {
            await _context.SaveChangesAsync();
            return Hr;
        }


        public async Task<bool> DeleteHr(Hr hr)
        {
            _context.Hrs.Remove(hr);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<Hr> GetHrByUnique(Hr Hr)
        {
            var existingHr = await _context.Hrs.FirstOrDefaultAsync(a => a.HrFirstName == Hr.HrFirstName && a.HrLastName == Hr.HrLastName && a.HrEmail == Hr.HrEmail );

            return existingHr;
        }

        public async Task<Hr> GetDeletedHr(int HrId)
        {
            var deletedHr = await _context.Hrs.FirstOrDefaultAsync(a => a.HrId == HrId && a.DeletedBy != null);

            return deletedHr;
        }
    }
}
