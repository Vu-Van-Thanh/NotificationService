using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NotificationService.Core.Domain.Entities
{
    public class EmailManager
    {
        [Key]
        public Guid EmailManagerId { get; set; }
        
        // The email being managed
        public Guid EmailId { get; set; }
        
        // The user managing the email
        public string ManagerEmail { get; set; }
        public string ManagerName { get; set; }
        
        // Action details
        public string Action { get; set; } // Read, Reply, Forward, Delete, etc.
        public DateTime ActionDate { get; set; } = DateTime.UtcNow;
        public string Notes { get; set; }
        
        // Navigation property for Email
        [ForeignKey("EmailId")]
        public virtual Email Email { get; set; }
    }
} 