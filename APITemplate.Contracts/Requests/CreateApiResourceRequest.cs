using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace APITemplate.Contracts.Requests
{
    public record CreateApiResourceRequest
    {
        [Required]
        public string Name { get; set; }

        [Required]
        public string Audience { get; set; }

    }

}
