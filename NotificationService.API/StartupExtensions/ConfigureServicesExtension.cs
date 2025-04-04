using NotificationService.API.Repositories;
using Microsoft.EntityFrameworkCore;
using NotificationService.Core.Extensions;
using NotificationService.Core.RepositoryContracts;
using NotificationService.Core.Services.CommonServiceContract;
using NotificationService.Infrastructure.AppDbContext;
using Microsoft.OpenApi.Models;
using NotificationService.Core.Services.SeparateService;

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


            // thêm service
            services.AddAutoMapper(typeof(MappingProfile));
            services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
            services.AddScoped(typeof(IService<,>), typeof(Service<,>));
            services.AddScoped<IEmailLogService, EmailLogService>();
            services.AddScoped<IEmailTemplateService, EmailTemplateService>();
            services.Configure<MailSettings>(builder.Configuration.GetSection("MailSettings"));
            services.AddTransient<IEmailSender, EmailSender>();
            // cấu hình swagger
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
