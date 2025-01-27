using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HumanResourcesAPI.EntityData.EntityModels;

namespace HumanResources.DataServices.Abstract
{
    public interface ISalaryDataServices
    {
        Task<List<Salary>> GetAllSalary();

        Task<Salary> GetSalaryById(int salaryID);

        Task<Salary> AddSalary(Salary salary);

        Task<Salary> UpdateSalary(Salary salary);

        Task<bool> DeleteSalary(Salary salary);

        Task<Salary> GetSalaryByUnique(Salary salary);

        Task<Salary> GetDeletedSalary(int salaryID);
    }
}
