using System.ComponentModel.DataAnnotations;

namespace StaySimple.AuthService.DTOs
{
    public class LoginDto
    {
        [Required]
        [StringLength(200)]
        public string Email { get; set; } = string.Empty;

        [Required]
        public string Password { get; set; } = string.Empty;
    }
}
