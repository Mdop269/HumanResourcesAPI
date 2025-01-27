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
    public class BonusDataServices : IBonusDataServices
    {
        private readonly HumanResourcesContext _context;

        public BonusDataServices(HumanResourcesContext context)
        {
            _context = context;
        }

        public async Task<List<Bonu>> GetAllBonus()
        {
            var result = await _context.Bonus.Where(a => a.DeletedBy == null).ToListAsync();

            return result;
        }

        public async Task<Bonu> GetBonusById(int bonusID)
        {
            var bonus = await _context.Bonus.FirstOrDefaultAsync(a => a.BonusId == bonusID && a.DeletedBy == null);

            return bonus;
        }

        public async Task<Bonu> AddBonus(Bonu bonus)
        {
            await _context.Bonus.AddAsync(bonus);

            await _context.SaveChangesAsync();


            return bonus;
        }

        public async Task<Bonu> UpdateBonus(Bonu bonus)
        {
            await _context.SaveChangesAsync();

            return bonus;
        }


        public async Task<bool> DeleteBonus(Bonu bonus)
        {
            _context.Bonus.Remove(bonus);
            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<Bonu> GetBonusByUnique(Bonu bonus)
        {
            var existingBonus = await _context.Bonus.FirstOrDefaultAsync(a => a.EmployeeId == bonus.EmployeeId && a.DateAwarded == bonus.DateAwarded);

            return existingBonus;
        }

        public async Task<Bonu> GetDeletedEmBonus(int bonusID)
        {
            var deletedBonus = await _context.Bonus.FirstOrDefaultAsync(a => a.BonusId == bonusID && a.DeletedBy != null);

            return deletedBonus;

        }
    }
}
