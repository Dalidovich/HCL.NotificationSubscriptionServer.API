using HCL.NotificationSubscriptionServer.API.BLL.Interfaces;
using HCL.NotificationSubscriptionServer.API.BLL.Services;
using HCL.NotificationSubscriptionServer.API.Domain.DTO;
using HCL.NotificationSubscriptionServer.API.Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace HCL.NotificationSubscriptionServer.API.Controllers
{
    public class NotificationController : ControllerBase
    {
        private readonly ILogger<NotificationController> _logger;
        private readonly INotificationService _notificationService;

        public NotificationController(ILogger<NotificationController> logger, INotificationService notificationService)
        {
            _logger = logger;
            _notificationService = notificationService;
        }

        [Authorize]
        [HttpPost("v1/Relationship")]
        public async Task<IActionResult> DeleteNotification([FromQuery] Guid ownId, [FromQuery] Guid id)
        {
            var notification = await _notificationService.GetNotificationOData().Data.Where(x => x.Id == id).Include(x=>x.Relationship).SingleOrDefaultAsync();
            if (notification == null)
            {
                return NotFound();
            }
            else if (notification.Relationship.AccountSlaveId == ownId)
            {
                var resourse = await _notificationService.DeleteNotification(id);
                return NoContent();
            }
            return Forbid();
        }

        [Authorize(Roles = "admin")]
        [HttpDelete("v1/Relationship")]
        public async Task<IActionResult> DeleteRelationship([FromQuery] Guid id)
        {
            var notification = await _notificationService.GetNotificationOData().Data.Where(x => x.Id == id).SingleOrDefaultAsync();
            if (notification == null)
            {
                return NotFound();
            }
            var resourse = await _notificationService.DeleteNotification(id);

            return NoContent();
        }
    }
}
