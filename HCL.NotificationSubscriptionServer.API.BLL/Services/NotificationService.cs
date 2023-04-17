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
            try
            {
                var notifications = _relationshipService.GetRelationshipOData().Data
                    .Where(x => x.AccountMasterId == accountId || x.Status == RelationshipStatus.Subscription)
                    .Select(x => new Notification()
                    {
                        ArticleId = articleId,
                        RelationshipId = (Guid)x.Id
                    });
                await _notificationRepositories.AddRangeAsync(notifications);

                return new StandartResponse<bool>()
                {
                    Data = await _notificationRepositories.SaveAsync(),
                    StatusCode = StatusCode.NotificationCreate
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"[CreateNotification] : {ex.Message}");

                return new StandartResponse<bool>()
                {
                    Message = ex.Message,
                    StatusCode = StatusCode.InternalServerError,
                };
            }
        }

        public async Task<BaseResponse<bool>> DeleteNotification(Guid id)
        {
            try
            {
                _notificationRepositories.Delete(new Notification(id));

                return new StandartResponse<bool>()
                {
                    Data = await _notificationRepositories.SaveAsync(),
                    StatusCode = StatusCode.NotificationDelete
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"[DeleteNotification] : {ex.Message}");

                return new StandartResponse<bool>()
                {
                    Message = ex.Message,
                    StatusCode = StatusCode.InternalServerError,
                };
            }
        }

        public BaseResponse<IQueryable<Notification>> GetNotificationOData()
        {
            try
            {
                var contents =_notificationRepositories.GetAsync();
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
            catch (Exception ex)
            {
                _logger.LogError(ex, $"[GetNotificationOData] : {ex.Message}");

                return new StandartResponse<IQueryable<Notification>>()
                {
                    Message = ex.Message,
                    StatusCode = StatusCode.InternalServerError,
                };
            }
        }
    }
}
