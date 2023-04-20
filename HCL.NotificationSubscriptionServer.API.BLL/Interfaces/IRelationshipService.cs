using HCL.NotificationSubscriptionServer.API.Domain.Entities;
using HCL.NotificationSubscriptionServer.API.Domain.InnerResponse;

namespace HCL.NotificationSubscriptionServer.API.BLL.Interfaces
{
    public interface IRelationshipService
    {
        public Task<BaseResponse<Relationship>> CreateRelationship(Relationship DTO);
        public Task<BaseResponse<bool>> DeleteRelationship(Guid id);
        public BaseResponse<IQueryable<Relationship>> GetRelationshipOData();
    }
}