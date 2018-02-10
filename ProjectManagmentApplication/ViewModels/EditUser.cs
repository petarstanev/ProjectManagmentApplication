using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Web.UI.WebControls;

namespace ProjectManagementApplication.ViewModels
{
    public class EditUser
    {
        [DataType(DataType.EmailAddress)]
        [EmailAddress]
        public string Email { get; set; }

        [Required(ErrorMessage = "Name is required")]
        [StringLength(50, ErrorMessage = "Name must be between 3 and 50 characters", MinimumLength = 3)]
        public string Name { get; set; }

        [Required(ErrorMessage = "Current password is required")]
        [DataType(DataType.Password)]
        [Display(Name = "Current password")]
        public string CurrentPassword { get; set; }

        [DataType(DataType.Password)] 
        [Display(Name = "New password")]
        public string NewPassword { get; set; }
        
        [DataType(DataType.Password)]
        [Compare(nameof(NewPassword), ErrorMessage = "Confirm password should match your password.")]
        [Display(Name = "Confirm password")]
        public string ConfirmPassword { get; set; }
        
    }
}