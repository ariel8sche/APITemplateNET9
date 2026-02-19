using APITemplate.Contracts.Interfaces;
using APITemplate.Contracts.Models;
using APITemplate.Data;
using APITemplate.Data.Entities;
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
        public async Task<ActionResult<List<ClientDto>>> Get()
        {
            _logger.LogInformation("HTTP GET /clients requested");

            var clients = await _clientService.GetAll();

            return Ok(clients);
        }

        [HttpGet("{clientPk}")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(List<ClientDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<List<ClientDto>>> Get(int clientPk)
        {
            // Llamar al servicio para obtener el cliente moqueado
            _logger.LogInformation("HTTP GET /clients/{ClientPk} requested", clientPk);

            var clientes = await _clientService.GetById(clientPk);

            return Ok(clientes);
        }
    }
}
