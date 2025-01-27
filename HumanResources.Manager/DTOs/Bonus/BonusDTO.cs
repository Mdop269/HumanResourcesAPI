using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HumanResourcesAPI.EntityData.EntityModels;

namespace HumanResources.Manager.DTOs
{
    public class BonusDTO
    {
        public int EmployeeId { get; set; }

        public decimal BonusAmount { get; set; }

        public string Reason { get; set; } = null!;

        public DateOnly DateAwarded { get; set; }

        public int CreatedBy { get; set; }

        public int? ChangedBy { get; set; }

        public static Bonu MapToEntity(BonusDTO bonusDTO)
        {
            return new Bonu
            {
                EmployeeId = bonusDTO.EmployeeId,
                BonusAmount = (decimal)bonusDTO.BonusAmount,
                Reason = bonusDTO.Reason,
                DateAwarded = bonusDTO.DateAwarded,
                CreatedBy = bonusDTO.CreatedBy,
                ChangedBy = bonusDTO.ChangedBy,
            };
        }

        public static BonusDTO MapToDTO(Bonu bonus) => new BonusDTO
        {

            EmployeeId = bonus.EmployeeId,
            BonusAmount = (decimal)bonus.BonusAmount,
            Reason = bonus.Reason,
            DateAwarded = bonus.DateAwarded,
            CreatedBy = bonus.CreatedBy,
            ChangedBy = bonus.ChangedBy,

        };
    }
}
