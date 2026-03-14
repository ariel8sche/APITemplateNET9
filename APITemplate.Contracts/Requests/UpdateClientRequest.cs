using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace APITemplate.Contracts.Requests
{
    public record UpdateClientRequest
    {

        [Required]
        public string Name { get; set; } = string.Empty;

        [Required]
        public bool IsActive { get; set; }

    }

}
