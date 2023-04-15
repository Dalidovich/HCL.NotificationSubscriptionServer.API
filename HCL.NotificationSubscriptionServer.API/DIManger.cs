namespace HCL.NotificationSubscriptionServer.API
{
    public static class DIManger
    {
        public static void AddRepositores(this WebApplicationBuilder webApplicationBuilder)
        {
            //webApplicationBuilder.Services.AddScoped<IAccountRepository, AccountRepository>();
        }

        public static void AddServices(this WebApplicationBuilder webApplicationBuilder)
        {
            //webApplicationBuilder.Services.AddScoped<IAccountService, AccountService>();
            //webApplicationBuilder.Services.AddScoped<IRegistrationService, RegistrationService>();
        }
    }
}
