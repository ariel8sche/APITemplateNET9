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
            _logger.LogInformation("HTTP GET /apiScope requested");

            var apiScope = await _apiScopeService.GetAll();

            var response = new ApiScopeListResponse
            {
                ApiScopes = apiScope
            };

            return Ok(response);
        }

        [HttpGet("{apiScopePk}")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(ApiScopeListResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ApiScopeListResponse>> GetById(int apiScopePk)
        {
            // Llamar al servicio para obtener el api Scope moqueado
            _logger.LogInformation("HTTP GET /apiScope/{ApiScopePk} requested", apiScopePk);

            var apiScope = await _apiScopeService.GetById(apiScopePk);

            var response = new ApiScopeListResponse
            {
                ApiScopes = apiScope
            };

            return Ok(response);
        }

        [HttpPost]
        [ProducesResponseType(typeof(CreateApiScopeResponse), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<CreateApiScopeResponse>> Create([FromBody] CreateApiScopeRequest request)
        {
            _logger.LogInformation("HTTP POST /api/apiScope requested");

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var createdApiScope = await _apiScopeService.Create(request);

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
            _logger.LogInformation("HTTP PUT /api/apiScope/{ApiScopePk} requested", apiScopePk);

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var updated = await _apiScopeService.Update(apiScopePk, request);

            if (!updated)
                return NotFound();

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
            _logger.LogInformation("HTTP DELETE /api/apiScope/{apiScopePk} requested", apiScopePk);

            var deleted = await _apiScopeService.Delete(apiScopePk);

            if (!deleted)
                return NotFound();

            return NoContent();
        }
    }
}
