using HCL.NotificationSubscriptionServer.API.BLL.Interfaces;
using HCL.NotificationSubscriptionServer.API.DAL.Repositories.Interfaces;
using HCL.NotificationSubscriptionServer.API.Domain.Entities;
using HCL.NotificationSubscriptionServer.API.Domain.Enums;
using HCL.NotificationSubscriptionServer.API.Domain.InnerResponse;
using Microsoft.Extensions.Logging;

namespace HCL.NotificationSubscriptionServer.API.BLL.Services
{
    public class NotificationService : INotificationService
    {
        private readonly INotificationRepositories _notificationRepositories;
        private readonly IRelationshipService _relationshipService;
        protected readonly ILogger<INotificationService> _logger;

        public NotificationService(INotificationRepositories notificationRepositories, ILogger<INotificationService> logger
            , IRelationshipService relationshipService)
        {
            _notificationRepositories = notificationRepositories;
            _logger = logger;
            _relationshipService = relationshipService;
        }

        public async Task<BaseResponse<bool>> CreateNotification(string articleId, Guid accountId)
        {
            var notifications = _relationshipService.GetRelationshipOData().Data
                    ?.Where(x => x.AccountMasterId == accountId || x.Status == RelationshipStatus.Subscription)
                    ?.Select(x => new Notification()
                    {
                        ArticleId = articleId,
                        RelationshipId = (Guid)x.Id
                    });
            if (notifications != null)
            {
                await _notificationRepositories.AddRangeAsync(notifications);

                return new StandartResponse<bool>()
                {
                    Data = await _notificationRepositories.SaveAsync(),
                    StatusCode = StatusCode.NotificationCreate
                };
            }

            return new StandartResponse<bool>()
            {
                Data = false,
                StatusCode = StatusCode.NotificationCreate
            };
        }

        public async Task<BaseResponse<bool>> DeleteNotification(Guid id)
        {
            _notificationRepositories.Delete(new Notification(id));

            return new StandartResponse<bool>()
            {
                Data = await _notificationRepositories.SaveAsync(),
                StatusCode = StatusCode.NotificationDelete
            };
        }

        public BaseResponse<IQueryable<Notification>> GetNotificationOData()
        {
            var contents = _notificationRepositories.GetAsync();
            if (contents.Count() == 0)
            {
                return new StandartResponse<IQueryable<Notification>>()
                {
                    Message = "entity not found"
                };
            }

            return new StandartResponse<IQueryable<Notification>>()
            {
                Data = contents,
                StatusCode = StatusCode.NotificationRead
            };
        }
    }
}
