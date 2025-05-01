using NotificationService.Core.Domain.Entities;

namespace NotificationService.Core.RepositoryContracts.SeparateRepository
{
    public interface IMailboxRepository : IRepository<Mailbox>
    {
        Task<Mailbox> GetByEmployeeEmailAsync(string email);
    }
} 