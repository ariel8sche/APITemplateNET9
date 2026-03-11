using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace APITemplate.Contracts.Models
{
    public class CreateClientResponse
    {
        public string ClientId { get; set; }
        public bool Success { get; set; }

    }

}
