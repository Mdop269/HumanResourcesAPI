using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HumanResourcesAPI.EntityData.EntityModels;

namespace HumanResources.DataServices.Abstract
{
    public interface IStatusDataServices
    {
        Task<List<Status>> GetAllStatus();

        Task<Status> GetStatusById(int StatusId);

        Task<Status> AddStatus(Status status);

        Task<Status> UpdateStatus(Status status);

        Task<bool> DeleteStatus(int StatusId);

        Task<Status> GetStatusByUnique(Status status);

        Task<Status> AddStatusSP(Status status);
    }
}
