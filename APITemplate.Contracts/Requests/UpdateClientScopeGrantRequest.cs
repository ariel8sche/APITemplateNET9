using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace APITemplate.Contracts.Requests
{
    public record UpdateClientScopeGrantRequest
    {
        [Required]
        public bool IsActive { get; set; }

    }

}
