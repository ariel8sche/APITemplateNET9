using System.ComponentModel.DataAnnotations;

namespace APITemplate.Contracts.Requests
{
    public class CreateUserRequest
    {
        [Required(ErrorMessage = "Username is required")]
        [StringLength(100, MinimumLength = 3, ErrorMessage = "Username must be between 3 and 100 characters")]
        public string Username { get; set; } = null!;

        [Required(ErrorMessage = "Password is required")]
        [StringLength(256, MinimumLength = 8, ErrorMessage = "Password must be at least 8 characters")]
        public string Password { get; set; } = null!;

        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Invalid email format")]
        [StringLength(200, ErrorMessage = "Email must not exceed 200 characters")]
        public string Email { get; set; } = null!;
    }
}