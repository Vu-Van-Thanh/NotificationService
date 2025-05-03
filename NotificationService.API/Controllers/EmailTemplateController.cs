using Microsoft.AspNetCore.Mvc;
using NotificationService.Core.DTO;
using NotificationService.Core.Services.SeparateService;

namespace NotificationService.API.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class EmailTemplateController : ControllerBase
    {
        private readonly IEmailTemplateService _emailTemplateService;
        private readonly IEmailSender _emailSender;

        public EmailTemplateController(IEmailTemplateService emailTemplateService, IEmailSender emailSender)
        {
            _emailTemplateService = emailTemplateService;
            _emailSender = emailSender;
        }


        // Lấy tất cả SalaryBase dưới dạng DTO
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var result = await _emailTemplateService.GetAllAsync();
            return Ok(result);
        }

        // Lấy SalaryBase theo ID dưới dạng DTO
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var result = await _emailTemplateService.GetByIdAsync(id);
            if (result == null)
            {
                return NotFound();
            }

            return Ok(result);
        }

        // Thêm hoặc cập nhật SalaryBase
        [HttpPost]
        public async Task<IActionResult> Upsert(EmailTemplateDTO dto)
        {
            var result = await _emailTemplateService.UpsertAsync(dto, s => s.TemplateId == dto.TemplateId);
            return Ok(result);
        }

        // Xóa SalaryBase
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            await _emailTemplateService.DeleteAsync(id);
            return NoContent();
        }

        [HttpPost("upload")]
        public async Task<IActionResult> UploadTemplate([FromBody] IFormFile file)
        {
            var result = await _emailTemplateService.LoadTemplateFromWord(file);
            return Ok(result);
        }

        [HttpPost("send")]
        public async Task<IActionResult> SendBulkEmail([FromBody] List<PersonalizedEmail> emails)
        {
            if (emails == null || !emails.Any())
            {
                return BadRequest("Danh sách email không được rỗng.");
            }

            try
            {
                await _emailSender.SendBulkEmailAsync(emails);
                return Ok(new { Message = "Gửi email thành công." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "Lỗi khi gửi email.", Error = ex.Message });
            }
        }


    }
}
