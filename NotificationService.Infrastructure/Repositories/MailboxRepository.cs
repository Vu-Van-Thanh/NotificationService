using Microsoft.EntityFrameworkCore;
using NotificationService.API.Repositories;
using NotificationService.Core.Domain.Entities;
using NotificationService.Core.RepositoryContracts.SeparateRepository;
using NotificationService.Infrastructure.AppDbContext;

namespace NotificationService.Infrastructure.Repositories
{
    public class MailboxRepository : Repository<Mailbox>, IMailboxRepository
    {
        private readonly ApplicationDbContext _context;
        
        public MailboxRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }
        
        public async Task<Mailbox> GetByEmployeeEmailAsync(string email)
        {
            return await _context.Mailboxes
                .FirstOrDefaultAsync(m => m.EmployeeEmail == email);
        }

        public async Task<Mailbox> GetByEmployeeIdAsync(string employeeId)
        {
            return await _context.Mailboxes
                .FirstOrDefaultAsync(m => m.EmployeeId == employeeId);
        }
    }
} 