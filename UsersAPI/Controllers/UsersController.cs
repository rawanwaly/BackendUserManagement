using ClosedXML.Excel;
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
        [HttpPut("deactivate/{id:int}")]
        public async Task<IActionResult> Deactivate(int id)
        {
            _logger.LogInformation("Deactivating user with Id={id}", id);

            var success = await _service.DeactivateAsync(id);
            if (!success)
            {
                _logger.LogWarning("Deactivate failed. User with Id={id} not found", id);
                return NotFound();
            }

            _logger.LogInformation("User with Id={id} deactivated successfully", id);
            return NoContent();
        }
        [HttpPut("deactivate-selected")]
        public async Task<IActionResult> DeactivateSelected([FromBody] List<int> ids)
        {
            if (ids == null || !ids.Any())
                return BadRequest("No user IDs provided.");

            _logger.LogInformation("Deactivating {count} selected users at {time}", ids.Count, DateTime.UtcNow);

            await _service.DeactivateAllAsync(ids);

            _logger.LogInformation("Selected users deactivated successfully");
            return NoContent();
        }
        [HttpPut("activate/{id:int}")]
        public async Task<IActionResult> Activate(int id)
        {
            _logger.LogInformation("Activating user with Id={id}", id);

            var success = await _service.ActivateAsync(id);
            if (!success)
            {
                _logger.LogWarning("Activate failed. User with Id={id} not found or already active", id);
                return NotFound();
            }

            _logger.LogInformation("User with Id={id} activated successfully", id);
            return NoContent();
        }

        [HttpPut("activate-selected")]
        public async Task<IActionResult> ActivateSelected([FromBody] List<int> ids)
        {
            if (ids == null || !ids.Any()) return BadRequest("No user IDs provided.");

            _logger.LogInformation("Activating selected users at {time}", DateTime.UtcNow);

            var success = await _service.ActivateSelectedAsync(ids);
            if (!success)
            {
                return NotFound("No users were activated (maybe already active).");
            }

            _logger.LogInformation("Selected users activated successfully");
            return NoContent();
        }

        [HttpPost("export-excel")]
        public async Task<IActionResult> ExportToExcel([FromBody] List<int> ids)
        {
            _logger.LogInformation("Exporting {count} selected users to Excel at {time}", ids.Count, DateTime.UtcNow);

            var users = await _service.GetForExportByIdsAsync(ids);
            if (!users.Any())
            {
                return NotFound("No users found to export.");
            }

            using var workbook = new XLWorkbook();
            var worksheet = workbook.Worksheets.Add("Users");

            var columns = new (string Header, Func<UserGetDto, object?> Value)[]
            {
                ("Id", u => u.Id),
                ("First Name (EN)", u => u.FirstNameEN),
                ("Last Name (EN)", u => u.LastNameEN),
                ("First Name (AR)", u => u.FirstNameAR),
                ("Last Name (AR)", u => u.LastNameAR),
                ("Email", u => u.Email),
                ("Mobile", u => u.MobileNumber),
                ("Marital Status", u => u.MaritalStatus),
                ("Address ", u => u.Address),
                ("Is Active", u => u.isActive ? "Active" : "Inactive"),

             };

            for (int i = 0; i < columns.Length; i++)
            {
                worksheet.Cell(1, i + 1).Value = columns[i].Header;
            }

            int row = 2;
            foreach (var user in users)
            {
                for (int col = 0; col < columns.Length; col++)
                {
                    worksheet.Cell(row, col + 1).Value = columns[col].Value(user)?.ToString();
                }
                row++;
            }

            worksheet.Columns().AdjustToContents();

            using var stream = new MemoryStream();
            workbook.SaveAs(stream);

            return File(
                stream.ToArray(),
                "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                $"Users_{DateTime.UtcNow:yyyyMMddHHmmss}.xlsx"
            );
        }


    }
}
