using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace APITemplate.Contracts.Dtos
{
    public record ClientDto
    {
        public int ClientPk { get; init; }
        public string ClientId { get; init; } = null!;
        public string? Name { get; init; }
        public bool IsActive { get; init; }
        public DateTime CreatedAt { get; init; }
    }
}
