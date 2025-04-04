using AutoMapper;
using NotificationService.Core.Domain.Entities;
using NotificationService.Core.DTO;
using NotificationService.Core.RepositoryContracts;
using NotificationService.Core.Services.CommonServiceContract;


namespace NotificationService.Core.Services.SeparateService
{
    public interface IEmailLogService : IService<EmailLog, EmailLogDTO>
    {

    }
    public class EmailLogService : Service<EmailLog, EmailLogDTO>, IEmailLogService
    {
        public EmailLogService(IRepository<EmailLog> repository, IMapper mapper)
            : base(repository, mapper)
        {

        }

    }
}
