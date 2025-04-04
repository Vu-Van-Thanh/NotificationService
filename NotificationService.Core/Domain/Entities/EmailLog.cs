using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using NotificationService.Core.Enums;


namespace NotificationService.Core.Domain.Entities
{
    public class EmailLog
    {
        [Key]
        public Guid EmailLogId { get; set; }
        public string ReceiveMail { get; set; }
        
        public Guid TemaplateId { get; set; }

        public string EmailHeader { get; set; } 
        public string EmailBody { get; set; }
        public MailStatus Status { get; set; } 
        public DateTime CreatedAt { get; set; }


        [ForeignKey("TemplateId")] 
        public virtual EmailTemplate? EmailTemplate { get; set; } 
    }
}
