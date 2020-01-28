using DAL.Entities;
using System.Collections.Generic;

namespace DAL.Repositories
{
    public interface IUserRepository
    {
        IEnumerable<User> GetAll();
        User Get(int id);
        User Get(string username, string passwordHash);
        User Update(User updatedUser);
        User AddUser(User user);
        User Delete(int id);
        int Commit();
    }
}