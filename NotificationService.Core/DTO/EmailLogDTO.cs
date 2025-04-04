using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NotificationService.Core.Enums;

namespace NotificationService.Core.DTO
{
    public class EmailLogDTO
    {
        public Guid EmailLogId { get; set; }
        public string ReceiveMail { get; set; }

        public Guid TemaplateId { get; set; }

        public string EmailHeader { get; set; }
        public string EmailBody { get; set; }
        public MailStatus Status { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
