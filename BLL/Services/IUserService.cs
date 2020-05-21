using BLL.Models;
using DAL.Entities;
using System.Threading.Tasks;

namespace BLL.Services
{
    public interface IUserService
    {
        int Create(BllIdentity userIdentity);
        string LogIn(BllLoginUser user);
        User Authenticate(string username, string password);
        int CreateRole(string name);
        int AddUserToRole(int userId, int roleId);
        Task TestCache(string value);
    }
}