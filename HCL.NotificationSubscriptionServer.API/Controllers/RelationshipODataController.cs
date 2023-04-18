using HCL.NotificationSubscriptionServer.API.BLL.Interfaces;
using HCL.NotificationSubscriptionServer.API.Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Routing.Controllers;

namespace HCL.NotificationSubscriptionServer.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RelationshipODataController : ODataController
    {
        private readonly ILogger<RelationshipODataController> _logger;
        private readonly IRelationshipService _relationshipService;

        public RelationshipODataController(ILogger<RelationshipODataController> logger, IRelationshipService relationshipService)
        {
            _logger = logger;
            _relationshipService = relationshipService;
        }

        [HttpGet("odata/v1/Relationship")]
        [EnableQuery]
        public IQueryable<Relationship> GetRelationship()
        {
            return _relationshipService.GetRelationshipOData().Data;
        }
    }
}
