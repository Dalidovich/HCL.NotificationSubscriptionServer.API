using HCL.NotificationSubscriptionServer.API.BackgroundHostedServices;
using HCL.NotificationSubscriptionServer.API.BLL.Interfaces;
using HCL.NotificationSubscriptionServer.API.BLL.Services;
using HCL.NotificationSubscriptionServer.API.DAL.Repositories;
using HCL.NotificationSubscriptionServer.API.DAL.Repositories.Interfaces;
using HCL.NotificationSubscriptionServer.API.Domain.DTO;
using HCL.NotificationSubscriptionServer.API.Domain.Entities;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.OData;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OData.ModelBuilder;
using System.Text;

namespace HCL.NotificationSubscriptionServer.API
{
    public static class DIManger
    {
        public static void AddRepositores(this WebApplicationBuilder webApplicationBuilder)
        {
            webApplicationBuilder.Services.AddScoped<INotificationRepositories, NotificationRepositories>();
            webApplicationBuilder.Services.AddScoped<IRelationshipRepositories, RelationshipsRepositories>();
        }

        public static void AddServices(this WebApplicationBuilder webApplicationBuilder)
        {
            webApplicationBuilder.Services.AddScoped<INotificationService, NotificationService>();
            webApplicationBuilder.Services.AddScoped<IRelationshipService, RelationshipService>();
            webApplicationBuilder.Services.AddScoped<IKafkaConsumerService, KafkaConsumerService>();
        }

        public static void AddODataProperty(this WebApplicationBuilder webApplicationBuilder)
        {
            var odataBuilder = new ODataConventionModelBuilder();
            odataBuilder.EntitySet<Notification>("Notification");
            odataBuilder.EntitySet<Relationship>("Relationship");

            webApplicationBuilder.Services.AddControllers().AddOData(opt =>
            {
                opt.Count().Filter().Expand().Select().OrderBy().SetMaxTop(5000);
                opt.TimeZone = TimeZoneInfo.Utc;
            });
        }

        public static void AddKafkaProperty(this WebApplicationBuilder webApplicationBuilder)
        {
            webApplicationBuilder.Services.Configure<KafkaSettings>(webApplicationBuilder.Configuration.GetSection("KafkaSettings"));
            webApplicationBuilder.Services.AddSingleton(serviceProvider => serviceProvider.GetRequiredService<IOptions<KafkaSettings>>().Value);
        }

        public static void AddAuthProperty(this WebApplicationBuilder webApplicationBuilder)
        {
            var secretKey = webApplicationBuilder.Configuration.GetSection("JWTSettings:SecretKey").Value;
            var issuer = webApplicationBuilder.Configuration.GetSection("JWTSettings:Issuer").Value;
            var audience = webApplicationBuilder.Configuration.GetSection("JWTSettings:Audience").Value;
            var signingKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));

            webApplicationBuilder.Services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(options =>
            {

                options.RequireHttpsMetadata = true;
                options.SaveToken = true;
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidIssuer = issuer,
                    ValidateAudience = true,
                    ValidAudience = audience,
                    ValidateLifetime = true,
                    IssuerSigningKey = signingKey,
                    ValidateIssuerSigningKey = true,
                };
            });
        }

        public static void AddHostedServices(this WebApplicationBuilder webApplicationBuilder)
        {
            webApplicationBuilder.Services.AddHostedService<KafkaConsumerHostedService>();
            webApplicationBuilder.Services.AddHostedService<CheckDBHostedService>();
        }
    }
}