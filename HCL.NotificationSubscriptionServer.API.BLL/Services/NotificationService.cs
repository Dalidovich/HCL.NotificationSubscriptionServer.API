using HCL.NotificationSubscriptionServer.API.BLL.Interfaces;
using HCL.NotificationSubscriptionServer.API.DAL.Repositories.Interfaces;
using HCL.NotificationSubscriptionServer.API.Domain.DTO;
using HCL.NotificationSubscriptionServer.API.Domain.Entities;
using HCL.NotificationSubscriptionServer.API.Domain.Enums;
using HCL.NotificationSubscriptionServer.API.Domain.InnerResponse;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace HCL.NotificationSubscriptionServer.API.BLL.Services
{
    public class NotificationService : INotificationService
    {
        private readonly INotificationRepositories _notificationRepositories;
        protected readonly ILogger<INotificationService> _logger;

        public NotificationService(INotificationRepositories notificationRepositories, ILogger<INotificationService> logger)
        {
            _notificationRepositories = notificationRepositories;
            _logger = logger;
        }

        public async Task<BaseResponse<Notification>> CreateNotification(Notification notification)
        {
            try
            {
                var createdAccount = await _notificationRepositories.AddAsync(notification);
                await _notificationRepositories.SaveAsync();

                return new StandartResponse<Notification>()
                {
                    Data = createdAccount,
                    StatusCode = StatusCode.NotificationCreate
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"[CreateNotification] : {ex.Message}");

                return new StandartResponse<Notification>()
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
