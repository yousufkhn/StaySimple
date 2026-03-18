using System.ComponentModel.DataAnnotations;

namespace StaySimple.AuthService.DTOs
{
    public class LoginDto
    {
        [Required]
        [EmailAddress(ErrorMessage = "Please provide a valid email address.")]
        [StringLength(200)]
        public string Email { get; set; } = string.Empty;

        [Required]
        [MinLength(8, ErrorMessage = "Password must be at least 8 characters long.")]
        public string Password { get; set; } = string.Empty;
    }
}
