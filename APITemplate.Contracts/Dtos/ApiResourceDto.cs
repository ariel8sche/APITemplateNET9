using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace APITemplate.Contracts.Dtos
{
    public record ApiResourceDto
    {
        public int ApiResourcePk { get; init; }
        public string Name { get; init; } = null!;
        public string Audience { get; init; } = null!;
        public bool IsActive { get; init; }
        public DateTime CreatedAt { get; init; }
    }
}
