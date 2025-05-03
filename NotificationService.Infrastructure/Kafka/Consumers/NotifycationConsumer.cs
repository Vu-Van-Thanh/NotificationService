using System.Text.Json;
using Confluent.Kafka;
using NotificationService.Core.DTO;
using NotificationService.Infrastructure.Kafka.Handlers;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using NotificationService.Infrastructure.Kafka.KafkaEntity;

namespace NotificationService.Infrastructure.Kafka.Consumers
{
    public class NotifycationConsumer : BackgroundService
    {
        private readonly IConsumer<string, string> _consumer;
        private readonly IServiceProvider _serviceProvider;
        private readonly KafkaSettings _kafkaSettings;

        public NotifycationConsumer(IConfiguration config, IServiceProvider serviceProvider, IOptions<KafkaSettings> kafkaOptions)
        {
            _kafkaSettings = kafkaOptions.Value;
           
            try
            {
                ConsumerConfig consumerConfig = new ConsumerConfig
                {
                    BootstrapServers = _kafkaSettings?.BootstrapServers,
                    GroupId = _kafkaSettings?.GroupId,
                    AutoOffsetReset = AutoOffsetReset.Earliest
                };

                List<string> allTopics = _kafkaSettings.ConsumeTopicNames
                                .SelectMany(entry => entry.Value)
                                .Distinct()
                                .ToList();

                // Khởi tạo Kafka consumer
                _consumer = new ConsumerBuilder<string, string>(consumerConfig).Build();
                _consumer.Subscribe(allTopics);
            }
            catch (Exception ex)
            {
                // Log lỗi nếu không thể khởi tạo hoặc subscribe Kafka consumer
                Console.WriteLine($"Lỗi khi khởi tạo Kafka consumer: {ex.Message}");
                throw; // Ném lại exception nếu không thể tiếp tục khởi tạo
            }

            _serviceProvider = serviceProvider;
        }
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                var result = _consumer.Consume(stoppingToken);
                var topic = result.Topic;
                var message = result.Message.Value;
                using var scope = _serviceProvider.CreateScope();

                switch (topic)
                {
                    case "mailbox-create":
                        var importHandler = scope.ServiceProvider.GetRequiredService<IKafkaHandler<KafkaRequest<MailBoxCreate>>>();
                        var importData = JsonSerializer.Deserialize<KafkaRequest<MailBoxCreate>>(message);
                        await importHandler.HandleAsync(importData);
                        break;
                    case "get-template":
                        var getTemplateHandler = scope.ServiceProvider.GetRequiredService<IKafkaHandler<KafkaRequest<TemplateRequest>>>();
                        var getTemplateData = JsonSerializer.Deserialize<KafkaRequest<TemplateRequest>>(message);
                        await getTemplateHandler.HandleAsync(getTemplateData);
                        break;
                    case "send-mail":
                        var sendMailHandler = scope.ServiceProvider.GetRequiredService<IKafkaHandler<KafkaRequest<List<MailContent>>>>();
                        var sendMailData = JsonSerializer.Deserialize<KafkaRequest<List<MailContent>>>(message);
                        await sendMailHandler.HandleAsync(sendMailData);
                        break;

                   
                }
            }
        }

        public override Task StopAsync(CancellationToken cancellationToken)
        {
            _consumer.Close();    // Dừng Kafka consumer một cách "gracefully"
            _consumer.Dispose();  // Giải phóng tài nguyên
            return base.StopAsync(cancellationToken); // Gọi base nếu có thêm xử lý mặc định
        }

    }
}
