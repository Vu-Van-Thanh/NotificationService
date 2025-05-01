using Microsoft.EntityFrameworkCore;
using NotificationService.API.Repositories;
using NotificationService.Core.Domain.Entities;
using NotificationService.Core.RepositoryContracts.SeparateRepository;
using NotificationService.Infrastructure.AppDbContext;

namespace NotificationService.Infrastructure.Repositories
{
    public class MailboxManagerRepository : Repository<MailboxManager>, IMailboxManagerRepository
    {
        private readonly ApplicationDbContext _context;
        
        public MailboxManagerRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }
        
        public async Task<IEnumerable<MailboxManager>> GetByManagerEmailAsync(string managerEmail)
        {
            return await _context.MailboxManagers
                .Where(mm => mm.ManagerEmail == managerEmail)
                .ToListAsync();
        }
        
        public async Task<IEnumerable<MailboxManager>> GetByMailboxIdAsync(Guid mailboxId)
        {
            return await _context.MailboxManagers
                .Where(mm => mm.MailboxId == mailboxId)
                .ToListAsync();
        }
    }
} 