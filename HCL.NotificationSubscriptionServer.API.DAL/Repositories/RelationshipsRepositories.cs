using HCL.NotificationSubscriptionServer.API.DAL.Repositories.Interfaces;
using HCL.NotificationSubscriptionServer.API.Domain.Entities;

namespace HCL.NotificationSubscriptionServer.API.DAL.Repositories
{
    public class RelationshipsRepositories : IRelationshipRepositories
    {
        private readonly AppDBContext _db;

        public RelationshipsRepositories(AppDBContext db)
        {
            _db = db;
        }

        public async Task<Relationship> AddAsync(Relationship entity)
        {
            var createdEntity = await _db.Relationships.AddAsync(entity);
            return createdEntity.Entity;
        }

        public bool Delete(Relationship entity)
        {
            _db.Relationships.Remove(entity);
            return true;
        }

        public IQueryable<Relationship> GetAsync()
        {
            return _db.Relationships;
        }

        public async Task<bool> SaveAsync()
        {
            await _db.SaveChangesAsync();
            return true;
        }
    }
}
