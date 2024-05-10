using System.ComponentModel.DataAnnotations;

namespace Talabat.API.DTOs.AccountsDTO
{
    public class RegisterDTO
    {
        [Required]
        public string DisplayName { get; set; }
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        [Phone]
        public string PhoneNumber { get; set; }
        //[Required]
        //[RegularExpression("^(?=.*[a-z])(?=.*[A-Z])(?=.*\\d)(?=.*[@#$%^&*!()])(?=.{8,})$",
        //   ErrorMessage = "Password must be at least one lowercase letter,one Uppercase,one Special Char ")]
        public string Password { get; set; }
    }
}
