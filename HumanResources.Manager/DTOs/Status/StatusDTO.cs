using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HumanResourcesAPI.EntityData.EntityModels;

namespace HumanResources.Manager.DTOs
{
    public class StatusDTO
    {
        [Required, Length(1, 49)]
        public string StatusName { get; set; } = null!;
        public static Status MapToEntity(StatusDTO statusDTO)
        {
            return new Status
            {
                StatusName = statusDTO.StatusName,
            };
        }

        public static StatusDTO MapToDTO(Status status)
        {
            return new StatusDTO
            {
                StatusName = status.StatusName,
            };
        }
    }
}
