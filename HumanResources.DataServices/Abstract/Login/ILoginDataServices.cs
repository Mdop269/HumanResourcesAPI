using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HumanResourcesAPI.EntityData.EntityModels;

namespace HumanResources.DataServices.Abstract
{
    public interface ILoginDataServices
    {
        Task<List<Login>> GetAllLogin();

        Task<Login> GetLoginById(int loginID);

        Task<Login> AddLogin(Login login);

        Task<Login> UpdateLogin(Login login);

        Task<bool> DeleteLogin(Login login);

        Task<Login> GetLoginByUnique(Login login);

        Task<Login> GetLogin(string username, string password);

        Task<Login> GetUsername(string username);


    }
}
