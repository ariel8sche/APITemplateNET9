using APITemplate.Contracts.Interfaces;
using APITemplate.Contracts.Requests;
using APITemplate.Contracts.Responses;
using APITemplate.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace APITemplate.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]

    public class ClientsController : ControllerBase
    {

        private readonly ILogger<ClientsController> _logger;
        private readonly IClientService _clientService;

        public ClientsController(ILogger<ClientsController> logger, IClientService clientService)
        {
            _logger = logger;
            _clientService = clientService;
        }

        [HttpGet]
        [Produces("application/json")]
        [ProducesResponseType(typeof(ClientListResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ClientListResponse>> GetAll()
        {
            _logger.LogInformation("GetAll: fetching all clients");

            var clients = await _clientService.GetAll();

            _logger.LogDebug("GetAll: retrieved {Count} clients", clients?.Count ?? 0);

            var response = new ClientListResponse
            {
                Clients = clients
            };

            return Ok(response);
        }

        [HttpGet("{clientPk}")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(ClientListResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ClientListResponse>> GetById(int clientPk)
        {
            _logger.LogInformation("GetById: fetching client with ClientPk={ClientPk}", clientPk);

            var clients = await _clientService.GetById(clientPk);

            if (clients is null || clients.Count == 0)
                _logger.LogWarning("GetById: no client found for ClientPk={ClientPk}", clientPk);
            else
                _logger.LogDebug("GetById: retrieved {Count} client(s) for ClientPk={ClientPk}", clients.Count, clientPk);

            var response = new ClientListResponse
            {
                Clients = clients
            };

            return Ok(response);
        }

        [HttpPost]
        [ProducesResponseType(typeof(CreateClientResponse), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<CreateClientResponse>> Create([FromBody] CreateClientRequest request)
        {
            _logger.LogInformation("Create: HTTP POST /api/clients requested (ClientId={ClientId})", request.ClientId);

            if (!ModelState.IsValid)
            {
                _logger.LogWarning("Create: invalid model state for ClientId={ClientId}", request.ClientId);
                return BadRequest(ModelState);
            }

            var createdClient = await _clientService.Create(request);

            _logger.LogInformation("Create: created client ClientPk={ClientPk} ClientId={ClientId}", createdClient, request.ClientId);

            var response = new CreateClientResponse
            {
                Success = true,
                ClientPk = createdClient,
            };

            return CreatedAtAction(
                nameof(GetById),
                new { clientPk = createdClient },
                response);
        }

        [HttpPut("{clientPk}")]
        [ProducesResponseType(typeof(UpdateClientResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<UpdateClientResponse>> Update(
            int clientPk,
            [FromBody] UpdateClientRequest request)
        {
            _logger.LogInformation("Update: HTTP PUT /api/clients/{ClientPk} requested", clientPk);

            if (!ModelState.IsValid)
            {
                _logger.LogWarning("Update: invalid model state for ClientPk={ClientPk}", clientPk);
                return BadRequest(ModelState);
            }

            var updated = await _clientService.Update(clientPk, request);

            if (!updated)
            {
                _logger.LogWarning("Update: client not found ClientPk={ClientPk}", clientPk);
                return NotFound();
            }

            _logger.LogInformation("Update: updated client ClientPk={ClientPk} (IsActive={IsActive})", clientPk, request.IsActive);

            return Ok(new UpdateClientResponse
            {
                Success = true,
                ClientPk = clientPk
            });
        }

        [HttpDelete("{clientPk}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Delete(int clientPk)
        {
            _logger.LogInformation("Delete: HTTP DELETE /api/clients/{ClientPk} requested", clientPk);

            var deleted = await _clientService.Delete(clientPk);

            if (!deleted)
            {
                _logger.LogWarning("Delete: client not found ClientPk={ClientPk}", clientPk);
                return NotFound();
            }

            _logger.LogInformation("Delete: deleted client ClientPk={ClientPk}", clientPk);

            return NoContent();
        }
    }
}
