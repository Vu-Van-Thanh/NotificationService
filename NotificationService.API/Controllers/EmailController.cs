using Microsoft.AspNetCore.Mvc;
using NotificationService.Core.DTO;
using NotificationService.Core.Services.SeparateService;

namespace NotificationService.API.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class EmailController : ControllerBase
    {
        private readonly IEmailService _emailService;
        private readonly IEmailManagerService _emailManagerService;
        private readonly IMailboxService _mailboxService;

        public EmailController(
            IEmailService emailService,
            IEmailManagerService emailManagerService,
            IMailboxService mailboxService)
        {
            _emailService = emailService;
            _emailManagerService = emailManagerService;
            _mailboxService = mailboxService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var result = await _emailService.GetAllAsync();
            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var result = await _emailService.GetByIdAsync(id);
            if (result == null)
            {
                return NotFound();
            }

            return Ok(result);
        }
        [HttpGet("employee")]
        public async Task<IActionResult> GetByEmployeeEmail(string employeeID)
        {
            Guid mailboxId =  (await _mailboxService.GetMailboxIdByEmployeeIdAsync(employeeID));
            var result = await _emailService.GetByMailboxIdAsync(mailboxId);
            if (result == null)
            {
                return NotFound();
            }

            return Ok(result);
        }

        [HttpGet("mailbox/{mailboxId}")]
        public async Task<IActionResult> GetByMailboxId(Guid mailboxId)
        {
            var result = await _emailService.GetByMailboxIdAsync(mailboxId);
            return Ok(result);
        }

        [HttpGet("mailbox/{mailboxId}/unread")]
        public async Task<IActionResult> GetUnreadByMailboxId(Guid mailboxId)
        {
            var result = await _emailService.GetUnreadByMailboxIdAsync(mailboxId);
            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> Upsert(EmailDTO dto)
        {
            var result = await _emailService.UpsertAsync(dto, e => e.EmailId == dto.EmailId);
            return Ok(result);
        }

        [HttpPost("send")]
        public async Task<IActionResult> SendEmail([FromBody] SendEmailRequest request)
        {
            var result = await _emailService.SendEmailToMailboxAsync(
                request.MailboxId,
                request.Subject,
                request.Body,
                request.Sender);
            
            // Log action
            await _emailManagerService.LogEmailActionAsync(
                result.EmailId,
                request.Sender,
                "System",
                "Send",
                "Email sent to mailbox");
                
            return Ok(result);
        }

        [HttpPost("sendList")]
        public async Task<IActionResult> SendEmail([FromBody] List<SendEmailRequest> request)
        {
            List<EmailDTO> resulList = new List<EmailDTO>();
            foreach (var emailRequest in request)
            {
                EmailDTO result = await _emailService.SendEmailToMailboxAsync(
                emailRequest.MailboxId,
                emailRequest.Subject,
                emailRequest.Body,
                emailRequest.Sender);

                // Log action
                await _emailManagerService.LogEmailActionAsync(
                    result.EmailId,
                    emailRequest.Sender,
                    "System",
                    "Send",
                    "Email sent to mailbox");
                resulList.Add(result);
            }


            return Ok(resulList);
        }

        [HttpPut("{id}/mark-as-read")]
        public async Task<IActionResult> MarkAsRead(Guid id, [FromBody] MarkAsReadRequest request)
        {
            var result = await _emailService.MarkAsReadAsync(id);
            
            // Log action
            await _emailManagerService.LogEmailActionAsync(
                id,
                request.ReaderEmail,
                request.ReaderName,
                "Read",
                "Email marked as read");
                
            return Ok(result);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            await _emailService.DeleteAsync(id);
            return NoContent();
        }
    }

    public class SendEmailRequest
    {
        public Guid MailboxId { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }
        public string Sender { get; set; }
    }

    public class MarkAsReadRequest
    {
        public string ReaderEmail { get; set; }
        public string ReaderName { get; set; }
    }
} 