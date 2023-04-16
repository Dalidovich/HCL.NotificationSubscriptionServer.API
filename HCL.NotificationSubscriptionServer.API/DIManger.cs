using HCL.NotificationSubscriptionServer.API.BLL.Interfaces;
using HCL.NotificationSubscriptionServer.API.BLL.Services;
using HCL.NotificationSubscriptionServer.API.DAL;
using HCL.NotificationSubscriptionServer.API.DAL.Repositories;
using HCL.NotificationSubscriptionServer.API.DAL.Repositories.Interfaces;
using HCL.NotificationSubscriptionServer.API.Domain.Entities;
using HCL.NotificationSubscriptionServer.API.Domain.Enums;
using Microsoft.AspNetCore.Http.Connections;
using Microsoft.AspNetCore.OData;
using Microsoft.Extensions.Configuration;
using Microsoft.OData.ModelBuilder;

namespace HCL.NotificationSubscriptionServer.API
{
    public static class DIManger
    {
        public static void AddRepositores(this WebApplicationBuilder webApplicationBuilder)
        {
            webApplicationBuilder.Services.AddScoped<INotificationRepositories, NotificationRepositories>();
            webApplicationBuilder.Services.AddTransient<IRelationshipRepositories, RelationshipsRepositories>();
        }

        public static void AddServices(this WebApplicationBuilder webApplicationBuilder)
        {
            webApplicationBuilder.Services.AddScoped<INotificationService, NotificationService>();
            webApplicationBuilder.Services.AddScoped<IRelationshipService, RelationshipService>();
        }
        public static void AddODataProperty(this WebApplicationBuilder webApplicationBuilder)
        {
            var odataBuilder = new ODataConventionModelBuilder();
            odataBuilder.EntitySet<Notification>("Notification");
            odataBuilder.EntitySet<Relationship>("Relationship");

            webApplicationBuilder.Services.AddControllers().AddOData(opt =>
            {
                opt.Count().Filter().Expand().Select().OrderBy().SetMaxTop(5000)
                    .AddRouteComponents("odata", odataBuilder.GetEdmModel());
                opt.TimeZone = TimeZoneInfo.Utc;
            });
        }

    }
}
