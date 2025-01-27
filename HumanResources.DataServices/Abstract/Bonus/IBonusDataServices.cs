using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HumanResourcesAPI.EntityData.EntityModels;

namespace HumanResources.DataServices.Abstract
{
    public interface IBonusDataServices
    {
        Task<List<Bonu>> GetAllBonus();

        Task<Bonu> GetBonusById(int bonusID);

        Task<Bonu> AddBonus(Bonu bonus);

        Task<Bonu> UpdateBonus(Bonu bonus);

        Task<bool> DeleteBonus(Bonu bonus);

        Task<Bonu> GetBonusByUnique(Bonu bonus);

        Task<Bonu> GetDeletedEmBonus(int bonusID);
    }
}
