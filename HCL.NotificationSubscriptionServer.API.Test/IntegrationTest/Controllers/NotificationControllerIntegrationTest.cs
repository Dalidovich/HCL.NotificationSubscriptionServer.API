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
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace HCL.NotificationSubscriptionServer.API.Test.IntegrationTest.Controllers
{
    public class NotificationControllerIntegrationTest : IAsyncLifetime
    {
        private IContainer pgContainer = TestContainerBuilder.CreatePostgreSQLContainer();
        private WebApplicationFactory<Program> webHost;
        private NotificationController controller;

        public async Task InitializeAsync()
        {
            await pgContainer.StartAsync();
            webHost = CustomTestHostBuilder.Build(TestContainerBuilder.npgsqlUser, TestContainerBuilder.npgsqlPassword
                , "localhost", pgContainer.GetMappedPublicPort(5432), TestContainerBuilder.npgsqlDB);

            var scope = webHost.Services.CreateScope();
            var appDBContext = scope.ServiceProvider.GetRequiredService<AppDBContext>();
            var notificationRepository = new NotificationRepository(appDBContext);
            var relationshipRepository = new RelationshipsRepository(appDBContext);
            var relationService = new RelationshipService(relationshipRepository, StandartMockBuilder.mockLoggerRelatServ);
            var notifService = new NotificationService(notificationRepository, relationService, StandartMockBuilder.mockLoggerNotifServ);
            controller = new NotificationController(notifService, StandartMockBuilder.mockLoggerNotificationController);
        }

        public async Task DisposeAsync()
        {
            await pgContainer.StopAsync();
        }

        [Fact]
        public async Task DeleteNotification_WhenExistNotificationIsMine_ReturnNoContent()
        {
            //Arrange
            var accountId = Guid.NewGuid();
            var relationId = Guid.NewGuid();
            var notificationId = Guid.NewGuid();

            var relationships = new List<Relationship>()
            {
                new Relationship()
                {
                    Id = relationId,
                    AccountMasterId = Guid.NewGuid(),
                    AccountSlaveId = accountId,
                    Status = RelationshipStatus.Subscription,
                }
            };
            var notifications = new List<Notification>()
            {
                new Notification()
                {
                    Id = notificationId,
                    ArticleId = Guid.NewGuid().ToString(),
                    RelationshipId = relationId,
                }
            };

            await TestDBFiller.AddRelationshipInDBNotTracked(webHost, relationships);
            await TestDBFiller.AddNotificationInDBNotTracked(webHost, notifications);


            //Act
            var noContentResult = await controller.DeleteNotification(accountId, notificationId) as NoContentResult;

            //Assert
            noContentResult.Should().NotBeNull();
        }

        [Fact]
        public async Task DeleteNotification_WhenExistNotificationIsNotMine_ReturnForbid()
        {
            //Arrange
            var accountId = Guid.NewGuid();
            var relationId = Guid.NewGuid();
            var notificationId = Guid.NewGuid();
            var relationships = new List<Relationship>()
            {
                new Relationship()
                {
                    Id = relationId,
                    AccountMasterId = Guid.NewGuid(),
                    AccountSlaveId = accountId,
                    Status = RelationshipStatus.Subscription,
                }
            };
            var notifications = new List<Notification>()
            {
                new Notification()
                {
                    Id = notificationId,
                    ArticleId = Guid.NewGuid().ToString(),
                    RelationshipId = relationId,
                }
            };

            await TestDBFiller.AddRelationshipInDBNotTracked(webHost, relationships);
            await TestDBFiller.AddNotificationInDBNotTracked(webHost, notifications);

            //Act
            var forbidResult = await controller.DeleteNotification(Guid.NewGuid(), notificationId) as ForbidResult;

            //Assert
            forbidResult.Should().NotBeNull();
        }

        [Fact]
        public async Task DeleteNotification_WhenNotExistNotificationIsNotMine_ReturnNotFound()
        {
            //Arrange
            var accountId = Guid.NewGuid();
            var relationId = Guid.NewGuid();
            var notificationId = Guid.NewGuid();
            var relationships = new List<Relationship>()
            {
                new Relationship()
                {
                    Id = relationId,
                    AccountMasterId = Guid.NewGuid(),
                    AccountSlaveId = accountId,
                    Status = RelationshipStatus.Subscription,
                }
            };
            var notifications = new List<Notification>()
            {
                new Notification()
                {
                    Id = notificationId,
                    ArticleId = Guid.NewGuid().ToString(),
                    RelationshipId = relationId,
                }
            };

            await TestDBFiller.AddRelationshipInDBNotTracked(webHost, relationships);
            await TestDBFiller.AddNotificationInDBNotTracked(webHost, notifications);

            //Act
            var forbidResult = await controller.DeleteNotification(Guid.NewGuid(), Guid.NewGuid()) as ForbidResult;

            //Assert
            forbidResult.Should().NotBeNull();
        }

        [Fact]
        public async Task DeleteNotification_WhenExistNotification_ReturnNoContent()
        {
            //Arrange
            var accountId = Guid.NewGuid();
            var relationId = Guid.NewGuid();
            var notificationId = Guid.NewGuid();
            var relationships = new List<Relationship>()
            {
                new Relationship()
                {
                    Id = relationId,
                    AccountMasterId = Guid.NewGuid(),
                    AccountSlaveId = accountId,
                    Status = RelationshipStatus.Subscription,
                }
            };
            var notifications = new List<Notification>()
            {
                new Notification()
                {
                    Id = notificationId,
                    ArticleId = Guid.NewGuid().ToString(),
                    RelationshipId = relationId,
                }
            };

            await TestDBFiller.AddRelationshipInDBNotTracked(webHost, relationships);
            await TestDBFiller.AddNotificationInDBNotTracked(webHost, notifications);

            //Act
            var noContentResult = await controller.DeleteNotification(notificationId) as NoContentResult;

            //Assert
            noContentResult.Should().NotBeNull();
        }

        [Fact]
        public async Task DeleteNotification_WhenNotExistNotification_ReturnNotFound()
        {
            //Arrange
            var accountId = Guid.NewGuid();
            var relationId = Guid.NewGuid();
            var notificationId = Guid.NewGuid();
            var relationships = new List<Relationship>()
            {
                new Relationship()
                {
                    Id = relationId,
                    AccountMasterId = Guid.NewGuid(),
                    AccountSlaveId = accountId,
                    Status = RelationshipStatus.Subscription,
                }
            };
            var notifications = new List<Notification>()
            {
                new Notification()
                {
                    Id = notificationId,
                    ArticleId = Guid.NewGuid().ToString(),
                    RelationshipId = relationId,
                }
            };

            await TestDBFiller.AddRelationshipInDBNotTracked(webHost, relationships);
            await TestDBFiller.AddNotificationInDBNotTracked(webHost, notifications);

            //Act
            var notFoundResult = await controller.DeleteNotification(Guid.NewGuid()) as NotFoundResult;

            //Assert
            notFoundResult.Should().NotBeNull();
        }
    }
}
