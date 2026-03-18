using System.ComponentModel.DataAnnotations;

namespace StaySimple.AuthService.Models
{
    public class User
    {
        /// <summary>
        /// I was wondering why the data validation was splitted between data classes and fluent api in the dbcontext, the reason is
        /// Layer	                        Responsibility
        /// App (Annotations / DTOs)        Prevent bad input(in app)
        /// DB (Fluent API)                 Prevent bad storage(in db)
        /// </summary>
        public int Id { get; set; }
        [Required, MaxLength(100)] public string Name { get; set; } = "";
        [Required, MaxLength(200)] public string Email { get; set; } = "";
        [Required] public string PasswordHash { get; set; } = "";
        [Required, MaxLength(20)] public string Role { get; set; } = "User";
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    }
}
