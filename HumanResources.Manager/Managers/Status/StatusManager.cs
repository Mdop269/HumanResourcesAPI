using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HumanResources.DataServices.Abstract;
using HumanResources.Manager.DTOs;
using HumanResources.Manager.Validation.Status;

namespace HumanResources.Manager.Managers
{
    public class StatusManager
    {
        private readonly IStatusDataServices _statusDataServices;

        public StatusManager(IStatusDataServices statusDataServices)
        {
            _statusDataServices = statusDataServices;
        }

        public async Task<List<StatusDTO>> GetAllStatus()
        {
            var result = await _statusDataServices.GetAllStatus();

            return result.Select(x => StatusDTO.MapToDTO(x)).ToList();
        }

        public async Task<StatusDTO> GetStatusById(int statusID)
        {
            var validation = new Dictionary<string, string>();

            if (statusID <= 0)
                validation.Add("statusID", "Invalid statusID");

            if (validation.Count == 0)
            {

                var result = await _statusDataServices.GetStatusById(statusID);
                if (result is null)
                {
                    throw new StatusValidationException($"Status with ID {statusID} does not exist.");
                }
                return StatusDTO.MapToDTO(result);

            }
            else
            {
                throw new StatusValidationException("Validation failed: " + string.Join(", ", validation.Select(kv => $"{kv.Key}: {kv.Value}")));
            }
        }

        public async Task<String> UpsertStatus(StatusDTO statusDTO)
        {
            var validation = new Dictionary<string, string>();
            if (statusDTO is null)
                validation.Add("Null", "Status Cant Be Null");

            if (string.IsNullOrWhiteSpace(statusDTO.StatusName))
            {
                validation.Add("EmptyStatusName", "Staus Name Cant Be Null or WhiteSpace");

            }

            if (validation.Count == 0)
            {
                var statusEntity = StatusDTO.MapToEntity(statusDTO);

                var checkingData = await _statusDataServices.GetStatusByUnique(statusEntity);
                if (checkingData is not null)
                {
                    var updatedData = await _statusDataServices.UpdateStatus(statusEntity);
                    return "Updated";
                }
                else
                {
                    var addData = await _statusDataServices.AddStatus(statusEntity);
                    return "Added";
                }
            }
            else
            {
                throw new StatusValidationException("Validation failed: " + string.Join(", ", validation.Select(kv => $"{kv.Key}: {kv.Value}")));

            }
        }

        public async Task<bool> DeleteStatus(int statusID)
        {
            var validation = new Dictionary<string, string>();

            if (statusID <= 0)
                validation.Add("statusID", "Invalid statusID");

            if (validation.Count == 0)
            {
                var checkingData = await _statusDataServices.GetStatusById(statusID);

                if (checkingData is null)
                {
                    throw new StatusValidationException($"Status with ID {statusID} does not exist.");
                }
                var result = await _statusDataServices.DeleteStatus(statusID);

                return result;

            }
            else
            {
                throw new StatusValidationException("Validation failed: " + string.Join(", ", validation.Select(kv => $"{kv.Key}: {kv.Value}")));
            }
        }

        public async Task<String> UpsertStatusSP(StatusDTO statusDTO)
        {
            var validation = new Dictionary<string, string>();
            if (statusDTO is null)
                validation.Add("Null", "Status Cant Be Null");

            if (string.IsNullOrWhiteSpace(statusDTO.StatusName))
            {
                validation.Add("EmptyStatusName", "Staus Name Cant Be Null or WhiteSpace");

            }

            if (validation.Count == 0)
            {
                var statusEntity = StatusDTO.MapToEntity(statusDTO);

                var addData = await _statusDataServices.AddStatusSP(statusEntity);
                return "Added";
                
            }
            else
            {
                throw new StatusValidationException("Validation failed: " + string.Join(", ", validation.Select(kv => $"{kv.Key}: {kv.Value}")));

            }
        }
    }
}
