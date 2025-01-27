using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HumanResourcesAPI.EntityData.EntityModels;

namespace HumanResources.Manager.DTOs
{
    public class PromotionDTO
    {
        public int EmployeeId { get; set; }

        public int OldRoleId { get; set; }

        public int NewRoleId { get; set; }

        public DateOnly PromotionDate { get; set; }

        public string Reason { get; set; } = null!;

        public int CreatedBy { get; set; }

        public int? ChangedBy { get; set; }

        public static Promotion MapToEntity(PromotionDTO promotionDTO)
        {
            return new Promotion
            {
                EmployeeId = promotionDTO.EmployeeId,
                OldRoleId = promotionDTO.OldRoleId,
                NewRoleId = promotionDTO.NewRoleId,
                PromotionDate = promotionDTO.PromotionDate,
                Reason = promotionDTO.Reason,
                CreatedBy = promotionDTO.CreatedBy,
                ChangedBy = promotionDTO.ChangedBy,
            };
        }

        public static PromotionDTO MapToDTO(Promotion promotion) => new PromotionDTO
        {

            EmployeeId = promotion.EmployeeId,
            OldRoleId = promotion.OldRoleId,
            NewRoleId = promotion.NewRoleId,
            PromotionDate = promotion.PromotionDate,
            Reason = promotion.Reason,
            CreatedBy = promotion.CreatedBy,
            ChangedBy = promotion.ChangedBy,
        };
    }
}
