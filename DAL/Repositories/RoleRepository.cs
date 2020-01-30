using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DAL.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace DAL.Repositories
{
    public class RoleRepository : IRoleRepository
    {
        private readonly AppDbContext _db;

        public RoleRepository(AppDbContext db)
        {
            _db = db;
        }

        public Role GetById(int id)
        {
            return _db.Roles.FirstOrDefault(r => r.Id == id);
        }

        public IEnumerable<Role> GetAll()
        {
            return _db.Roles;
        }

        public IEnumerable<UserRole> GetUserRoles(int userId)
        {
            return _db.UserRoles.Include(ur => ur.Role).Where(ur => ur.UserId == userId);
        }

        public Role AddRole(Role role)
        {
            _db.Roles.Add(role);
            return role;
        }

        public UserRole AddUserRole(UserRole userRole)
        {
            _db.UserRoles.Add(userRole);
            return userRole;
        }

        public Role Update(Role updatedRole)
        {
            EntityEntry entity = _db.Roles.Attach(updatedRole);
            entity.State = EntityState.Modified;
            return updatedRole;
        }

        public Role Delete(int id)
        {
            Role role = _db.Roles.FirstOrDefault(r => r.Id == id);
            if (role != null)
                _db.Roles.Remove(role);
            return role;
        }

        public int Commit()
        {
            return _db.SaveChanges();
        }
    }
}