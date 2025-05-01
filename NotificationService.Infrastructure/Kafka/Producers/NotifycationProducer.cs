﻿using Confluent.Kafka;
using EmployeeService.Infrastructure.Kafka;
using Microsoft.Extensions.Options;
using System.Text.Json;

namespace NotificationService.Infrastructure.Kafka.Producer
{
    public class NotifycationProducer : IEventProducer
    {
        private readonly IProducer<string, string> _producer;
        private readonly KafkaSettings _kafkaSettings;


        public NotifycationProducer(IOptions<KafkaSettings> kafkaSettings)
        {
            _kafkaSettings = kafkaSettings.Value;
            var producerConfig = new ProducerConfig
            {
                BootstrapServers = _kafkaSettings.BootstrapServers
            };

            _producer = new ProducerBuilder<string, string>(producerConfig).Build();
        }

        public async Task PublishAsync<T>(string topic, int? partition, string? key,T @event)
        {
            var message = JsonSerializer.Serialize(@event);
            var kafkaMessage = new Message<string, string>
            {
                Key = key,
                Value = message
            };
            if(partition.HasValue)
            {
                await _producer.ProduceAsync(new TopicPartition(topic, new Partition(partition.Value)), kafkaMessage);
            }
            else
            {
                await _producer.ProduceAsync(topic, kafkaMessage);
            }    
            
        }
    }

}
