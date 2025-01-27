using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HumanResourcesAPI.EntityData.EntityModels;
using HumanResourcesAPI.EntityData;
using Microsoft.EntityFrameworkCore;
using HumanResources.DataServices.Abstract;
using System.Numerics;

namespace HumanResources.DataServices.Services
{
    public class EmployeeDataServices : IEmployeeDataServices
    {
        private readonly HumanResourcesContext _context;

        public EmployeeDataServices(HumanResourcesContext context)
        {
            _context = context;
        }

        public async Task<List<Employee>> GetAllEmployee()
        {
            var result = await _context.Employees.Where(a => a.DeletedBy == null ).ToListAsync();

            return result;
        }

        public async Task<Employee> GetEmployeeById(int employeeID)
        {
            var employee = await _context.Employees.FirstOrDefaultAsync(a => a.EmployeeId == employeeID && a.DeletedBy ==null);

            return employee;
        }

        public async Task<Employee> AddEmployee(Employee employee)
        {
            await _context.Employees.AddAsync(employee);

            await _context.SaveChangesAsync();


            return employee;
        }

        public async Task<Employee> UpdateEmployee(Employee employee)
        {
            await _context.SaveChangesAsync();

            return employee;
        }


        public async Task<bool> DeleteEmployee(Employee employee)
        {
            _context.Employees.Remove(employee);
            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<Employee> GetEmployeeByUnique(Employee employee)
        {
            var existingEmployee = await _context.Employees.FirstOrDefaultAsync(a => a.FirstName == employee.FirstName && a.LastName == employee.LastName && a.Email == employee.Email);

            return existingEmployee;
        }

        public async Task<Employee> GetDeletedEmployee(int employeeID)
        {
            var deletedEmployee = await _context.Employees.FirstOrDefaultAsync(a => a.EmployeeId == employeeID && a.DeletedBy != null);

            return deletedEmployee;

        }

        public async Task<Employee> GetPhoneNoOfEmployee(string phone)
        {
            var employeeWithPhone = await _context.Employees.FirstOrDefaultAsync(a => a.Phone == phone && a.DeletedBy == null);

            return employeeWithPhone;
        }

        public async Task<Employee> GetEmailOfEmployee(string email)
        {
            var employeeWithEmail = await _context.Employees.FirstOrDefaultAsync(a => a.Email == email && a.DeletedBy == null);

            return employeeWithEmail;

        }
    }
}
