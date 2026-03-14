using APITemplate.Contracts.Interfaces;
using APITemplate.Contracts.Dtos;
using APITemplate.Contracts.Requests;
using APITemplate.Data.AuthModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace APITemplate.Services.Services
{
    public class ClientScopeGrantService : IClientScopeGrantService
    {
        private readonly AuthContext _db;

        private readonly ILogger<ClientScopeGrantService> _logger;

        public ClientScopeGrantService(ILogger<ClientScopeGrantService> logger, AuthContext db)
        {
            _logger = logger;
            _db = db;
        }

        public async Task<List<ClientScopeGrantDto>> GetById(int grantPk)
        {
            _logger.LogInformation("Fetching client scope grant with GrantPk {GrantPk}", grantPk);

            var clientScopeGrants = await _db.ClientScopeGrants
                .Where(e => e.GrantPk == grantPk)
                .Select(e => new ClientScopeGrantDto
                {
                    GrantPk = grantPk,
                    ClientPk = e.ClientPk,
                    ScopePk = e.ScopePk,
                    GrantedAt = e.GrantedAt,
                    RevokedAt = e.RevokedAt,
                    IsActive = e.IsActive
                })
                .ToListAsync();

            return clientScopeGrants;
        }

        public async Task<List<ClientScopeGrantDto>> GetAll()
        {
            _logger.LogInformation("Fetching all client scope grants");

            var clientScopeGrant = await _db.ClientScopeGrants
                .OrderBy(e => e.GrantPk)
                .Select(e => new ClientScopeGrantDto
                {
                    GrantPk = e.GrantPk,
                    ClientPk = e.ClientPk,
                    ScopePk = e.ScopePk,
                    GrantedAt = e.GrantedAt,
                    RevokedAt = e.RevokedAt,
                    IsActive = e.IsActive
                })
                .ToListAsync();

            return clientScopeGrant;
        }

        public async Task<int> Create(CreateClientScopeGrantRequest request)
        {
            _logger.LogInformation("Creating new client scope grant");

            var clientScopeGrant = new ClientScopeGrant
            {
                ClientPk = request.ClientPk,
                ScopePk = request.ScopePk,
                IsActive = true,
                GrantedAt = DateTime.UtcNow
            };

            _db.ClientScopeGrants.Add(clientScopeGrant);

            await _db.SaveChangesAsync();

            return clientScopeGrant.GrantPk;
        }

        public async Task<bool> Update(int grantPk, UpdateClientScopeGrantRequest request)
        {
            _logger.LogInformation("Updating client with grantPk {grantPk}", grantPk);

            var client = await _db.ClientScopeGrants.FirstOrDefaultAsync(e => e.GrantPk == grantPk);

            if (client is null) {
                _logger.LogWarning("Client with grantPk {grantPk} not found", grantPk);
                return false;
            }

            client.IsActive = request.IsActive;

            await _db.SaveChangesAsync();

            _logger.LogInformation("Updated client with grantPk {grantPk}", grantPk);

            return true;
        }

        public async Task<bool> Delete(int grantPk)
        {
            _logger.LogInformation("Deleting client with grantPk {grantPk}", grantPk);

            var deletedClient = await _db.ClientScopeGrants.FirstOrDefaultAsync(e => e.GrantPk == grantPk);

            if (deletedClient is null) {
                _logger.LogWarning("Client with grantPk {grantPk} not found", grantPk);
                return false;
            }

            deletedClient.IsActive = false;
            await _db.SaveChangesAsync();

            _logger.LogInformation("Deleted client with grantPk {grantPk}", grantPk);

            return true;
        }
    }
}
