using HCL.NotificationSubscriptionServer.API.BLL.Interfaces;
using HCL.NotificationSubscriptionServer.API.Domain.DTO;
using HCL.NotificationSubscriptionServer.API.Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HCL.NotificationSubscriptionServer.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RelationshipController : ControllerBase
    {
        private readonly IRelationshipService _relationshipService;

        public RelationshipController(IRelationshipService relationshipService)
        {
            _relationshipService = relationshipService;
        }

        [Authorize]
        [HttpPost("v1/Relationship")]
        public async Task<IActionResult> CreateRelationship([FromQuery] RelationshipDTO articleDTO)
        {
            var resourse = await _relationshipService.CreateRelationship(new Relationship(articleDTO));
            if (resourse.Data != null)
            {

                return Created("", new { articleId = resourse.Data.Id });
            }

            return NotFound();
        }

        [Authorize]
        [HttpDelete("v1/OwnRelationship")]
        public async Task<IActionResult> DeleteRelationship([FromQuery] Guid ownId, [FromQuery] Guid id)
        {
            var relation=await _relationshipService.GetRelationshipOData().Data
                ?.Where(x => x.Id == id)
                .SingleOrDefaultAsync();
            if (relation == null)
            {

                return NotFound();
            }
            else if (relation.AccountSlaveId == ownId)
            {
                var resourse = await _relationshipService.DeleteRelationship(id);

                return NoContent();
            }

            return Forbid();
        }

        [Authorize(Roles = "admin")]
        [HttpDelete("v1/Relationship")]
        public async Task<IActionResult> DeleteRelationship([FromQuery] Guid id)
        {
            var relation = await _relationshipService.GetRelationshipOData().Data
                ?.Where(x => x.Id == id)
                .SingleOrDefaultAsync();
            if (relation == null)
            {

                return NotFound();
            }
            var resourse = await _relationshipService.DeleteRelationship(id);

            return NoContent();
        }
    }
}