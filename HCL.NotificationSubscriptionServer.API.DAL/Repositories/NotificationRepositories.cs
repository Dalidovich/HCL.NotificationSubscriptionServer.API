using HCL.NotificationSubscriptionServer.API.DAL.Repositories.Interfaces;
using HCL.NotificationSubscriptionServer.API.Domain.Entities;

namespace HCL.NotificationSubscriptionServer.API.DAL.Repositories
{
    public class NotificationRepositories : INotificationRepositories
    {
        private readonly AppDBContext _db;

        public NotificationRepositories(AppDBContext db)
        {
            _db = db;
        }

        public async Task<Notification> AddAsync(Notification entity)
        {
            var createdEntity = await _db.Notifications.AddAsync(entity);
            return createdEntity.Entity;
        }

        public  async Task AddRangeAsync(IEnumerable<Notification> notifications)
        {
            await _db.Notifications.AddRangeAsync(notifications);
        }

        public async Task AddRangeAsync(IQueryable<Notification> notifications)
        {
            await _db.Notifications.AddRangeAsync(notifications);
        }

        public bool Delete(Notification entity)
        {
            _db.Notifications.Remove(entity);
            return true;
        }

        public IQueryable<Notification> GetAsync()
        {
            return _db.Notifications.AsQueryable();
        }

        public async Task<bool> SaveAsync()
        {
            await _db.SaveChangesAsync();
            return true;
        }
    }
}
