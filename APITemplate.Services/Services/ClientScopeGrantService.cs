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
            _logger.LogInformation("GetById: fetching client scope grant GrantPk={GrantPk}", grantPk);

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

            _logger.LogDebug("GetById: retrieved {Count} client scope grant(s) for GrantPk={GrantPk}", clientScopeGrants.Count, grantPk);

            if (clientScopeGrants is null || clientScopeGrants.Count == 0)
            {
                _logger.LogWarning("GetById: no client scope grant found for GrantPk={GrantPk}", grantPk);
            }

            return clientScopeGrants;
        }

        public async Task<List<ClientScopeGrantDto>> GetAll()
        {
            _logger.LogInformation("GetAll: fetching all client scope grants");

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

            _logger.LogDebug("GetAll: retrieved {Count} client scope grants", clientScopeGrant.Count);

            return clientScopeGrant;
        }

        public async Task<int> Create(CreateClientScopeGrantRequest request)
        {
            _logger.LogInformation("Create: creating client scope grant for ClientPk={ClientPk} ScopePk={ScopePk}", request.ClientPk, request.ScopePk);

            var clientScopeGrant = new ClientScopeGrant
            {
                ClientPk = request.ClientPk,
                ScopePk = request.ScopePk,
                IsActive = true,
                GrantedAt = DateTime.UtcNow
            };

            _db.ClientScopeGrants.Add(clientScopeGrant);

            await _db.SaveChangesAsync();

            _logger.LogInformation("Create: created client scope grant GrantPk={GrantPk} ClientPk={ClientPk} ScopePk={ScopePk}", clientScopeGrant.GrantPk, clientScopeGrant.ClientPk, clientScopeGrant.ScopePk);

            return clientScopeGrant.GrantPk;
        }

        public async Task<bool> Update(int grantPk, UpdateClientScopeGrantRequest request)
        {
            _logger.LogInformation("Update: updating client scope grant GrantPk={GrantPk}", grantPk);

            var client = await _db.ClientScopeGrants.FirstOrDefaultAsync(e => e.GrantPk == grantPk);

            if (client is null)
            {
                _logger.LogWarning("Update: client scope grant not found GrantPk={GrantPk}", grantPk);
                return false;
            }

            client.IsActive = request.IsActive;

            await _db.SaveChangesAsync();

            _logger.LogInformation("Update: updated client scope grant GrantPk={GrantPk} (IsActive={IsActive})", grantPk, request.IsActive);

            return true;
        }

        public async Task<bool> Delete(int grantPk)
        {
            _logger.LogInformation("Delete: deactivating client scope grant GrantPk={GrantPk}", grantPk);

            var deletedClient = await _db.ClientScopeGrants.FirstOrDefaultAsync(e => e.GrantPk == grantPk);

            if (deletedClient is null)
            {
                _logger.LogWarning("Delete: client scope grant not found GrantPk={GrantPk}", grantPk);
                return false;
            }

            deletedClient.IsActive = false;
            await _db.SaveChangesAsync();

            _logger.LogInformation("Delete: deactivated client scope grant GrantPk={GrantPk}", grantPk);

            return true;
        }
    }
}