namespace HCL.NotificationSubscriptionServer.API.Domain.Enums
{
    public enum StatusCode
    {
        EntityNotFound = 0,

        SubscriptionCreate = 1,
        SubscriptionDelete = 2,
        SubscriptionRead = 3,

        NotificationCreate = 11,
        NotificationDelete = 12,
        NotificationRead = 13,

        OK = 200,
        OKNoContent = 204,
        InternalServerError = 500,
    }
}
