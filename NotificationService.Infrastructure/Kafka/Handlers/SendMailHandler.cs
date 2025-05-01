using EmployeeService.API.Kafka.Producer;
using NotificationService.Core.DTO;
using NotificationService.Core.Services.SeparateService;
using NotificationService.Infrastructure.Kafka.KafkaEntity;

namespace NotificationService.Infrastructure.Kafka.Handlers
{
    public class SendMailHandler : IKafkaHandler<KafkaRequest<List<MailContent>>>
    {
        private readonly IEmailService _emailService;
        private readonly IMailboxService _mailboxService;
        private readonly IEventProducer _eventProducer; 
        
        public SendMailHandler(IEmailService emailService, IMailboxService mailboxService, IEventProducer eventProducer)
        {
            _emailService = emailService;
            _mailboxService = mailboxService;
            _eventProducer = eventProducer;
        }
        public async Task HandleAsync(KafkaRequest<List<MailContent>> message)
        {
            List<string>? failedEmployeeId = new List<string>();
           foreach (var mailContent in message.Filter)
           {
                try{
                    Guid mailboxId = await _mailboxService.GetMailboxIdByEmployeeIdAsync(mailContent.To);
                    EmailDTO email = await _emailService.SendEmailToMailboxAsync(mailboxId, mailContent.Subject, mailContent.Body, mailContent.From);
                }
                catch (Exception ex)
                {
                    failedEmployeeId.Add(mailContent.To);
                }
           }
           KafkaResponse<MailSendResponse> response = new KafkaResponse<MailSendResponse>
           {
               RequestType = "Send-Mail-Response",
               CorrelationId = message.CorrelationId,
               Timestamp = DateTime.UtcNow,
               Filter = new MailSendResponse
               {
                   FailedEmployeeId = failedEmployeeId
               }
           };
           await _eventProducer.PublishAsync("Send-Mail-Response",null, "Send-Mail-Response", response);
        }

    }
}
