using Confluent.Kafka;
using HCL.NotificationSubscriptionServer.API.BLL.Interfaces;
using HCL.NotificationSubscriptionServer.API.Domain.DTO;
using HCL.NotificationSubscriptionServer.API.Domain.DTO.Builders;
using Microsoft.Extensions.Logging;
using System.Text.Json;

namespace HCL.NotificationSubscriptionServer.API.BLL.Services
{
    public class KafkaConsumerService : IKafkaConsumerService
    {
        private readonly string _topic;
        private readonly string _bootstrapServers;
        private readonly IConsumer<string, string> _consumer;
        private readonly INotificationService _notificationService;
        private readonly ILogger<KafkaConsumerService> _logger;

        public KafkaConsumerService(KafkaSettings kafkaSettings, INotificationService notificationService
            , ILogger<KafkaConsumerService> logger)
        {
            _logger = logger;
            _notificationService = notificationService;
            _bootstrapServers = kafkaSettings.BootstrapServers;
            _topic = kafkaSettings.Topic;

            var config = new ConsumerConfig
            {
                BootstrapServers = _bootstrapServers,
                GroupId = "notificationsCreator",
                AutoOffsetReset = AutoOffsetReset.Earliest,
                EnableAutoCommit = true,
            };

            _consumer = new ConsumerBuilder<string, string>(config).Build();
        }

        public void Subscribe()
        {
            _consumer.Subscribe(_topic);

            var log = new LogDTOBuidlder("Subscribe()")
            .BuildMessage($"kafka consumer subscribe on topic - {_topic}")
            .Build();
            _logger.LogInformation(JsonSerializer.Serialize(log));
        }

        public async Task Listen()
        {
            var cr = _consumer.Consume(TimeSpan.FromSeconds(1));
            if (cr != null)
            {
                var log = new LogDTOBuidlder("Subscribe()")
                .BuildMessage($"kafka consumer consume message")
                .Build();
                _logger.LogInformation(JsonSerializer.Serialize(log));

                await _notificationService.CreateNotification(cr.Message.Key, new Guid(cr.Message.Value));
            }
        }

        public void Dispose()
        {
            _consumer.Dispose();
        }
    }
}