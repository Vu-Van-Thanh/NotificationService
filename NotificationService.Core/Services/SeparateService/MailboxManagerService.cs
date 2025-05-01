using AutoMapper;
using NotificationService.Core.Domain.Entities;
using NotificationService.Core.DTO;
using NotificationService.Core.RepositoryContracts;
using NotificationService.Core.RepositoryContracts.SeparateRepository;
using NotificationService.Core.Services.CommonServiceContract;

namespace NotificationService.Core.Services.SeparateService
{
    public interface IMailboxManagerService : IService<MailboxManager, MailboxManagerDTO>
    {
        Task<IEnumerable<MailboxManagerDTO>> GetByManagerEmailAsync(string managerEmail);
        Task<IEnumerable<MailboxManagerDTO>> GetByMailboxIdAsync(Guid mailboxId);
        Task<MailboxManagerDTO> AssignManagerToMailboxAsync(Guid mailboxId, string managerEmail, string managerName, bool canWrite = false, bool canDelete = false, bool isOwner = false);
    }

    public class MailboxManagerService : Service<MailboxManager, MailboxManagerDTO>, IMailboxManagerService
    {
        private readonly IMailboxManagerRepository _mailboxManagerRepository;
        private readonly IMailboxRepository _mailboxRepository;
        private readonly IMapper _mapper;

        public MailboxManagerService(
            IMailboxManagerRepository mailboxManagerRepository,
            IMailboxRepository mailboxRepository,
            IMapper mapper)
            : base(mailboxManagerRepository, mapper)
        {
            _mailboxManagerRepository = mailboxManagerRepository;
            _mailboxRepository = mailboxRepository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<MailboxManagerDTO>> GetByManagerEmailAsync(string managerEmail)
        {
            var managers = await _mailboxManagerRepository.GetByManagerEmailAsync(managerEmail);
            return _mapper.Map<IEnumerable<MailboxManagerDTO>>(managers);
        }

        public async Task<IEnumerable<MailboxManagerDTO>> GetByMailboxIdAsync(Guid mailboxId)
        {
            var managers = await _mailboxManagerRepository.GetByMailboxIdAsync(mailboxId);
            return _mapper.Map<IEnumerable<MailboxManagerDTO>>(managers);
        }

        public async Task<MailboxManagerDTO> AssignManagerToMailboxAsync(
            Guid mailboxId, 
            string managerEmail, 
            string managerName, 
            bool canWrite = false, 
            bool canDelete = false, 
            bool isOwner = false)
        {
            // Kiểm tra mailbox có tồn tại không
            var mailbox = await _mailboxRepository.GetByIdAsync(mailboxId);
            if (mailbox == null)
            {
                throw new Exception($"Mailbox with ID {mailboxId} not found");
            }

            var mailboxManager = new MailboxManager
            {
                ManagerId = Guid.NewGuid(),
                MailboxId = mailboxId,
                ManagerEmail = managerEmail,
                ManagerName = managerName,
                IsActive = true,
                CreatedAt = DateTime.UtcNow,
                CanRead = true,
                CanWrite = canWrite,
                CanDelete = canDelete,
                IsOwner = isOwner
            };

            var result = await _mailboxManagerRepository.UpsertAsync(
                mailboxManager, 
                mm => mm.MailboxId == mailboxId && mm.ManagerEmail == managerEmail);
                
            return _mapper.Map<MailboxManagerDTO>(result);
        }
    }
} 