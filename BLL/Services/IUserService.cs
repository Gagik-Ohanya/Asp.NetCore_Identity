using BLL.Models;

namespace BLL.Services
{
    public interface IUserService
    {
        int Create(BllIdentity userIdentity);
        string LogIn(BllLoginUser user);
        string Authenticate(BllLoginUser user);
    }
}