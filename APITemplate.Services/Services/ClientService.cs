using APITemplate.Contracts.Interfaces;
using APITemplate.Contracts.Models;
using APITemplate.Data.AuthModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace APITemplate.Services.Services
{
    public class ClientService : IClientService
    {
        private readonly NeonDbContext _db;

        private readonly ILogger<ClientService> _logger;

        public ClientService(ILogger<ClientService> logger, NeonDbContext db)
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

        public async Task<int> Create(CreateClientRequest request)
        {
            _logger.LogInformation("Creating new client with ClientId {ClientId}", request.ClientId);

            var client = new Client
            {
                ClientId = request.ClientId,
                Name = request.Name,
                IsActive = true,
                CreatedAt = DateTime.UtcNow
            };

            _db.Clients.Add(client);

            await _db.SaveChangesAsync();

            return client.ClientPk;
        }

        public async Task<bool> Update(int clientPk, UpdateClientRequest request)
        {
            _logger.LogInformation("Updating client with ClientPk {ClientPk}", clientPk);

            var client = await _db.Clients.FirstOrDefaultAsync(e => e.ClientPk == clientPk);

            if (client is null) {
                _logger.LogWarning("Client with ClientPk {ClientPk} not found", clientPk);
                return false;
            }

            client.Name = request.Name;
            client.ClientId = request.ClientId;

            await _db.SaveChangesAsync();

            _logger.LogInformation("Updated client with ClientPk {ClientPk}", clientPk);

            return true;
        }

        public async Task<bool> Delete(int clientPk)
        {
            _logger.LogInformation("Deleting client with ClientPk {ClientPk}", clientPk);

            var deletedClient = await _db.Clients.FirstOrDefaultAsync(e => e.ClientPk == clientPk);

            if (deletedClient is null) {
                _logger.LogWarning("Client with ClientPk {ClientPk} not found", clientPk);
                return false;
            }

            deletedClient.IsActive = false;
            await _db.SaveChangesAsync();

            _logger.LogInformation("Deleted client with ClientPk {ClientPk}", clientPk);

            return true;
        }
    }
}
