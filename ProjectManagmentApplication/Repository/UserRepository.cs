using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Web;
using AutoMapper;
using ProjectManagementApplication.Helpers;
using ProjectManagementApplication.Models;
using ProjectManagementApplication.ViewModels;

namespace ProjectManagementApplication.Repository
{
    public class UserRepository
    {
        private Context db;
        SessionContext sessionContext = new SessionContext();

        public UserRepository()
        {
            db = new Context();
        }

        public User RegisterUser(RegisterUser user)
        {
            user.Password = HashingHelper.HashPassword(user.Password);

            var config = new MapperConfiguration(cfg => {
                cfg.CreateMap<RegisterUser, User>();
            });
            IMapper iMapper = config.CreateMapper();            
            User modelUser = iMapper.Map<RegisterUser, User>(user);

            db.Users.Add(modelUser);
            db.SaveChanges();

            return modelUser;
        }

        public bool CheckEmailIsUnique(RegisterUser user){
            bool emailExist = db.Users.Any(u => u.Email == user.Email);
            return !emailExist;
        }


        public bool CheckIfUserExist(User user)
        {
            User foundUser = db.Users.FirstOrDefault(u => u.Email == user.Email & u.Password == user.Password);
            return foundUser != null;
        }

        public User GetByEmailAndPassword(LoginUser user)
        {
            var config = new MapperConfiguration(cfg => {
                cfg.CreateMap<LoginUser, User>();
            });
            IMapper iMapper = config.CreateMapper();
            User searchUser = iMapper.Map<LoginUser, User>(user);

            searchUser.Password = HashingHelper.HashPassword(searchUser.Password);
            return db.Users.FirstOrDefault(u => u.Email == searchUser.Email && u.Password == searchUser.Password);
        }

        public void UpdateUsername(string updatedUsername)
        {
            User sessionUser = sessionContext.GetUserData();
            sessionUser.Name = updatedUsername;

            UpdateDbAndSession(sessionUser);
        }

        public void UpdatePassword(string password)
        {
            password = HashingHelper.HashPassword(password);
            User sessionUser = sessionContext.GetUserData();
            sessionUser.Password = password;

            UpdateDbAndSession(sessionUser);
        }

        private void UpdateDbAndSession(User sessionUser)
        {
            db.Users.AddOrUpdate(sessionUser);
            db.SaveChanges();
            sessionContext.SetAuthenticationToken(sessionUser.UserId.ToString(), sessionUser);
        }
    }
}