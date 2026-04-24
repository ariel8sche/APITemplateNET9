using APITemplate.Contracts.Interfaces;
using APITemplate.Contracts.Requests;
using APITemplate.Contracts.Responses;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace APITemplate.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]

    public class UsersController : ControllerBase
    {
        private readonly ILogger<UsersController> _logger;
        private readonly IUserService _userService;

        public UsersController(ILogger<UsersController> logger, IUserService userService)
        {
            _logger = logger;
            _userService = userService;
        }

        [HttpGet]
        [Produces("application/json")]
        [ProducesResponseType(typeof(UserListResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<UserListResponse>> GetAll()
        {
            _logger.LogInformation("GetAll: fetching all users");

            var users = await _userService.GetAll();

            _logger.LogDebug("GetAll: retrieved {Count} users", users?.Count ?? 0);

            var response = new UserListResponse
            {
                Users = users
            };

            return Ok(response);
        }

        [HttpGet("{userId}")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(UserListResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<UserListResponse>> GetById(int userId)
        {
            _logger.LogInformation("GetById: fetching user UserId={UserId}", userId);

            var users = await _userService.GetById(userId);

            if (users is null || users.Count == 0)
            {
                _logger.LogWarning("GetById: no user found for UserId={UserId}", userId);
            }
            else
            {
                _logger.LogDebug("GetById: retrieved {Count} user(s) for UserId={UserId}", users.Count, userId);
            }

            var response = new UserListResponse
            {
                Users = users
            };

            return Ok(response);
        }

        [HttpPost]
        [ProducesResponseType(typeof(CreateUserResponse), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<CreateUserResponse>> Create([FromBody] CreateUserRequest request)
        {
            _logger.LogInformation("Create: HTTP POST /api/users requested (Username={Username})", request.Username);

            if (!ModelState.IsValid)
            {
                _logger.LogWarning("Create: invalid model state for Username={Username}", request.Username);
                return BadRequest(ModelState);
            }

            var createdUser = await _userService.Create(request);

            _logger.LogInformation("Create: created user UserId={UserId} Username={Username}", createdUser, request.Username);

            var response = new CreateUserResponse
            {
                Success = true,
                UserId = createdUser,
            };

            return CreatedAtAction(
                nameof(GetById),
                new { userId = createdUser },
                response);
        }

        [HttpPut("{userId}")]
        [ProducesResponseType(typeof(UpdateUserResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<UpdateUserResponse>> Update(
            int userId,
            [FromBody] UpdateUserRequest request)
        {
            _logger.LogInformation("Update: HTTP PUT /api/users/{UserId} requested", userId);

            if (!ModelState.IsValid)
            {
                _logger.LogWarning("Update: invalid model state for UserId={UserId}", userId);
                return BadRequest(ModelState);
            }

            var updated = await _userService.Update(userId, request);

            if (!updated)
            {
                _logger.LogWarning("Update: user not found UserId={UserId}", userId);
                return NotFound();
            }

            _logger.LogInformation("Update: updated user UserId={UserId} (IsActive={IsActive})", userId, request.IsActive);

            return Ok(new UpdateUserResponse
            {
                Success = true,
                UserId = userId
            });
        }

        [HttpDelete("{userId}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Delete(int userId)
        {
            _logger.LogInformation("Delete: HTTP DELETE /api/users/{UserId} requested", userId);

            var deleted = await _userService.Delete(userId);

            if (!deleted)
            {
                _logger.LogWarning("Delete: user not found UserId={UserId}", userId);
                return NotFound();
            }

            _logger.LogInformation("Delete: deactivated user UserId={UserId}", userId);

            return NoContent();
        }
    }
}