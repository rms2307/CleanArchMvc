using System.ComponentModel.DataAnnotations;

namespace CleanArchMvc.Application.DTOs.Account
{
    public class AdminRegisterUserDTO
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [Phone]
        public string PhoneNumber { get; set; }

        [Required]
        [StringLength(20, ErrorMessage = "The {0} must be at least {2} and at max " +
            "{1} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [Compare("Password", ErrorMessage = "Passwords don´t match")]
        public string ConfirmPassword { get; set; }

        [Required]
        public string Role { get; set; }
    }
}