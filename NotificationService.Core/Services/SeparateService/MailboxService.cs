using AutoMapper;
using NotificationService.Core.Domain.Entities;
using NotificationService.Core.DTO;
using NotificationService.Core.RepositoryContracts;
using NotificationService.Core.RepositoryContracts.SeparateRepository;
using NotificationService.Core.Services.CommonServiceContract;

namespace NotificationService.Core.Services.SeparateService
{
    public interface IMailboxService : IService<Mailbox, MailboxDTO>
    {
        Task<MailboxDTO> GetByEmployeeEmailAsync(string email);
        Task<MailboxDTO> CreateMailboxForEmployeeAsync(string employeeEmail, string name, string description);
    }

    public class MailboxService : Service<Mailbox, MailboxDTO>, IMailboxService
    {
        private readonly IMailboxRepository _mailboxRepository;
        private readonly IMapper _mapper;

        public MailboxService(IMailboxRepository mailboxRepository, IMapper mapper)
            : base(mailboxRepository, mapper)
        {
            _mailboxRepository = mailboxRepository;
            _mapper = mapper;
        }

        public async Task<MailboxDTO> GetByEmployeeEmailAsync(string email)
        {
            var mailbox = await _mailboxRepository.GetByEmployeeEmailAsync(email);
            return _mapper.Map<MailboxDTO>(mailbox);
        }

        public async Task<MailboxDTO> CreateMailboxForEmployeeAsync(string employeeEmail, string name, string description)
        {
            var mailbox = new Mailbox
            {
                MailboxId = Guid.NewGuid(),
                EmployeeEmail = employeeEmail,
                Name = name,
                Description = description,
                IsActive = true,
                CreatedAt = DateTime.UtcNow
            };

            var result = await _mailboxRepository.UpsertAsync(mailbox, m => m.EmployeeEmail == employeeEmail);
            return _mapper.Map<MailboxDTO>(result);
        }
    }
} 