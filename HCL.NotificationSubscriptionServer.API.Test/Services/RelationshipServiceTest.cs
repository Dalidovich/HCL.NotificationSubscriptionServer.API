using FluentAssertions;
using HCL.NotificationSubscriptionServer.API.BLL.Services;
using HCL.NotificationSubscriptionServer.API.Domain.Entities;
using HCL.NotificationSubscriptionServer.API.Domain.Enums;
using Xunit;

namespace HCL.NotificationSubscriptionServer.API.Test.Services
{
    public class RelationshipServiceTest
    {
        [Fact]
        public async Task CreateRelationship_WithRightData_ReturnNewRelationship()
        {
            //Arrange
            List<Relationship> relationships = new List<Relationship>();

            var relationshipRep = StandartMockBuilder.CreateRelationshipRepositoryMock(relationships);

            var relationServ = new RelationshipService(relationshipRep.Object, StandartMockBuilder.mockLoggerRelatServ);

            var relationship = new Relationship()
            {
                AccountMasterId = Guid.NewGuid(),
                AccountSlaveId = Guid.NewGuid(),
                Status = RelationshipStatus.Subscription
            };

            //Act
            var result = await relationServ.CreateRelationship(relationship);

            //Assert
            result.Should().NotBeNull();
            result.Data.Should().NotBeNull();
            result.Data.Status.Should().Be(RelationshipStatus.Subscription);
            result.StatusCode.Should().Be(StatusCode.SubscriptionCreate);
            relationships.Should().NotBeEmpty();
        }

        [Fact]
        public async Task DeleteRelationship_WithExistRelationship_ReturnBooleanTrue()
        {
            //Arrange
            var relationId = Guid.NewGuid();
            List<Relationship> relationships = new List<Relationship>()
            {
                new Relationship
                {
                    Id = relationId,
                    AccountMasterId = Guid.NewGuid(),
                    AccountSlaveId = Guid.NewGuid(),
                    Status = RelationshipStatus.Subscription
                }
            };
            var relationshipRep = StandartMockBuilder.CreateRelationshipRepositoryMock(relationships);

            var relationServ = new RelationshipService(relationshipRep.Object, StandartMockBuilder.mockLoggerRelatServ);

            //Act
            var result = await relationServ.DeleteRelationship(relationId);

            //Assert
            result.Should().NotBeNull();
            result.Data.Should().BeTrue();
            result.StatusCode.Should().Be(StatusCode.SubscriptionDelete);
            relationships.Should().BeEmpty();
        }

        [Fact]
        public async Task DeleteRelationship_WithNotExistRelationship_ReturnBooleanTrue()
        {
            //Arrange
            List<Relationship> relationships = new List<Relationship>()
            {
                new Relationship
                {
                    Id = Guid.NewGuid(),
                    AccountMasterId = Guid.NewGuid(),
                    AccountSlaveId = Guid.NewGuid(),
                    Status = RelationshipStatus.Subscription
                }
            };
            var relationshipRep = StandartMockBuilder.CreateRelationshipRepositoryMock(relationships);

            var relationServ = new RelationshipService(relationshipRep.Object, StandartMockBuilder.mockLoggerRelatServ);

            //Act
            var result = await relationServ.DeleteRelationship(Guid.NewGuid());

            //Assert
            result.Should().NotBeNull();
            relationships.Should().NotBeEmpty();
        }

        [Fact]
        public async Task GetRelationship_WithExistRelationship_ReturnNotification()
        {
            //Arrange
            List<Relationship> relationships = new List<Relationship>()
            {
                new Relationship
                {
                    Id = Guid.NewGuid(),
                    AccountMasterId = Guid.NewGuid(),
                    AccountSlaveId = Guid.NewGuid(),
                    Status = RelationshipStatus.Subscription
                }
            };
            var relationshipRep = StandartMockBuilder.CreateRelationshipRepositoryMock(relationships);

            var relationServ = new RelationshipService(relationshipRep.Object, StandartMockBuilder.mockLoggerRelatServ);

            //Act
            var result = relationServ.GetRelationshipOData();

            //Assert
            result.Should().NotBeNull();
            result.StatusCode.Should().Be(StatusCode.SubscriptionRead);
            relationships.Should().NotBeEmpty();
        }

        [Fact]
        public async Task GetRelationship_WithNotExistRelationship_ReturnEmptyResponse()
        {
            //Arrange
            List<Relationship> relationships = new List<Relationship>();

            var relationshipRep = StandartMockBuilder.CreateRelationshipRepositoryMock(relationships);

            var relationServ = new RelationshipService(relationshipRep.Object, StandartMockBuilder.mockLoggerRelatServ);

            //Act
            var result = relationServ.GetRelationshipOData();

            //Assert
            result.Should().NotBeNull();
            result.StatusCode.Should().Be(StatusCode.EntityNotFound);
            result.Data.Should().BeNull();
            result.Message.Should().NotBeNull();
            relationships.Should().BeEmpty();
        }
    }
}
