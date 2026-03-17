using APITemplate.Contracts.Interfaces;
using APITemplate.Contracts.Requests;
using APITemplate.Contracts.Responses;
using APITemplate.Data;
using APITemplate.Services.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace APITemplate.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]

    public class ApiResourcesController : ControllerBase
    {

        private readonly ILogger<ApiResourcesController> _logger;
        private readonly IApiResourceService _apiResourceService;

        public ApiResourcesController(ILogger<ApiResourcesController> logger, IApiResourceService apiResourceService)
        {
            _logger = logger;
            _apiResourceService = apiResourceService;
        }

        [HttpGet]
        [Produces("application/json")]
        [ProducesResponseType(typeof(ApiResourceListResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ApiResourceListResponse>> GetAll()
        {
            _logger.LogInformation("GetAll: fetching all API resources");

            var apiResources = await _apiResourceService.GetAll();

            _logger.LogDebug("GetAll: retrieved {Count} api resources", apiResources?.Count ?? 0);

            var response = new ApiResourceListResponse
            {
                ApiResources = apiResources
            };

            return Ok(response);
        }

        [HttpGet("{apiResourcePk}")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(ApiResourceListResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ApiResourceListResponse>> GetById(int apiResourcePk)
        {
            _logger.LogInformation("GetById: fetching API resource ApiResourcePk={ApiResourcePk}", apiResourcePk);

            var apiResources = await _apiResourceService.GetById(apiResourcePk);

            if (apiResources is null || apiResources.Count == 0)
            {
                _logger.LogWarning("GetById: no API resource found for ApiResourcePk={ApiResourcePk}", apiResourcePk);
            }
            else
            {
                _logger.LogDebug("GetById: retrieved {Count} api resource(s) for ApiResourcePk={ApiResourcePk}", apiResources.Count, apiResourcePk);
            }

            var response = new ApiResourceListResponse
            {
                ApiResources = apiResources
            };

            return Ok(response);
        }

        [HttpPost]
        [ProducesResponseType(typeof(CreateApiResourceResponse), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<CreateApiResourceResponse>> Create([FromBody] CreateApiResourceRequest request)
        {
            _logger.LogInformation("Create: HTTP POST /api/apiresources requested (Name={Name})", request.Name);

            if (!ModelState.IsValid)
            {
                _logger.LogWarning("Create: invalid model state for API resource Name={Name}", request.Name);
                return BadRequest(ModelState);
            }

            var createdApiResource = await _apiResourceService.Create(request);

            _logger.LogInformation("Create: created API resource ApiResourcePk={ApiResourcePk} Name={Name}", createdApiResource, request.Name);

            var response = new CreateApiResourceResponse
            {
                Success = true,
                ApiResourcePk = createdApiResource,
            };

            return CreatedAtAction(
                nameof(GetById),
                new { apiResourcePk = createdApiResource },
                response);
        }

        [HttpPut("{apiResourcePk}")]
        [ProducesResponseType(typeof(UpdateApiResourceResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<UpdateApiResourceResponse>> Update(
            int apiResourcePk,
            [FromBody] UpdateApiResourceRequest request)
        {
            _logger.LogInformation("Update: HTTP PUT /api/apiresources/{ApiResourcePk} requested", apiResourcePk);

            if (!ModelState.IsValid)
            {
                _logger.LogWarning("Update: invalid model state for ApiResourcePk={ApiResourcePk}", apiResourcePk);
                return BadRequest(ModelState);
            }

            var updated = await _apiResourceService.Update(apiResourcePk, request);

            if (!updated)
            {
                _logger.LogWarning("Update: API resource not found ApiResourcePk={ApiResourcePk}", apiResourcePk);
                return NotFound();
            }

            _logger.LogInformation("Update: updated API resource ApiResourcePk={ApiResourcePk} (IsActive={IsActive})", apiResourcePk, request.IsActive);

            return Ok(new UpdateApiResourceResponse
            {
                Success = true,
                ApiResourcePk = apiResourcePk
            });
        }

        [HttpDelete("{apiResourcePk}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Delete(int apiResourcePk)
        {
            _logger.LogInformation("Delete: HTTP DELETE /api/apiresources/{ApiResourcePk} requested", apiResourcePk);

            var deleted = await _apiResourceService.Delete(apiResourcePk);

            if (!deleted)
            {
                _logger.LogWarning("Delete: API resource not found ApiResourcePk={ApiResourcePk}", apiResourcePk);
                return NotFound();
            }

            _logger.LogInformation("Delete: deactivated API resource ApiResourcePk={ApiResourcePk}", apiResourcePk);

            return NoContent();
        }
    }
}
