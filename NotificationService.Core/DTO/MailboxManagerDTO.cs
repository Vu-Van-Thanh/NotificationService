using System;

namespace NotificationService.Core.DTO
{
    public class MailboxManagerDTO
    {
        public Guid ManagerId { get; set; }
        public string ManagerName { get; set; }
        public string ManagerEmail { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        
        public Guid MailboxId { get; set; }
        
        public bool CanRead { get; set; }
        public bool CanWrite { get; set; }
        public bool CanDelete { get; set; }
        public bool IsOwner { get; set; }
    }
} 