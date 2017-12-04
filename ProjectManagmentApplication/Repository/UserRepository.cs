using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ProjectManagmentApplication.Models;

namespace ProjectManagmentApplication.Repository
{
    public class UserRepository
    {
        private Context Context;

        public UserRepository()
        {
            Context = new Context();
        }

        public void AddUser(User user)
        {
            Context.Users.Add(user);
            Context.SaveChanges();
        }

        public bool CheckIfUserExist(User user)
        {
            User foundUser = Context.Users.FirstOrDefault(u => u.Email == user.Email & u.Password == user.Password);
            return foundUser != null;
        }

        public User GetByEmailAndPassword(User user)
        {
            return Context.Users.FirstOrDefault(u => u.Email == user.Email & u.Password == user.Password);
        }
    }
}