using HCL.NotificationSubscriptionServer.API.BLL.Interfaces;
using HCL.NotificationSubscriptionServer.API.BLL.Services;
using HCL.NotificationSubscriptionServer.API.Controllers;
using HCL.NotificationSubscriptionServer.API.DAL.Repositories.Interfaces;
using HCL.NotificationSubscriptionServer.API.Domain.Entities;
using Microsoft.Extensions.Logging;
using MockQueryable.Moq;
using Moq;

namespace HCL.NotificationSubscriptionServer.API.Test
{
    public class StandartMockBuilder
    {
        private static Notification _addNotification(Notification notification, List<Notification> notifications)
        {
            var notific = new Notification()
            {
                Id=Guid.NewGuid(),
                ArticleId=notification.ArticleId,
                RelationshipId=notification.RelationshipId,
                Relationship=notification.Relationship,
            };
            notifications.Add(notific);

            return notific;
        }

        public static readonly ILogger<NotificationController> mockLoggerNotificationController = new Mock<ILogger<NotificationController>>().Object;
        public static readonly ILogger<NotificationService> mockLoggerNotifServ = new Mock<ILogger<NotificationService>>().Object;
        public static readonly ILogger<RelationshipController> mockLoggerRelationController = new Mock<ILogger<RelationshipController>>().Object;
        public static readonly ILogger<RelationshipService> mockLoggerRelatServ = new Mock<ILogger<RelationshipService>>().Object;

        private static Relationship _addRelationship(Relationship relationship, List<Relationship> relationships)
        {
            var relation = new Relationship()
            {
                Id=Guid.NewGuid(),
                AccountMasterId=relationship.AccountMasterId,
                AccountSlaveId=relationship.AccountSlaveId,
                Status=relationship.Status,
            };
            relationships.Add(relation);

            return relation;
        }

        public static Mock<INotificationRepository> CreateNotificationRepositoryMock(List<Notification> notifications)
        {
            var repo = new Mock<INotificationRepository>();
            var collectionQuerybleMock = notifications.BuildMock();
            repo
                .Setup(r => r.AddAsync(It.IsAny<Notification>()))
                .ReturnsAsync((Notification notific) =>
                {

                    return _addNotification(notific, notifications);
                });

            repo.Setup(r => r.Delete(It.IsAny<Notification>()))
                .Returns((Notification notification) =>
                {
                    var del = notifications.Where(x=>x.Id==notification.Id).SingleOrDefault();

                    if (del != null)
                    {
                        notifications.Remove(del);

                        return true;
                    }

                    return false;
                });

            repo.Setup(r => r.SaveAsync())
                .ReturnsAsync(true);

            repo.Setup(r => r.GetAsync())
                .Returns(collectionQuerybleMock);

            repo.Setup(r => r.AddRangeAsync(It.IsAny<IEnumerable<Notification>>()))
                .Callback((IEnumerable<Notification> notificationsEnum) =>
                {
                    notificationsEnum.ToList().ForEach(x => _addNotification(x, notifications));
                });

            repo.Setup(r => r.AddRangeAsync(It.IsAny<IQueryable<Notification>>()))
                .Callback((IEnumerable<Notification> notificationsEnum) =>
                {
                    notificationsEnum.ToList().ForEach(x => _addNotification(x, notifications));
                });

            return repo;
        }

        public static Mock<IRelationshipRepository> CreateRelationshipRepositoryMock(List<Relationship> relationships)
        {
            var repo = new Mock<IRelationshipRepository>();
            var collectionQuerybleMock = relationships.BuildMock();
            repo.Setup(r => r.AddAsync(It.IsAny<Relationship>()))
                .ReturnsAsync((Relationship relation) =>
                {

                    return _addRelationship(relation, relationships);
                });

            repo.Setup(r => r.Delete(It.IsAny<Relationship>()))
                .Returns((Relationship relation) =>
                {
                    var del = relationships.Where(x => x.Id == relation.Id).SingleOrDefault();

                    if (del != null)
                    {
                        relationships.Remove(del);

                        return true;
                    }

                    return false;
                });

            repo.Setup(r => r.SaveAsync())
                .ReturnsAsync(true);

            repo.Setup(r => r.GetAsync())
                .Returns(collectionQuerybleMock);

            return repo;
        }

        public static Mock<IKafkaConsumerService> CreateKafkaConsumerServiceMock(INotificationService notificationService, Guid accountId)
        {
            var serv= new Mock<IKafkaConsumerService>();
            serv.Setup(s => s.Subscribe());

            serv.Setup(s => s.Listen())
                .Returns(notificationService.CreateNotification(Guid.NewGuid().ToString(), accountId));

            return serv;
        }
    }
}