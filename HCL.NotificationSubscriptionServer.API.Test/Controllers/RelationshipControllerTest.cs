using FluentAssertions;
using HCL.NotificationSubscriptionServer.API.BLL.Services;
using HCL.NotificationSubscriptionServer.API.Controllers;
using HCL.NotificationSubscriptionServer.API.Domain.DTO;
using HCL.NotificationSubscriptionServer.API.Domain.Entities;
using HCL.NotificationSubscriptionServer.API.Domain.Enums;
using Microsoft.AspNetCore.Mvc;
using Xunit;

namespace HCL.NotificationSubscriptionServer.API.Test.Controllers
{
    public class RelationshipControllerTest
    {
        [Fact]
        public async Task CreateRelationship_WithRightData_ReturnNewRelation()
        {
            //Arrange
            List<Relationship> relationships = new List<Relationship>();

            var relationshipRep = StandartMockBuilder.CreateRelationshipRepositoryMock(relationships);

            var relationServ = new RelationshipService(relationshipRep.Object, StandartMockBuilder.mockLoggerRelatServ);
            var controller = new RelationshipController(relationServ, StandartMockBuilder.mockLoggerRelationController);

            var relationshipDto = new RelationshipDTO()
            {
                AccountMasterId = Guid.NewGuid(),
                AccountSlaveId= Guid.NewGuid(),
                Status=RelationshipStatus.Subscription
            };

            //Act
            var result = await controller.CreateRelationship(relationshipDto) as CreatedResult;

            //Assert
            result.Should().NotBeNull();
            relationships.Should().NotBeEmpty();
        }

        [Fact]
        public async Task DeleteRelationship_WhenExistRelationshipIsMine_ReturnNoContent()
        {
            //Arrange
            var accountId = Guid.NewGuid();
            var relationshipId = Guid.NewGuid();
            List<Relationship> relationships = new List<Relationship>
            {
                new Relationship()
                {
                    Id = relationshipId,
                    AccountMasterId = Guid.NewGuid(),
                    AccountSlaveId = accountId,
                    Status = RelationshipStatus.Subscription
                }
            };

            var relationshipRep = StandartMockBuilder.CreateRelationshipRepositoryMock(relationships);

            var relationServ = new RelationshipService(relationshipRep.Object, StandartMockBuilder.mockLoggerRelatServ);
            var controller = new RelationshipController(relationServ, StandartMockBuilder.mockLoggerRelationController);

            //Act
            var noContentResult = await controller.DeleteRelationship(accountId, relationshipId) as NoContentResult;

            //Assert
            noContentResult.Should().NotBeNull();
            relationships.Should().BeEmpty();
        }

        [Fact]
        public async Task DeleteRelationship_WhenExistRelationshipIsNotMine_ReturnForbid()
        {
            //Arrange
            var relationshipId = Guid.NewGuid();
            List<Relationship> relationships = new List<Relationship>
            {
                new Relationship()
                {
                    Id = relationshipId,
                    AccountMasterId = Guid.NewGuid(),
                    AccountSlaveId = Guid.NewGuid(),
                    Status = RelationshipStatus.Subscription
                }
            };

            var relationshipRep = StandartMockBuilder.CreateRelationshipRepositoryMock(relationships);

            var relationServ = new RelationshipService(relationshipRep.Object, StandartMockBuilder.mockLoggerRelatServ);
            var controller = new RelationshipController(relationServ, StandartMockBuilder.mockLoggerRelationController);

            //Act
            var forbidResult = await controller.DeleteRelationship(Guid.NewGuid(), relationshipId) as ForbidResult;

            //Assert
            forbidResult.Should().NotBeNull();
            relationships.Should().NotBeEmpty();
        }

        [Fact]
        public async Task DeleteRelationship_WhenNotExistRelationshipIsNotMine_ReturnNotFound()
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

            var relationshipRep = StandartMockBuilder.CreateRelationshipRepositoryMock(relationships);

            var relationServ = new RelationshipService(relationshipRep.Object, StandartMockBuilder.mockLoggerRelatServ);
            var controller = new RelationshipController(relationServ, StandartMockBuilder.mockLoggerRelationController);

            //Act
            var notFoundResult = await controller.DeleteRelationship(Guid.NewGuid(), Guid.NewGuid()) as NotFoundResult;

            //Assert
            notFoundResult.Should().NotBeNull();
            relationships.Should().NotBeEmpty();
        }

        [Fact]
        public async Task DeleteRelationship_WhenExistRelationship_ReturnNoContent()
        {
            //Arrange
            var relationshipId = Guid.NewGuid();
            List<Relationship> relationships = new List<Relationship>
            {
                new Relationship()
                {
                    Id = relationshipId,
                    AccountMasterId = Guid.NewGuid(),
                    AccountSlaveId = Guid.NewGuid(),
                    Status = RelationshipStatus.Subscription
                }
            };

            var relationshipRep = StandartMockBuilder.CreateRelationshipRepositoryMock(relationships);

            var relationServ = new RelationshipService(relationshipRep.Object, StandartMockBuilder.mockLoggerRelatServ);
            var controller = new RelationshipController(relationServ, StandartMockBuilder.mockLoggerRelationController);

            //Act
            var noContentResult = await controller.DeleteRelationship(relationshipId) as NoContentResult;

            //Assert
            noContentResult.Should().NotBeNull();
            relationships.Should().BeEmpty();
        }

        [Fact]
        public async Task DeleteRelationship_WhenNotExistRelationship_ReturnNotFound()
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

            var relationshipRep = StandartMockBuilder.CreateRelationshipRepositoryMock(relationships);

            var relationServ = new RelationshipService(relationshipRep.Object, StandartMockBuilder.mockLoggerRelatServ);
            var controller = new RelationshipController(relationServ, StandartMockBuilder.mockLoggerRelationController);

            //Act
            var notFoundResult = await controller.DeleteRelationship(Guid.NewGuid()) as NotFoundResult;

            //Assert
            notFoundResult.Should().NotBeNull();
            relationships.Should().NotBeEmpty();
        }
    }
}
