using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace APITemplate.Contracts.Models
{
    public class ClientListResponse
    {
        public List<ClientDto> Clients { get; set; } = new();
    }
}
