using DotNet.Testcontainers.Containers;
using FluentAssertions;
using HCL.NotificationSubscriptionServer.API.BLL.Services;
using HCL.NotificationSubscriptionServer.API.Controllers;
using HCL.NotificationSubscriptionServer.API.DAL;
using HCL.NotificationSubscriptionServer.API.DAL.Repositories;
using HCL.NotificationSubscriptionServer.API.Domain.Entities;
using HCL.NotificationSubscriptionServer.API.Domain.Enums;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace HCL.NotificationSubscriptionServer.API.Test.IntegrationTest.Controllers
{
    public class NotificationControllerIntegrationTest : IAsyncLifetime
    {
        private IContainer pgContainer = TestContainerBuilder.CreatePostgreSQLContainer();
        private WebApplicationFactory<Program> webHost;

        public async Task InitializeAsync()
        {
            await pgContainer.StartAsync();
            webHost = CustomTestHostBuilder.Build(TestContainerBuilder.npgsqlUser, TestContainerBuilder.npgsqlPassword
                , "localhost", pgContainer.GetMappedPublicPort(5432), TestContainerBuilder.npgsqlDB);
        }

        public async Task DisposeAsync()
        {
            await pgContainer.StopAsync();
        }

        [Fact]
        public async Task DeleteNotification_WhenExistNotificationIsMine_ReturnNoContent()
        {
            //Arrange
            using var scope = this.webHost.Services.CreateScope();
            var appDBContext = scope.ServiceProvider.GetRequiredService<AppDBContext>();
            var notificationRep = new NotificationRepository(appDBContext);
            var relationshipRep = new RelationshipsRepository(appDBContext);

            var accountId = Guid.NewGuid();
            var relationId = Guid.NewGuid();
            var addedRelation = await relationshipRep.AddAsync(new Relationship()
            {
                Id = relationId,
                AccountMasterId = Guid.NewGuid(),
                AccountSlaveId = accountId,
                Status = RelationshipStatus.Subscription,
            });
            await relationshipRep.SaveAsync();

            var notificationId = Guid.NewGuid();
            var addedNotification = await notificationRep.AddAsync(new Notification()
            {
                Id = notificationId,
                ArticleId = Guid.NewGuid().ToString(),
                RelationshipId = relationId,
            });
            await notificationRep.SaveAsync();
            scope.Dispose();

            var webHost = CustomTestHostBuilder.Build(TestContainerBuilder.npgsqlUser, TestContainerBuilder.npgsqlPassword
                , "localhost", pgContainer.GetMappedPublicPort(5432), TestContainerBuilder.npgsqlDB,false);

            using var scopeForAct = webHost.Services.CreateScope();

            appDBContext = scopeForAct.ServiceProvider.GetRequiredService<AppDBContext>();
            notificationRep = new NotificationRepository(appDBContext);
            relationshipRep = new RelationshipsRepository(appDBContext);

            var relationServ = new RelationshipService(relationshipRep, StandartMockBuilder.mockLoggerRelatServ);
            var notifServ = new NotificationService(notificationRep, relationServ, StandartMockBuilder.mockLoggerNotifServ);
            var controller = new NotificationController(notifServ, StandartMockBuilder.mockLoggerNotificationController);

            //Act
            var noContentResult = await controller.DeleteNotification(accountId, notificationId) as NoContentResult;

            //Assert
            noContentResult.Should().NotBeNull();
        }

        [Fact]
        public async Task DeleteNotification_WhenExistNotificationIsNotMine_ReturnForbid()
        {
            //Arrange
            using var scope = this.webHost.Services.CreateScope();
            var appDBContext = scope.ServiceProvider.GetRequiredService<AppDBContext>();
            var notificationRep = new NotificationRepository(appDBContext);
            var relationshipRep = new RelationshipsRepository(appDBContext);

            var accountId = Guid.NewGuid();
            var relationId = Guid.NewGuid();
            var addedRelation = await relationshipRep.AddAsync(new Relationship()
            {
                Id = relationId,
                AccountMasterId = Guid.NewGuid(),
                AccountSlaveId = accountId,
                Status = RelationshipStatus.Subscription,
            });
            await relationshipRep.SaveAsync();

            var notificationId = Guid.NewGuid();
            var addedNotification = await notificationRep.AddAsync(new Notification()
            {
                Id = notificationId,
                ArticleId = Guid.NewGuid().ToString(),
                RelationshipId = relationId,
            });
            await notificationRep.SaveAsync();
            scope.Dispose();

            var webHost = CustomTestHostBuilder.Build(TestContainerBuilder.npgsqlUser, TestContainerBuilder.npgsqlPassword
                , "localhost", pgContainer.GetMappedPublicPort(5432), TestContainerBuilder.npgsqlDB, false);

            using var scopeForAct = webHost.Services.CreateScope();

            appDBContext = scopeForAct.ServiceProvider.GetRequiredService<AppDBContext>();
            notificationRep = new NotificationRepository(appDBContext);
            relationshipRep = new RelationshipsRepository(appDBContext);

            var relationServ = new RelationshipService(relationshipRep, StandartMockBuilder.mockLoggerRelatServ);
            var notifServ = new NotificationService(notificationRep, relationServ, StandartMockBuilder.mockLoggerNotifServ);
            var controller = new NotificationController(notifServ, StandartMockBuilder.mockLoggerNotificationController);

            //Act
            var forbidResult = await controller.DeleteNotification(Guid.NewGuid(), notificationId) as ForbidResult;

            //Assert
            forbidResult.Should().NotBeNull();
        }

        [Fact]
        public async Task DeleteNotification_WhenNotExistNotificationIsNotMine_ReturnNotFound()
        {
            //Arrange
            using var scope = this.webHost.Services.CreateScope();
            var appDBContext = scope.ServiceProvider.GetRequiredService<AppDBContext>();
            var notificationRep = new NotificationRepository(appDBContext);
            var relationshipRep = new RelationshipsRepository(appDBContext);

            var accountId = Guid.NewGuid();
            var relationId = Guid.NewGuid();
            var addedRelation = await relationshipRep.AddAsync(new Relationship()
            {
                Id = relationId,
                AccountMasterId = Guid.NewGuid(),
                AccountSlaveId = accountId,
                Status = RelationshipStatus.Subscription,
            });
            await relationshipRep.SaveAsync();

            var notificationId = Guid.NewGuid();
            var addedNotification = await notificationRep.AddAsync(new Notification()
            {
                Id = notificationId,
                ArticleId = Guid.NewGuid().ToString(),
                RelationshipId = relationId,
            });
            await notificationRep.SaveAsync();
            scope.Dispose();

            var webHost = CustomTestHostBuilder.Build(TestContainerBuilder.npgsqlUser, TestContainerBuilder.npgsqlPassword
                , "localhost", pgContainer.GetMappedPublicPort(5432), TestContainerBuilder.npgsqlDB, false);

            using var scopeForAct = webHost.Services.CreateScope();

            appDBContext = scopeForAct.ServiceProvider.GetRequiredService<AppDBContext>();
            notificationRep = new NotificationRepository(appDBContext);
            relationshipRep = new RelationshipsRepository(appDBContext);

            var relationServ = new RelationshipService(relationshipRep, StandartMockBuilder.mockLoggerRelatServ);
            var notifServ = new NotificationService(notificationRep, relationServ, StandartMockBuilder.mockLoggerNotifServ);
            var controller = new NotificationController(notifServ, StandartMockBuilder.mockLoggerNotificationController);

            //Act
            var forbidResult = await controller.DeleteNotification(Guid.NewGuid(), Guid.NewGuid()) as ForbidResult;

            //Assert
            forbidResult.Should().NotBeNull();
        }

        [Fact]
        public async Task DeleteNotification_WhenExistNotification_ReturnNoContent()
        {
            //Arrange
            using var scope = this.webHost.Services.CreateScope();
            var appDBContext = scope.ServiceProvider.GetRequiredService<AppDBContext>();
            var notificationRep = new NotificationRepository(appDBContext);
            var relationshipRep = new RelationshipsRepository(appDBContext);

            var accountId = Guid.NewGuid();
            var relationId = Guid.NewGuid();
            var addedRelation = await relationshipRep.AddAsync(new Relationship()
            {
                Id = relationId,
                AccountMasterId = Guid.NewGuid(),
                AccountSlaveId = accountId,
                Status = RelationshipStatus.Subscription,
            });
            await relationshipRep.SaveAsync();

            var notificationId = Guid.NewGuid();
            var addedNotification = await notificationRep.AddAsync(new Notification()
            {
                Id = notificationId,
                ArticleId = Guid.NewGuid().ToString(),
                RelationshipId = relationId,
            });
            await notificationRep.SaveAsync();
            scope.Dispose();

            var webHost = CustomTestHostBuilder.Build(TestContainerBuilder.npgsqlUser, TestContainerBuilder.npgsqlPassword
                , "localhost", pgContainer.GetMappedPublicPort(5432), TestContainerBuilder.npgsqlDB, false);

            using var scopeForAct = webHost.Services.CreateScope();

            appDBContext = scopeForAct.ServiceProvider.GetRequiredService<AppDBContext>();
            notificationRep = new NotificationRepository(appDBContext);
            relationshipRep = new RelationshipsRepository(appDBContext);

            var relationServ = new RelationshipService(relationshipRep, StandartMockBuilder.mockLoggerRelatServ);
            var notifServ = new NotificationService(notificationRep, relationServ, StandartMockBuilder.mockLoggerNotifServ);
            var controller = new NotificationController(notifServ, StandartMockBuilder.mockLoggerNotificationController);

            //Act
            var noContentResult = await controller.DeleteNotification(notificationId) as NoContentResult;

            //Assert
            noContentResult.Should().NotBeNull();
        }

        [Fact]
        public async Task DeleteNotification_WhenNotExistNotification_ReturnNotFound()
        {
            //Arrange
            using var scope = this.webHost.Services.CreateScope();
            var appDBContext = scope.ServiceProvider.GetRequiredService<AppDBContext>();
            var notificationRep = new NotificationRepository(appDBContext);
            var relationshipRep = new RelationshipsRepository(appDBContext);

            var accountId = Guid.NewGuid();
            var relationId = Guid.NewGuid();
            var addedRelation = await relationshipRep.AddAsync(new Relationship()
            {
                Id = relationId,
                AccountMasterId = Guid.NewGuid(),
                AccountSlaveId = accountId,
                Status = RelationshipStatus.Subscription,
            });
            await relationshipRep.SaveAsync();

            var notificationId = Guid.NewGuid();
            var addedNotification = await notificationRep.AddAsync(new Notification()
            {
                Id = notificationId,
                ArticleId = Guid.NewGuid().ToString(),
                RelationshipId = relationId,
            });
            await notificationRep.SaveAsync();
            scope.Dispose();

            var webHost = CustomTestHostBuilder.Build(TestContainerBuilder.npgsqlUser, TestContainerBuilder.npgsqlPassword
                , "localhost", pgContainer.GetMappedPublicPort(5432), TestContainerBuilder.npgsqlDB, false);

            using var scopeForAct = webHost.Services.CreateScope();

            appDBContext = scopeForAct.ServiceProvider.GetRequiredService<AppDBContext>();
            notificationRep = new NotificationRepository(appDBContext);
            relationshipRep = new RelationshipsRepository(appDBContext);

            var relationServ = new RelationshipService(relationshipRep, StandartMockBuilder.mockLoggerRelatServ);
            var notifServ = new NotificationService(notificationRep, relationServ, StandartMockBuilder.mockLoggerNotifServ);
            var controller = new NotificationController(notifServ, StandartMockBuilder.mockLoggerNotificationController);

            //Act
            var notFoundResult = await controller.DeleteNotification(Guid.NewGuid()) as NotFoundResult;

            //Assert
            notFoundResult.Should().NotBeNull();
        }
    }
}
