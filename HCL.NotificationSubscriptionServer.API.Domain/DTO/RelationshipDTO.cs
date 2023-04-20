using HCL.NotificationSubscriptionServer.API.Domain.Enums;
using System.ComponentModel.DataAnnotations;

namespace HCL.NotificationSubscriptionServer.API.Domain.DTO
{
    public class RelationshipDTO
    {
        [Required(ErrorMessage = "Need AccountMasterId")]
        public Guid AccountMasterId { get; set; }

        [Required(ErrorMessage = "Need AccountSlaveId")]
        public Guid AccountSlaveId { get; set; }

        [Required(ErrorMessage = "Need Status")]
        public RelationshipStatus Status { get; set; }
    }
}