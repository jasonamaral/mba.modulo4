namespace Pagamentos.API.Configuration
{
    public static class MediatRConfig
    {
        public static WebApplicationBuilder AddMediatRConfig(this WebApplicationBuilder builder)
        {
            builder.Services.AddMediatR(cfg =>
                cfg.RegisterServicesFromAssemblies(AppDomain.CurrentDomain.GetAssemblies())
            );

            return builder;
        }
    }
}
