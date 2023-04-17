using HCL.NotificationSubscriptionServer.API.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HCL.NotificationSubscriptionServer.API.DAL.Repositories.Interfaces
{
    public interface INotificationRepositories : BaseRepositories<Notification>
    {
        public Task AddRangeAsync(IEnumerable<Notification> notifications);
        public Task AddRangeAsync(IQueryable<Notification> notifications);
    }
}
