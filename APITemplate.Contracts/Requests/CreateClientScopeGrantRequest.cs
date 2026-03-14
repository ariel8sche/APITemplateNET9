using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace APITemplate.Contracts.Requests
{
    public record CreateClientScopeGrantRequest
    {
        [Required]
        public int ClientPk { get; set; }

        [Required]
        public int ScopePk { get; set; }

    }

}
