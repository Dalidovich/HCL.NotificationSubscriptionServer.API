using Confluent.Kafka;
using HCL.NotificationSubscriptionServer.API.BLL.Interfaces;
using HCL.NotificationSubscriptionServer.API.Domain.DTO;

namespace HCL.NotificationSubscriptionServer.API.BLL.Services
{
    public class KafkaConsumerService : IKafkaConsumerService
    {
        private readonly string _topic;
        private readonly string _bootstrapServers;
        private readonly IConsumer<string, string> _consumer;
        private readonly INotificationService _notificationService;

        public KafkaConsumerService(KafkaSettings kafkaSettings, INotificationService notificationService)
        {
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
        }

        public async Task Listen()
        {
            var cr = _consumer.Consume(TimeSpan.FromSeconds(1));
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