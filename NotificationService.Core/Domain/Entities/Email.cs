using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using NotificationService.Core.Enums;

namespace NotificationService.Core.Domain.Entities
{
    public class Email
    {
        [Key]
        public Guid EmailId { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }
        public string Sender { get; set; }
        public DateTime ReceivedAt { get; set; }
        public bool IsRead { get; set; } = false;
        public bool IsStarred { get; set; } = false;
        public bool IsDeleted { get; set; } = false;
        public MailStatus Status { get; set; } = MailStatus.Pendding;

        public Guid? RefId { get; set; } 

        // Foreign key for Mailbox
        public Guid MailboxId { get; set; }
        
        // Navigation properties
        [ForeignKey("MailboxId")]
        public virtual Mailbox Mailbox { get; set; }
        
        public virtual ICollection<EmailManager> EmailManagers { get; set; } = new List<EmailManager>();
    }
} 