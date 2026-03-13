using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace APITemplate.Contracts.Responses
{
    public class CreateClientResponse
    {
        public int ClientPk { get; set; }
        public bool Success { get; set; }

    }

}
