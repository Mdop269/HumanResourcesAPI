using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HumanResources.DataServices.Abstract;
using HumanResourcesAPI.EntityData.EntityModels;
using HumanResourcesAPI.EntityData;
using Microsoft.EntityFrameworkCore;

namespace HumanResources.DataServices.Services
{
    public class LoginDataServices : ILoginDataServices
    {
        private readonly HumanResourcesContext _context;

        public LoginDataServices(HumanResourcesContext context)
        {
            _context = context;
        }

        public async Task<List<Login>> GetAllLogin()
        {
            var result = await _context.Logins.ToListAsync();

            return result;
        }

        public async Task<Login> GetLoginById(int loginID)
        {
            var login = await _context.Logins.FirstOrDefaultAsync(a => a.LoginId == loginID);

            return login;
        }

        public async Task<Login> AddLogin(Login login)
        {
            await _context.Logins.AddAsync(login);

            await _context.SaveChangesAsync();


            return login;
        }

        public async Task<Login> UpdateLogin(Login login)
        {
            await _context.SaveChangesAsync();

            return login;
        }


        public async Task<bool> DeleteLogin(Login login)
        {
            _context.Logins.Remove(login);
            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<Login> GetLoginByUnique(Login login)
        {
            var existingLogin = await _context.Logins.FirstOrDefaultAsync(a => a.EmployeeId == login.EmployeeId);

            return existingLogin;
        }

        public async Task<Login> GetLogin(string username, string password)
        {
            var user = await _context.Logins.FirstOrDefaultAsync(a => a.Username == username && a.Password == password);

            return user;
        }

        public async Task<Login> GetUsername(string username)
        {
            var user = await _context.Logins.FirstOrDefaultAsync(a => a.Username == username );

            return user;
        }
    }
}
