using Microsoft.AspNetCore.Mvc;
using NotificationService.Core.DTO;
using NotificationService.Core.Services.SeparateService;

namespace NotificationService.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MailboxManagerController : ControllerBase
    {
        private readonly IMailboxManagerService _mailboxManagerService;

        public MailboxManagerController(IMailboxManagerService mailboxManagerService)
        {
            _mailboxManagerService = mailboxManagerService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var result = await _mailboxManagerService.GetAllAsync();
            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var result = await _mailboxManagerService.GetByIdAsync(id);
            if (result == null)
            {
                return NotFound();
            }

            return Ok(result);
        }

        [HttpGet("manager/{email}")]
        public async Task<IActionResult> GetByManagerEmail(string email)
        {
            var result = await _mailboxManagerService.GetByManagerEmailAsync(email);
            return Ok(result);
        }

        [HttpGet("mailbox/{mailboxId}")]
        public async Task<IActionResult> GetByMailboxId(Guid mailboxId)
        {
            var result = await _mailboxManagerService.GetByMailboxIdAsync(mailboxId);
            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> Upsert(MailboxManagerDTO dto)
        {
            var result = await _mailboxManagerService.UpsertAsync(dto, mm => mm.ManagerId == dto.ManagerId);
            return Ok(result);
        }

        [HttpPost("assign")]
        public async Task<IActionResult> AssignManager([FromBody] AssignManagerRequest request)
        {
            var result = await _mailboxManagerService.AssignManagerToMailboxAsync(
                request.MailboxId,
                request.ManagerEmail,
                request.ManagerName,
                request.CanWrite,
                request.CanDelete,
                request.IsOwner);
                
            return Ok(result);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            await _mailboxManagerService.DeleteAsync(id);
            return NoContent();
        }
    }

    public class AssignManagerRequest
    {
        public Guid MailboxId { get; set; }
        public string ManagerEmail { get; set; }
        public string ManagerName { get; set; }
        public bool CanWrite { get; set; }
        public bool CanDelete { get; set; }
        public bool IsOwner { get; set; }
    }
} 