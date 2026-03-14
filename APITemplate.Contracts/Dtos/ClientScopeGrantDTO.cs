using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace APITemplate.Contracts.Dtos
{
    public record ClientScopeGrantDto
    {
        public int GrantPk { get; set; }

        public int ClientPk { get; set; }

        public int ScopePk { get; set; }

        public DateTime GrantedAt { get; set; }

        public DateTime? RevokedAt { get; set; }

        public bool IsActive { get; set; }
    }
}
