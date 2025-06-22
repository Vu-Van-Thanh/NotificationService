using Microsoft.AspNetCore.Mvc;
using NotificationService.Core.DTO;
using NotificationService.Core.Services.SeparateService;

namespace NotificationService.API.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class MailboxController : ControllerBase
    {
        private readonly IMailboxService _mailboxService;

        public MailboxController(IMailboxService mailboxService)
        {
            _mailboxService = mailboxService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var result = await _mailboxService.GetAllAsync();
            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var result = await _mailboxService.GetByIdAsync(id);
            if (result == null)
            {
                return NotFound();
            }

            return Ok(result);
        }

        [HttpGet("employee/{email}")]
        public async Task<IActionResult> GetByEmployeeEmail(string email)
        {
            var result = await _mailboxService.GetByEmployeeEmailAsync(email);
            if (result == null)
            {
                return NotFound();
            }

            return Ok(result);
        }

        [HttpGet("ListMailBoxID/{employeeIds}")]
        public async Task<IActionResult> GetMailBoxId(string employeeIds)
        {
            var result = await _mailboxService.GetMailboxListByEmployeeIdList(employeeIds);
            if (result == null)
            {
                return NotFound();
            }

            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> Upsert(MailboxDTO dto)
        {
            var result = await _mailboxService.UpsertAsync(dto, m => m.MailboxId == dto.MailboxId);
            return Ok(result);
        }

        [HttpPost("create-for-employee")]
        public async Task<IActionResult> CreateForEmployee([FromBody] CreateMailboxRequest request)
        {
            var result = await _mailboxService.CreateMailboxForEmployeeAsync(
                request.EmployeeId,
                request.EmployeeEmail, 
                request.Name, 
                request.Description);
                
            return Ok(result);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            await _mailboxService.DeleteAsync(id);
            return NoContent();
        }
    }

    public class CreateMailboxRequest
    {
        public string EmployeeEmail { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string EmployeeId { get; set; }
    }
} 