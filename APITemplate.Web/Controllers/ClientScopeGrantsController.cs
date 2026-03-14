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

    public class ClientScopeGrantsController : ControllerBase
    {

        private readonly ILogger<ClientScopeGrantsController> _logger;
        private readonly IClientScopeGrantService _clientScopeGrantService;

        public ClientScopeGrantsController(ILogger<ClientScopeGrantsController> logger, IClientScopeGrantService clientScopeGrantService)
        {
            _logger = logger;
            _clientScopeGrantService = clientScopeGrantService;
        }

        [HttpGet]
        [Produces("application/json")]
        [ProducesResponseType(typeof(ClientScopeGrantListResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ClientScopeGrantListResponse>> GetAll()
        {
            _logger.LogInformation("HTTP GET /ClientScopeGrant requested");

            var clientScopeGrant = await _clientScopeGrantService.GetAll();

            var response = new ClientScopeGrantListResponse
            {
                ClientScopeGrants = clientScopeGrant
            };

            return Ok(response);
        }

        [HttpGet("{clientScopeGrantPk}")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(ClientScopeGrantListResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ClientScopeGrantListResponse>> GetById(int clientScopeGrantPk)
        {
            // Llamar al servicio para obtener el client scope grant moqueado
            _logger.LogInformation("HTTP GET /clientScopeGrant/{clientScopeGrantPk} requested", clientScopeGrantPk);

            var clientScopeGrant = await _clientScopeGrantService.GetById(clientScopeGrantPk);

            var response = new ClientScopeGrantListResponse
            {
                ClientScopeGrants = clientScopeGrant
            };

            return Ok(response);
        }

        [HttpPost]
        [ProducesResponseType(typeof(CreateClientScopeGrantResponse), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<CreateClientScopeGrantResponse>> Create([FromBody] CreateClientScopeGrantRequest request)
        {
            _logger.LogInformation("HTTP POST /api/clientScopeGrant requested");

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var createdClientScopeGrant = await _clientScopeGrantService.Create(request);

            var response = new CreateClientScopeGrantResponse
            {
                Success = true,
                GrantPk = createdClientScopeGrant,
            };

            return CreatedAtAction(
                nameof(GetById),
                new { ClientScopeGrantPk = createdClientScopeGrant },
                response);
        }

        [HttpPut("{clientScopeGrantPk}")]
        [ProducesResponseType(typeof(UpdateClientScopeGrantResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<UpdateClientScopeGrantResponse>> Update(
            int clientScopeGrantPk,
            [FromBody] UpdateClientScopeGrantRequest request)
        {
            _logger.LogInformation("HTTP PUT /api/clientScopeGrant/{clientScopeGrantPk} requested", clientScopeGrantPk);

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var updated = await _clientScopeGrantService.Update(clientScopeGrantPk, request);

            if (!updated)
                return NotFound();

            return Ok(new UpdateClientScopeGrantResponse
            {
                Success = true,
                GrantPk = clientScopeGrantPk
            });
        }

        [HttpDelete("{clientScopeGrantPk}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Delete(int clientScopeGrantPk)
        {
            _logger.LogInformation("HTTP DELETE /api/clientScopeGrant/{clientScopeGrantPk} requested", clientScopeGrantPk);

            var deleted = await _clientScopeGrantService.Delete(clientScopeGrantPk);

            if (!deleted)
                return NotFound();

            return NoContent();
        }
    }
}
