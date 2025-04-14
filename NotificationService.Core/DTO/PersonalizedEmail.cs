using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NotificationService.Core.DTO
{
    public class PersonalizedEmail
    {
        public string? RecipientEmail { get; set; }
        public string? Subject { get; set; }
        public string? HtmlBody { get; set; }
    }
}
