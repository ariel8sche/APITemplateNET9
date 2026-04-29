using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace APITemplate.Contracts.Responses
{
    public class UpdateAccessTokenResponse
    {
        public bool Success { get; set; }

        public int AccessTokenId { get; set; }
    }
}
