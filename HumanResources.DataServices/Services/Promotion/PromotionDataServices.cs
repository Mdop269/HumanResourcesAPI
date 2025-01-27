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
    public class PromotionDataServices : IPromotionDataServices
    {
        private readonly HumanResourcesContext _context;

        public PromotionDataServices(HumanResourcesContext context)
        {
            _context = context;
        }

        public async Task<List<Promotion>> GetAllPromotion()
        {
            var result = await _context.Promotions.Where(a => a.DeletedBy == null).ToListAsync();

            return result;
        }

        public async Task<Promotion> GetPromotionById(int promotionID)
        {
            var promotion = await _context.Promotions.FirstOrDefaultAsync(a => a.PromotionId == promotionID && a.DeletedBy == null);

            return promotion;
        }

        public async Task<Promotion> AddPromotion(Promotion promotion)
        {
            await _context.Promotions.AddAsync(promotion);

            await _context.SaveChangesAsync();


            return promotion;
        }

        public async Task<Promotion> UpdatePromotion(Promotion promotion)
        {
            await _context.SaveChangesAsync();

            return promotion;
        }


        public async Task<bool> DeletePromotion(Promotion promotion)
        {
            _context.Promotions.Remove(promotion);
            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<Promotion> GetPromotionByUnique(Promotion promotion)
        {
            var existingPromotion = await _context.Promotions.FirstOrDefaultAsync(a => a.EmployeeId == promotion.EmployeeId && a.OldRoleId == promotion.OldRoleId && a.NewRoleId == promotion.NewRoleId);

            return existingPromotion;
        }

        public async Task<Promotion> GetDeletedPromotion(int promotionID)
        {
            var deletedPromotion = await _context.Promotions.FirstOrDefaultAsync(a => a.PromotionId == promotionID && a.DeletedBy != null);

            return deletedPromotion;

        }
    }
}
