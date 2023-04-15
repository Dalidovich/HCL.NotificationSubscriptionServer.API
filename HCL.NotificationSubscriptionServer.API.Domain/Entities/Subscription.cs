using HCL.NotificationSubscriptionServer.API.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace HCL.NotificationSubscriptionServer.API.Domain.Entities
{
    public class Relationship
    {
        public Guid? Id { get; set; }
        public Guid? AccountMasterId { get; set; }
        public Guid? AccountSlaveId { get; set; }
        public RelationshipStatus Status { get; set; }
    }
}
