using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NotificationService.Core.DTO;
using NotificationService.Core.Services.SeparateService;

namespace NotificationService.API.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class EmailLogController : ControllerBase
    {
        private readonly IEmailLogService _emailLogService;

        public EmailLogController(IEmailLogService emailLogService)
        {
            _emailLogService = emailLogService;
        }


        // Lấy tất cả SalaryBase dưới dạng DTO
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var result = await _emailLogService.GetAllAsync();
            return Ok(result);
        }

        // Lấy SalaryBase theo ID dưới dạng DTO
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var result = await _emailLogService.GetByIdAsync(id);
            if (result == null)
            {
                return NotFound();
            }

            return Ok(result);
        }

        // Thêm hoặc cập nhật SalaryBase
        [HttpPost]
        public async Task<IActionResult> Upsert(EmailLogDTO dto)
        {
            var result = await _emailLogService.UpsertAsync(dto, s => s.EmailLogId == dto.EmailLogId);
            return Ok(result);
        }

        // Xóa SalaryBase
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            await _emailLogService.DeleteAsync(id);
            return NoContent();
        }
    }
}
