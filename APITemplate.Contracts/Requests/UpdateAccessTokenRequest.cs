using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace APITemplate.Contracts.Requests
{
    public class UpdateAccessTokenRequest
    {
        public DateTime ExpiresAt { get; set; }
        public bool IsRevoked { get; set; }
    }
}
