using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using UserManagement.Application.Dtos.UsersDtos;
using UserManagement.Application.Interfaces;
using UserManagement.Application.Shared;

namespace UserManagement.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _service;
        private readonly ILogger<UsersController> _logger;
        private readonly IConfiguration _config;

        public UsersController(IUserService service, ILogger<UsersController> logger, IConfiguration config)
        {
            _service = service;
            _logger = logger;
            _config = config;
        }
        [HttpGet("paged")]
        public async Task<IActionResult> GetPaged([FromQuery] GridParams gridParams)
        {

            var result = await _service.GetPagedUsersAsync(gridParams);

            return Ok(result);
        }
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            _logger.LogInformation("Fetching all users at {time}", DateTime.UtcNow);

            var users = await _service.GetAllAsync();

            _logger.LogInformation("Retrieved {count} users", users.Count());
            return Ok(users);
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(int id)
        {
            _logger.LogInformation("Fetching user with Id={id}", id);

            var user = await _service.GetByIdAsync(id);
            if (user == null)
            {
                _logger.LogWarning("User with Id={id} not found", id);
                return NotFound();
            }

            _logger.LogInformation("User with Id={id} retrieved successfully", id);
            return Ok(user);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] UserPostDto dto)
        {
            _logger.LogInformation("Creating new user with Email={email}", dto.Email);

            var user = await _service.AddAsync(dto);

            _logger.LogInformation("User created successfully with Id={id}", user.Id);
            return CreatedAtAction(nameof(GetById), new { id = user.Id }, user);
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(int id, [FromBody] UserPutDto dto)
        {
            _logger.LogInformation("Updating user with Id={id}", id);

            var updatedUser = await _service.UpdateAsync(id, dto);
            if (updatedUser == null)
            {
                _logger.LogWarning("Update failed. User with Id={id} not found", id);
                return NotFound();
            }

            _logger.LogInformation("User with Id={id} updated successfully", id);
            return Ok(updatedUser);
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            _logger.LogInformation("Deleting user with Id={id}", id);

            var deleted = await _service.DeleteAsync(id);
            if (!deleted)
            {
                _logger.LogWarning("Delete failed. User with Id={id} not found", id);
                return NotFound();
            }

            _logger.LogInformation("User with Id={id} deleted successfully", id);
            return NoContent();
        }
        [HttpGet("check-email")]
        public async Task<IActionResult> CheckEmail(string email,int? excludeId)
        {
            var exists = await _service.IsEmailExistsAsync(email, excludeId);
            return Ok(exists);
        }

        [HttpGet("check-mobile")]
        public async Task<IActionResult> CheckMobile(string mobile, int? excludeId)
        {
            var exists = await _service.IsMobileExistsAsync(mobile, excludeId);
            return Ok(exists);
        }
        [HttpGet("all-ids")]
        public async Task<IActionResult> GetAllIds([FromQuery] string? search)
        {
            var ids = await _service.GetAllIdsAsync(search);
            return Ok(ids);
        }


    }
}
