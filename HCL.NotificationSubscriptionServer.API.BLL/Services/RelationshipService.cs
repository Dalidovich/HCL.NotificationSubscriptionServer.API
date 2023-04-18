using HCL.NotificationSubscriptionServer.API.BLL.Interfaces;
using HCL.NotificationSubscriptionServer.API.DAL.Repositories.Interfaces;
using HCL.NotificationSubscriptionServer.API.Domain.Entities;
using HCL.NotificationSubscriptionServer.API.Domain.Enums;
using HCL.NotificationSubscriptionServer.API.Domain.InnerResponse;

namespace HCL.NotificationSubscriptionServer.API.BLL.Services
{
    public class RelationshipService : IRelationshipService
    {
        private readonly IRelationshipRepositories _relationshipRepositories;

        public RelationshipService(IRelationshipRepositories relationshipRepositories)
        {
            _relationshipRepositories = relationshipRepositories;
        }

        public async Task<BaseResponse<Relationship>> CreateRelationship(Relationship relationship)
        {
            var createdRelationship = await _relationshipRepositories.AddAsync(relationship);
            await _relationshipRepositories.SaveAsync();

            return new StandartResponse<Relationship>()
            {
                Data = createdRelationship,
                StatusCode = StatusCode.NotificationCreate
            };
        }

        public async Task<BaseResponse<bool>> DeleteRelationship(Guid id)
        {
            _relationshipRepositories.Delete(new Relationship(id));

            return new StandartResponse<bool>()
            {
                Data = await _relationshipRepositories.SaveAsync(),
                StatusCode = StatusCode.NotificationDelete
            };
        }

        public BaseResponse<IQueryable<Relationship>> GetRelationshipOData()
        {
            var contents = _relationshipRepositories.GetAsync();
            if (contents.Count() == 0)
            {

                return new StandartResponse<IQueryable<Relationship>>()
                {
                    Message = "entity not found"
                };
            }

            return new StandartResponse<IQueryable<Relationship>>()
            {
                Data = contents,
                StatusCode = StatusCode.NotificationRead
            };
        }
    }
}