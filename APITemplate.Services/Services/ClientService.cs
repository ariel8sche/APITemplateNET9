using APITemplate.Contracts.Interfaces;
using APITemplate.Contracts.Models;
using APITemplate.Data;
using APITemplate.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace APITemplate.Services.Services
{
    public class ClientService : IClientService
    {
        private readonly MyDbContext _db;

        private readonly ILogger<ClientService> _logger;

        public ClientService(ILogger<ClientService> logger, MyDbContext db)
        {
            _logger = logger;
            _db = db;
        }

        public async Task<List<ClientDto>> GetById(int clientPk)
        {
            _logger.LogInformation("Fetching client with ClientPk {ClientPk}", clientPk);

            var client = await _db.Clients
                .Where(e => e.ClientPk == clientPk)
                .Select(e => new ClientDto
                {
                    ClientPk = e.ClientPk,
                    ClientId = e.ClientId,
                    Name = e.Name,
                    IsActive = e.IsActive
                })
                .ToListAsync();

            return client;
        }

        public async Task<List<ClientDto>> GetAll()
        {
            _logger.LogInformation("Fetching all clients");

            var items = await _db.Clients
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

            return items;
        }

        public async Task<ClientDto> Create(CreateClientDto dto)
        {
            _logger.LogInformation("Creating new client with ClientId {ClientId}", dto.ClientId);

            var entity = new Client
            {
                ClientId = dto.ClientId,
                Name = dto.Name,
                IsActive = dto.IsActive,
                CreatedAt = DateTime.UtcNow
            };

            _db.Clients.Add(entity);
            await _db.SaveChangesAsync();

            return new ClientDto
            {
                ClientPk = entity.ClientPk,
                ClientId = entity.ClientId,
                Name = entity.Name,
                IsActive = entity.IsActive,
                CreatedAt = entity.CreatedAt
            };
        }

    }
}
