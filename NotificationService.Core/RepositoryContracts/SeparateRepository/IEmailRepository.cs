using NotificationService.Core.Domain.Entities;

namespace NotificationService.Core.RepositoryContracts.SeparateRepository
{
    public interface IEmailRepository : IRepository<Email>
    {
        Task<IEnumerable<Email>> GetByMailboxIdAsync(Guid mailboxId);
        Task<IEnumerable<Email>> GetUnreadByMailboxIdAsync(Guid mailboxId);
    }
} 