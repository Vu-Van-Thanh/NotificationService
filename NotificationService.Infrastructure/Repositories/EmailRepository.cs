using Microsoft.EntityFrameworkCore;
using NotificationService.API.Repositories;
using NotificationService.Core.Domain.Entities;
using NotificationService.Core.RepositoryContracts.SeparateRepository;
using NotificationService.Infrastructure.AppDbContext;

namespace NotificationService.Infrastructure.Repositories
{
    public class EmailRepository : Repository<Email>, IEmailRepository
    {
        private readonly ApplicationDbContext _context;
        
        public EmailRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }
        
        public async Task<IEnumerable<Email>> GetByMailboxIdAsync(Guid mailboxId)
        {
            return await _context.Emails
                .Where(e => e.MailboxId == mailboxId)
                .ToListAsync();
        }
        
        public async Task<IEnumerable<Email>> GetUnreadByMailboxIdAsync(Guid mailboxId)
        {
            return await _context.Emails
                .Where(e => e.MailboxId == mailboxId && !e.IsRead)
                .ToListAsync();
        }
    }
} 