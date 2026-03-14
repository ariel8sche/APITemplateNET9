using APITemplate.Contracts.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace APITemplate.Contracts.Responses
{
    public class ClientScopeGrantListResponse
    {
        public List<ClientScopeGrantDto> ClientScopeGrants { get; set; } = new();
    }
}
