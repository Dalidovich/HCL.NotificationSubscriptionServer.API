using HCL.NotificationSubscriptionServer.API.DAL;
using HCL.NotificationSubscriptionServer.API.Domain.Entities;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;

namespace HCL.NotificationSubscriptionServer.API.Test.IntegrationTest
{
    public class TestDBFiller
    {
        public static async Task AddRelationshipInDBNotTracked(WebApplicationFactory<Program> webHost, List<Relationship> relationships)
        {
            var scope = webHost.Services.CreateScope();
            var commentAppDBContext = scope.ServiceProvider.GetRequiredService<AppDBContext>();
            await commentAppDBContext.Relationships.AddRangeAsync(relationships);
            await commentAppDBContext.SaveChangesAsync();
            scope.Dispose();
        }
        public static async Task AddNotificationInDBNotTracked(WebApplicationFactory<Program> webHost, List<Notification> notifications)
        {
            var scope = webHost.Services.CreateScope();
            var commentAppDBContext = scope.ServiceProvider.GetRequiredService<AppDBContext>();
            await commentAppDBContext.Notifications.AddRangeAsync(notifications);
            await commentAppDBContext.SaveChangesAsync();
            scope.Dispose();
        }
    }
}
