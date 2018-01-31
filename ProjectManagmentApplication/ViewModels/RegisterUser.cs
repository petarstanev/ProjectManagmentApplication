using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace ProjectManagementApplication.ViewModels
{
    public class RegisterUser : LoginUser
    {
        [Required(ErrorMessage = "Name is required")]
        [StringLength(50, ErrorMessage = "Name must be between 3 and 50 characters", MinimumLength = 3)]
        public string Name { get; set; }

        [DataType(DataType.Password)]
        [Compare(nameof(Password), ErrorMessage = "Confirm password should match your password.")]
        [NotMapped]
        public string ConfirmPassword { get; set; }
    }
}