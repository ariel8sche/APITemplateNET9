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
            _logger.LogInformation("Fetching api scope with ApiScopePk {ApiScopePk}", apiScopePk);

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

            return apiScope;
        }

        public async Task<List<ApiScopeDto>> GetAll()
        {
            _logger.LogInformation("Fetching all api Scopes");

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

            return items;
        }

        public async Task<int> Create(CreateApiScopeRequest request)
        {
            _logger.LogInformation("Creating new api scope with ScopeName {ScopeName}", request.ScopeName);

            var apiScope = new ApiScope
            {
                ScopeName = request.ScopeName,
                ApiResourcePk = request.ApiResourcePk,
                Description = request.Description,
                IsActive = true,
            };

            _db.ApiScopes.Add(apiScope);

            await _db.SaveChangesAsync();

            return apiScope.ScopePk;
        }

        public async Task<bool> Update(int apiScopePk, UpdateApiScopeRequest request)
        {
            _logger.LogInformation("Updating api scope with ApiScopePk {ApiScopePk}", apiScopePk);

            var apiScope = await _db.ApiScopes.FirstOrDefaultAsync(e => e.ScopePk == apiScopePk);

            if (apiScope is null) {
                _logger.LogWarning("Api scope with ApiScopePk {ApiScopePk} not found", apiScopePk);
                return false;
            }

            apiScope.ScopeName = request.ScopeName;
            apiScope.Description = request.Description;

            await _db.SaveChangesAsync();

            _logger.LogInformation("Updated api scope with ApiScopePk {ApiScopePk}", apiScopePk);

            return true;
        }

        public async Task<bool> Delete(int apiScopePk)
        {
            _logger.LogInformation("Deleting api scope with ApiScopePk {ApiScopePk}", apiScopePk);

            var deletedApiScope = await _db.ApiScopes.FirstOrDefaultAsync(e => e.ScopePk == apiScopePk);

            if (deletedApiScope is null) {
                _logger.LogWarning("Api scope with ApiScopePk {ApiScopePk} not found", apiScopePk);
                return false;
            }

            deletedApiScope.IsActive = false;
            await _db.SaveChangesAsync();

            _logger.LogInformation("Deleted api Scope with ApiScopePk {ApiScopePk}", apiScopePk);

            return true;
        }
    }
}
