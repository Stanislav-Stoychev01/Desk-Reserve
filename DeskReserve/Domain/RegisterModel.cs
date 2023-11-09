using System.ComponentModel.DataAnnotations;

namespace DeskReserve.Domain
{
    public class RegisterModel
    {
        [Required(ErrorMessage = "User Name is required")]
        public string UserName { get; set; }

        [EmailAddress]
        [Required(ErrorMessage = "Email is required")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Password is required")]
        public string Password { get; set; }

        // public string FirstName { get; set; }

        // public string LastName { get; set; }
    }
}
