using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace ProjectManagmentApplication.Models
{
    public class User
    {
        public int UserId { get; set; }

        [Required(ErrorMessage = "Valid email is required")]
        [DataType(DataType.EmailAddress)]
        [EmailAddress]
        public string Email { get; set; }

        [Required(ErrorMessage = "Name is required")]
        [StringLength(50, ErrorMessage = "Name must be between 3 and 50 characters", MinimumLength = 3)]
        public string Name { get; set; }

        [Required(ErrorMessage = "Password is required")]
        [StringLength(255, ErrorMessage = "Password must be between 6 and 255 characters", MinimumLength = 6)]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        

        [DataType(DataType.Password)]
        [Compare(nameof(Password), ErrorMessage = "Confirm password should match your password.")]
        [NotMapped]
        public string ConfirmPassword { get; set; }
    }
}