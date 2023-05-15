using HCL.NotificationSubscriptionServer.API.DAL;
using HCL.NotificationSubscriptionServer.API.Domain.Enums;
using HCL.NotificationSubscriptionServer.API.Middleware;
using Microsoft.EntityFrameworkCore;

namespace HCL.NotificationSubscriptionServer.API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.AddRepositores();
            builder.AddServices();
            builder.AddKafkaProperty();
            builder.AddODataProperty();
            builder.AddHostedServices();
            builder.AddAuthProperty();
            builder.AddElasticserchProperty();

            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            builder.Services.AddDbContext<AppDBContext>(opt => opt.UseNpgsql(
               builder.Configuration.GetConnectionString(StandartConst.NameConnection)));

            var app = builder.Build();

            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }
            app.UseMiddleware<ExceptionHandlingMiddleware>();

            app.UseHttpsRedirection();
            app.UseAuthorization();
            app.MapControllers();

            app.Run();
        }
    }
}