using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace ProjectManagmentApplication.ViewModels
{
    public class LoginUser
    {
        [Required(ErrorMessage = "Valid email is required to login")]
        [DataType(DataType.EmailAddress)]
        [EmailAddress]
        public string Email { get; set; }

        [Required(ErrorMessage = "Password is required to login")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}