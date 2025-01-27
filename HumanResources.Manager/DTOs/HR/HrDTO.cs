using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HumanResourcesAPI.EntityData.EntityModels;

namespace HumanResources.Manager.DTOs.HR
{
    public class HrDTO
    {
        public string HrFirstName { get; set; } = null!;

        public string HrLastName { get; set; } = null!;

        public string HrEmail { get; set; } = null!;

        public int HrRoleId { get; set; }

        public string CreatedBy { get; set; } = null!;

        public string? ChangedBy { get; set; }


        public static Hr MapToEntity(HrDTO hrDTO)
        {
            return new Hr
            {
                HrFirstName = hrDTO.HrFirstName,
                HrLastName = hrDTO.HrLastName,
                HrEmail = hrDTO.HrEmail,
                HrRoleId = hrDTO.HrRoleId,
                CreatedBy = hrDTO.CreatedBy,
                ChangedBy = hrDTO.ChangedBy,
            };
        }

        public static HrDTO MapToDTO(Hr hr) => new HrDTO
        {

            HrFirstName = hr.HrFirstName,
            HrLastName = hr.HrLastName,
            HrEmail = hr.HrEmail,
            HrRoleId = hr.HrRoleId,
            CreatedBy = hr.CreatedBy,
            ChangedBy = hr.ChangedBy,
        };
    }
}
