using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace APITemplate.Contracts.Dtos
{
    public record ApiScopeDto
    {
        public int ApiScopePk { get; init; }
        public int ApiResourcePk { get; init; }
        public string ScopeName { get; init; } = null!;
        public string Description { get; init; } = null!;
        public bool IsActive { get; init; }
    }
}
