using System.ComponentModel.DataAnnotations;



namespace NotificationService.Core.Domain.Entities
{
    public class EmailTemplate
    {
        [Key]
        public Guid TemplateId { get; set; } 
        public string TemplateName { get; set; }

        public string TemplateBody { get; set; }  
        public string TemplateHeader { get; set; }
        public string? SearchSQLCMD { get; set; }
        public Guid? DepartmentId { get; set; }

        public virtual ICollection<EmailLog>? EmailLogs { get; set; }
    }
}
