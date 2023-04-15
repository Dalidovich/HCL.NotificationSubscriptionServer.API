namespace HCL.NotificationSubscriptionServer.API.Domain.Entities
{
    public class Notification
    {
        public Guid? Id { get; set; }
        public Guid ArticleId { get; set; }
        public Guid AccountId { get; set; }
    }
}
