using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HumanResourcesAPI.EntityData.EntityModels;

namespace HumanResources.DataServices.Abstract
{
    public interface IPromotionDataServices
    {
        Task<List<Promotion>> GetAllPromotion();

        Task<Promotion> GetPromotionById(int promotionID);

        Task<Promotion> AddPromotion(Promotion promotion);

        Task<Promotion> UpdatePromotion(Promotion promotion);

        Task<bool> DeletePromotion(Promotion promotion);

        Task<Promotion> GetPromotionByUnique(Promotion promotion);

        Task<Promotion> GetDeletedPromotion(int promotionID);
    }
}
