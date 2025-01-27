using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HumanResourcesAPI.EntityData.EntityModels;

namespace HumanResources.DataServices.Abstract
{
    public interface ILeaveDataServices
    {
        Task<List<Leave>> GetAllLeave();

        Task<Leave> GetLeaveById(int leaveID);

        Task<Leave> AddLeave(Leave leave);

        Task<Leave> UpdateLeave(Leave leave);

        Task<bool> DeleteLeave(Leave leave);

        Task<Leave> GetLeaveByUnique(Leave leave);

        Task<Leave> GetDeletedLeave(int leaveID);
    }
}
