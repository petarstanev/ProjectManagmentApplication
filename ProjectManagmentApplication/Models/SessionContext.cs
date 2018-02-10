using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Script.Serialization;
using System.Web.Security;
using ProjectManagementApplication.Models;

namespace ProjectManagementApplication.Models
{

    public class SessionContext
    {
        public void SetAuthenticationToken(string name, User userData)
        {
            string data = null;
            if (userData != null)
                data = new JavaScriptSerializer().Serialize(userData);

            FormsAuthenticationTicket ticket = new FormsAuthenticationTicket(1, name, DateTime.Now, DateTime.Now.AddDays(1), false, userData.UserId.ToString());

            string cookieData = FormsAuthentication.Encrypt(ticket);
            HttpCookie cookie = new HttpCookie(FormsAuthentication.FormsCookieName, cookieData)
            {
                HttpOnly = true,
                Expires = ticket.Expiration
            };

            HttpContext.Current.Response.Cookies.Add(cookie);
        }

        public User GetUserData()
        {
            User userData = null;

            HttpCookie cookie = HttpContext.Current.Request.Cookies[FormsAuthentication.FormsCookieName];
            if (cookie != null)
            {
                FormsAuthenticationTicket ticket = FormsAuthentication.Decrypt(cookie.Value);
                Context db = new Context();
                userData = db.Users.Find(Int32.Parse(ticket.UserData));
            }

            return userData;
        }
    }
}