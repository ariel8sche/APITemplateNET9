using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace APITemplate.Contracts.Requests
{
    public class CreateAccessTokenRequest
    {
        public int UserId { get; set; }

        public int ClientPk { get; set; }

        public DateTime ExpiresAt { get; set; }
    }
}
