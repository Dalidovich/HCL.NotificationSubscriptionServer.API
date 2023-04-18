using HCL.NotificationSubscriptionServer.API.Domain.DTO;
using HCL.NotificationSubscriptionServer.API.Domain.Enums;

namespace HCL.NotificationSubscriptionServer.API.Domain.Entities
{
    public class Relationship
    {
        public Guid? Id { get; set; }
        public Guid AccountMasterId { get; set; }
        public Guid AccountSlaveId { get; set; }
        public RelationshipStatus Status { get; set; }
        public List<Notification> notifications { get; set; }=new List<Notification>();

        public Relationship(RelationshipDTO DTO)
        {
            AccountMasterId = DTO.AccountMasterId;
            AccountSlaveId = DTO.AccountSlaveId;
            Status = DTO.Status;
        }

        public Relationship(Guid? id)
        {
            Id = id;
        }

        public Relationship()
        {
        }
    }
}