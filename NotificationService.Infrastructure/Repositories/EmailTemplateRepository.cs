using DepartmentService.API.Repositories;
using NotificationService.Core.Domain.Entities;
using NotificationService.Core.RepositoryContracts.SeparateRepository;
using NotificationService.Infrastructure.AppDbContext;


namespace NotificationService.Infrastructure.Repositories
{
    public class EmailTemplateRepository : Repository<EmailTemplate>, IEmailTemplateRepository
    {
        public EmailTemplateRepository(ApplicationDbContext context) : base(context)
        {
        }
    }
}
