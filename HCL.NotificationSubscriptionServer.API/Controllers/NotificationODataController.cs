using HCL.NotificationSubscriptionServer.API.BLL.Interfaces;
using HCL.NotificationSubscriptionServer.API.Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Routing.Controllers;

namespace HCL.NotificationSubscriptionServer.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class NotificationODataController : ODataController
    {
        private readonly INotificationService _notificationService;

        public NotificationODataController(INotificationService notificationService)
        {
            _notificationService = notificationService;
        }

        [HttpGet("odata/v1/Notification")]
        [EnableQuery]
        public IQueryable<Notification> GetNotification()
        {

            return _notificationService.GetNotificationOData().Data;
        }
    }
}