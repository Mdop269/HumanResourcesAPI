using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HumanResourcesAPI.EntityData.EntityModels;

namespace HumanResources.DataServices.Abstract
{
    public interface IEmployeeDataServices
    {
        Task<List<Employee>> GetAllEmployee();

        Task<Employee> GetEmployeeById(int employeeID);

        Task<Employee> AddEmployee(Employee employee);

        Task<Employee> UpdateEmployee(Employee employee);

        Task<bool> DeleteEmployee(Employee employee); 

        Task<Employee> GetEmployeeByUnique(Employee employee);

        Task<Employee> GetDeletedEmployee(int employeeID);

        Task<Employee> GetPhoneNoOfEmployee(string phone);

        Task<Employee> GetEmailOfEmployee(string email);


    }
}
