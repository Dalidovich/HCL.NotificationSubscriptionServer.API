using HCL.NotificationSubscriptionServer.API.Domain.Enums;

namespace HCL.NotificationSubscriptionServer.API.Domain.InnerResponse
{
    public class StandartResponse<T> : BaseResponse<T>
    {
        public override string Message { get; set; } = null!;
        public override StatusCode StatusCode { get; set; }
        public override T Data { get; set; }
    }
}
