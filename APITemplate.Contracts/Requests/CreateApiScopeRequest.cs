using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace APITemplate.Contracts.Requests
{
    public record CreateApiScopeRequest
    {
        [Required]
        public int ApiResourcePk { get; set; }

        [Required]
        public string ScopeName { get; set; }

        [Required]
        public string Description { get; set; }

    }

}
