using HCL.NotificationSubscriptionServer.API.BLL.Interfaces;
using HCL.NotificationSubscriptionServer.API.DAL.Repositories.Interfaces;
using HCL.NotificationSubscriptionServer.API.Domain.DTO.Builders;
using HCL.NotificationSubscriptionServer.API.Domain.Entities;
using HCL.NotificationSubscriptionServer.API.Domain.Enums;
using HCL.NotificationSubscriptionServer.API.Domain.InnerResponse;
using Microsoft.Extensions.Logging;
using System.Text.Json;

namespace HCL.NotificationSubscriptionServer.API.BLL.Services
{
    public class RelationshipService : IRelationshipService
    {
        private readonly IRelationshipRepository _relationshipRepositories;
        private readonly ILogger<RelationshipService> _logger;

        public RelationshipService(IRelationshipRepository relationshipRepositories, ILogger<RelationshipService> logger)
        {
            _relationshipRepositories = relationshipRepositories;
            _logger = logger;
        }

        public async Task<BaseResponse<Relationship>> CreateRelationship(Relationship relationship)
        {
            var createdRelationship = await _relationshipRepositories.AddAsync(relationship);
            await _relationshipRepositories.SaveAsync();

            var log = new LogDTOBuidlder("CreateRelationship(relationship)")
            .BuildMessage("create relationship")
            .BuildSuccessState(true)
            .Build();
            _logger.LogInformation(JsonSerializer.Serialize(log));

            return new StandartResponse<Relationship>()
            {
                Data = createdRelationship,
                StatusCode = StatusCode.SubscriptionCreate
            };
        }

        public async Task<BaseResponse<bool>> DeleteRelationship(Guid id)
        {
            _relationshipRepositories.Delete(new Relationship(id));

            var log = new LogDTOBuidlder("DeleteRelationship(id)")
            .BuildMessage("delete relationship")
            .BuildSuccessState(true)
            .Build();
            _logger.LogInformation(JsonSerializer.Serialize(log));

            return new StandartResponse<bool>()
            {
                Data = await _relationshipRepositories.SaveAsync(),
                StatusCode = StatusCode.SubscriptionDelete
            };
        }

        public BaseResponse<IQueryable<Relationship>> GetRelationshipOData()
        {
            var log = new LogDTOBuidlder("GetRelationshipOData()");
            var contents = _relationshipRepositories.GetAsync();
            if (contents.Count() == 0)
            {
                log.BuildMessage("no relationship");
                _logger.LogInformation(JsonSerializer.Serialize(log.Build()));

                return new StandartResponse<IQueryable<Relationship>>()
                {
                    Message = "entity not found"
                };
            }

            log.BuildMessage("get relationship");
            _logger.LogInformation(JsonSerializer.Serialize(log.Build()));

            return new StandartResponse<IQueryable<Relationship>>()
            {
                Data = contents,
                StatusCode = StatusCode.SubscriptionRead
            };
        }
    }
}