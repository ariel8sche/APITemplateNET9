using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace APITemplate.Contracts.Dtos
{
    public record ClientSecretDto
    {
        public int ClientSecretPk { get; set; }

        public int ClientPk { get; set; }

        public string SecretHash { get; set; } = null!;

        public DateTime CreatedAt { get; set; }

        public DateTime? ExpiresAt { get; set; }

        public bool IsRevoked { get; set; }

        public string? Description { get; set; }
    }
}
