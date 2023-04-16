﻿namespace HCL.NotificationSubscriptionServer.API.DAL.Repositories.Interfaces
{
    public interface BaseRepositories<T>
    {
        public Task<T> AddAsync(T entity);
        public bool Delete(T entity);
        public IQueryable<T> GetAsync();
        public Task<bool> SaveAsync();
    }
}
