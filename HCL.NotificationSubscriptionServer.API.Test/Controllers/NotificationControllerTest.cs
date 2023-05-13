using FluentAssertions;
using HCL.NotificationSubscriptionServer.API.BLL.Services;
using HCL.NotificationSubscriptionServer.API.Controllers;
using HCL.NotificationSubscriptionServer.API.Domain.Entities;
using HCL.NotificationSubscriptionServer.API.Domain.Enums;
using Microsoft.AspNetCore.Mvc;
using Xunit;

namespace HCL.NotificationSubscriptionServer.API.Test.Controllers
{
    public class NotificationControllerTest
    {
        [Fact]
        public async Task DeleteNotification_WhenExistNotificationIsMine_ReturnNoContent()
        {
            //Arrange
            var accountId = Guid.NewGuid();
            List<Relationship> relationships = new List<Relationship>
            {
                new Relationship()
                {
                    Id = Guid.NewGuid(),
                    AccountMasterId = Guid.NewGuid(),
                    AccountSlaveId = accountId,
                    Status = RelationshipStatus.Subscription
                }
            };

            var notificationId = Guid.NewGuid();
            List<Notification> notifications = new List<Notification>
            {
                new Notification()
                {
                    Id = notificationId,
                    ArticleId = Guid.NewGuid().ToString(),
                    Relationship = relationships.First(),
                    RelationshipId = (Guid)relationships.First().Id
                }
            };

            var notificationRep = StandartMockBuilder.CreateNotificationRepositoryMock(notifications);
            var relationshipRep = StandartMockBuilder.CreateRelationshipRepositoryMock(relationships);

            var relationServ = new RelationshipService(relationshipRep.Object, StandartMockBuilder.mockLoggerRelatServ);
            var notifServ = new NotificationService(notificationRep.Object, relationServ, StandartMockBuilder.mockLoggerNotifServ);
            var controller = new NotificationController(notifServ, StandartMockBuilder.mockLoggerNotificationController);

            //Act
            var noContentResult = await controller.DeleteNotification(accountId, notificationId) as NoContentResult;

            //Assert
            noContentResult.Should().NotBeNull();
            notifications.Should().BeEmpty();
        }

        [Fact]
        public async Task DeleteNotification_WhenExistNotificationIsNotMine_ReturnForbid()
        {
            //Arrange
            var accountId = Guid.NewGuid();
            List<Relationship> relationships = new List<Relationship>
            {
                new Relationship()
                {
                    Id = Guid.NewGuid(),
                    AccountMasterId = Guid.NewGuid(),
                    AccountSlaveId = accountId,
                    Status = RelationshipStatus.Subscription
                }
            };

            var notificationId = Guid.NewGuid();
            List<Notification> notifications = new List<Notification>
            {
                new Notification()
                {
                    Id = notificationId,
                    ArticleId = Guid.NewGuid().ToString(),
                    Relationship = relationships.First(),
                    RelationshipId = (Guid)relationships.First().Id
                }
            };

            var notificationRep = StandartMockBuilder.CreateNotificationRepositoryMock(notifications);
            var relationshipRep = StandartMockBuilder.CreateRelationshipRepositoryMock(relationships);

            var relationServ = new RelationshipService(relationshipRep.Object, StandartMockBuilder.mockLoggerRelatServ);
            var notifServ = new NotificationService(notificationRep.Object, relationServ, StandartMockBuilder.mockLoggerNotifServ);
            var controller = new NotificationController(notifServ, StandartMockBuilder.mockLoggerNotificationController);

            //Act
            var forbidResult = await controller.DeleteNotification(Guid.NewGuid(), notificationId) as ForbidResult;

            //Assert
            forbidResult.Should().NotBeNull();
            notifications.Should().NotBeEmpty();
        }

