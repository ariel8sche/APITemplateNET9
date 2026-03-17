using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace APITemplate.Contracts.Requests
{
    public record CreateClientSecretRequest
    {
        [Required]
        public int ClientPk { get; set; }

        [Required]
        public string SecretHash { get; set; } = null!;

        public DateTime? ExpiresAt { get; set; }

        public string? Description { get; set; }

    }

}
