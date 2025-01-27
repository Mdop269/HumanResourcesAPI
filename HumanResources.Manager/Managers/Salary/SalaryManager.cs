using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HumanResources.DataServices.Abstract;
using HumanResources.DataServices.Services;
using HumanResources.Manager.DTOs;
using HumanResources.Manager.Validation;
using HumanResources.Manager.Validation.Employee;

namespace HumanResources.Manager.Managers
{
    public class SalaryManager
    {
        //this is better approach for getting thw date rather then new date() in this if the system date doesnt update it will be a prob
        private readonly DateOnly currentDate = DateOnly.FromDateTime(DateTime.Now);

        private readonly IHRDataServices _hrDataServices;
        private readonly IEmployeeDataServices _employeeDataServices;
        private readonly ISalaryDataServices _salaryDataServices;



        public SalaryManager(
            IEmployeeDataServices employeeDataServices,
            IHRDataServices hrDataServices,
            ISalaryDataServices salaryDataServices


            )
        {
            _employeeDataServices = employeeDataServices;
            _hrDataServices = hrDataServices;
            _salaryDataServices = salaryDataServices;
        }

        public async Task<List<SalaryDTO>> GetAllSalary()
        {
            var result = await _salaryDataServices.GetAllSalary();

            return result.Select(x => SalaryDTO.MapToDTO(x)).ToList();
        }

        public async Task<SalaryDTO> GetSalaryById(int salaryID)
        {
            var validation = new Dictionary<string, string>();

            if (salaryID <= 0)
                validation.Add("salaryID", "Invalid salaryID");

            if (validation.Count == 0)
            {

                var result = await _salaryDataServices.GetSalaryById(salaryID);
                if (result is null)
                {
                    throw new SalaryValidationException($"Salary with ID {salaryID} does not exist.");
                }
                return SalaryDTO.MapToDTO(result);

            }
            else
            {
                throw new SalaryValidationException("Validation failed: " + string.Join(", ", validation.Select(kv => $"{kv.Key}: {kv.Value}")));
            }
        }

        public async Task<String> UpsertSalary(SalaryDTO salaryDTO)
        {
            var validation = new Dictionary<string, string>();
            if (salaryDTO is null)
            {
                validation.Add("Null", "Salary Cant Be Null");
            }
            else if (salaryDTO.BaseSalary <= 0)
            {
                validation.Add("BaseSalary", "BaseSalary Cant Be Empty");

            }
            else if (salaryDTO.Deductions <= 0)
            {
                validation.Add("Deduction", "Deduction Cant Be Empty");
            }
            else if (salaryDTO.EmployeeId <= 0 || await _employeeDataServices.GetEmployeeById(salaryDTO.EmployeeId) == null)
            {
                validation.Add("WrongEmployeeId", "EmployeeId is Invalid");
            }
            else if (salaryDTO.CreatedBy >= 1 && await _hrDataServices.GetHrById((int)salaryDTO.CreatedBy) == null)
            {
                validation.Add("WrongCreatedBY", "Created By ID is Invalid");
            }
            else if (salaryDTO.ChangedBy >= 1 && await _hrDataServices.GetHrById((int)salaryDTO.ChangedBy) == null)
            {
                validation.Add("WrongChangedBY", "Changed By ID is Invalid");
            }


            if (validation.Count == 0)
            {
                var salaryEntity = SalaryDTO.MapToEntity(salaryDTO);

                var checkingData = await _salaryDataServices.GetSalaryByUnique(salaryEntity);
                if (checkingData is not null)
                {
                    if (checkingData.DeletedBy is not null || checkingData.DeletedOn is not null)
                    {
                        checkingData.BaseSalary = salaryDTO.BaseSalary;
                        checkingData.Deductions = salaryDTO.Deductions;
                        var totalSalary = salaryDTO.BaseSalary - salaryDTO.Deductions;
                        checkingData.TotalSalary = totalSalary;
                        checkingData.PayDate = salaryDTO.PayDate;
                        checkingData.CreatedBy = salaryDTO.CreatedBy;
                        checkingData.CreatedOn = currentDate;
                        checkingData.ChangedBy = null;
                        checkingData.ChangedOn = null;
                        checkingData.DeletedBy = null;
                        checkingData.DeletedOn = null;

                        var updatedDeletedData = await _salaryDataServices.UpdateSalary(salaryEntity);
                        return "UpdatedDeletedData";

                    }
                    else
                    {
                        if (salaryEntity.ChangedBy <= 0)
                        {
                            validation.Add("ChangedByCantbeEmpty", "Changed by is not Present");
                        }

                        if (validation.Count == 0)
                        {
                            checkingData.BaseSalary = salaryDTO.BaseSalary;
                            checkingData.Deductions = salaryDTO.Deductions;
                            var totalSalary = salaryDTO.BaseSalary - salaryDTO.Deductions;
                            checkingData.TotalSalary = totalSalary;
                            checkingData.PayDate = salaryDTO.PayDate;
                            checkingData.ChangedBy = salaryDTO.ChangedBy;
                            checkingData.ChangedOn = currentDate;

                            var updatedData = await _salaryDataServices.UpdateSalary(salaryEntity);
                            return "Updated";
                        }
                        else
                        {
                            throw new SalaryValidationException("Validation failed: " + string.Join(", ", validation.Select(kv => $"{kv.Key}: {kv.Value}")));
                        }
                    }
                }
                else
                {
                    if (salaryEntity.CreatedBy <= 0)
                    {
                        validation.Add("CreatedOnCantByEmpty", "Created By Cant Be null");
                    }

                    if (validation.Count == 0)
                    {
                        var totalSalary = salaryDTO.BaseSalary - salaryDTO.Deductions;
                        salaryEntity.TotalSalary = totalSalary;
                        salaryEntity.CreatedOn = currentDate;
                        salaryEntity.ChangedBy = null;
                        var addData = await _salaryDataServices.AddSalary(salaryEntity);

                        return "Added";
                    }
                    else
                    {
                        throw new SalaryValidationException("Validation failed: " + string.Join(", ", validation.Select(kv => $"{kv.Key}: {kv.Value}")));
                    }

                }
            }
            else
            {
                throw new SalaryValidationException("Validation failed: " + string.Join(", ", validation.Select(kv => $"{kv.Key}: {kv.Value}")));

            }
        }

        public async Task<bool> DeleteSalaryWithSoftDelete(int salaryID, int deletedBy)
        {
            var validation = new Dictionary<string, string>();

            if (salaryID <= 0 || deletedBy <= 0)
            {
                validation.Add("Enter Valid Details", " salaryID Or DeletedBy Is Invalid");
            }
            else if (await _hrDataServices.GetHrById(deletedBy) == null)
            {
                validation.Add("Hr Invalid", $" HR with {deletedBy} is Invalid");
            }
            else if (await _salaryDataServices.GetSalaryById(salaryID) == null)
            {
                validation.Add("Employee Invalid", $" Employee with {salaryID} is Invalid");
            }
            if (validation.Count == 0)
            {
                var salaryData = await _salaryDataServices.GetSalaryById(salaryID);

                salaryData.DeletedBy = deletedBy;
                salaryData.DeletedOn = currentDate;

                var result = await _salaryDataServices.UpdateSalary(salaryData);

                return true;
            }
            else
            {
                throw new SalaryValidationException("Validation failed: " + string.Join(", ", validation.Select(kv => $"{kv.Key}: {kv.Value}")));
            }
        }
    }
}
