using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HumanResources.DataServices.Abstract;
using HumanResources.DataServices.Services;
using HumanResources.Manager.DTOs;
using HumanResources.Manager.DTOs.HR;
using HumanResources.Manager.Validation.HR;
using HumanResources.Manager.Validation.Role;
using HumanResourcesAPI.EntityData.EntityModels;

namespace HumanResources.Manager.Managers.HR
{
    public class HrManager
    {
        //this is better approach for getting thw date rather then new date() in this if the system date doesnt update it will be a prob
        private readonly DateOnly currentDate = DateOnly.FromDateTime(DateTime.Now);

        private readonly IHRDataServices _hrDataServices;
        private readonly IRoleDataServices _roleDataServices;

        public HrManager(IHRDataServices hrDataServices, IRoleDataServices roleDataServices)
        {
            _hrDataServices = hrDataServices;
            _roleDataServices = roleDataServices;
        }

        public async Task<List<HrDTO>> GetAllHr()
        {
            var result =  await _hrDataServices.GetAllHr();

            return result.Select(x => HrDTO.MapToDTO(x)).ToList();

        }

        public async Task<HrDTO> GetHrById(int HrId)
        {
            var validation = new Dictionary<string, string>();

            if (HrId <= 0)
            {
                validation.Add("HrID", "Invalid HrID");
            }

            if (validation.Count == 0)
            {
                var result = await _hrDataServices.GetHrById(HrId);
                if (result is null)
                {
                    throw new HrValidationException($"HR with ID {HrId} does not exist.");
                }
                return HrDTO.MapToDTO(result);
            }
            else
            {
                throw new HrValidationException("Validation failed: " + string.Join(", ", validation.Select(kv => $"{kv.Key}: {kv.Value}")));
            }
        }

        public async Task<String> UpsertHR(HrDTO hrDTO)
        {
            var validation = new Dictionary<string, string>();
            if (hrDTO is null)
                validation.Add("Null", "HR Cant Be Null");

            if (string.IsNullOrWhiteSpace(hrDTO.HrFirstName))
            {
                validation.Add("EmptyHrFirstName", "First Name Cant Be Null or WhiteSpace");

            }
            else if (string.IsNullOrWhiteSpace(hrDTO.HrLastName))
            {
                validation.Add("EmptyHrLastName", "Last Name Cant Be Null or WhiteSpace");
            }
            else if (string.IsNullOrWhiteSpace(hrDTO.HrEmail))
            {
                validation.Add("EmptyHrEmail", "Email Cant Be Null or WhiteSpace");
            }
            else if (string.IsNullOrWhiteSpace(hrDTO.HrEmail))
            {
                validation.Add("EmptyHrEmail", "Email Cant Be Null or WhiteSpace");
            }
            else if (string.IsNullOrWhiteSpace(hrDTO.CreatedBy))
            {
                validation.Add("EmptyCreatedBY", "Created By Cant Be Null or WhiteSpace");
            }

            if (validation.Count == 0)
            {
                
                var hrEntity = HrDTO.MapToEntity(hrDTO);

                
                var checkingData = await _hrDataServices.GetHrByUnique(hrEntity);
                if (checkingData is not null)
                {
                    if(checkingData.DeletedBy is not null || checkingData.DeletedOn is not null)
                    { 
                        checkingData.HrRoleId = hrDTO.HrRoleId;
                        checkingData.CreatedBy = hrDTO.CreatedBy;
                        checkingData.CreatedOn = currentDate;
                        checkingData.ChangedBy = null;
                        checkingData.ChangedOn = null;
                        checkingData.DeletedBy = null;
                        checkingData.DeletedOn = null;

                        var updatedDeletedData = await _hrDataServices.UpdateHr(checkingData);
                        return "UpdatedDeletedData";

                    }
                    else
                    {
                        if (hrEntity.ChangedBy is null || string.IsNullOrWhiteSpace(hrEntity.ChangedBy))
                        {
                            validation.Add("ChangedByCantbeFill", "Changed by is not Present");
                        }

                        var roleId = await _roleDataServices.GetRoleById(hrEntity.HrRoleId);
                        if(roleId is null)
                        {
                            validation.Add("RoleID is Wrong", "Role ID is not Available");
                        }
                        
                        if(validation.Count == 0)
                        {
                            checkingData.HrRoleId = hrDTO.HrRoleId;
                            checkingData.ChangedBy = hrDTO.ChangedBy;
                            checkingData.ChangedOn = currentDate;
                            var updatedData = await _hrDataServices.UpdateHr(hrEntity);
                            return "Updated";
                        }
                        else
                        {
                            throw new HrValidationException("Validation failed: " + string.Join(", ", validation.Select(kv => $"{kv.Key}: {kv.Value}")));
                        }
                    }
                    
                }
                else
                {
                   
                    if(hrEntity.CreatedBy is null || string.IsNullOrWhiteSpace(hrEntity.CreatedBy))
                    {
                        validation.Add("CreatedOnCantbeEmpty", "Created On Cant Be null");
                    } 

                    var roleId = await _roleDataServices.GetRoleById(hrEntity.HrRoleId);

                    if (roleId is null)
                    {
                        validation.Add("RoleID is Wrong", "Role ID is not Available");
                    }

                    if (validation.Count == 0)
                    {
                        hrEntity.CreatedOn = currentDate;
                        hrEntity.ChangedBy = null;
                        var addData = await _hrDataServices.AddHr(hrEntity);

                        return "Added";
                    }
                    else
                    {
                        throw new HrValidationException("Validation failed: " + string.Join(", ", validation.Select(kv => $"{kv.Key}: {kv.Value}")));
                    }

                }
            }
            else
            {
                throw new HrValidationException("Validation failed: " + string.Join(", ", validation.Select(kv => $"{kv.Key}: {kv.Value}")));
            }
        }


        public async Task<bool> DeleteHrWithSoftDelete(int HrId, string deletedBy)
        {
            var validation = new Dictionary<string, string>();

            if (HrId <= 0 || string.IsNullOrWhiteSpace(deletedBy))
            {
                validation.Add("Enter Valid Details", " HrId Or DeletedBy Is Invalid");
            }
            else if (deletedBy.Length <= 5)
            {
                validation.Add("Name Invalid", " Name length cant be less then 5");
            }
            if (validation.Count == 0)
            {
                var hrData = await _hrDataServices.GetHrById(HrId);
                if (hrData is null)
                {
                    throw new HrValidationException($"HR with ID {HrId} does not exist.");
                } 

                hrData.DeletedBy = deletedBy;
                hrData.DeletedOn = currentDate;

                var result = await _hrDataServices.UpdateHr(hrData);

                return true;
            }
            else
            {
                throw new HrValidationException("Validation failed: " + string.Join(", ", validation.Select(kv => $"{kv.Key}: {kv.Value}")));

            }
        
        }
    }
}
