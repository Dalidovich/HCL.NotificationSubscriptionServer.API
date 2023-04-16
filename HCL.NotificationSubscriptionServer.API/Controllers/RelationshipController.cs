using HCL.NotificationSubscriptionServer.API.BLL.Interfaces;
using HCL.NotificationSubscriptionServer.API.Domain.DTO;
using HCL.NotificationSubscriptionServer.API.Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;

namespace HCL.NotificationSubscriptionServer.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RelationshipController : ControllerBase
    {
        private readonly ILogger<RelationshipController> _logger;
        private readonly IRelationshipService _relationshipService;

        public RelationshipController(ILogger<RelationshipController> logger, IRelationshipService relationshipService)
        {
            _logger = logger;
            _relationshipService = relationshipService;
        }


        [Authorize]
        [HttpPost("v1/Relationship")]
        public async Task<IActionResult> CreateRelationship([FromQuery] RelationshipDTO articleDTO)
        {
            if (ModelState.IsValid)
            {
                var resourse = await _relationshipService.CreateRelationship(new Relationship(articleDTO));
                if (resourse.Data != null)
                {
                    return Created("", new { articleId = resourse.Data.Id });
                }

                return NotFound();
            }

            return BadRequest();
        }

        [Authorize]
        [HttpPost("v1/Relationship")]
        public async Task<IActionResult> DeleteRelationship([FromQuery] Guid ownId, [FromQuery] Guid id)
        {
            var relation=await _relationshipService.GetRelationshipOData().Data.Where(x => x.Id == id).SingleOrDefaultAsync();
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
            var relation = await _relationshipService.GetRelationshipOData().Data.Where(x => x.Id == id).SingleOrDefaultAsync();
            if (relation == null)
            {
                return NotFound();
            }
            var resourse = await _relationshipService.DeleteRelationship(id);

            return NoContent();
        }
    }
}
