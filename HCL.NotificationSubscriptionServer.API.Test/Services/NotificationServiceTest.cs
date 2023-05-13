using FluentAssertions;
using HCL.NotificationSubscriptionServer.API.BLL.Services;
using HCL.NotificationSubscriptionServer.API.Domain.Entities;
using HCL.NotificationSubscriptionServer.API.Domain.Enums;
using Xunit;

namespace HCL.NotificationSubscriptionServer.API.Test.Services
{
    public class NotificationServiceTest 
    {
        [Fact]
        public async Task CreateNotification_WithExistRightRelation_ReturnNewNotification()
        {
            //Arrange
            var masterAccountId = Guid.NewGuid();
            List<Relationship> relationships = new List<Relationship>
            {
                new Relationship()
                {
                    Id = Guid.NewGuid(),
                    AccountMasterId = masterAccountId,
                    AccountSlaveId =  Guid.NewGuid(),
                    Status = RelationshipStatus.Subscription
                }
            };

            List<Notification> notifications = new List<Notification>();

            var notificationRep = StandartMockBuilder.CreateNotificationRepositoryMock(notifications);
            var relationshipRep = StandartMockBuilder.CreateRelationshipRepositoryMock(relationships);

            var relationServ = new RelationshipService(relationshipRep.Object, StandartMockBuilder.mockLoggerRelatServ);
            var notifServ = new NotificationService(notificationRep.Object, relationServ, StandartMockBuilder.mockLoggerNotifServ);

            //Act
            var result = await notifServ.CreateNotification("article 1",masterAccountId);

            //Assert
            result.Should().NotBeNull();
            result.StatusCode.Should().Be(StatusCode.NotificationCreate);
            result.Data.Should().BeTrue();
            notifications.Should().NotBeEmpty();
        }

        [Fact]
        public async Task CreateNotification_WithExistWrongRelation_Returtn_()
        {
            //Arrange
            var masterAccountId = Guid.NewGuid();
            List<Relationship> relationships = new List<Relationship>
            {
                new Relationship()
                {
                    Id = Guid.NewGuid(),
                    AccountMasterId = masterAccountId,
                    AccountSlaveId =  Guid.NewGuid(),
                    Status = RelationshipStatus.Normal
                }
            };

            List<Notification> notifications = new List<Notification>();

            var notificationRep = StandartMockBuilder.CreateNotificationRepositoryMock(notifications);
            var relationshipRep = StandartMockBuilder.CreateRelationshipRepositoryMock(relationships);

            var relationServ = new RelationshipService(relationshipRep.Object, StandartMockBuilder.mockLoggerRelatServ);
            var notifServ = new NotificationService(notificationRep.Object, relationServ, StandartMockBuilder.mockLoggerNotifServ);

            //Act
            var result = await notifServ.CreateNotification("article 1", masterAccountId);

            //Assert
            result.Should().NotBeNull();
            result.StatusCode.Should().Be(StatusCode.NotificationCreate);
            notifications.Should().BeEmpty();
        }

        [Fact]
        public async Task CreateNotification_WithNotExistRightRelation_RetutnBooleanTrue()
        {
            //Arrange
            List<Relationship> relationships = new List<Relationship>();
            List<Notification> notifications = new List<Notification>();

            var notificationRep = StandartMockBuilder.CreateNotificationRepositoryMock(notifications);
            var relationshipRep = StandartMockBuilder.CreateRelationshipRepositoryMock(relationships);

            var relationServ = new RelationshipService(relationshipRep.Object, StandartMockBuilder.mockLoggerRelatServ);
            var notifServ = new NotificationService(notificationRep.Object, relationServ, StandartMockBuilder.mockLoggerNotifServ);

            //Act
            var result = await notifServ.CreateNotification("article 1", Guid.NewGuid());

            //Assert
            result.Should().NotBeNull();
            result.StatusCode.Should().Be(StatusCode.NotificationCreate);
            result.Data.Should().BeFalse();
            notifications.Should().BeEmpty();
        }

