using System.ComponentModel.DataAnnotations;

namespace APITemplate.Contracts.Requests
{
    public class UpdateUserRequest
    {
        [StringLength(200, ErrorMessage = "Email must not exceed 200 characters")]
        [EmailAddress(ErrorMessage = "Invalid email format")]
        public string? Email { get; set; }

        public bool IsActive { get; set; }
    }
}