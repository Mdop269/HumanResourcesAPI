using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HumanResources.DataServices.Abstract;
using HumanResourcesAPI.EntityData.EntityModels;
using HumanResourcesAPI.EntityData;
using Microsoft.EntityFrameworkCore;

namespace HumanResources.DataServices.Services
{
    public class SalaryDataServices : ISalaryDataServices
    {
        private readonly HumanResourcesContext _context;

        public SalaryDataServices(HumanResourcesContext context)
        {
            _context = context;
        }

        public async Task<List<Salary>> GetAllSalary()
        {
            var result = await _context.Salaries.Where(a => a.DeletedBy == null).ToListAsync();

            return result;
        }

        public async Task<Salary> GetSalaryById(int salaryID)
        {
            var salary = await _context.Salaries.FirstOrDefaultAsync(a => a.SalaryId == salaryID && a.DeletedBy == null);

            return salary;
        }

        public async Task<Salary> AddSalary(Salary salary)
        {
            await _context.Salaries.AddAsync(salary);

            await _context.SaveChangesAsync();


            return salary;
        }

        public async Task<Salary> UpdateSalary(Salary salary)
        {
            await _context.SaveChangesAsync();

            return salary;
        }


        public async Task<bool> DeleteSalary(Salary salary)
        {
            _context.Salaries.Remove(salary);
            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<Salary> GetSalaryByUnique(Salary salary)
        {
            var existingSalary = await _context.Salaries.FirstOrDefaultAsync(a => a.EmployeeId == salary.EmployeeId );

            return existingSalary;
        }

        public async Task<Salary> GetDeletedSalary(int salaryID)
        {
            var deletedSalary = await _context.Salaries.FirstOrDefaultAsync(a => a.SalaryId == salaryID && a.DeletedBy != null);

            return deletedSalary;

        }
    }
}
