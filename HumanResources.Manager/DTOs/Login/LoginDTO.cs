using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HumanResourcesAPI.EntityData.EntityModels;

namespace HumanResources.Manager.DTOs
{
    public class LoginDTO
    {
        public string? Username { get; set; }

        public string Password { get; set; } = null!;

        public int? SaltKeyId { get; set; }

        public int StatusId { get; set; }

        public int EmployeeId { get; set; }

        [Required, DataType(DataType.Date)]
        public DateTime LoginTime { get; set; }

        [DataType(DataType.Date)]
        public DateTime? LogoutTime { get; set; }

        public static Login MapToEntity(LoginDTO loginDTO)
        {
            return new Login
            {
                Username = loginDTO.Username,
                Password = loginDTO.Password,
                SaltKeyId = loginDTO.SaltKeyId,
                StatusId = loginDTO.StatusId,
                EmployeeId = loginDTO.EmployeeId,
                LoginTime = loginDTO.LoginTime,
                LogoutTime = loginDTO.LogoutTime,
            };
        }

        public static LoginDTO MapToDTO(Login login) => new LoginDTO
        {
            Username = login.Username,
            Password = login.Password,
            SaltKeyId = login.SaltKeyId,
            StatusId = login.StatusId,
            EmployeeId = login.EmployeeId,
            LoginTime = login.LoginTime,
            LogoutTime = login.LogoutTime,
        };
    }
}
