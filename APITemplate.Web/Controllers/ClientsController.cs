using APITemplate.Data;
using APITemplate.Data.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace APITemplate.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]

    public class ClientsController : ControllerBase
    {
        private readonly MyDbContext _db;
        public ClientsController(MyDbContext db)
        {
            _db = db;
        }

        // usando APITemplate.Data.Entities;
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ClientDto>>> Get()
        {
            var items = await _db.Clients
                .AsNoTracking()
                .OrderBy(c => c.ClientPk)
                .Select(c => new ClientDto
                {
                    ClientPk = c.ClientPk,
                    ClientId = c.ClientId,
                    Name = c.Name,
                    IsActive = c.IsActive,
                    CreatedAt = c.CreatedAt
                })
                .ToListAsync();

            return Ok(items);
        }

        // Request DTO
        public class CreateClientRequest
        {
            [Required]
            [StringLength(200)]
            public string ClientId { get; set; } = null!;

            [StringLength(250)]
            public string? Name { get; set; }

            public bool IsActive { get; set; } = true;
        }

        // Controller POST
        [HttpPost]
        public async Task<ActionResult<ClientDto>> Create([FromBody] CreateClientRequest request)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var exists = await _db.Clients.AnyAsync(c => c.ClientId == request.ClientId);
            if (exists) return Conflict(new { message = "ClientId ya existe." });

            var entity = new Client
            {
                ClientId = request.ClientId,
                Name = request.Name,
                IsActive = request.IsActive,
                CreatedAt = DateTime.UtcNow
            };

            _db.Clients.Add(entity);
            await _db.SaveChangesAsync();

            var dto = new ClientDto
            {
                ClientPk = entity.ClientPk,
                ClientId = entity.ClientId,
                Name = entity.Name,
                IsActive = entity.IsActive,
                CreatedAt = entity.CreatedAt
            };

            return CreatedAtRoute("GetClientById", new { id = entity.ClientPk }, dto);
        }


        // DTO (si no lo tenés ya)
        public record ClientDto
        {
            public int ClientPk { get; init; }
            public string ClientId { get; init; } = null!;
            public string? Name { get; init; }
            public bool IsActive { get; init; }
            public DateTime CreatedAt { get; init; }
        }

    }
}
