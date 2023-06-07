using HCL.NotificationSubscriptionServer.API.BLL.Interfaces;
using HCL.NotificationSubscriptionServer.API.Domain.DTO;
using HCL.NotificationSubscriptionServer.API.Domain.DTO.Builders;
using HCL.NotificationSubscriptionServer.API.Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;

namespace HCL.NotificationSubscriptionServer.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RelationshipController : ControllerBase
    {
        private readonly IRelationshipService _relationshipService;
        private readonly ILogger<RelationshipController> _logger;

        public RelationshipController(IRelationshipService relationshipService, ILogger<RelationshipController> logger)
        {
            _relationshipService = relationshipService;
            _logger = logger;
        }

        [Authorize]
        [HttpPost("v1/relationship")]
        public async Task<IActionResult> CreateRelationship([FromQuery] RelationshipDTO articleDTO)
        {
            var resourse = await _relationshipService.CreateRelationship(new Relationship(articleDTO));
            if (resourse.Data != null)
            {
                var log = new LogDTOBuidlder("CreateRelationship(articleDTO)")
                .BuildMessage("create relationship")
                .BuildSuccessState(resourse.Data != null)
                .BuildStatusCode(201)
                .Build();
                _logger.LogInformation(JsonSerializer.Serialize(log));

                return Created("", new { relationshipId = resourse.Data.Id });
            }

            return NotFound();
        }

        [Authorize]
        [HttpDelete("v1/relationship/account")]
        public async Task<IActionResult> DeleteRelationship([FromQuery] Guid ownId, [FromQuery] Guid id)
        {
            var response = _relationshipService.GetRelationshipOData();
            Relationship relation;
            if (response.Data == null)
            {

                return NotFound();
            }
            relation = await response.Data
                ?.Where(x => x.Id == id)
                .AsNoTracking()
                .SingleOrDefaultAsync();
            if (relation != null && relation.AccountSlaveId == ownId)
            {
                var resourse = await _relationshipService.DeleteRelationship((Guid)relation.Id);
                var log = new LogDTOBuidlder("DeleteRelationship(ownId,id)")
                .BuildMessage("authenticated account delete own relationship")
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
        public async Task<IActionResult> DeleteRelationship([FromQuery] Guid id)
        {
            var response = _relationshipService.GetRelationshipOData();
            if (response.Data == null)
            {

                return NotFound();
            }
            var relation = await response.Data
                ?.Where(x => x.Id == id)
                .AsNoTracking()
                .SingleOrDefaultAsync();
            if (relation != null)
            {
                var resourse = await _relationshipService.DeleteRelationship(id);
                var log = new LogDTOBuidlder("DeleteRelationship(id)")
                    .BuildMessage("admin account delete relationship")
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