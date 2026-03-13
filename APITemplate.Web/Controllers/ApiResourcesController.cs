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
            _logger.LogInformation("HTTP GET /apiResource requested");

            var apiResource = await _apiResourceService.GetAll();

            var response = new ApiResourceListResponse
            {
                ApiResources = apiResource
            };

            return Ok(response);
        }

        [HttpGet("{apiResourcePk}")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(ApiResourceListResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ApiResourceListResponse>> GetById(int apiResourcePk)
        {
            // Llamar al servicio para obtener el api resource moqueado
            _logger.LogInformation("HTTP GET /apiResource/{ApiResourcePk} requested", apiResourcePk);

            var apiResource = await _apiResourceService.GetById(apiResourcePk);

            var response = new ApiResourceListResponse
            {
                ApiResources = apiResource
            };

            return Ok(response);
        }

        [HttpPost]
        [ProducesResponseType(typeof(CreateApiResourceResponse), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<CreateApiResourceResponse>> Create([FromBody] CreateApiResourceRequest request)
        {
            _logger.LogInformation("HTTP POST /api/apiResource requested");

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var createdApiResource = await _apiResourceService.Create(request);

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
            _logger.LogInformation("HTTP PUT /api/apiResource/{ApiResourcePk} requested", apiResourcePk);

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var updated = await _apiResourceService.Update(apiResourcePk, request);

            if (!updated)
                return NotFound();

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
            _logger.LogInformation("HTTP DELETE /api/apiResource/{apiResourcePk} requested", apiResourcePk);

            var deleted = await _apiResourceService.Delete(apiResourcePk);

            if (!deleted)
                return NotFound();

            return NoContent();
        }
    }
}
