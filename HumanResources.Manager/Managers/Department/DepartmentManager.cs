using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HumanResources.DataServices.Abstract;
using HumanResources.Manager.DTOs;
using HumanResources.Manager.Validation.Department;

namespace HumanResources.Manager.Managers
{
    public class DepartmentManager
    {
        private IDepartmentDataServices _departmentDataService;

        public DepartmentManager(IDepartmentDataServices departmentDataService)
        {
            _departmentDataService = departmentDataService;
        }

        public async Task<List<DepartmentDTO>> GetAllDepartments()
        {
            var result = await _departmentDataService.GetAllDepartments();

            return result.Select(x => DepartmentDTO.MapToDTO(x)).ToList();
        }

        public async Task<DepartmentDTO> GetDepartmentById(int DepId)
        {
            var validation = new Dictionary<string, string>();
            if (DepId <= 0)
                validation.Add("DepId", "Invalid DepId");

            if (validation.Count == 0)
            {
                var result = await _departmentDataService.GetDepartmentById(DepId);
                if (result is null)
                {
                    throw new DepartmentValidationException($"Department with {DepId} Does Not exist! ");
                }
                return DepartmentDTO.MapToDTO(result);
            }
            else
            {
                throw new DepartmentValidationException("Validation Failed: " + string.Join(", ", validation.Select(kv => $"{kv.Key}: {kv.Value}")));
            }

        }

        public async Task<String> UpsertDepartment(DepartmentDTO departmentDTO)
        {
            var validation = new Dictionary<string, string>();
            if (departmentDTO is null)
            {
                validation.Add("Null", "Departmetn Cannot Be Null");
            }
            else if (string.IsNullOrWhiteSpace(departmentDTO.DepartmentName))
            {
                validation.Add("DepartmentName", "DepartmentName Cannot Be Null OR empty");
            }
            else if (string.IsNullOrWhiteSpace(departmentDTO.Description))
            {
                validation.Add("DepartmentDescription", "DepartmentDescription Cannot Be Null OR empty");
            }

            if (validation.Count == 0)
            {
                var departmentEntity = DepartmentDTO.MapToEntity(departmentDTO);

                var checkingData = await _departmentDataService.GetDepartmentByUnique(departmentEntity);
                if (checkingData is not null)
                {
                    var updatedDepartment = await _departmentDataService.UpdateDepartment(departmentEntity);
                    return "Updated";
                }
                else
                {
                    var addedNewDepartment = await _departmentDataService.AddDepartment(departmentEntity);
                    return "Added";
                }
            }
            else
            {
                throw new DepartmentValidationException("Validation Failed: " + string.Join(", ", validation.Select(kv => $"{kv.Key}: {kv.Value}")));
            }
        }

        public async Task<bool> DeleteDepartment(int DepID)
        {
            var validation = new Dictionary<string, string>();

            if (DepID <= 0)
                validation.Add("DepId", "Invalid DepId");

            if (validation.Count == 0)
            {
                var checkingData = await _departmentDataService.GetDepartmentById(DepID);
                if (checkingData is null)
                {
                    throw new DepartmentValidationException($"Department With {DepID} Does Not Exist");
                }
                else
                {
                    var result = await _departmentDataService.DeleteDepartment(DepID);

                    return result;
                }
            }
            else
            {
                throw new DepartmentValidationException($"Validation Failed: " + string.Join(", ", validation.Select(kv => $"{kv.Key}: {kv.Value}")));
            }
        }
    }
}
