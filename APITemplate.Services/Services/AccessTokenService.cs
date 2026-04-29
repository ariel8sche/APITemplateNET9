using APITemplate.Contracts.Dtos;
using APITemplate.Contracts.Interfaces;
using APITemplate.Contracts.Requests;
using APITemplate.Data.AuthModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;


namespace APITemplate.Services.Services
{
    public class AccessTokenService : IAccessTokenService
    {
        private readonly AuthContext _db;

        private readonly ILogger<AccessTokenService> _logger;

        public AccessTokenService(ILogger<AccessTokenService> logger, AuthContext db)
        {
            _logger = logger;
            _db = db;
        }
        public async Task<int> Create(CreateAccessTokenRequest request)
        {
            _logger.LogInformation("Create: creating access token");

            if (!await _db.Users.AnyAsync(u => u.UserId == request.UserId))
                throw new InvalidOperationException("User not found");

            if (!await _db.Clients.AnyAsync(c => c.ClientPk == request.ClientPk))
                throw new InvalidOperationException("Client not found");

            var accessToken = new AccessToken
            {
                UserId = request.UserId,
                ClientPk = request.ClientPk,
                Token = GenerateToken(),
                IsRevoked = false,
                CreatedAt = DateTime.UtcNow,
                ExpiresAt = request.ExpiresAt
            };

            _db.AccessTokens.Add(accessToken);

            await _db.SaveChangesAsync();

            _logger.LogInformation("Create: created access token AccessTokenId={AccessTokenId}", accessToken.AccessTokenId);

            return accessToken.AccessTokenId;
        }

        public async Task<bool> Delete(int accessTokenId)
        {
            _logger.LogInformation("Delete: deactivating access token AccessTokenId={AccessTokenId}", accessTokenId);

            var deletedAccessToken = await _db.AccessTokens.FirstOrDefaultAsync(e => e.AccessTokenId == accessTokenId);

            if (deletedAccessToken is null)
            {
                _logger.LogWarning("Delete: access token not found AccessTokenId={AccessTokenId}", accessTokenId);
                return false;
            }

            deletedAccessToken.IsRevoked = true;
            await _db.SaveChangesAsync();

            _logger.LogInformation("Delete: deactivated access token AccessTokenId={AccessTokenId}", accessTokenId);

            return true;
        }

        public async Task<List<AccessTokenDto>> GetAll()
        {
            _logger.LogInformation("GetAll: fetching all access tokens");

            var items = await _db.AccessTokens
                .OrderBy(c => c.AccessTokenId)
                .Select(c => new AccessTokenDto
                {
                    AccessTokenId = c.AccessTokenId,
                    UserId = c.UserId,
                    ClientPk = c.ClientPk,
                    IsRevoked = c.IsRevoked,
                    CreatedAt = c.CreatedAt,
                    ExpiresAt = c.ExpiresAt
                })
                .ToListAsync();

            _logger.LogDebug("GetAll: retrieved {Count} access tokens", items?.Count ?? 0);

            return items;
        }

        public async Task<List<AccessTokenDto>> GetById(int accessTokenId)
        {
            _logger.LogInformation("GetById: fetching access token AccessTokenId={accessTokenId}", accessTokenId);

            var accessTokens = await _db.AccessTokens
                .Where(e => e.AccessTokenId == accessTokenId)
                .Select(e => new AccessTokenDto
                {
                    AccessTokenId = e.AccessTokenId,
                    ClientPk = e.ClientPk,
                    UserId = e.UserId,
                    IsRevoked = e.IsRevoked,
                    CreatedAt = e.CreatedAt,
                    ExpiresAt = e.ExpiresAt
                })
                .ToListAsync();

            if (accessTokens is null || accessTokens.Count == 0)
            {
                _logger.LogWarning("GetById: no access token found for AccessTokenId={accessTokenId}", accessTokenId);
            }
            else
            {
                _logger.LogDebug("GetById: retrieved {Count} accessToken(s) for AccessTokenId={accessTokenId}", accessTokens.Count, accessTokens);
            }

            return accessTokens;
        }

        public async Task<bool> Update(int accessTokenId, UpdateAccessTokenRequest request)
        {
            _logger.LogInformation("Update: updating access token AccessTokenId={AccessTokenId}",accessTokenId);

            var accessToken = await _db.AccessTokens.FirstOrDefaultAsync(e => e.AccessTokenId == accessTokenId);

            if (accessToken is null)
            {
                _logger.LogWarning("Update: access token not found AccessTokenId={AccessTokenId}", accessTokenId);
                return false;
            }

            accessToken.ExpiresAt = request.ExpiresAt;

            // Determinar revocación: se revoca si:
            // - ya estaba revocado, o
            // - la fecha de expiración ya pasó.
            var now = DateTime.UtcNow;
            var isExpired = accessToken.ExpiresAt <= now;
            accessToken.IsRevoked = accessToken.IsRevoked || isExpired;

            await _db.SaveChangesAsync();

            _logger.LogInformation("Update: updated access token AccessTokenId={AccessTokenId} (IsRevoked={IsRevoked})", accessTokenId, accessToken.IsRevoked);
            _logger.LogDebug("Update: ExpiresAt={ExpiresAt} IsExpired={IsExpired}", accessToken.ExpiresAt, isExpired);

            return true;
        }

        private string GenerateToken()
        {
            var bytes = RandomNumberGenerator.GetBytes(32);
            return Convert.ToBase64String(bytes);
        }
    }
}
