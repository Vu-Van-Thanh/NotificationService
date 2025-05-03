using Microsoft.AspNetCore.Mvc;
using NotificationService.Core.DTO;
using NotificationService.Core.Services.SeparateService;

namespace NotificationService.API.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class EmailManagerController : ControllerBase
    {
        private readonly IEmailManagerService _emailManagerService;

        public EmailManagerController(IEmailManagerService emailManagerService)
        {
            _emailManagerService = emailManagerService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var result = await _emailManagerService.GetAllAsync();
            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var result = await _emailManagerService.GetByIdAsync(id);
            if (result == null)
            {
                return NotFound();
            }

            return Ok(result);
        }

        [HttpGet("email/{emailId}")]
        public async Task<IActionResult> GetByEmailId(Guid emailId)
        {
            var result = await _emailManagerService.GetByEmailIdAsync(emailId);
            return Ok(result);
        }

        [HttpGet("manager/{email}")]
        public async Task<IActionResult> GetByManagerEmail(string email)
        {
            var result = await _emailManagerService.GetByManagerEmailAsync(email);
            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> Upsert(EmailManagerDTO dto)
        {
            var result = await _emailManagerService.UpsertAsync(dto, em => em.EmailManagerId == dto.EmailManagerId);
            return Ok(result);
        }

        [HttpPost("log-action")]
        public async Task<IActionResult> LogAction([FromBody] LogEmailActionRequest request)
        {
            var result = await _emailManagerService.LogEmailActionAsync(
                request.EmailId,
                request.ManagerEmail,
                request.ManagerName,
                request.Action,
                request.Notes);
                
            return Ok(result);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            await _emailManagerService.DeleteAsync(id);
            return NoContent();
        }
    }

    public class LogEmailActionRequest
    {
        public Guid EmailId { get; set; }
        public string ManagerEmail { get; set; }
        public string ManagerName { get; set; }
        public string Action { get; set; }
        public string Notes { get; set; }
    }
} 