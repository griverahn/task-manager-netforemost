using Microsoft.AspNetCore.Mvc;
using NetForemost.Models;
using NetForemost.Services;

namespace NetForemost.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly UserService _userService;
        private readonly ILogger<UsersController> _logger;

        public UsersController(UserService userService, ILogger<UsersController> logger)
        {
            _userService = userService;
            _logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllUsers()
        {
            _logger.LogInformation("GetAllUsers called.");
            var users = await _userService.GetAllUsersAsync();
            return Ok(users);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetUserById(int id)
        {
            _logger.LogInformation($"GetUserById called with ID: {id}");
            var user = await _userService.GetUserByIdAsync(id);
            if (user == null)
            {
                _logger.LogWarning($"User with ID: {id} not found.");
                return NotFound();
            }
            return Ok(user);
        }

        [HttpPost]
        public async Task<IActionResult> CreateUser(User user)
        {
            _logger.LogInformation("CreateUser called.");
            var createdUser = await _userService.CreateUserAsync(user);
            return CreatedAtAction(nameof(GetUserById), new { id = createdUser.Id }, createdUser);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateUser(int id, User user)
        {
            if (id != user.Id)
            {
                _logger.LogWarning("UpdateUser called with mismatched ID.");
                return BadRequest();
            }
            _logger.LogInformation($"UpdateUser called for ID: {id}");
            await _userService.UpdateUserAsync(user);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            _logger.LogInformation($"DeleteUser called for ID: {id}");
            await _userService.DeleteUserAsync(id);
            return NoContent();
        }
    }
}
