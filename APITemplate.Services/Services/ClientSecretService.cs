using APITemplate.Contracts.Interfaces;
using APITemplate.Contracts.Dtos;
using APITemplate.Contracts.Requests;
using APITemplate.Data.AuthModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Security.Cryptography;
using System.Text;

namespace APITemplate.Services.Services
{
    public class ClientSecretService : IClientSecretService
    {
        private readonly AuthContext _db;

        private readonly ILogger<ClientSecretService> _logger;

        public ClientSecretService(ILogger<ClientSecretService> logger, AuthContext db)
        {
            _logger = logger;
            _db = db;
        }

        public async Task<List<ClientSecretDto>> GetById(int clientSecretPk)
        {
            _logger.LogInformation("GetById: fetching client secret with ClientSecretPk={ClientSecretPk}", clientSecretPk);

            var clientSecrets = await _db.ClientSecrets
                .Where(e => e.ClientSecretPk == clientSecretPk)
                .Select(e => new ClientSecretDto
                {
                    ClientSecretPk = clientSecretPk,
                    ClientPk = e.ClientPk,
                    SecretHash = e.SecretHash,
                    CreatedAt = e.CreatedAt,
                    ExpiresAt = e.ExpiresAt,
                    IsRevoked = e.IsRevoked,
                    Description = e.Description
                })
                .ToListAsync();

            _logger.LogDebug("GetById: retrieved {Count} client secret(s) for ClientSecretPk={ClientSecretPk}", clientSecrets.Count, clientSecretPk);

            return clientSecrets;
        }

        public async Task<List<ClientSecretDto>> GetAll()
        {
            _logger.LogInformation("GetAll: fetching all client secrets");

            var clientSecret = await _db.ClientSecrets
                .OrderBy(e => e.ClientSecretPk)
                .Select(e => new ClientSecretDto
                {
                    ClientSecretPk = e.ClientSecretPk,
                    ClientPk = e.ClientPk,
                    SecretHash = e.SecretHash,
                    CreatedAt = e.CreatedAt,
                    ExpiresAt = e.ExpiresAt,
                    IsRevoked = e.IsRevoked,
                    Description = e.Description
                })
                .ToListAsync();

            _logger.LogDebug("GetAll: retrieved {Count} client secrets", clientSecret.Count);

            return clientSecret;
        }

        public async Task<int> Create(CreateClientSecretRequest request)
        {
            _logger.LogInformation("Create: creating new client secret for ClientPk={ClientPk}", request.ClientPk);

            var storedHash = ComputeSha256Hash(request.SecretHash);

            var clientSecret = new ClientSecret
            {
                ClientPk = request.ClientPk,
                SecretHash = storedHash,
                CreatedAt = DateTime.UtcNow,
                ExpiresAt = request.ExpiresAt,
                IsRevoked = false,
                Description = request.Description
            };

            _db.ClientSecrets.Add(clientSecret);

            await _db.SaveChangesAsync();

            _logger.LogInformation("Create: created client secret with ClientSecretPk={ClientSecretPk} for ClientPk={ClientPk}", clientSecret.ClientSecretPk, clientSecret.ClientPk);

            return clientSecret.ClientSecretPk;
        }

        public async Task<bool> Update(int clientSecretPk, UpdateClientSecretRequest request)
        {
            _logger.LogInformation("Update: updating client secret with ClientSecretPk={ClientSecretPk}", clientSecretPk);

            var client = await _db.ClientSecrets.FirstOrDefaultAsync(e => e.ClientSecretPk == clientSecretPk);

            if (client is null) {
                _logger.LogWarning("Update: client secret not found ClientSecretPk={ClientSecretPk}", clientSecretPk);
                return false;
            }

            client.Description = request.Description;
            client.ExpiresAt = request.ExpiresAt;

            // Determinar revocación: se revoca si:
            // - ya estaba revocado, o
            // - la fecha de expiración ya pasó.
            var now = DateTime.UtcNow;
            var isExpired = client.ExpiresAt.HasValue && client.ExpiresAt.Value <= now;
            client.IsRevoked = client.IsRevoked || isExpired;

            await _db.SaveChangesAsync();

            _logger.LogInformation("Update: updated client secret ClientSecretPk={ClientSecretPk} (IsRevoked={IsRevoked}, ExpiresAt={ExpiresAt}, IsExpired={IsExpired})",
                clientSecretPk, client.IsRevoked, client.ExpiresAt, isExpired);

            return true;
        }

        public async Task<bool> Delete(int clientSecretPk)
        {
            _logger.LogInformation("Delete: revoking client secret with ClientSecretPk={ClientSecretPk}", clientSecretPk);

            var deletedClient = await _db.ClientSecrets.FirstOrDefaultAsync(e => e.ClientSecretPk == clientSecretPk);

            if (deletedClient is null) {
                _logger.LogWarning("Delete: client secret not found ClientSecretPk={ClientSecretPk}", clientSecretPk);
                return false;
            }

            deletedClient.IsRevoked = true;
            await _db.SaveChangesAsync();

            _logger.LogInformation("Delete: revoked client secret ClientSecretPk={ClientSecretPk}", clientSecretPk);

            return true;
        }

        // Helpers

        private static string ComputeSha256Hash(string secret)
        {
            if (secret is null) throw new ArgumentNullException(nameof(secret));

            using var sha = SHA256.Create();
            var bytes = Encoding.UTF8.GetBytes(secret);
            var hash = sha.ComputeHash(bytes);
            return Convert.ToBase64String(hash);
        }
    }
}