        [Fact]
        public async Task DeleteNotification_WhenNotExistNotificationIsNotMine_ReturnNotFound()
        {
            //Arrange
            var accountId = Guid.NewGuid();
            List<Relationship> relationships = new List<Relationship>
            {
                new Relationship()
                {
                    Id = Guid.NewGuid(),
                    AccountMasterId = Guid.NewGuid(),
                    AccountSlaveId = accountId,
                    Status = RelationshipStatus.Subscription
                }
            };

            var notificationId = Guid.NewGuid();
            List<Notification> notifications = new List<Notification>
            {
                new Notification()
                {
                    Id = notificationId,
                    ArticleId = Guid.NewGuid().ToString(),
                    Relationship = relationships.First(),
                    RelationshipId = (Guid)relationships.First().Id
                }
            };

            var notificationRep = StandartMockBuilder.CreateNotificationRepositoryMock(notifications);
            var relationshipRep = StandartMockBuilder.CreateRelationshipRepositoryMock(relationships);

            var relationServ = new RelationshipService(relationshipRep.Object, StandartMockBuilder.mockLoggerRelatServ);
            var notifServ = new NotificationService(notificationRep.Object, relationServ, StandartMockBuilder.mockLoggerNotifServ);
            var controller = new NotificationController(notifServ, StandartMockBuilder.mockLoggerNotificationController);

            //Act
            var notFoundResult = await controller.DeleteNotification(Guid.NewGuid(), Guid.NewGuid()) as NotFoundResult;

            //Assert
            notFoundResult.Should().NotBeNull();
            notifications.Should().NotBeEmpty();
        }

        [Fact]
        public async Task DeleteNotification_WhenExistNotification_ReturnNoContent()
        {
            //Arrange
            var accountId = Guid.NewGuid();
            List<Relationship> relationships = new List<Relationship>
            {
                new Relationship()
                {
                    Id = Guid.NewGuid(),
                    AccountMasterId = Guid.NewGuid(),
                    AccountSlaveId = accountId,
                    Status = RelationshipStatus.Subscription
                }
            };

            var notificationId = Guid.NewGuid();
            List<Notification> notifications = new List<Notification>
            {
                new Notification()
                {
                    Id = notificationId,
                    ArticleId = Guid.NewGuid().ToString(),
                    Relationship = relationships.First(),
                    RelationshipId = (Guid)relationships.First().Id
                }
            };

            var notificationRep = StandartMockBuilder.CreateNotificationRepositoryMock(notifications);
            var relationshipRep = StandartMockBuilder.CreateRelationshipRepositoryMock(relationships);

            var relationServ = new RelationshipService(relationshipRep.Object, StandartMockBuilder.mockLoggerRelatServ);
            var notifServ = new NotificationService(notificationRep.Object, relationServ, StandartMockBuilder.mockLoggerNotifServ);
            var controller = new NotificationController(notifServ, StandartMockBuilder.mockLoggerNotificationController);

            //Act
            var noContentResult = await controller.DeleteNotification(notificationId) as NoContentResult;

            //Assert
            noContentResult.Should().NotBeNull();
            notifications.Should().BeEmpty();
        }

        [Fact]
        public async Task DeleteNotification_WhenNotExistNotification_ReturnNotFound()
        {
            //Arrange
            var accountId = Guid.NewGuid();
            List<Relationship> relationships = new List<Relationship>
            {
                new Relationship()
                {
                    Id = Guid.NewGuid(),
                    AccountMasterId = Guid.NewGuid(),
                    AccountSlaveId = accountId,
                    Status = RelationshipStatus.Subscription
                }
            };

            var notificationId = Guid.NewGuid();
            List<Notification> notifications = new List<Notification>
            {
                new Notification()
                {
                    Id = notificationId,
                    ArticleId = Guid.NewGuid().ToString(),
                    Relationship = relationships.First(),
                    RelationshipId = (Guid)relationships.First().Id
                }
            };

            var notificationRep = StandartMockBuilder.CreateNotificationRepositoryMock(notifications);
            var relationshipRep = StandartMockBuilder.CreateRelationshipRepositoryMock(relationships);

            var relationServ = new RelationshipService(relationshipRep.Object, StandartMockBuilder.mockLoggerRelatServ);
            var notifServ = new NotificationService(notificationRep.Object, relationServ, StandartMockBuilder.mockLoggerNotifServ);
            var controller = new NotificationController(notifServ, StandartMockBuilder.mockLoggerNotificationController);

            //Act
            var notFoundResult = await controller.DeleteNotification(Guid.NewGuid()) as NotFoundResult;

            //Assert
            notFoundResult.Should().NotBeNull();
            notifications.Should().NotBeEmpty();
        }
    }
}
