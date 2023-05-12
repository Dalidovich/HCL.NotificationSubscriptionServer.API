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
    public class NotificationService : INotificationService
    {
        private readonly INotificationRepositories _notificationRepositories;
        private readonly IRelationshipService _relationshipService;
        private readonly ILogger<NotificationService> _logger;

        public NotificationService(INotificationRepositories notificationRepositories, IRelationshipService relationshipService
            , ILogger<NotificationService> logger)
        {
            _notificationRepositories = notificationRepositories;
            _relationshipService = relationshipService;
            _logger = logger;
        }

        public async Task<BaseResponse<bool>> CreateNotification(string articleId, Guid accountId)
        {
            var log = new LogDTOBuidlder("CreateNotification()");
            var notifications = _relationshipService.GetRelationshipOData().Data
                    ?.Where(x => x.AccountMasterId == accountId && x.Status == RelationshipStatus.Subscription)
                    ?.Select(x => new Notification()
                    {
                        ArticleId = articleId,
                        RelationshipId = (Guid)x.Id
                    });

            if (notifications != null)
            {
                await _notificationRepositories.AddRangeAsync(notifications);
                log.BuildMessage("create notification");
                _logger.LogInformation(JsonSerializer.Serialize(log.Build()));

                return new StandartResponse<bool>()
                {
                    Data = await _notificationRepositories.SaveAsync(),
                    StatusCode = StatusCode.NotificationCreate
                };
            }
            log.BuildMessage("no relation for create notification");
            _logger.LogInformation(JsonSerializer.Serialize(log.Build()));

            return new StandartResponse<bool>()
            {
                Data = false,
                StatusCode = StatusCode.NotificationCreate
            };
        }

        public async Task<BaseResponse<bool>> DeleteNotification(Guid id)
        {
            _notificationRepositories.Delete(new Notification(id));

            var log = new LogDTOBuidlder("DeleteNotification(id)")
                .BuildMessage("delete notification")
                .BuildSuccessState(true)
                .Build();
            _logger.LogInformation(JsonSerializer.Serialize(log));

            return new StandartResponse<bool>()
            {
                Data = await _notificationRepositories.SaveAsync(),
                StatusCode = StatusCode.NotificationDelete
            };
        }

        public BaseResponse<IQueryable<Notification>> GetNotificationOData()
        {
            var log = new LogDTOBuidlder("GetNotificationOData()");
            var contents = _notificationRepositories.GetAsync();
            if (contents.Count() == 0)
            {
                log.BuildMessage("no notification");
                _logger.LogInformation(JsonSerializer.Serialize(log.Build()));

                return new StandartResponse<IQueryable<Notification>>()
                {
                    Message = "entity not found"
                };
            }

            log.BuildMessage("get notification");
            _logger.LogInformation(JsonSerializer.Serialize(log.Build()));

            return new StandartResponse<IQueryable<Notification>>()
            {
                Data = contents,
                StatusCode = StatusCode.NotificationRead
            };
        }
    }
}