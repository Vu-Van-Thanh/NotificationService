using AutoMapper;
using NotificationService.Core.Domain.Entities;
using NotificationService.Core.DTO;
using NotificationService.Core.RepositoryContracts;
using NotificationService.Core.RepositoryContracts.SeparateRepository;
using NotificationService.Core.Services.CommonServiceContract;

namespace NotificationService.Core.Services.SeparateService
{
    public interface IEmailManagerService : IService<EmailManager, EmailManagerDTO>
    {
        Task<IEnumerable<EmailManagerDTO>> GetByEmailIdAsync(Guid emailId);
        Task<IEnumerable<EmailManagerDTO>> GetByManagerEmailAsync(string managerEmail);
        Task<EmailManagerDTO> LogEmailActionAsync(Guid emailId, string managerEmail, string managerName, string action, string notes);
    }

    public class EmailManagerService : Service<EmailManager, EmailManagerDTO>, IEmailManagerService
    {
        private readonly IEmailManagerRepository _emailManagerRepository;
        private readonly IEmailRepository _emailRepository;
        private readonly IMapper _mapper;

        public EmailManagerService(
            IEmailManagerRepository emailManagerRepository,
            IEmailRepository emailRepository,
            IMapper mapper)
            : base(emailManagerRepository, mapper)
        {
            _emailManagerRepository = emailManagerRepository;
            _emailRepository = emailRepository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<EmailManagerDTO>> GetByEmailIdAsync(Guid emailId)
        {
            var managers = await _emailManagerRepository.GetByEmailIdAsync(emailId);
            return _mapper.Map<IEnumerable<EmailManagerDTO>>(managers);
        }

        public async Task<IEnumerable<EmailManagerDTO>> GetByManagerEmailAsync(string managerEmail)
        {
            var managers = await _emailManagerRepository.GetByManagerEmailAsync(managerEmail);
            return _mapper.Map<IEnumerable<EmailManagerDTO>>(managers);
        }

        public async Task<EmailManagerDTO> LogEmailActionAsync(
            Guid emailId, 
            string managerEmail, 
            string managerName, 
            string action, 
            string notes)
        {
            // Kiểm tra email có tồn tại không
            var email = await _emailRepository.GetByIdAsync(emailId);
            if (email == null)
            {
                throw new Exception($"Email with ID {emailId} not found");
            }

            var emailManager = new EmailManager
            {
                EmailManagerId = Guid.NewGuid(),
                EmailId = emailId,
                ManagerEmail = managerEmail,
                ManagerName = managerName,
                Action = action,
                ActionDate = DateTime.UtcNow,
                Notes = notes
            };

            var result = await _emailManagerRepository.UpsertAsync(
                emailManager, 
                em => em.EmailManagerId == emailManager.EmailManagerId);
                
            return _mapper.Map<EmailManagerDTO>(result);
        }
    }
} 