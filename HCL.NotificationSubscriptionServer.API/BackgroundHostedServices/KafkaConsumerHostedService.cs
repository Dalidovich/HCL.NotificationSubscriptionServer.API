using HCL.NotificationSubscriptionServer.API.BLL.Interfaces;
using Microsoft.AspNetCore.Mvc.ModelBinding.Binders;
using Microsoft.Extensions.DependencyInjection;

namespace HCL.NotificationSubscriptionServer.API.BackgroundHostedServices
{
    public class KafkaConsumerHostedService : BackgroundService
    {
        private IKafkaConsumerService _kafkaConsumerService;
        private readonly IServiceScopeFactory _serviceScopeFactory;

        public KafkaConsumerHostedService(IServiceScopeFactory serviceScopeFactory)
        {
            _serviceScopeFactory = serviceScopeFactory;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            using var scope = _serviceScopeFactory.CreateScope();
            _kafkaConsumerService = scope.ServiceProvider.GetRequiredService<IKafkaConsumerService>();
            _kafkaConsumerService.Subscribe();
            while (!stoppingToken.IsCancellationRequested)
            {
                await Task.Delay(5);
                await _kafkaConsumerService.Listen();
            }
        }
    }
}
