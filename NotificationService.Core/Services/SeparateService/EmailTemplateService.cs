using AutoMapper;
using DocumentFormat.OpenXml.Packaging;
using Microsoft.AspNetCore.Http;
using NotificationService.Core.Domain.Entities;
using NotificationService.Core.DTO;
using NotificationService.Core.RepositoryContracts;
using NotificationService.Core.Services.CommonServiceContract;


namespace NotificationService.Core.Services.SeparateService
{
    public interface IEmailTemplateService : IService<EmailTemplate, EmailTemplateDTO>
    {
        Task<EmailTemplateDTO> LoadTemplateFromWord(IFormFile formFile);
       
    }
    public class EmailTemplateService : Service<EmailTemplate, EmailTemplateDTO>, IEmailTemplateService
    {
        private readonly IRepository<EmailTemplate> _repository;
        public EmailTemplateService(IRepository<EmailTemplate> repository, IMapper mapper)
            : base(repository, mapper)
        {
            _repository = repository;
        }

        public async Task<EmailTemplateDTO> LoadTemplateFromWord(IFormFile formFile)
        {
            if (formFile == null || formFile.Length == 0)
            {
                throw new ArgumentException("File không hợp lệ.");
            }

            using var stream = formFile.OpenReadStream();
            using var wordDocument = WordprocessingDocument.Open(stream, false);
            var body = wordDocument.MainDocumentPart.Document.Body;

            if (body == null)
            {
                throw new Exception("Không thể đọc nội dung file Word.");
            }

            string textContent = body.InnerText; 
            EmailTemplateDTO template = ParseWordContent(textContent);
            await _repository.UpsertAsync(new EmailTemplate
            {
                TemplateHeader = template.TemplateHeader,
                TemplateBody = template.TemplateBody,
                SearchSQLCMD = template.SearchSQLCMD
            }, s => s.TemplateHeader == template.TemplateHeader && s.TemplateBody == template.TemplateBody);

            return template;
        }


        private EmailTemplateDTO ParseWordContent(string content)
        {
            EmailTemplateDTO template = new EmailTemplateDTO();
            var sections = content.Split(new string[] { "Header:", "Body:", "SearchCMDSQL:" }, StringSplitOptions.RemoveEmptyEntries);

            if (sections.Length >= 1) template.TemplateHeader = sections[0].Trim();
            if (sections.Length >= 2) template.TemplateBody = sections[1].Trim();
            if (sections.Length >= 3) template.SearchSQLCMD = sections[2].Trim();

            return template;
        }
    }
}
