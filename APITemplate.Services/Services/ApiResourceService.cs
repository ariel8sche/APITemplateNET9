using APITemplate.Contracts.Dtos;
using APITemplate.Contracts.Interfaces;
using APITemplate.Contracts.Requests;
using APITemplate.Data.AuthModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace APITemplate.Services.Services
{
    public class ApiResourceService : IApiResourceService
    {
        private readonly AuthContext _db;

        private readonly ILogger<ApiResourceService> _logger;

        public ApiResourceService(ILogger<ApiResourceService> logger, AuthContext db)
        {
            _logger = logger;
            _db = db;
        }

        public async Task<List<ApiResourceDto>> GetById(int ApiResourcePk)
        {
            _logger.LogInformation("GetById: fetching API resource ApiResourcePk={ApiResourcePk}", ApiResourcePk);

            var apiResource = await _db.ApiResources
                .Where(e => e.ApiResourcePk == ApiResourcePk)
                .Select(e => new ApiResourceDto
                {
                    ApiResourcePk = e.ApiResourcePk,
                    Name = e.Name,
                    Audience = e.Audience,
                    IsActive = e.IsActive,
                    CreatedAt = e.CreatedAt
                })
                .ToListAsync();

            _logger.LogDebug("GetById: retrieved {Count} api resource(s) for ApiResourcePk={ApiResourcePk}", apiResource.Count, ApiResourcePk);

            if (apiResource is null || apiResource.Count == 0)
            {
                _logger.LogWarning("GetById: no API resource found for ApiResourcePk={ApiResourcePk}", ApiResourcePk);
            }

            return apiResource;
        }

        public async Task<List<ApiResourceDto>> GetAll()
        {
            _logger.LogInformation("GetAll: fetching all API resources");

            var items = await _db.ApiResources
                .OrderBy(e => e.ApiResourcePk)
                .Select(e => new ApiResourceDto
                {
                    ApiResourcePk = e.ApiResourcePk,
                    Name = e.Name,
                    Audience = e.Audience,
                    IsActive = e.IsActive,
                    CreatedAt = e.CreatedAt
                })
                .ToListAsync();

            _logger.LogDebug("GetAll: retrieved {Count} api resources", items.Count);

            return items;
        }

        public async Task<int> Create(CreateApiResourceRequest request)
        {
            _logger.LogInformation("Create: creating API resource Name={Name}", request.Name);

            var apiResource = new ApiResource
            {
                Name = request.Name,
                Audience = request.Audience,
                IsActive = true,
                CreatedAt = DateTime.UtcNow
            };

            _db.ApiResources.Add(apiResource);

            await _db.SaveChangesAsync();

            _logger.LogInformation("Create: created API resource ApiResourcePk={ApiResourcePk} Name={Name}", apiResource.ApiResourcePk, apiResource.Name);

            return apiResource.ApiResourcePk;
        }

        public async Task<bool> Update(int apiResourcePk, UpdateApiResourceRequest request)
        {
            _logger.LogInformation("Update: updating API resource ApiResourcePk={ApiResourcePk}", apiResourcePk);

            var apiResource = await _db.ApiResources.FirstOrDefaultAsync(e => e.ApiResourcePk == apiResourcePk);

            if (apiResource is null) {
                _logger.LogWarning("Update: API resource not found ApiResourcePk={ApiResourcePk}", apiResourcePk);
                return false;
            }

            apiResource.Name = request.Name;
            apiResource.IsActive = request.IsActive;

            await _db.SaveChangesAsync();

            _logger.LogInformation("Update: updated API resource ApiResourcePk={ApiResourcePk} (IsActive={IsActive})", apiResourcePk, request.IsActive);

            return true;
        }

        public async Task<bool> Delete(int apiResourcePk)
        {
            _logger.LogInformation("Delete: deactivating API resource ApiResourcePk={ApiResourcePk}", apiResourcePk);

            var deletedApiResource = await _db.ApiResources.FirstOrDefaultAsync(e => e.ApiResourcePk == apiResourcePk);

            if (deletedApiResource is null) {
                _logger.LogWarning("Delete: API resource not found ApiResourcePk={ApiResourcePk}", apiResourcePk);
                return false;
            }

            deletedApiResource.IsActive = false;
            await _db.SaveChangesAsync();

            _logger.LogInformation("Delete: deactivated API resource ApiResourcePk={ApiResourcePk}", apiResourcePk);

            return true;
        }
    }
}
