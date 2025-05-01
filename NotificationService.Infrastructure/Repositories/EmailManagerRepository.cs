using Microsoft.EntityFrameworkCore;
using NotificationService.API.Repositories;
using NotificationService.Core.Domain.Entities;
using NotificationService.Core.RepositoryContracts.SeparateRepository;
using NotificationService.Infrastructure.AppDbContext;

namespace NotificationService.Infrastructure.Repositories
{
    public class EmailManagerRepository : Repository<EmailManager>, IEmailManagerRepository
    {
        private readonly ApplicationDbContext _context;
        
        public EmailManagerRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }
        
        public async Task<IEnumerable<EmailManager>> GetByEmailIdAsync(Guid emailId)
        {
            return await _context.EmailManagers
                .Where(em => em.EmailId == emailId)
                .ToListAsync();
        }
        
        public async Task<IEnumerable<EmailManager>> GetByManagerEmailAsync(string managerEmail)
        {
            return await _context.EmailManagers
                .Where(em => em.ManagerEmail == managerEmail)
                .ToListAsync();
        }
    }
} 