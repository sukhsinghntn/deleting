using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DynamicFormsApp.Shared.Models;

namespace DynamicFormsApp.Shared.Services
{
    public interface IUserService
    {
        Task<bool> ValidateUser(UserModel user);
        Task<UserModel> GetUserData(string userName);
        Task<List<UserModel>> GetAllUsers();
        Task<List<UserModel>> SearchUsers(string term);
    }
}
