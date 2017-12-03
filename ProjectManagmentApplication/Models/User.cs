using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ProjectManagmentApplication.Models
{
    public class User
    {
        public int UserId { get; set; }
        public string Email { get; set; }
        public string PasswordHash { get; set; }
        public string Name { get; set; }
    }
}