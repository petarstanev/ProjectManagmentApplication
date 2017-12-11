using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AutoMapper;
using ProjectManagmentApplication.Helpers;
using ProjectManagmentApplication.Models;
using ProjectManagmentApplication.ViewModels;

namespace ProjectManagmentApplication.Repository
{
    public class UserRepository
    {
        private Context Context;

        public UserRepository()
        {
            Context = new Context();
        }

        public void RegisterUser(RegisterUser user)
        {
            user.Password = HashingHelper.HashPassword(user.Password);

            var config = new MapperConfiguration(cfg => {
                cfg.CreateMap<RegisterUser, User>();
            });
            IMapper iMapper = config.CreateMapper();            
            User modelUser = iMapper.Map<RegisterUser, User>(user);

            Context.Users.Add(modelUser);
            Context.SaveChanges();
        }

        public bool CheckEmailIsUnique(RegisterUser user){
            bool emailExist = Context.Users.Any(u => u.Email == user.Email);
            return !emailExist;
        }


        public bool CheckIfUserExist(User user)
        {
            User foundUser = Context.Users.FirstOrDefault(u => u.Email == user.Email & u.Password == user.Password);
            return foundUser != null;
        }

        public User GetByEmailAndPassword(LoginUser user)
        {
            user.Password = HashingHelper.HashPassword(user.Password);
            return Context.Users.FirstOrDefault(u => u.Email == user.Email & u.Password == user.Password);
        }
    }
}