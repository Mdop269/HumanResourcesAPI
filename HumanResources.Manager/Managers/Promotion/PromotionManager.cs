using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HumanResources.DataServices.Abstract;
using HumanResources.Manager.DTOs;
using HumanResources.Manager.Validation.Employee;
using HumanResources.Manager.Validation.Promotion;

namespace HumanResources.Manager.Managers
{
    public class PromotionManager
    {
        //this is better approach for getting thw date rather then new date() in this if the system date doesnt update it will be a prob
        private readonly DateOnly currentDate = DateOnly.FromDateTime(DateTime.Now);

        private readonly IHRDataServices _hrDataServices;
        private readonly IEmployeeDataServices _employeeDataServices;
        private readonly IPromotionDataServices _promotionDataServices;
        private readonly IRoleDataServices _roleDataServices;



        public PromotionManager(
            IEmployeeDataServices employeeDataServices,
            IHRDataServices hrDataServices,
            IPromotionDataServices promotionDataServices,
            IRoleDataServices roleDataServices


            )
        {
            _employeeDataServices = employeeDataServices;
            _hrDataServices = hrDataServices;
            _promotionDataServices = promotionDataServices;
            _roleDataServices = roleDataServices;
        }

        public async Task<List<PromotionDTO>> GetAllPromotion()
        {
            var result = await _promotionDataServices.GetAllPromotion();

            return result.Select(x => PromotionDTO.MapToDTO(x)).ToList();
        }

        public async Task<PromotionDTO> GetPromotionById(int promotionID)
        {
            var validation = new Dictionary<string, string>();

            if (promotionID <= 0)
                validation.Add("promotionID", "Invalid promotionID");

            if (validation.Count == 0)
            {

                var result = await _promotionDataServices.GetPromotionById(promotionID);
                if (result is null)
                {
                    throw new PromotionValidationException($"Promotion with ID {promotionID} does not exist.");
                }
                return PromotionDTO.MapToDTO(result);

            }
            else
            {
                throw new PromotionValidationException("Validation failed: " + string.Join(", ", validation.Select(kv => $"{kv.Key}: {kv.Value}")));
            }
        }

        public async Task<String> UpsertPromotion(PromotionDTO promotionDTO)
        {
            var validation = new Dictionary<string, string>();
            if (promotionDTO is null)
            {
                validation.Add("Null", "Employee Cant Be Null");
            }
            else if (string.IsNullOrWhiteSpace(promotionDTO.Reason) || promotionDTO.Reason.Length < 15)
            {
                validation.Add("EmptyReason", "Reason Cant Be Null or WhiteSpace or length cant be below 15 ");
            }
            else if (promotionDTO.OldRoleId <= 0 || await _roleDataServices.GetRoleById(promotionDTO.OldRoleId) == null)
            {
                validation.Add("OldRoleId", "OldROleID is Invalid");
            }
            else if (promotionDTO.NewRoleId <= 0 || await _roleDataServices.GetRoleById(promotionDTO.OldRoleId) == null)
            {
                validation.Add("NewRoleID", "NewRoleID is Invalid");
            }
            else if (promotionDTO.EmployeeId <= 0 || await _employeeDataServices.GetEmployeeById(promotionDTO.EmployeeId) == null)
            {
                validation.Add("WrongEmployeeId", "EmployeeId is Invalid");
            }
            else if (promotionDTO.CreatedBy >= 1 && await _hrDataServices.GetHrById((int)promotionDTO.CreatedBy) == null)
            {
                validation.Add("WrongCreatedBY", "Created By ID is Invalid");
            }
            else if (promotionDTO.ChangedBy >= 1 && await _hrDataServices.GetHrById((int)promotionDTO.ChangedBy) == null)
            {
                validation.Add("WrongChangedBY", "Changed By ID is Invalid");
            }


            if (validation.Count == 0)
            {
                var promotionEntity = PromotionDTO.MapToEntity(promotionDTO);

                var checkingData = await _promotionDataServices.GetPromotionByUnique(promotionEntity);
                if (checkingData is not null)
                {
                    if (checkingData.DeletedBy is not null || checkingData.DeletedOn is not null)
                    {
                       
                        checkingData.NewRoleId = promotionDTO.NewRoleId; 
                        checkingData.PromotionDate = currentDate;
                        checkingData.Reason = promotionDTO.Reason;
                        checkingData.CreatedBy = promotionDTO.CreatedBy;
                        checkingData.CreatedOn = currentDate;
                        checkingData.ChangedBy = null;
                        checkingData.ChangedOn = null;
                        checkingData.DeletedBy = null;
                        checkingData.DeletedOn = null;

                        var updatedDeletedData = await _promotionDataServices.UpdatePromotion(promotionEntity);
                        return "UpdatedDeletedData";

                    }
                    else
                    {
                        if (promotionEntity.ChangedBy <= 0)
                        {
                            validation.Add("ChangedByCantbeEmpty", "Changed by is not Present");
                        }

                        if (validation.Count == 0)
                        {

                            checkingData.NewRoleId = promotionDTO.NewRoleId;
                            checkingData.Reason = promotionDTO.Reason;
                            checkingData.ChangedBy = promotionDTO.ChangedBy;
                            checkingData.ChangedOn = currentDate;

                            var updatedData = await _promotionDataServices.UpdatePromotion(promotionEntity);
                            return "Updated";
                        }
                        else
                        {
                            throw new PromotionValidationException("Validation failed: " + string.Join(", ", validation.Select(kv => $"{kv.Key}: {kv.Value}")));
                        }
                    }
                }
                else
                {
                    if (promotionEntity.CreatedBy <= 0)
                    {
                        validation.Add("CreatedOnCantByEmpty", "Created By Cant Be null");
                    }

                    if (validation.Count == 0)
                    {
                        
                        promotionEntity.PromotionDate = currentDate;
                        promotionEntity.CreatedOn = currentDate;
                        promotionEntity.ChangedBy = null;
                        var addData = await _promotionDataServices.AddPromotion(promotionEntity);

                        return "Added";
                    }
                    else
                    {
                        throw new PromotionValidationException("Validation failed: " + string.Join(", ", validation.Select(kv => $"{kv.Key}: {kv.Value}")));
                    }

                }
            }
            else
            {
                throw new PromotionValidationException("Validation failed: " + string.Join(", ", validation.Select(kv => $"{kv.Key}: {kv.Value}")));

            }
        }

        public async Task<bool> DeletePromotionWithSoftDelete(int promotionID, int deletedBy)
        {
            var validation = new Dictionary<string, string>();

            if (promotionID <= 0 || deletedBy <= 0)
            {
                validation.Add("Enter Valid Details", " promotionID Or DeletedBy Is Invalid");
            }
            else if (await _hrDataServices.GetHrById(deletedBy) == null)
            {
                validation.Add("Hr Invalid", $" HR with {deletedBy} is Invalid");
            }
            else if (await _promotionDataServices.GetPromotionById(promotionID) == null)
            {
                validation.Add("Employee Invalid", $" Employee with {promotionID} is Invalid");
            }
            if (validation.Count == 0)
            {
                var EmployeeData = await _promotionDataServices.GetPromotionById(promotionID);

                EmployeeData.DeletedBy = deletedBy;
                EmployeeData.DeletedOn = currentDate;

                var result = await _promotionDataServices.UpdatePromotion(EmployeeData);

                return true;
            }
            else
            {
                throw new PromotionValidationException("Validation failed: " + string.Join(", ", validation.Select(kv => $"{kv.Key}: {kv.Value}")));
            }
        }
    }
}
