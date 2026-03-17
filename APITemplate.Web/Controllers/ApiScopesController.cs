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

    public class ApiScopesController : ControllerBase
    {

        private readonly ILogger<ApiScopesController> _logger;
        private readonly IApiScopeService _apiScopeService;

        public ApiScopesController(ILogger<ApiScopesController> logger, IApiScopeService apiScopeService)
        {
            _logger = logger;
            _apiScopeService = apiScopeService;
        }

        [HttpGet]
        [Produces("application/json")]
        [ProducesResponseType(typeof(ApiScopeListResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ApiScopeListResponse>> GetAll()
        {
            _logger.LogInformation("GetAll: fetching all API scopes");

            var apiScopes = await _apiScopeService.GetAll();

            _logger.LogDebug("GetAll: retrieved {Count} api scopes", apiScopes.Count);

            var response = new ApiScopeListResponse
            {
                ApiScopes = apiScopes
            };

            return Ok(response);
        }

        [HttpGet("{apiScopePk}")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(ApiScopeListResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ApiScopeListResponse>> GetById(int apiScopePk)
        {
            _logger.LogInformation("GetById: fetching API scope with ApiScopePk={ApiScopePk}", apiScopePk);

            var apiScopes = await _apiScopeService.GetById(apiScopePk);

            _logger.LogDebug("GetById: retrieved {Count} api scopes for ApiScopePk={ApiScopePk}", apiScopes.Count, apiScopePk);

            var response = new ApiScopeListResponse
            {
                ApiScopes = apiScopes
            };

            return Ok(response);
        }

        [HttpPost]
        [ProducesResponseType(typeof(CreateApiScopeResponse), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<CreateApiScopeResponse>> Create([FromBody] CreateApiScopeRequest request)
        {
            _logger.LogInformation("Create: HTTP POST /api/apiscopes requested for ApiResourcePk={ApiResourcePk} ScopeName={ScopeName}", request.ApiResourcePk, request.ScopeName);

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var createdApiScope = await _apiScopeService.Create(request);

            _logger.LogInformation("Create: created API scope ApiScopePk={ApiScopePk} for ApiResourcePk={ApiResourcePk} ScopeName={ScopeName}", createdApiScope, request.ApiResourcePk, request.ScopeName);

            var response = new CreateApiScopeResponse
            {
                Success = true,
                ApiScopePk = createdApiScope,
            };

            return CreatedAtAction(
                nameof(GetById),
                new { apiScopePk = createdApiScope },
                response);
        }

        [HttpPut("{apiScopePk}")]
        [ProducesResponseType(typeof(UpdateApiScopeResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<UpdateApiScopeResponse>> Update(
            int apiScopePk,
            [FromBody] UpdateApiScopeRequest request)
        {
            _logger.LogInformation("Update: HTTP PUT /api/apiscopes/{ApiScopePk} requested", apiScopePk);

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var updated = await _apiScopeService.Update(apiScopePk, request);

            if (!updated)
            {
                _logger.LogWarning("Update: API scope not found ApiScopePk={ApiScopePk}", apiScopePk);
                return NotFound();
            }

            _logger.LogInformation("Update: updated API scope ApiScopePk={ApiScopePk} (IsActive={IsActive})", apiScopePk, request.IsActive);

            return Ok(new UpdateApiScopeResponse
            {
                Success = true,
                ApiScopePk = apiScopePk
            });
        }

        [HttpDelete("{apiScopePk}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Delete(int apiScopePk)
        {
            _logger.LogInformation("Delete: HTTP DELETE /api/apiscopes/{ApiScopePk} requested", apiScopePk);

            var deleted = await _apiScopeService.Delete(apiScopePk);

            if (!deleted)
            {
                _logger.LogWarning("Delete: API scope not found ApiScopePk={ApiScopePk}", apiScopePk);
                return NotFound();
            }

            _logger.LogInformation("Delete: removed API scope ApiScopePk={ApiScopePk}", apiScopePk);

            return NoContent();
        }
    }
}
