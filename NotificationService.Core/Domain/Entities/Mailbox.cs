using System.ComponentModel.DataAnnotations;

namespace NotificationService.Core.Domain.Entities
{
    public class Mailbox
    {
        [Key]
        public Guid MailboxId { get; set; }
        public string EmployeeEmail { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public bool IsActive { get; set; } = true;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedAt { get; set; }

        // Navigation properties
        public virtual ICollection<Email> Emails { get; set; } = new List<Email>();
        public virtual ICollection<MailboxManager> Managers { get; set; } = new List<MailboxManager>();
    }
} 