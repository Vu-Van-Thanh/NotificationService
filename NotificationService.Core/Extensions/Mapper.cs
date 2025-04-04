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
           

        }
    }
}
