using AutoMapper;
using NotificationService.Core.Domain.Entities;
using NotificationService.Core.DTO;
using NotificationService.Core.Enums;
using NotificationService.Core.RepositoryContracts;
using NotificationService.Core.RepositoryContracts.SeparateRepository;
using NotificationService.Core.Services.CommonServiceContract;

namespace NotificationService.Core.Services.SeparateService
{
    public interface IEmailService : IService<Email, EmailDTO>
    {
        Task<IEnumerable<EmailDTO>> GetByMailboxIdAsync(Guid mailboxId);
        Task<IEnumerable<EmailDTO>> GetUnreadByMailboxIdAsync(Guid mailboxId);
        Task<EmailDTO> SendEmailToMailboxAsync(Guid mailboxId, string subject, string body, string sender);
        Task<EmailDTO> MarkAsReadAsync(Guid emailId);
    }

    public class EmailService : Service<Email, EmailDTO>, IEmailService
    {
        private readonly IEmailRepository _emailRepository;
        private readonly IMailboxRepository _mailboxRepository;
        private readonly IMapper _mapper;

        public EmailService(
            IEmailRepository emailRepository, 
            IMailboxRepository mailboxRepository,
            IMapper mapper)
            : base(emailRepository, mapper)
        {
            _emailRepository = emailRepository;
            _mailboxRepository = mailboxRepository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<EmailDTO>> GetByMailboxIdAsync(Guid mailboxId)
        {
            var emails = await _emailRepository.GetByMailboxIdAsync(mailboxId);
            return _mapper.Map<IEnumerable<EmailDTO>>(emails);
        }

        public async Task<IEnumerable<EmailDTO>> GetUnreadByMailboxIdAsync(Guid mailboxId)
        {
            var emails = await _emailRepository.GetUnreadByMailboxIdAsync(mailboxId);
            return _mapper.Map<IEnumerable<EmailDTO>>(emails);
        }

        public async Task<EmailDTO> SendEmailToMailboxAsync(Guid mailboxId, string subject, string body, string sender)
        {
            // Kiểm tra mailbox có tồn tại không
            var mailbox = await _mailboxRepository.GetByIdAsync(mailboxId);
            if (mailbox == null)
            {
                throw new Exception($"Mailbox with ID {mailboxId} not found");
            }

            var email = new Email
            {
                EmailId = Guid.NewGuid(),
                Subject = subject,
                Body = body,
                Sender = sender,
                ReceivedAt = DateTime.UtcNow,
                IsRead = false,
                IsStarred = false,
                IsDeleted = false,
                Status = MailStatus.Pendding,
                MailboxId = mailboxId
            };

            var result = await _emailRepository.UpsertAsync(email, e => e.EmailId == email.EmailId);
            return _mapper.Map<EmailDTO>(result);
        }

        public async Task<EmailDTO> MarkAsReadAsync(Guid emailId)
        {
            var email = await _emailRepository.GetByIdAsync(emailId);
            if (email == null)
            {
                throw new Exception($"Email with ID {emailId} not found");
            }

            email.IsRead = true;
            var result = await _emailRepository.UpsertAsync(email, e => e.EmailId == emailId);
            return _mapper.Map<EmailDTO>(result);
        }

        
    }
} 