using APITemplate.Contracts.Interfaces;
using APITemplate.Contracts.Requests;
using APITemplate.Contracts.Responses;
using APITemplate.Data.AuthModels;
using APITemplate.Services.Services;
using Microsoft.AspNetCore.Mvc;

namespace APITemplate.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccessTokensController : Controller
    {
        private readonly ILogger<AccessTokensController> _logger;
        private readonly IAccessTokenService _accessTokenService;

        public AccessTokensController(ILogger<AccessTokensController> logger, IAccessTokenService accessTokenService)
        {
            _logger = logger;
            _accessTokenService = accessTokenService;
        }

        [HttpGet]
        [Produces("application/json")]
        [ProducesResponseType(typeof(AccessTokenListResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<AccessTokenListResponse>> GetAll()
        {
            _logger.LogInformation("GetAll: fetching all access token");

            var accessTokens = await _accessTokenService.GetAll();

            _logger.LogDebug("GetAll: retrieved {Count} accessTokens", accessTokens?.Count ?? 0);

            var response = new AccessTokenListResponse
            {
                AccessTokens = accessTokens
            };

            return Ok(response);
        }

        [HttpGet("{accessTokenId}")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(AccessTokenListResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<AccessTokenListResponse>> GetById(int accessTokenId)
        {
            _logger.LogInformation("GetById: fetching access token AccessTokenId={AccessTokenId}", accessTokenId);

            var accessToken = await _accessTokenService.GetById(accessTokenId);

            if (accessToken is null || accessToken.Count == 0)
            {
                _logger.LogWarning("GetById: no access token found for AccessTokenId={AccessTokenId}", accessTokenId);
            }
            else
            {
                _logger.LogDebug("GetById: retrieved {Count} accessToken(s) for AccessTokenId={AccessTokenId}", accessToken.Count, accessTokenId);
            }

            var response = new AccessTokenListResponse
            {
                AccessTokens = accessToken
            };

            return Ok(response);
        }

        [HttpPost]
        [ProducesResponseType(typeof(CreateAccessTokenResponse), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<CreateAccessTokenResponse>> Create([FromBody] CreateAccessTokenRequest request)
        {
            _logger.LogInformation("Create: HTTP POST /api/accessToken requested");

            if (!ModelState.IsValid)
            {
                _logger.LogWarning("Create: invalid model state");
                return BadRequest(ModelState);
            }

            var createdAccessToken = await _accessTokenService.Create(request);

            _logger.LogInformation("Create: created access token");

            var response = new CreateAccessTokenResponse
            {
                Success = true,
                AccessTokenId = createdAccessToken,
            };

            return CreatedAtAction(
                nameof(GetById),
                new { accessTokenId = createdAccessToken },
                response);
        }

        [HttpPut("{accessTokenId}")]
        [ProducesResponseType(typeof(UpdateUserResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<UpdateAccessTokenResponse>> Update(int accessTokenId,
        [FromBody] UpdateAccessTokenRequest request)
        {
            _logger.LogInformation("Update: HTTP PUT /api/accessToken/{AccessTokenId} requested", accessTokenId);

            if (!ModelState.IsValid)
            {
                _logger.LogWarning("Update: invalid model state for AccessTokenId={AccessTokenId}", accessTokenId);
                return BadRequest(ModelState);
            }

            var updated = await _accessTokenService.Update(accessTokenId, request);

            if (!updated)
            {
                _logger.LogWarning("Update: access token not found AccessTokenId={AccessTokenId}", accessTokenId);
                return NotFound();
            }

            _logger.LogInformation("Update: updated access token AccessTokenId={AccessTokenId} (IsRevoked={IsRevoked})", accessTokenId, request.IsRevoked);

            return Ok(new UpdateAccessTokenResponse
            {
                Success = true,
                AccessTokenId = accessTokenId
            });
        }

        [HttpDelete("{accessTokenId}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Delete(int accessTokenId)
        {
            _logger.LogInformation("Delete: HTTP DELETE /api/accessToken/{AccessTokenId} requested", accessTokenId);

            var deleted = await _accessTokenService.Delete(accessTokenId);

            if (!deleted)
            {
                _logger.LogWarning("Delete: access token not found AccessTokenId={AccessTokenId}", accessTokenId);
                return NotFound();
            }

            _logger.LogInformation("Delete: deactivated access token AccessTokenId={AccessTokenId}", accessTokenId);

            return NoContent();
        }
    }
}
