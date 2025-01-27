using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HumanResourcesAPI.EntityData.EntityModels;

namespace HumanResources.DataServices.Abstract
{
    public interface IDepartmentDataServices
    {
        Task<List<Department>> GetAllDepartments();

        Task<Department> AddDepartment(Department department);

        Task<Department> GetDepartmentById(int DepId);

        Task<Department> UpdateDepartment(Department department);

        Task<bool> DeleteDepartment(int DepID);

        Task<Department> GetDepartmentByUnique(Department department);

    }   
}
