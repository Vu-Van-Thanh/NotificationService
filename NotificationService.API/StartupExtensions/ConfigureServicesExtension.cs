using NotificationService.API.Repositories;
using Microsoft.EntityFrameworkCore;
using NotificationService.Core.Extensions;
using NotificationService.Core.RepositoryContracts;
using NotificationService.Core.Services.CommonServiceContract;
using NotificationService.Infrastructure.AppDbContext;
using Microsoft.OpenApi.Models;
using NotificationService.Core.Services.SeparateService;
using NotificationService.Infrastructure.EmailSender;
using NotificationService.Core.RepositoryContracts.SeparateRepository;
using NotificationService.Infrastructure.Repositories;
using NotificationService.Infrastructure.Kafka.Handlers;
using NotificationService.Infrastructure.Kafka.KafkaEntity;
using EmployeeService.API.Kafka.Producer;
using NotificationService.Infrastructure.Kafka.Producer;
using NotificationService.Infrastructure.Kafka.Consumers.Manual;
using NotificationService.Infrastructure.Kafka.Consumers;
using NotificationService.Core.DTO;

namespace NotificationServiceRegistry
{
    public static class ConfigureServicesExtension
    {
        public static IServiceCollection ConfigureServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<ApplicationDbContext>(options =>
            {
                options.UseSqlServer(configuration.GetConnectionString("DefaultConnection"));
            });

            // Đăng ký Repository
            services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
            services.AddScoped<IMailboxRepository, MailboxRepository>();
            services.AddScoped<IEmailRepository, EmailRepository>();
            services.AddScoped<IEmailManagerRepository, EmailManagerRepository>();
            
            // Đăng ký Service
            services.AddScoped(typeof(IService<,>), typeof(Service<,>));
            services.AddScoped<IEmailLogService, EmailLogService>();
            services.AddScoped<IEmailTemplateService, EmailTemplateService>();
            services.AddScoped<IMailboxService, MailboxService>();
            services.AddScoped<IEmailService, EmailService>();
            services.AddScoped<IEmailManagerService, EmailManagerService>();
            services.AddScoped<IKafkaHandler<KafkaRequest<MailBoxCreate>>, MailBoxCreateHandler>();
            services.AddScoped<IEventProducer, NotifycationProducer>();
            services.AddScoped<IKafkaHandler<KafkaRequest<TemplateRequest>>, TemplateHandler>();
            services.AddScoped<IKafkaHandler<KafkaRequest<List<MailContent>>>, SendMailHandler>();
            services.AddHostedService<NotifycationConsumer>();

            // Đăng ký AutoMapper
            services.AddAutoMapper(typeof(MappingProfile));
            
            // Cấu hình swagger
            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "Notification API",
                    Version = "v1",
                    Description = "API quản lý người dùng trong hệ thống microservices",
                    Contact = new OpenApiContact
                    {
                        Name = "Hỗ trợ API",
                        Email = "vut4262@gmail.com",
                        Url = new Uri("https://example.com")
                    }
                });
     
            });

            return services;
        }
    }
}
