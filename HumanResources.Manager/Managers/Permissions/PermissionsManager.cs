using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HumanResources.DataServices.Abstract;
using HumanResources.Manager.DTOs.Permissions;
using HumanResources.Manager.Validation.Permissions;

namespace HumanResources.Manager.Managers
{
    public class PermissionsManager
    {
        private readonly IPermissionsDataServices _permissionsDataServices;

        public PermissionsManager(IPermissionsDataServices permissionsDataServices)
        {
            _permissionsDataServices = permissionsDataServices;
        }

        public async Task<List<PermissionsDTO>> GetAllPermission()
        {
            var result = await _permissionsDataServices.GetAllPermission();

            return result.Select(x => PermissionsDTO.MapToDTO(x)).ToList();
        }

        public async Task<PermissionsDTO> GetPermissionById(int PermID)
        {
            var validation = new Dictionary<string, string>();

            if (PermID <= 0)
                validation.Add("PermID", "Invalid PermID");

            if (validation.Count == 0)
            {

                var result = await _permissionsDataServices.GetPermissionById(PermID);
                if (result is null)
                {
                    throw new PermissionsValidationException($"Permission with ID {PermID} does not exist.");
                }
                return PermissionsDTO.MapToDTO(result);

            }
            else
            {
                throw new PermissionsValidationException("Validation failed: " + string.Join(", ", validation.Select(kv => $"{kv.Key}: {kv.Value}")));
            }
        }

        public async Task<String> UpsertPermission(PermissionsDTO permissionsDTO)
        {
            var validation = new Dictionary<string, string>();
            if (permissionsDTO is null)
                validation.Add("Null", "Permission Cant Be Null");

            if (string.IsNullOrWhiteSpace(permissionsDTO.PermissionName))
            {
                validation.Add("EmptyPermissionName", "Permission Name Cant Be Null or WhiteSpace");

            }
            else if (string.IsNullOrWhiteSpace(permissionsDTO.PermissionDescription))
            {
                validation.Add("EmptyPermissionDescription", "Permission Description Cant Be Null or WhiteSpace");

            }

            if (validation.Count == 0)
            {
                var permissionEntity = PermissionsDTO.MapToEntity(permissionsDTO);

                var checkingData = await _permissionsDataServices.GetPermissionByUnique(permissionEntity);
                if (checkingData is not null)
                {
                    var updatedData = await _permissionsDataServices.UpdatePermission(permissionEntity);
                    return "Updated";
                }
                else
                {
                    var addData = await _permissionsDataServices.AddPermission(permissionEntity);
                    return "Added";
                }
            }
            else
            {
                throw new PermissionsValidationException("Validation failed: " + string.Join(", ", validation.Select(kv => $"{kv.Key}: {kv.Value}")));

            }
        }

        public async Task<bool> DeletePermission(int PermID)
        {
            var validation = new Dictionary<string, string>();

            if (PermID <= 0)
                validation.Add("PermID", "Invalid PermID");

            if (validation.Count == 0)
            {
                var checkingData = await _permissionsDataServices.GetPermissionById(PermID);

                if (checkingData is null)
                {
                    throw new PermissionsValidationException($"Permission with ID {PermID} does not exist.");
                }
                var result = await _permissionsDataServices.DeletePermission(PermID);
                return result;

            }
            else
            {
                throw new PermissionsValidationException("Validation failed: " + string.Join(", ", validation.Select(kv => $"{kv.Key}: {kv.Value}")));
            }
        }
    }
}
