using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace APITemplate.Contracts.Dtos
{
    public record UserDto
    {
        public int UserId { get; init; }
        public string Username { get; init; } = null!;
        public string PasswordHash { get; init; } = null!;
        public string Email { get; init; } = null!;
        public bool IsActive { get; init; }
        public DateTime CreatedAt { get; init; }
    }
}
