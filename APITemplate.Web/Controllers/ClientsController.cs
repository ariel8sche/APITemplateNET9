using APITemplate.Contracts.Interfaces;
using APITemplate.Contracts.Models;
using APITemplate.Data;
using APITemplate.Data.NeonDb;
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
        [ProducesResponseType(typeof(List<ClientDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ClientListResponse>> GetAll ()
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
        [ProducesResponseType(typeof(List<ClientDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<List<ClientDto>>> GetById(int clientPk)
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
        [ProducesResponseType(typeof(ClientDto), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<ClientDto>> Create([FromBody] CreateClientRequest dto)
        {
            _logger.LogInformation("HTTP POST /api/clients requested");

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var createdClient = await _clientService.Create(dto);

            var response = new CreateClientResponse
            {
                Success = true,
                ClientId = createdClient.ClientId,
            };

            return CreatedAtAction(
                nameof(GetById),
                new { clientPk = createdClient.ClientPk },
                response);
        }
    }
}
