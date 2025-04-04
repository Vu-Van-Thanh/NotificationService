using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NotificationService.Core.DTO
{
    public class EmailTemplateDTO
    {
        public Guid TemplateId { get; set; }
        public string TemplateName { get; set; }

        public string TemplateBody { get; set; }
        public string TemplateHeader { get; set; }
        public string? SearchSQLCMD { get; set; }
        public Guid? DepartmentId { get; set; }
    }
}
