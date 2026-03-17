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
            _logger.LogInformation("GetAll: fetching all client scope grants");

            var clientScopeGrant = await _clientScopeGrantService.GetAll();

            _logger.LogDebug("GetAll: retrieved {Count} client scope grants", clientScopeGrant.Count);

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
            _logger.LogInformation("GetById: fetching client scope grant with GrantPk={ClientScopeGrantPk}", clientScopeGrantPk);

            var clientScopeGrant = await _clientScopeGrantService.GetById(clientScopeGrantPk);

            _logger.LogDebug("GetById: retrieved {Count} client scope grants for GrantPk={ClientScopeGrantPk}", clientScopeGrant.Count, clientScopeGrantPk);

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
            _logger.LogInformation("Create: HTTP POST /api/clientscopegrants requested for ClientPk={ClientPk} ScopePk={ScopePk}", request.ClientPk, request.ScopePk);

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var createdClientScopeGrant = await _clientScopeGrantService.Create(request);

            _logger.LogInformation("Create: created client scope grant GrantPk={GrantPk} for ClientPk={ClientPk} ScopePk={ScopePk}", createdClientScopeGrant, request.ClientPk, request.ScopePk);

            var response = new CreateClientScopeGrantResponse
            {
                Success = true,
                GrantPk = createdClientScopeGrant,
            };

            return CreatedAtAction(
                nameof(GetById),
                new { clientScopeGrantPk = createdClientScopeGrant },
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
            _logger.LogInformation("Update: HTTP PUT /api/clientscopegrants/{GrantPk} requested for GrantPk={ClientScopeGrantPk}", clientScopeGrantPk, clientScopeGrantPk);

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var updated = await _clientScopeGrantService.Update(clientScopeGrantPk, request);

            if (!updated)
            {
                _logger.LogWarning("Update: client scope grant not found GrantPk={ClientScopeGrantPk}", clientScopeGrantPk);
                return NotFound();
            }

            _logger.LogInformation("Update: updated client scope grant GrantPk={ClientScopeGrantPk} (IsActive={IsActive})", clientScopeGrantPk, request.IsActive);

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
            _logger.LogInformation("Delete: HTTP DELETE /api/clientscopegrants/{clientScopeGrantPk} requested for GrantPk={ClientScopeGrantPk}", clientScopeGrantPk, clientScopeGrantPk);

            var deleted = await _clientScopeGrantService.Delete(clientScopeGrantPk);

            if (!deleted)
            {
                _logger.LogWarning("Delete: client scope grant not found GrantPk={ClientScopeGrantPk}", clientScopeGrantPk);
                return NotFound();
            }

            _logger.LogInformation("Delete: removed client scope grant GrantPk={ClientScopeGrantPk}", clientScopeGrantPk);

            return NoContent();
        }
    }
}
