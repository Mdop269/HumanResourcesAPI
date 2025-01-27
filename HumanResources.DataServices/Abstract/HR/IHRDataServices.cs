using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HumanResourcesAPI.EntityData.EntityModels;

namespace HumanResources.DataServices.Abstract
{
    public interface IHRDataServices
    {
        Task<List<Hr>> GetAllHr();

        Task<Hr> GetHrById(int HrId);

        Task<Hr> AddHr(Hr Hr);

        Task<Hr> UpdateHr(Hr Hr);

        Task<bool> DeleteHr(Hr Hr);

        Task<Hr> GetHrByUnique(Hr Hr);

        Task<Hr> GetDeletedHr(int HrId);
    }
}
