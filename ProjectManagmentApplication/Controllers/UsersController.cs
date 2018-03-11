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
        public ActionResult Login(LoginUser user, string returnUrl)
        {
            if (ModelState.IsValid)
            {
                UserRepository repo = new UserRepository();

                User foundUser = repo.GetByEmailAndPassword(user);

                if (foundUser != null)
                {
                    sessionContext.SetAuthenticationToken(foundUser.UserId.ToString(), foundUser);
                    if (String.IsNullOrEmpty(returnUrl))
                    {
                        return RedirectToAction("Index", "Home");
                    }
                    else
                    {
                        return Redirect(returnUrl);
                    }
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
        [Authorize]
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

            return View(editUser);
        }

        // POST: Users/Account
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public ActionResult Account(EditUser editUser)
        {
            User sessionUser = sessionContext.GetUserData();

            if (ModelState.IsValid)
            {
                editUser.CurrentPassword = HashingHelper.HashPassword(editUser.CurrentPassword);
                UserRepository repo = new UserRepository();
                if (sessionUser.Password == editUser.CurrentPassword)
                {
                    if (editUser.Name != sessionUser.Name)
                    {
                        TempData["UpdateUsername"] = "Name was updated.";
                        repo.UpdateUsername(editUser.Name);
                    }

                    if (!String.IsNullOrEmpty(editUser.NewPassword))
                    {
                        TempData["UpdatePassword"] = "Password was updated.";
                        repo.UpdatePassword(editUser.NewPassword);
                    }
                }
                else
                {
                    ModelState.AddModelError("CurrentPassword", "The current password is not correct.");
                }
            }
            return View(new EditUser() { Email = editUser.Email, Name = editUser.Name });
        }

        public ActionResult Welcome()
        {
            return View();
        }
    }
}
