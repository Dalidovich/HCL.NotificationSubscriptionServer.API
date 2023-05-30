using FluentAssertions;
using HCL.NotificationSubscriptionServer.API.BLL.Services;
using HCL.NotificationSubscriptionServer.API.Controllers;
using HCL.NotificationSubscriptionServer.API.DAL.Repositories;
using HCL.NotificationSubscriptionServer.API.DAL;
using HCL.NotificationSubscriptionServer.API.Domain.DTO;
using HCL.NotificationSubscriptionServer.API.Domain.Entities;
using HCL.NotificationSubscriptionServer.API.Domain.Enums;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Xunit;
using DotNet.Testcontainers.Containers;

namespace HCL.NotificationSubscriptionServer.API.Test.IntegrationTest.Controllers
{
    public class RelationshipControllerIntegrationTest : IAsyncLifetime
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
        public async Task CreateRelationship_WithRightData_ReturnNewRelation()
        {
            //Arrange
            using var scope = this.webHost.Services.CreateScope();
            var appDBContext = scope.ServiceProvider.GetRequiredService<AppDBContext>();
            var relationshipRep = new RelationshipsRepository(appDBContext);

            var relationServ = new RelationshipService(relationshipRep, StandartMockBuilder.mockLoggerRelatServ);
            var controller = new RelationshipController(relationServ, StandartMockBuilder.mockLoggerRelationController);

            var relationshipDto = new RelationshipDTO()
            {
                AccountMasterId = Guid.NewGuid(),
                AccountSlaveId = Guid.NewGuid(),
                Status = RelationshipStatus.Subscription
            };

            //Act
            var result = await controller.CreateRelationship(relationshipDto) as CreatedResult;

            //Assert
            result.Should().NotBeNull();
        }

        [Fact]
        public async Task DeleteRelationship_WhenExistRelationshipIsMine_ReturnNoContent()
        {
            //Arrange
            var accountId = Guid.NewGuid();
            var relationshipId = Guid.NewGuid();
            using var scope = this.webHost.Services.CreateScope();
            var appDBContext = scope.ServiceProvider.GetRequiredService<AppDBContext>();
            var notificationRep = new NotificationRepository(appDBContext);
            var relationshipRep = new RelationshipsRepository(appDBContext);

            var addedRelation = await relationshipRep.AddAsync(new Relationship()
            {
                Id = relationshipId,
                AccountMasterId = Guid.NewGuid(),
                AccountSlaveId = accountId,
                Status = RelationshipStatus.Subscription
            });
            await relationshipRep.SaveAsync();
            scope.Dispose();

            var webHost = CustomTestHostBuilder.Build(TestContainerBuilder.npgsqlUser, TestContainerBuilder.npgsqlPassword
                , "localhost", pgContainer.GetMappedPublicPort(5432), TestContainerBuilder.npgsqlDB, false);
            using var scopeForAct = webHost.Services.CreateScope();

            appDBContext = scopeForAct.ServiceProvider.GetRequiredService<AppDBContext>();
            relationshipRep = new RelationshipsRepository(appDBContext);

            var relationServ = new RelationshipService(relationshipRep, StandartMockBuilder.mockLoggerRelatServ);
            var controller = new RelationshipController(relationServ, StandartMockBuilder.mockLoggerRelationController);

            //Act
            var noContentResult = await controller.DeleteRelationship(accountId, relationshipId) as NoContentResult;

            //Assert
            noContentResult.Should().NotBeNull();
        }

        [Fact]
        public async Task DeleteRelationship_WhenExistRelationshipIsNotMine_ReturnForbid()
        {
            //Arrange
            var relationshipId = Guid.NewGuid();
            using var scope = this.webHost.Services.CreateScope();
            var appDBContext = scope.ServiceProvider.GetRequiredService<AppDBContext>();
            var notificationRep = new NotificationRepository(appDBContext);
            var relationshipRep = new RelationshipsRepository(appDBContext);

            var addedRelation = await relationshipRep.AddAsync(new Relationship()
            {
                Id = relationshipId,
                AccountMasterId = Guid.NewGuid(),
                AccountSlaveId = Guid.NewGuid(),
                Status = RelationshipStatus.Subscription
            });
            await relationshipRep.SaveAsync();
            scope.Dispose();

            var webHost = CustomTestHostBuilder.Build(TestContainerBuilder.npgsqlUser, TestContainerBuilder.npgsqlPassword
                , "localhost", pgContainer.GetMappedPublicPort(5432), TestContainerBuilder.npgsqlDB, false);
            using var scopeForAct = webHost.Services.CreateScope();

            appDBContext = scopeForAct.ServiceProvider.GetRequiredService<AppDBContext>();
            relationshipRep = new RelationshipsRepository(appDBContext);

            var relationServ = new RelationshipService(relationshipRep, StandartMockBuilder.mockLoggerRelatServ);
            var controller = new RelationshipController(relationServ, StandartMockBuilder.mockLoggerRelationController);

            //Act
            var forbidResult = await controller.DeleteRelationship(Guid.NewGuid(), relationshipId) as ForbidResult;

            //Assert
            forbidResult.Should().NotBeNull();
        }

