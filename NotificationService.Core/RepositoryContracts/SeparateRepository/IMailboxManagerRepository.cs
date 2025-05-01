using NotificationService.Core.Domain.Entities;

namespace NotificationService.Core.RepositoryContracts.SeparateRepository
{
    public interface IMailboxManagerRepository : IRepository<MailboxManager>
    {
        Task<IEnumerable<MailboxManager>> GetByManagerEmailAsync(string managerEmail);
        Task<IEnumerable<MailboxManager>> GetByMailboxIdAsync(Guid mailboxId);
    }
} 