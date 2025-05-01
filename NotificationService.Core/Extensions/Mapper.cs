using AutoMapper;
using NotificationService.Core.Domain.Entities;
using NotificationService.Core.DTO;

namespace NotificationService.Core.Extensions
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<EmailLog, EmailLogDTO>();
            CreateMap<EmailLogDTO, EmailLog>();
            CreateMap<EmailTemplateDTO, EmailTemplate>();
            CreateMap<EmailTemplate, EmailTemplateDTO>();
            
            CreateMap<Mailbox, MailboxDTO>();
            CreateMap<MailboxDTO, Mailbox>();
            
            CreateMap<Email, EmailDTO>();
            CreateMap<EmailDTO, Email>();
            
            CreateMap<MailboxManager, MailboxManagerDTO>();
            CreateMap<MailboxManagerDTO, MailboxManager>();
            
            CreateMap<EmailManager, EmailManagerDTO>();
            CreateMap<EmailManagerDTO, EmailManager>();
        }
    }
}
