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

        public KafkaConsumerService(KafkaSettings kafkaSettings,INotificationService notificationService)
        {
            _bootstrapServers = kafkaSettings.BootstrapServers;
            _topic = kafkaSettings.Topic;
            var config = new ConsumerConfig
            {
                BootstrapServers = _bootstrapServers,
                GroupId="notificationsCreator",
                AutoOffsetReset= AutoOffsetReset.Earliest,
            };
            _consumer = new ConsumerBuilder<string, string>(config).Build();
            _notificationService = notificationService;
        }

        public async Task Listen()
        {
            _consumer.Subscribe(_topic);
            try
            {
                while (true)
                {
                    var cr = _consumer.Consume(TimeSpan.FromSeconds(5));
                    if (cr != null)
                    {
                        await _notificationService.CreateNotification(cr.Message.Key, new Guid(cr.Message.Value));
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"[CreateNotification] : {ex.Message}");
            }
            finally
            {
                _consumer.Close();
            }
        }

        public void Dispose()
        {
            _consumer.Dispose();
        }
    }
}