        [Fact]
        public async Task DeleteRelationship_WhenNotExistRelationshipIsNotMine_ReturnNotFound()
        {
            //Arrange
            using var scope = this.webHost.Services.CreateScope();
            var appDBContext = scope.ServiceProvider.GetRequiredService<AppDBContext>();
            var notificationRep = new NotificationRepository(appDBContext);
            var relationshipRep = new RelationshipsRepository(appDBContext);

            var addedRelation = await relationshipRep.AddAsync(new Relationship()
            {
                Id = Guid.NewGuid(),
                AccountMasterId = Guid.NewGuid(),
                AccountSlaveId = Guid.NewGuid(),
                Status = RelationshipStatus.Subscription
            });
            await relationshipRep.SaveAsync();
            scope.Dispose();

            var webHost = CustomTestHostBuilder.Build(TestContainerBuilder.npgsqlUser, TestContainerBuilder.npgsqlPassword
                , "localhost", pgContainer.GetMappedPublicPort(5432), TestContainerBuilder.npgsqlDB, false);
            using var scopeForAct = webHost.Services.CreateScope();

            appDBContext = scopeForAct.ServiceProvider.GetRequiredService<AppDBContext>();
            relationshipRep = new RelationshipsRepository(appDBContext);

            var relationServ = new RelationshipService(relationshipRep, StandartMockBuilder.mockLoggerRelatServ);
            var controller = new RelationshipController(relationServ, StandartMockBuilder.mockLoggerRelationController);

            //Act
            var forbidResult = await controller.DeleteRelationship(Guid.NewGuid(), Guid.NewGuid()) as ForbidResult;

            //Assert
            forbidResult.Should().NotBeNull();
        }

        [Fact]
        public async Task DeleteRelationship_WhenExistRelationship_ReturnNoContent()
        {
            //Arrange
            var relationshipId = Guid.NewGuid();
            using var scope = this.webHost.Services.CreateScope();
            var appDBContext = scope.ServiceProvider.GetRequiredService<AppDBContext>();
            var notificationRep = new NotificationRepository(appDBContext);
            var relationshipRep = new RelationshipsRepository(appDBContext);

            var addedRelation = await relationshipRep.AddAsync(new Relationship()
            {
                Id = relationshipId,
                AccountMasterId = Guid.NewGuid(),
                AccountSlaveId = Guid.NewGuid(),
                Status = RelationshipStatus.Subscription
            });
            await relationshipRep.SaveAsync();
            scope.Dispose();

            var webHost = CustomTestHostBuilder.Build(TestContainerBuilder.npgsqlUser, TestContainerBuilder.npgsqlPassword
                , "localhost", pgContainer.GetMappedPublicPort(5432), TestContainerBuilder.npgsqlDB, false);
            using var scopeForAct = webHost.Services.CreateScope();

            appDBContext = scopeForAct.ServiceProvider.GetRequiredService<AppDBContext>();
            relationshipRep = new RelationshipsRepository(appDBContext);

            var relationServ = new RelationshipService(relationshipRep, StandartMockBuilder.mockLoggerRelatServ);
            var controller = new RelationshipController(relationServ, StandartMockBuilder.mockLoggerRelationController);

            //Act
            var noContentResult = await controller.DeleteRelationship(relationshipId) as NoContentResult;

            //Assert
            noContentResult.Should().NotBeNull();
        }

        [Fact]
        public async Task DeleteRelationship_WhenNotExistRelationship_ReturnNotFound()
        {
            //Arrange
            using var scope = this.webHost.Services.CreateScope();
            var appDBContext = scope.ServiceProvider.GetRequiredService<AppDBContext>();
            var notificationRep = new NotificationRepository(appDBContext);
            var relationshipRep = new RelationshipsRepository(appDBContext);

            var addedRelation = await relationshipRep.AddAsync(new Relationship()
            {
                Id = Guid.NewGuid(),
                AccountMasterId = Guid.NewGuid(),
                AccountSlaveId = Guid.NewGuid(),
                Status = RelationshipStatus.Subscription
            });
            await relationshipRep.SaveAsync();
            scope.Dispose();

            var webHost = CustomTestHostBuilder.Build(TestContainerBuilder.npgsqlUser, TestContainerBuilder.npgsqlPassword
                , "localhost", pgContainer.GetMappedPublicPort(5432), TestContainerBuilder.npgsqlDB, false);
            using var scopeForAct = webHost.Services.CreateScope();

            appDBContext = scopeForAct.ServiceProvider.GetRequiredService<AppDBContext>();
            relationshipRep = new RelationshipsRepository(appDBContext);

            var relationServ = new RelationshipService(relationshipRep, StandartMockBuilder.mockLoggerRelatServ);
            var controller = new RelationshipController(relationServ, StandartMockBuilder.mockLoggerRelationController);

            //Act
            var notFoundResult = await controller.DeleteRelationship(Guid.NewGuid()) as NotFoundResult;

            //Assert
            notFoundResult.Should().NotBeNull();
        }
    }
}
