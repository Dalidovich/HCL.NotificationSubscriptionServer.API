using FluentAssertions;
using HCL.NotificationSubscriptionServer.API.BLL.Services;
using HCL.NotificationSubscriptionServer.API.Domain.Entities;
using HCL.NotificationSubscriptionServer.API.Domain.Enums;
using Xunit;

namespace HCL.NotificationSubscriptionServer.API.Test.Services
{
    public class KafkaListenerServiceTest
    {
        [Fact]
        public async Task CreateNotification_FromKafkaListenerWithExistRightRelation_ReturnNewNotification()
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

            var kafkaService = StandartMockBuilder.CreateKafkaConsumerServiceMock(notifServ, masterAccountId);

            //Act
            await kafkaService.Object.Listen();

            //Assert
            relationships.Should().NotBeEmpty();
        }

        [Fact]
        public async Task CreateNotification_FromKafkaListenerWithExistWrongRelation_Returtn_()
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

            var kafkaService = StandartMockBuilder.CreateKafkaConsumerServiceMock(notifServ, masterAccountId);

            //Act
            await kafkaService.Object.Listen();

            //Assert
            notifications.Should().BeEmpty();
        }

        [Fact]
        public async Task CreateNotification_FromKafkaListenerWithNotExistRightRelation_Retutn_()
        {
            //Arrange
            List<Relationship> relationships = new List<Relationship>();
            List<Notification> notifications = new List<Notification>();

            var notificationRep = StandartMockBuilder.CreateNotificationRepositoryMock(notifications);
            var relationshipRep = StandartMockBuilder.CreateRelationshipRepositoryMock(relationships);

            var relationServ = new RelationshipService(relationshipRep.Object, StandartMockBuilder.mockLoggerRelatServ);
            var notifServ = new NotificationService(notificationRep.Object, relationServ, StandartMockBuilder.mockLoggerNotifServ);

            var kafkaService = StandartMockBuilder.CreateKafkaConsumerServiceMock(notifServ, Guid.NewGuid());

            //Act
            await kafkaService.Object.Listen();

            //Assert
            notifications.Should().BeEmpty();
        }
    }
}
