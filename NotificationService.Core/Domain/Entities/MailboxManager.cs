using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NotificationService.Core.Domain.Entities
{
    public class MailboxManager
    {
        [Key]
        public Guid ManagerId { get; set; }
        public string ManagerName { get; set; }
        public string ManagerEmail { get; set; }
        public bool IsActive { get; set; } = true;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedAt { get; set; }
        
        // Foreign key for the mailbox being managed
        public Guid MailboxId { get; set; }
        
        // Navigation property
        [ForeignKey("MailboxId")]
        public virtual Mailbox Mailbox { get; set; }
        
        // Additional permissions/roles could be added here
        public bool CanRead { get; set; } = true;
        public bool CanWrite { get; set; } = false;
        public bool CanDelete { get; set; } = false;
        public bool IsOwner { get; set; } = false;
    }
} 