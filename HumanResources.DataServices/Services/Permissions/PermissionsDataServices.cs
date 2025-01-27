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
    public class PermissionsDataServices : IPermissionsDataServices
    {
        private readonly HumanResourcesContext _context;

        public PermissionsDataServices(HumanResourcesContext context)
        {
            _context = context;
        }

        public async Task<List<Permission>> GetAllPermission()
        {
            var result = await _context.Permissions.ToListAsync();

            return result;
        }

        public async Task<Permission> GetPermissionById(int PermID)
        {
            var permission = await _context.Permissions.FindAsync(PermID);

            return permission;
        }

        public async Task<Permission> AddPermission(Permission permission)
        {
            await _context.Permissions.AddAsync(permission);

            await _context.SaveChangesAsync();

            return permission;
        }

        public async Task<Permission> UpdatePermission(Permission permission)
        {
            var existingPermission = await _context.Permissions.FirstOrDefaultAsync(a => a.PermissionName == permission.PermissionName);
            existingPermission.PermissionName = permission.PermissionName;
            existingPermission.PermissionDescription = permission.PermissionDescription;
            await _context.SaveChangesAsync();
            return permission;
        }


        public async Task<bool> DeletePermission(int PermID)
        {
            var existingPermission = await _context.Permissions.FindAsync(PermID);
            _context.Permissions.Remove(existingPermission);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<Permission> GetPermissionByUnique(Permission permission)
        {
            var existingPermission = await _context.Permissions.FirstOrDefaultAsync(a => a.PermissionName == permission.PermissionName);

            return existingPermission;
        }
    }
}
