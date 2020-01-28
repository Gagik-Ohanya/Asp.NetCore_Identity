using DAL.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DAL.Repositories
{
    public class UserRepository  : IUserRepository
    {
        private readonly AppDbContext _db;
        public UserRepository(AppDbContext db)
        {
            _db = db;
        }

        public IEnumerable<User> GetAll()
        {
            return _db.Users;
        }

        public User Get(int id)
        {
            return _db.Users.FirstOrDefault(u => u.Id == id);
        }

        public User Get(string username, string passwordHash)
        {
            return _db.Users.FirstOrDefault(u => u.Username == username && u.PasswordHash == passwordHash);
        }

        public User AddUser(User newUser)
        {
            _db.Add(newUser);
            return newUser;
        }

        public User Update(User updatedUser)
        {
            EntityEntry entity = _db.Attach(updatedUser);
            entity.State = EntityState.Modified;
            return updatedUser;
        }

        public User Delete(int id)
        {
            User user = _db.Users.FirstOrDefault(u => u.Id == id);
            if (user != null)
                _db.Users.Remove(user);
            return user;
        }

        public int Commit()
        {
            return _db.SaveChanges();
        }

    }
}