using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HCL.NotificationSubscriptionServer.API.Domain.DTO
{
    public class NotificationDTO
    {
        public Guid ArticleId { get; set; }
        public Guid AccountId { get; set; }
    }
}
