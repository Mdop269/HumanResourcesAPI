using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HumanResources.DataServices.Abstract;
using HumanResourcesAPI.EntityData;
using HumanResourcesAPI.EntityData.EntityModels;
using Microsoft.EntityFrameworkCore;

namespace HumanResources.DataServices.Services
{
    public class RoleDataServices : IRoleDataServices
    {
        private readonly HumanResourcesContext _context;

        public RoleDataServices(HumanResourcesContext context)
        {
            _context = context;
        }

        public async Task<List<Role>> GetAllRole()
        {
            var result = await _context.Roles.ToListAsync();

            return result;
        }

        public async Task<Role> GetRoleById(int RoleId)
        {
            var role = await _context.Roles.FindAsync(RoleId);

            return role;
        }

        public async Task<Role> AddRole(Role role)
        {
            await _context.Roles.AddAsync(role);

            await _context.SaveChangesAsync();

            return role;
        }

        public async Task<Role> UpdateRole(Role role)
        {
            var existingRole = await _context.Roles.FirstOrDefaultAsync(a => a.RoleName == role.RoleName);
            existingRole.RoleName = role.RoleName;
            existingRole.RoleDescription = role.RoleDescription;
            await _context.SaveChangesAsync();
            return role;
        }


        public async Task<bool> DeleteRole(int RoleId)
        {
            var existingRole = await _context.Roles.FindAsync(RoleId);
            _context.Roles.Remove(existingRole);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<Role> GetRoleByUnique(Role role)
        {
            var existingRole = await _context.Roles.FirstOrDefaultAsync(a => a.RoleName == role.RoleName);

            return existingRole;
        }
    }
}
