using HCL.NotificationSubscriptionServer.API.Domain.DTO;

namespace HCL.NotificationSubscriptionServer.API.Domain.Entities
{
    public class Notification
    {
        public Guid? Id { get; set; }
        public Guid ArticleId { get; set; }
        public Guid RelationshipId { get; set; }
        public Relationship? Relationship { get; set; }

        public Notification()
        {
        }

        public Notification(Guid? id)
        {
            Id = id;
        }
    }
}
