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

    public class ClientSecretsController : ControllerBase
    {

        private readonly ILogger<ClientSecretsController> _logger;
        private readonly IClientSecretService _clientSecretService;

        public ClientSecretsController(ILogger<ClientSecretsController> logger, IClientSecretService clientSecretService)
        {
            _logger = logger;
            _clientSecretService = clientSecretService;
        }

        [HttpGet]
        [Produces("application/json")]
        [ProducesResponseType(typeof(ClientSecretListResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ClientSecretListResponse>> GetAll()
        {
            _logger.LogInformation("GetAll: fetching all client secrets");

            var clientSecret = await _clientSecretService.GetAll();

            _logger.LogDebug("GetAll: retrieved {Count} client secrets", clientSecret.Count);

            var response = new ClientSecretListResponse
            {
                ClientSecrets = clientSecret
            };

            return Ok(response);
        }

        [HttpGet("{clientSecretPk}")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(ClientSecretListResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ClientSecretListResponse>> GetById(int clientSecretPk)
        {
            _logger.LogInformation("GetById: fetching client secret with ClientSecretPk={ClientSecretPk}", clientSecretPk);

            var clientSecret = await _clientSecretService.GetById(clientSecretPk);

            _logger.LogDebug("GetById: retrieved {Count} client secrets for ClientSecretPk={ClientSecretPk}", clientSecret.Count, clientSecretPk);

            var response = new ClientSecretListResponse
            {
                ClientSecrets = clientSecret
            };

            return Ok(response);
        }

        [HttpPost]
        [ProducesResponseType(typeof(CreateClientSecretResponse), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<CreateClientSecretResponse>> Create([FromBody] CreateClientSecretRequest request)
        {
            _logger.LogInformation("Create: HTTP POST /api/clientsecrets requested for ClientPk={ClientPk}", request.ClientPk);

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var createdClientSecret = await _clientSecretService.Create(request);

            _logger.LogInformation("Create: created client secret ClientSecretPk={ClientSecretPk} for ClientPk={ClientPk}", createdClientSecret, request.ClientPk);

            var response = new CreateClientSecretResponse
            {
                Success = true,
                ClientSecretPk = createdClientSecret,
            };

            return CreatedAtAction(
                nameof(GetById),
                new { clientSecretPk = createdClientSecret },
                response);
        }

        [HttpPut("{clientSecretPk}")]
        [ProducesResponseType(typeof(UpdateClientSecretResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<UpdateClientSecretResponse>> Update(
            int clientSecretPk,
            [FromBody] UpdateClientSecretRequest request)
        {
            _logger.LogInformation("Update: HTTP PUT /api/clientsecrets/{ClientSecretPk} requested", clientSecretPk);

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var updated = await _clientSecretService.Update(clientSecretPk, request);

            if (!updated)
            {
                _logger.LogWarning("Update: client secret not found ClientSecretPk={ClientSecretPk}", clientSecretPk);
                return NotFound();
            }

            _logger.LogInformation("Update: updated client secret ClientSecretPk={ClientSecretPk}", clientSecretPk);

            return Ok(new UpdateClientSecretResponse
            {
                Success = true,
                ClientSecretPk = clientSecretPk
            });
        }

        [HttpDelete("{clientSecretPk}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Delete(int clientSecretPk)
        {
            _logger.LogInformation("Delete: HTTP DELETE /api/clientsecrets/{ClientSecretPk} requested", clientSecretPk);

            var deleted = await _clientSecretService.Delete(clientSecretPk);

            if (!deleted)
            {
                _logger.LogWarning("Delete: client secret not found ClientSecretPk={ClientSecretPk}", clientSecretPk);
                return NotFound();
            }

            _logger.LogInformation("Delete: revoked client secret ClientSecretPk={ClientSecretPk}", clientSecretPk);

            return NoContent();
        }
    }
}
