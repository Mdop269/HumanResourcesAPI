using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HumanResourcesAPI.EntityData.EntityModels;
using HumanResourcesAPI.EntityData;
using Microsoft.EntityFrameworkCore;
using HumanResources.DataServices.Abstract;

namespace HumanResources.DataServices.Services
{
    public class DepartmentDataServices : IDepartmentDataServices
    {
        private HumanResourcesContext _context;

        public DepartmentDataServices(HumanResourcesContext context)
        {
            _context = context;
        }
        public async Task<List<Department>> GetAllDepartments()
        {
            var result = await _context.Departments.ToListAsync();

            return result;
        }

        public async Task<Department> GetDepartmentById(int DepId)
        {
            var department = await _context.Departments.FindAsync(DepId);

            return department;
        }

        public async Task<Department> AddDepartment(Department department)
        {
            await _context.Departments.AddAsync(department);

            await _context.SaveChangesAsync();

            return department;
        }

        public async Task<Department> UpdateDepartment(Department department)
        {
            var existingDepartment = await _context.Departments.FirstOrDefaultAsync(a => a.DepartmentName == department.DepartmentName);
            existingDepartment.DepartmentName = department.DepartmentName;
            existingDepartment.Description = department.Description;
            await _context.SaveChangesAsync();
            return department;
        }

        public async Task<bool> DeleteDepartment(int DepID)
        {
            var existingDepartment = await _context.Departments.FindAsync(DepID);

            _context.Departments.Remove(existingDepartment);

            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<Department> GetDepartmentByUnique(Department department)
        {
            var existingDepartment = await _context.Departments.FirstOrDefaultAsync(a => a.DepartmentName == department.DepartmentName);

            return existingDepartment;
        }
    }
}
