using DAL.Entities;
using System.Collections.Generic;

namespace DAL.Repositories
{
    public interface IRoleRepository
    {
        Role GetById(int id);
        IEnumerable<Role> GetAll();
        IEnumerable<UserRole> GetUserRoles(int userId);
        Role Update(Role updatedRole);
        Role AddRole(Role role);
        UserRole AddUserRole(UserRole role);
        Role Delete(int id);
        int Commit();
    }
}