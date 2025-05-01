using System;

namespace NotificationService.Core.DTO
{
    public class EmailManagerDTO
    {
        public Guid EmailManagerId { get; set; }
        
        public Guid EmailId { get; set; }
        
        public string ManagerEmail { get; set; }
        public string ManagerName { get; set; }
        
        public string Action { get; set; }
        public DateTime ActionDate { get; set; }
        public string Notes { get; set; }
    }
} 