using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EmployeeService.API.Kafka.Producer;
using NotificationService.Core.DTO;
using NotificationService.Core.Services;
using NotificationService.Core.Services.SeparateService;
using NotificationService.Infrastructure.Kafka.KafkaEntity;

namespace NotificationService.Infrastructure.Kafka.Handlers
{
    public class MailBoxCreateHandler : IKafkaHandler<KafkaRequest<MailBoxCreate>>
    {
        private readonly IMailboxService _mailboxService;
        private readonly IEventProducer _eventProducer;
        
        public MailBoxCreateHandler(IMailboxService mailBoxService, IEventProducer eventProducer)
        {
            _mailboxService = mailBoxService;
            _eventProducer = eventProducer;
        }
        public async Task HandleAsync(KafkaRequest<MailBoxCreate> message)
        {
            MailboxDTO mailbox = await _mailboxService.CreateMailboxForEmployeeAsync(message.Filter.EmployeeId,message.Filter.EmployeeEmail, message.Filter.Name, "Tạo hòm thư cho user");
            KafkaResponse<MailBoxCreateResponse> kafkaResponse = new KafkaResponse<MailBoxCreateResponse>
            {
                RequestType = "MailBoxCreated",
                CorrelationId = message.CorrelationId,
                Timestamp = DateTime.UtcNow,
                Filter = new MailBoxCreateResponse { MailboxId = mailbox.MailboxId.ToString(), EmployeeEmail = mailbox.EmployeeEmail, IsActive = mailbox.IsActive, CreatedAt = mailbox.CreatedAt }
            };
            await _eventProducer.PublishAsync("mailbox-created", null, "mailbox-created", kafkaResponse);

        }


        
    }
}
