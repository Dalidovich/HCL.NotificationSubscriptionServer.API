using Confluent.Kafka;
using HCL.NotificationSubscriptionServer.API.BLL.Interfaces;
using HCL.NotificationSubscriptionServer.API.Domain.DTO;
using HCL.NotificationSubscriptionServer.API.Domain.Enums;
using HCL.NotificationSubscriptionServer.API.Domain.InnerResponse;
using Microsoft.Extensions.Logging;

namespace HCL.NotificationSubscriptionServer.API.BLL.Services
{
    public class KafkaConsumerService : IKafkaConsumerService
    {
        private readonly string _topic;
        private readonly string _bootstrapServers;
        private readonly IConsumer<string, string> _consumer;
        private readonly INotificationService _notificationService;
        private readonly ILogger<KafkaConsumerService> _logger;

        public KafkaConsumerService(KafkaSettings kafkaSettings,INotificationService notificationService, ILogger<KafkaConsumerService> logger)
        {
            _bootstrapServers = kafkaSettings.BootstrapServers;
            _topic = kafkaSettings.Topic;
            var config = new ConsumerConfig
            {
                BootstrapServers = _bootstrapServers,
                GroupId="notificationsCreator",
                AutoOffsetReset= AutoOffsetReset.Earliest,
                SaslUsername = kafkaSettings.User,
                SaslPassword = kafkaSettings.Password,
                EnableAutoCommit = true,
            };
            _consumer = new ConsumerBuilder<string, string>(config).Build();
            _notificationService = notificationService;
            _logger = logger;
        }

        public void Subscribe()
        {
            _consumer.Subscribe(_topic);
        }
        public async Task Listen()
        {
            var cr = _consumer.Consume(TimeSpan.FromSeconds(5));
            if (cr != null)
            {
                await _notificationService.CreateNotification(cr.Message.Key, new Guid(cr.Message.Value));
            }
        }

        public void Dispose()
        {
            _consumer.Dispose();
        }
    }
}
