using System;
using NotificationService.Core.Enums;

namespace NotificationService.Core.DTO
{
    public class EmailDTO
    {
        public Guid EmailId { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }
        public string Sender { get; set; }
        public DateTime ReceivedAt { get; set; }
        public bool IsRead { get; set; }
        public bool IsStarred { get; set; }
        public bool IsDeleted { get; set; }
        public MailStatus Status { get; set; }
        
        public Guid MailboxId { get; set; }
    }
} 