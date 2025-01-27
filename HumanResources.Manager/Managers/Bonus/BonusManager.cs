using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HumanResources.DataServices.Abstract;
using HumanResources.Manager.DTOs;
using HumanResources.Manager.Validation.Bonus;
using HumanResources.Manager.Validation.Employee;

namespace HumanResources.Manager.Managers
{
    public class BonusManager
    {
        //this is better approach for getting thw date rather then new date() in this if the system date doesnt update it will be a prob
        private readonly DateOnly currentDate = DateOnly.FromDateTime(DateTime.Now);

        private readonly IHRDataServices _hrDataServices;
        private readonly IEmployeeDataServices _employeeDataServices;
        private readonly IBonusDataServices _bonusDataServices;



        public BonusManager(
            IEmployeeDataServices employeeDataServices,
            IHRDataServices hrDataServices,
            IBonusDataServices bonusDataServices


            )
        {
            _employeeDataServices = employeeDataServices;
            _hrDataServices = hrDataServices;
            _bonusDataServices = bonusDataServices;
        }

        public async Task<List<BonusDTO>> GetAllBonus()
        {
            var result = await _bonusDataServices.GetAllBonus();

            return result.Select(x => BonusDTO.MapToDTO(x)).ToList();
        }

        public async Task<BonusDTO> GetBonusById(int bonusID)
        {
            var validation = new Dictionary<string, string>();

            if (bonusID <= 0)
                validation.Add("bonusID", "Invalid bonusID");

            if (validation.Count == 0)
            {

                var result = await _bonusDataServices.GetBonusById(bonusID);
                if (result is null)
                {
                    throw new BonusValidationException($"Employee with ID {bonusID} does not exist.");
                }
                return BonusDTO.MapToDTO(result);

            }
            else
            {
                throw new BonusValidationException("Validation failed: " + string.Join(", ", validation.Select(kv => $"{kv.Key}: {kv.Value}")));
            }
        }

        public async Task<String> UpsertBonus(BonusDTO bonusDTO)
        {
            var validation = new Dictionary<string, string>();
            if (bonusDTO is null)
            {
                validation.Add("Null", "Employee Cant Be Null");
            }
            else if (string.IsNullOrWhiteSpace(bonusDTO.Reason) || bonusDTO.Reason.Length < 15)
            {
                validation.Add("EmptyReason", "Reason Cant Be Null or WhiteSpace or length cant be below 15 ");
            }
            else if (bonusDTO.BonusAmount <= 0)
            {
                validation.Add("Bonus", "Bonus cant be 0 or below");
            }
            else if (bonusDTO.EmployeeId <= 0 || await _employeeDataServices.GetEmployeeById(bonusDTO.EmployeeId) == null)
            {
                validation.Add("WrongEmployeeId", "EmployeeId is Invalid");
            }        
            else if (bonusDTO.CreatedBy >= 1 && await _hrDataServices.GetHrById((int)bonusDTO.CreatedBy) == null)
            {
                validation.Add("WrongCreatedBY", "Created By ID is Invalid");
            }
            else if (bonusDTO.ChangedBy >= 1 && await _hrDataServices.GetHrById((int)bonusDTO.ChangedBy) == null)
            {
                validation.Add("WrongChangedBY", "Changed By ID is Invalid");
            }


            if (validation.Count == 0)
            {
                var bonusEntity = BonusDTO.MapToEntity(bonusDTO);

                var checkingData = await _bonusDataServices.GetBonusByUnique(bonusEntity);
                if (checkingData is not null)
                {
                    if (checkingData.DeletedBy is not null || checkingData.DeletedOn is not null)
                    {
                        checkingData.BonusAmount = bonusDTO.BonusAmount;
                        checkingData.Reason = bonusDTO.Reason;
                        checkingData.CreatedBy = bonusDTO.CreatedBy;
                        checkingData.CreatedOn = currentDate;
                        checkingData.ChangedBy = null;
                        checkingData.ChangedOn = null;
                        checkingData.DeletedBy = null;
                        checkingData.DeletedOn = null;

                        var updatedDeletedData = await _bonusDataServices.UpdateBonus(bonusEntity);
                        return "UpdatedDeletedData";

                    }
                    else
                    {
                        if (bonusEntity.ChangedBy <= 0)
                        {
                            validation.Add("ChangedByCantbeEmpty", "Changed by is not Present");
                        }

                        checkingData.BonusAmount = bonusDTO.BonusAmount;
                        checkingData.Reason = bonusDTO.Reason;
                        checkingData.ChangedBy = bonusDTO.ChangedBy;
                        checkingData.ChangedOn = currentDate;

                        var updatedData = await _bonusDataServices.UpdateBonus(bonusEntity);
                        return "Updated";
                    }
                }
                else
                {
                    if (bonusEntity.CreatedBy <= 0)
                    {
                        validation.Add("CreatedOnCantByEmpty", "Created By Cant Be null");
                    }

                    if (validation.Count == 0)
                    {
                        
                        bonusEntity.DateAwarded = currentDate;
                        bonusEntity.CreatedOn = currentDate;
                        bonusEntity.ChangedBy = null;
                        var addData = await _bonusDataServices.AddBonus(bonusEntity);

                        return "Added";
                    }
                    else
                    {
                        throw new BonusValidationException("Validation failed: " + string.Join(", ", validation.Select(kv => $"{kv.Key}: {kv.Value}")));
                    }

                }
            }
            else
            {
                throw new BonusValidationException("Validation failed: " + string.Join(", ", validation.Select(kv => $"{kv.Key}: {kv.Value}")));

            }
        }

        public async Task<bool> DeleteBonusWithSoftDelete(int bonusID, int deletedBy)
        {
            var validation = new Dictionary<string, string>();

            if (bonusID <= 0 || deletedBy <= 0)
            {
                validation.Add("Enter Valid Details", " bonusID Or DeletedBy Is Invalid");
            }
            else if (await _hrDataServices.GetHrById(deletedBy) == null)
            {
                validation.Add("Hr Invalid", $" HR with {deletedBy} is Invalid");
            }
            else if (await _bonusDataServices.GetBonusById(bonusID) == null)
            {
                validation.Add("Bonus Invalid", $" Bonus with {bonusID} is Invalid");
            }
            if (validation.Count == 0)
            {
                var bonusData = await _bonusDataServices.GetBonusById(bonusID);

                bonusData.DeletedBy = deletedBy;
                bonusData.DeletedOn = currentDate;

                var result = await _bonusDataServices.UpdateBonus(bonusData);

                return true;
            }
            else
            {
                throw new BonusValidationException("Validation failed: " + string.Join(", ", validation.Select(kv => $"{kv.Key}: {kv.Value}")));
            }
        }
    }
}
