using NotificationService.Core.Domain.Entities;

namespace NotificationService.Core.RepositoryContracts.SeparateRepository
{
    public interface IEmailManagerRepository : IRepository<EmailManager>
    {
        Task<IEnumerable<EmailManager>> GetByEmailIdAsync(Guid emailId);
        Task<IEnumerable<EmailManager>> GetByManagerEmailAsync(string managerEmail);
    }
} 