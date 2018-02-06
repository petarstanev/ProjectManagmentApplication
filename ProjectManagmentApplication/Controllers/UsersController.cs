using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using ProjectManagementApplication.Models;
using ProjectManagementApplication.Repository;
using ProjectManagementApplication.ViewModels;

namespace ProjectManagementApplication.Controllers
{
    public class UsersController : Controller
    {
        private Context db = new Context();

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
                    SessionContext sessionContext = new SessionContext();
                    sessionContext.SetAuthenticationToken(foundUser.UserId.ToString(), false, foundUser);
                    return RedirectToAction("Index", "Home");
                }
                ModelState.AddModelError("Email", "Email or password not matched");

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

        // GET: Users/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            User user = db.Users.Find(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);
        }

        // POST: Users/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "UserId,Email,Name,Password")] User user)
        {
            if (ModelState.IsValid)
            {
                db.Entry(user).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Edit");
            }
            return View(user);
        }

        public ActionResult Welcome()
        {
            return View();
        }
    }
}
