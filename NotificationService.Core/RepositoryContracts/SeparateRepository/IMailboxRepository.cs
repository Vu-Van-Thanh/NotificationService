using NotificationService.Core.Domain.Entities;

namespace NotificationService.Core.RepositoryContracts.SeparateRepository
{
    public interface IMailboxRepository : IRepository<Mailbox>
    {
        Task<Mailbox> GetByEmployeeEmailAsync(string email);
        Task<Mailbox> GetByEmployeeIdAsync(string employeeId);
        Task<List<Mailbox>> GetByEmployeeIdListAsync(IEnumerable<string> employeeIds);
    }
} 