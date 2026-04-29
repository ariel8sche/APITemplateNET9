using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace APITemplate.Data.AuthModels
{
    public class AccessToken
    {

        [Key]
        public int AccessTokenId { get; set; }

        public int UserId { get; set; }

        public int ClientPk { get; set; }

        [Required]
        [MaxLength(512)]
        public string Token { get; set; } = null!;

        public DateTime CreatedAt { get; set; }

        public DateTime ExpiresAt { get; set; }

        public bool IsRevoked { get; set; } = false;

        public User User { get; set; } = null!;
        public Client Client { get; set; } = null!;
    }
}
