using System.ComponentModel.DataAnnotations;

namespace ProjectManagementApplication.ViewModels
{
    public class EditUser : RegisterUser
    {
        [Required(ErrorMessage = "Current password is required")]
        [DataType(DataType.Password)]
        public string CurrentPassword { get; set; }
    }
}