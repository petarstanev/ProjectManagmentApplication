using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using AutoMapper;
using ProjectManagementApplication.App_Start;
using ProjectManagementApplication.Helpers;
using ProjectManagementApplication.Models;
using ProjectManagementApplication.Repository;
using ProjectManagementApplication.ViewModels;

namespace ProjectManagementApplication.Controllers
{
    public class UsersController : Controller
    {
        private Context db = new Context();
        SessionContext sessionContext = new SessionContext();

        // GET: Users/Register
        public ActionResult Register()
        {
            return View();
        }

        // POST: Users/Register
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Register([Bind(Include = "Email,Name,Password,ConfirmPassword")] RegisterUser user)
        {
            if (ModelState.IsValid)
            {
                UserRepository repo = new UserRepository();
                if (repo.CheckEmailIsUnique(user))
                {
                    repo.RegisterUser(user);
                    return RedirectToAction("Welcome");
                }
                ModelState.AddModelError("Email", "Email is already registered.");
            }
            return View(user);
        }

        // GET: Users/Login
        public ActionResult Login()
        {
            return View();
        }

        // POST: Users/Login
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login([Bind(Include = "Email,Password")] LoginUser user)
        {
            if (ModelState.IsValid)
            {
                UserRepository repo = new UserRepository();

                User foundUser = repo.GetByEmailAndPassword(user);

                if (foundUser != null)
                {
                    sessionContext.SetAuthenticationToken(foundUser.UserId.ToString(), false, foundUser);
                    return RedirectToAction("Index", "Home");
                }
                ModelState.AddModelError("Password", "The email or password you entered are incorrect.");

            }

            return View(user);
        }

        // GET: Users/Logout
        public ViewResult Logout()
        {
            return View();
        }

        // POST: Users/Logout
        [HttpPost]
        [ActionName("Logout")]
        public ActionResult LogoutPost()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Index", "Home");
        }

        // GET: Users/Account
        public ActionResult Account()
        {
            User sessionUser = sessionContext.GetUserData();

            if (sessionUser == null)
            {
                RedirectToAction("Login");
            }

            var config = new AutoMapperConfiguration().Configure();
            IMapper iMapper = config.CreateMapper();

            EditUser editUser = iMapper.Map<User, EditUser>(sessionUser);
            editUser.Password = String.Empty;

            return View(editUser);
        }

        // POST: Users/Account
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Account(EditUser editUser)
        {
            User sessionUser = sessionContext.GetUserData();

            if (ModelState.IsValid)
            {
                editUser.CurrentPassword = HashingHelper.HashPassword(editUser.CurrentPassword);

                if (sessionUser.Password == editUser.CurrentPassword)
                {
                    sessionUser.Name = editUser.Name;
                    sessionUser.Password = HashingHelper.HashPassword(editUser.Password);

                    db.Entry(sessionUser).State = EntityState.Modified;
                    db.SaveChanges();
                    
                    editUser.ConfirmPassword = String.Empty;
                    editUser.Password = String.Empty;
                }
                else
                {
                    ModelState.AddModelError("CurrentPassword", "The current password is not correct.");
                }
                return View(editUser);
            }
            editUser.CurrentPassword = String.Empty;
            editUser.Email = sessionUser.Email;
            return View(editUser);
        }


        public ActionResult Welcome()
        {
            return View();
        }
    }
}
