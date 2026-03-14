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
            _logger.LogInformation("Fetching api resource with ApiResourcePk {ApiResourcePk}", ApiResourcePk);

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

            return apiResource;
        }

        public async Task<List<ApiResourceDto>> GetAll()
        {
            _logger.LogInformation("Fetching all api resources");

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

            return items;
        }

        public async Task<int> Create(CreateApiResourceRequest request)
        {
            _logger.LogInformation("Creating new api resource with Name {Name}", request.Name);

            var apiResource = new ApiResource
            {
                Name = request.Name,
                Audience = request.Audience,
                IsActive = true,
                CreatedAt = DateTime.UtcNow
            };

            _db.ApiResources.Add(apiResource);

            await _db.SaveChangesAsync();

            return apiResource.ApiResourcePk;
        }

        public async Task<bool> Update(int apiResourcePk, UpdateApiResourceRequest request)
        {
            _logger.LogInformation("Updating api resource with ApiResourcePk {ApiResourcePk}", apiResourcePk);

            var apiResource = await _db.ApiResources.FirstOrDefaultAsync(e => e.ApiResourcePk == apiResourcePk);

            if (apiResource is null) {
                _logger.LogWarning("Api resource with ApiResourcePk {ApiResourcePk} not found", apiResourcePk);
                return false;
            }

            apiResource.Name = request.Name;
            apiResource.Audience = request.Audience;

            await _db.SaveChangesAsync();

            _logger.LogInformation("Updated api resource with ApiResourcePk {ApiResourcePk}", apiResourcePk);

            return true;
        }

        public async Task<bool> Delete(int apiResourcePk)
        {
            _logger.LogInformation("Deleting api resource with ApiResourcePk {ApiResourcePk}", apiResourcePk);

            var deletedApiResource = await _db.ApiResources.FirstOrDefaultAsync(e => e.ApiResourcePk == apiResourcePk);

            if (deletedApiResource is null) {
                _logger.LogWarning("Api resource with ApiResourcePk {ApiResourcePk} not found", apiResourcePk);
                return false;
            }

            deletedApiResource.IsActive = false;
            await _db.SaveChangesAsync();

            _logger.LogInformation("Deleted api resource with ApiResourcePk {ApiResourcePk}", apiResourcePk);

            return true;
        }
    }
}
