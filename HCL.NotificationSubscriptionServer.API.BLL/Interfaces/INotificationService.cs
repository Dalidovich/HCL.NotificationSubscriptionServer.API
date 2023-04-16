using HCL.NotificationSubscriptionServer.API.Domain.DTO;
using HCL.NotificationSubscriptionServer.API.Domain.Entities;
using HCL.NotificationSubscriptionServer.API.Domain.InnerResponse;

namespace HCL.NotificationSubscriptionServer.API.BLL.Interfaces
{
    public interface INotificationService
    {
        public Task<BaseResponse<Notification>> CreateNotification(Notification DTO);
        public Task<BaseResponse<bool>> DeleteNotification(Guid id);
        public BaseResponse<IQueryable<Notification>> GetNotificationOData();
    }
}