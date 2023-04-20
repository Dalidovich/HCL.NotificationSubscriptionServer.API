using HCL.NotificationSubscriptionServer.API.Domain.Entities;

namespace HCL.NotificationSubscriptionServer.API.DAL.Repositories.Interfaces
{
    public interface INotificationRepositories : BaseRepositories<Notification>
    {
        public Task AddRangeAsync(IEnumerable<Notification> notifications);
        public Task AddRangeAsync(IQueryable<Notification> notifications);
    }
}