        [Fact]
        public async Task DeleteNotification_WithExistRelationship_ReturnBooleanTrue()
        {
            //Arrange
            List<Relationship> relationships = new List<Relationship>
            {
                new Relationship()
                {
                    Id = Guid.NewGuid(),
                    AccountMasterId = Guid.NewGuid(),
                    AccountSlaveId = Guid.NewGuid(),
                    Status = RelationshipStatus.Subscription
                }
            };

            var notificationId=Guid.NewGuid();
            List<Notification> notifications = new List<Notification>()
            {
                new Notification()
                {
                    Id= notificationId,
                }
            };

            var notificationRep = StandartMockBuilder.CreateNotificationRepositoryMock(notifications);
            var relationshipRep = StandartMockBuilder.CreateRelationshipRepositoryMock(relationships);

            var relationServ = new RelationshipService(relationshipRep.Object, StandartMockBuilder.mockLoggerRelatServ);
            var notifServ = new NotificationService(notificationRep.Object, relationServ, StandartMockBuilder.mockLoggerNotifServ);

            //Act
            var result = await notifServ.DeleteNotification(notificationId);

            //Assert
            result.Should().NotBeNull();
            result.Data.Should().BeTrue();
            result.StatusCode.Should().Be(StatusCode.NotificationDelete);
            notifications.Should().BeEmpty();
        }

        [Fact]
        public async Task DeleteNotification_WithNotExistRelationship_ReturnBooleanTrue()
        {
            //Arrange
            List<Relationship> relationships = new List<Relationship>
            {
                new Relationship()
                {
                    Id = Guid.NewGuid(),
                    AccountMasterId = Guid.NewGuid(),
                    AccountSlaveId = Guid.NewGuid(),
                    Status = RelationshipStatus.Subscription
                }
            };

            List<Notification> notifications = new List<Notification>()
            {
                new Notification()
                {
                    Id= Guid.NewGuid(),
                }
            };

            var notificationRep = StandartMockBuilder.CreateNotificationRepositoryMock(notifications);
            var relationshipRep = StandartMockBuilder.CreateRelationshipRepositoryMock(relationships);

            var relationServ = new RelationshipService(relationshipRep.Object, StandartMockBuilder.mockLoggerRelatServ);
            var notifServ = new NotificationService(notificationRep.Object, relationServ, StandartMockBuilder.mockLoggerNotifServ);

            //Act
            var result = await notifServ.DeleteNotification(Guid.NewGuid());

            //Assert
            result.Should().NotBeNull();
            notifications.Should().NotBeEmpty();
        }

        [Fact]
        public async Task GetNotification_WithExistRelationship_ReturnNotification()
        {
            //Arrange
            List<Relationship> relationships = new List<Relationship>
            {
                new Relationship()
                {
                    Id = Guid.NewGuid(),
                    AccountMasterId = Guid.NewGuid(),
                    AccountSlaveId = Guid.NewGuid(),
                    Status = RelationshipStatus.Subscription
                }
            };

            var notificationId = Guid.NewGuid();
            List<Notification> notifications = new List<Notification>()
            {
                new Notification()
                {
                    Id= notificationId,
                }
            };

            var notificationRep = StandartMockBuilder.CreateNotificationRepositoryMock(notifications);
            var relationshipRep = StandartMockBuilder.CreateRelationshipRepositoryMock(relationships);

            var relationServ = new RelationshipService(relationshipRep.Object, StandartMockBuilder.mockLoggerRelatServ);
            var notifServ = new NotificationService(notificationRep.Object, relationServ, StandartMockBuilder.mockLoggerNotifServ);

            //Act
            var result = notifServ.GetNotificationOData();

            //Assert
            result.Should().NotBeNull();
            result.StatusCode.Should().Be(StatusCode.NotificationRead);
            notifications.Should().NotBeEmpty();
        }

        [Fact]
        public async Task GetNotification_WithNotExistRelationship_ReturnEmptyResponse()
        {
            //Arrange
            List<Relationship> relationships = new List<Relationship>
            {
                new Relationship()
                {
                    Id = Guid.NewGuid(),
                    AccountMasterId = Guid.NewGuid(),
                    AccountSlaveId = Guid.NewGuid(),
                    Status = RelationshipStatus.Subscription
                }
            };

            List<Notification> notifications = new List<Notification>();

            var notificationRep = StandartMockBuilder.CreateNotificationRepositoryMock(notifications);
            var relationshipRep = StandartMockBuilder.CreateRelationshipRepositoryMock(relationships);

            var relationServ = new RelationshipService(relationshipRep.Object, StandartMockBuilder.mockLoggerRelatServ);
            var notifServ = new NotificationService(notificationRep.Object, relationServ, StandartMockBuilder.mockLoggerNotifServ);

            //Act
            var result = notifServ.GetNotificationOData();

            //Assert
            result.Should().NotBeNull();
            result.StatusCode.Should().Be(StatusCode.EntityNotFound);
            result.Data.Should().BeNull();
            result.Message.Should().NotBeNull();
            notifications.Should().BeEmpty();
        }
    }
}
