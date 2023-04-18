namespace HCL.NotificationSubscriptionServer.API.BLL.Interfaces
{
    public interface IKafkaConsumerService:IDisposable
    {
        public Task Listen();
        public void Subscribe();
    }
}