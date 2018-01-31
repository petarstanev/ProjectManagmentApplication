using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace ProjectManagementApplication.ViewModels
{
    public class LoginUser
    {
        [Required(ErrorMessage = "Valid email is required")]
        [DataType(DataType.EmailAddress)]
        [EmailAddress]
        public string Email { get; set; }

        [Required(ErrorMessage = "Password is required")]
        [StringLength(255, ErrorMessage = "Password must be between 6 and 255 characters", MinimumLength = 6)]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}