using HCL.NotificationSubscriptionServer.API.BLL.Services;
using HCL.NotificationSubscriptionServer.API.Controllers;
using HCL.NotificationSubscriptionServer.API.DAL;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace HCL.NotificationSubscriptionServer.API.Test.IntegrationTest
{
    public class CustomTestHostBuilder
    {
        public static WebApplicationFactory<Program> Build(string dbUser, string dbPassword, string dbServer, ushort dbPort, string dbName,bool rebuildDb=true)
        {
            var npgsqlConnectionString = $"User Id={dbUser}; Password={dbPassword}; Server={dbServer}; " +
                $"Port={dbPort}; Database={dbName}; IntegratedSecurity=true; Pooling=true";

            return new WebApplicationFactory<Program>().WithWebHostBuilder(builder =>
            {
                builder.ConfigureTestServices(async services =>
                {
                    var dbContextDescriptor = services.SingleOrDefault(d =>
                             d.ServiceType == typeof(DbContextOptions<AppDBContext>));

                    services.Remove(dbContextDescriptor);

                    services.AddDbContext<AppDBContext>(options =>
                    {
                        options.UseNpgsql(npgsqlConnectionString);
                    });

                    if (rebuildDb)
                    {
                        var serviceProvider = services.BuildServiceProvider();
                        using var scope = serviceProvider.CreateScope();
                        var scopedServices = scope.ServiceProvider;
                        var context = scopedServices.GetRequiredService<AppDBContext>();
                        context.Database.EnsureDeleted();
                        context.Database.EnsureCreated();
                    }
                });
            });
        }
    }
}
