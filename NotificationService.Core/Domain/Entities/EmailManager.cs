using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NotificationService.Core.Domain.Entities
{
    public class EmailManager
    {
        [Key]
        public Guid EmailManagerId { get; set; }
        
        public Guid EmailId { get; set; }
        
        public string ManagerEmail { get; set; }
        public string ManagerName { get; set; }
        
        // Action details
        public string Action { get; set; } 
        public DateTime ActionDate { get; set; } = DateTime.UtcNow;
        public string Notes { get; set; }
        
        [ForeignKey("EmailId")]
        public virtual Email Email { get; set; }
    }
} 