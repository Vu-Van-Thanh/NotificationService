using System;
using System.Collections.Generic;

namespace NotificationService.Core.DTO
{
    public class MailboxDTO
    {
        public Guid MailboxId { get; set; }
        public string EmployeeEmail { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }
} 