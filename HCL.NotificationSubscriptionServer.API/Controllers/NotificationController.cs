using HCL.NotificationSubscriptionServer.API.BLL.Interfaces;
using HCL.NotificationSubscriptionServer.API.Domain.DTO.Builders;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Data;
using System.Text.Json;

namespace HCL.NotificationSubscriptionServer.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class NotificationController : ControllerBase
    {
        private readonly INotificationService _notificationService;
        private readonly ILogger<NotificationController> _logger;

        public NotificationController(INotificationService notificationService, ILogger<NotificationController> logger)
        {
            _notificationService = notificationService;
            _logger = logger;
        }

        [Authorize]
        [HttpDelete("v1/relationship/account")]
        public async Task<IActionResult> DeleteNotification([FromQuery] Guid ownId, [FromQuery] Guid id)
        {
            var response = _notificationService.GetNotificationOData();
            if (response.Data == null)
            {

                return NotFound();
            }
            var notification = await response.Data
                ?.Where(x => x.Id == id)
                .Include(x => x.Relationship)
                .AsNoTracking()
                .SingleOrDefaultAsync();
            if (notification != null && notification.Relationship.AccountSlaveId == ownId)
            {
                var resourse = await _notificationService.DeleteNotification((Guid)notification.Id);
                var log = new LogDTOBuidlder("DeleteNotification(ownId,id)")
                .BuildMessage("authenticated account delete own notification")
                .BuildSuccessState(resourse.Data)
                .BuildStatusCode(204)
                .Build();
                _logger.LogInformation(JsonSerializer.Serialize(log));

                return NoContent();
            }

            return Forbid();
        }

        [Authorize(Roles = "admin")]
        [HttpDelete("v1/relationship/admin")]
        public async Task<IActionResult> DeleteNotification([FromQuery] Guid id)
        {
            var response = _notificationService.GetNotificationOData();
            if (response.Data == null)
            {

                return NotFound();
            }
            var notification = await response.Data
                ?.Where(x => x.Id == id)
                .AsNoTracking()
                .SingleOrDefaultAsync();
            if (notification != null)
            {
                var resourse = await _notificationService.DeleteNotification((Guid)notification.Id);
                var log = new LogDTOBuidlder("DeleteNotification(id)")
                    .BuildMessage("admin account delete notification")
                    .BuildSuccessState(resourse.Data)
                    .BuildStatusCode(204)
                    .Build();
                _logger.LogInformation(JsonSerializer.Serialize(log));

                return NoContent();
            }

            return NotFound();
        }
    }
}