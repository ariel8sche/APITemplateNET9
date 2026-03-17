using APITemplate.Contracts.Interfaces;
using APITemplate.Contracts.Dtos;
using APITemplate.Contracts.Requests;
using APITemplate.Data.AuthModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace APITemplate.Services.Services
{
    public class ClientService : IClientService
    {
        private readonly AuthContext _db;

        private readonly ILogger<ClientService> _logger;

        public ClientService(ILogger<ClientService> logger, AuthContext db)
        {
            _logger = logger;
            _db = db;
        }

        public async Task<List<ClientDto>> GetById(int clientPk)
        {
            _logger.LogInformation("GetById: fetching client ClientPk={ClientPk}", clientPk);

            var clients = await _db.Clients
                .Where(e => e.ClientPk == clientPk)
                .Select(e => new ClientDto
                {
                    ClientPk = e.ClientPk,
                    ClientId = e.ClientId,
                    Name = e.Name,
                    IsActive = e.IsActive
                })
                .ToListAsync();

            if (clients is null || clients.Count == 0)
            {
                _logger.LogWarning("GetById: no client found for ClientPk={ClientPk}", clientPk);
            }
            else
            {
                _logger.LogDebug("GetById: retrieved {Count} client(s) for ClientPk={ClientPk}", clients.Count, clientPk);
            }

            return clients;
        }

        public async Task<List<ClientDto>> GetAll()
        {
            _logger.LogInformation("GetAll: fetching all clients");

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

            _logger.LogDebug("GetAll: retrieved {Count} clients", items?.Count ?? 0);

            return items;
        }

        public async Task<int> Create(CreateClientRequest request)
        {
            _logger.LogInformation("Create: creating client ClientId={ClientId}", request.ClientId);

            var client = new Client
            {
                ClientId = request.ClientId,
                Name = request.Name,
                IsActive = true,
                CreatedAt = DateTime.UtcNow
            };

            _db.Clients.Add(client);

            await _db.SaveChangesAsync();

            _logger.LogInformation("Create: created client ClientPk={ClientPk} ClientId={ClientId}", client.ClientPk, client.ClientId);

            return client.ClientPk;
        }

        public async Task<bool> Update(int clientPk, UpdateClientRequest request)
        {
            _logger.LogInformation("Update: updating client ClientPk={ClientPk}", clientPk);

            var client = await _db.Clients.FirstOrDefaultAsync(e => e.ClientPk == clientPk);

            if (client is null) {
                _logger.LogWarning("Update: client not found ClientPk={ClientPk}", clientPk);
                return false;
            }

            client.Name = request.Name;
            client.IsActive = request.IsActive;

            await _db.SaveChangesAsync();

            _logger.LogInformation("Update: updated client ClientPk={ClientPk} (IsActive={IsActive})", clientPk, request.IsActive);

            return true;
        }

        public async Task<bool> Delete(int clientPk)
        {
            _logger.LogInformation("Delete: deactivating client ClientPk={ClientPk}", clientPk);

            var deletedClient = await _db.Clients.FirstOrDefaultAsync(e => e.ClientPk == clientPk);

            if (deletedClient is null) {
                _logger.LogWarning("Delete: client not found ClientPk={ClientPk}", clientPk);
                return false;
            }

            deletedClient.IsActive = false;
            await _db.SaveChangesAsync();

            _logger.LogInformation("Delete: deactivated client ClientPk={ClientPk}", clientPk);

            return true;
        }
    }
}
