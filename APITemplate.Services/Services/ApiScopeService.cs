using APITemplate.Contracts.Dtos;
using APITemplate.Contracts.Interfaces;
using APITemplate.Contracts.Requests;
using APITemplate.Data.AuthModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace APITemplate.Services.Services
{
    public class ApiScopeService : IApiScopeService
    {
        private readonly AuthContext _db;

        private readonly ILogger<ApiScopeService> _logger;

        public ApiScopeService(ILogger<ApiScopeService> logger, AuthContext db)
        {
            _logger = logger;
            _db = db;
        }

        public async Task<List<ApiScopeDto>> GetById(int apiScopePk)
        {
            _logger.LogInformation("GetById: fetching API scope ApiScopePk={ApiScopePk}", apiScopePk);

            var apiScope = await _db.ApiScopes
                .Where(e => e.ScopePk == apiScopePk)
                .Select(e => new ApiScopeDto
                {
                    ApiScopePk = apiScopePk,
                    ApiResourcePk = e.ApiResourcePk,
                    ScopeName = e.ScopeName,
                    Description = e.Description,
                    IsActive = e.IsActive
                })
                .ToListAsync();

            _logger.LogDebug("GetById: retrieved {Count} api scope(s) for ApiScopePk={ApiScopePk}", apiScope.Count, apiScopePk);

            if (apiScope is null || apiScope.Count == 0)
            {
                _logger.LogWarning("GetById: no API scope found for ApiScopePk={ApiScopePk}", apiScopePk);
            }

            return apiScope;
        }

        public async Task<List<ApiScopeDto>> GetAll()
        {
            _logger.LogInformation("GetAll: fetching all API scopes");

            var items = await _db.ApiScopes
                .OrderBy(e => e.ScopePk)
                .Select(e => new ApiScopeDto
                {
                    ApiScopePk = e.ScopePk,
                    ApiResourcePk = e.ApiResourcePk,
                    ScopeName = e.ScopeName,
                    Description = e.Description,
                    IsActive = e.IsActive
                })
                .ToListAsync();

            _logger.LogDebug("GetAll: retrieved {Count} api scopes", items.Count);

            return items;
        }

        public async Task<int> Create(CreateApiScopeRequest request)
        {
            _logger.LogInformation("Create: creating API scope ScopeName={ScopeName} ApiResourcePk={ApiResourcePk}", request.ScopeName, request.ApiResourcePk);

            var apiScope = new ApiScope
            {
                ScopeName = request.ScopeName,
                ApiResourcePk = request.ApiResourcePk,
                Description = request.Description,
                IsActive = true,
            };

            _db.ApiScopes.Add(apiScope);

            await _db.SaveChangesAsync();

            _logger.LogInformation("Create: created API scope ScopePk={ScopePk} ScopeName={ScopeName}", apiScope.ScopePk, apiScope.ScopeName);

            return apiScope.ScopePk;
        }

        public async Task<bool> Update(int apiScopePk, UpdateApiScopeRequest request)
        {
            _logger.LogInformation("Update: updating API scope ApiScopePk={ApiScopePk}", apiScopePk);

            var apiScope = await _db.ApiScopes.FirstOrDefaultAsync(e => e.ScopePk == apiScopePk);

            if (apiScope is null) {
                _logger.LogWarning("Update: API scope not found ApiScopePk={ApiScopePk}", apiScopePk);
                return false;
            }

            apiScope.Description = request.Description;
            apiScope.IsActive = request.IsActive;

            await _db.SaveChangesAsync();

            _logger.LogInformation("Update: updated API scope ApiScopePk={ApiScopePk} (IsActive={IsActive})", apiScopePk, request.IsActive);

            return true;
        }

        public async Task<bool> Delete(int apiScopePk)
        {
            _logger.LogInformation("Delete: deactivating API scope ApiScopePk={ApiScopePk}", apiScopePk);

            var deletedApiScope = await _db.ApiScopes.FirstOrDefaultAsync(e => e.ScopePk == apiScopePk);

            if (deletedApiScope is null) {
                _logger.LogWarning("Delete: API scope not found ApiScopePk={ApiScopePk}", apiScopePk);
                return false;
            }

            deletedApiScope.IsActive = false;
            await _db.SaveChangesAsync();

            _logger.LogInformation("Delete: deactivated API scope ApiScopePk={ApiScopePk}", apiScopePk);

            return true;
        }
    }
}
