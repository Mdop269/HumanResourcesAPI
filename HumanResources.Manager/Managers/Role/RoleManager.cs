using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HumanResources.DataServices.Abstract;
using HumanResources.Manager.DTOs;
using HumanResources.Manager.Validation.Role;

namespace HumanResources.Manager.Managers
{
    public class RoleManager
    {
        private readonly IRoleDataServices _dataServices;

        public RoleManager(IRoleDataServices dataServices)
        {
            _dataServices = dataServices;
        }

        public async Task<List<RoleDTO>> GetAllRole()
        {
            var result = await _dataServices.GetAllRole();

            return result.Select(x => RoleDTO.MapToDTO(x)).ToList();
        }

        public async Task<RoleDTO> GetRoleById(int RoleId)
        {
            var validation = new Dictionary<string, string>();

            if (RoleId <= 0)
                validation.Add("RoleId", "Invalid RoleId");

            if (validation.Count == 0)
            {

                var result = await _dataServices.GetRoleById(RoleId);
                if (result is null)
                {
                    throw new RoleValidationException($"Role with ID {RoleId} does not exist.");
                }
                return RoleDTO.MapToDTO(result);

            }
            else
            {
                throw new RoleValidationException("Validation failed: " + string.Join(", ", validation.Select(kv => $"{kv.Key}: {kv.Value}")));
            }
        }

        public async Task<String> UpsertRole(RoleDTO roleDTO)
        {
            var validation = new Dictionary<string, string>();
            if (roleDTO is null)
                validation.Add("Null", "Role Cant Be Null");

            if (string.IsNullOrWhiteSpace(roleDTO.RoleName))
            {
                validation.Add("EmptyRoleName", "Role Name Cant Be Null or WhiteSpace");

            }
            else if (string.IsNullOrWhiteSpace(roleDTO.RoleDescription))
            {
                validation.Add("EmptyRoleDescription", "Role Description Cant Be Null or WhiteSpace");

            }

            if (validation.Count == 0)
            {
                var roleEntity = RoleDTO.MapToEntity(roleDTO);

                var checkingData = await _dataServices.GetRoleByUnique(roleEntity);
                if (checkingData is not null)
                {
                    var updatedData = await _dataServices.UpdateRole(roleEntity);
                    return "Updated";
                }
                else
                {
                    var addData = await _dataServices.AddRole(roleEntity);
                    return "Added";
                }
            }
            else
            {
                throw new RoleValidationException("Validation failed: " + string.Join(", ", validation.Select(kv => $"{kv.Key}: {kv.Value}")));

            }
        }

        public async Task<bool> DeleteRole(int RoleId)
        {
            var validation = new Dictionary<string, string>();

            if (RoleId <= 0)
                validation.Add("RoleId", "Invalid RoleId");

            if (validation.Count == 0)
            {
                var checkingData = await _dataServices.GetRoleById(RoleId);

                if (checkingData is null)
                {
                    throw new RoleValidationException($"Role with ID {RoleId} does not exist.");
                }
                var result = await _dataServices.DeleteRole(RoleId);

                return result;

            }
            else
            {
                throw new RoleValidationException("Validation failed: " + string.Join(", ", validation.Select(kv => $"{kv.Key}: {kv.Value}")));
            }

        }
    }
}
