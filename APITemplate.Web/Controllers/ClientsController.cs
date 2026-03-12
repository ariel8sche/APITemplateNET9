using APITemplate.Contracts.Interfaces;
using APITemplate.Contracts.Models;
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
            _logger.LogInformation("HTTP GET /clients requested");

            var clients = await _clientService.GetAll();

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
            // Llamar al servicio para obtener el cliente moqueado
            _logger.LogInformation("HTTP GET /clients/{ClientPk} requested", clientPk);

            var clients = await _clientService.GetById(clientPk);

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
            _logger.LogInformation("HTTP POST /api/clients requested");

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var createdClient = await _clientService.Create(request);

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
            _logger.LogInformation("HTTP PUT /api/clients/{ClientPk} requested", clientPk);

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var updated = await _clientService.Update(clientPk, request);

            if (!updated)
                return NotFound();

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
            _logger.LogInformation("HTTP DELETE /api/clients/{ClientPk} requested", clientPk);

            var deleted = await _clientService.Delete(clientPk);

            if (!deleted)
                return NotFound();

            return NoContent();
        }
    }
}
