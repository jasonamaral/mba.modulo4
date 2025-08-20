namespace Pagamentos.API.Configuration
{
    public static class MediatRConfig
    {
        public static WebApplicationBuilder AddMediatrConfig(this WebApplicationBuilder builder)
        {
            builder.Services.AddMediatR(cfg =>
                cfg.RegisterServicesFromAssemblies(AppDomain.CurrentDomain.GetAssemblies())
            );

            //builder.Services.AddMediatR(cfg =>
            //                cfg.RegisterServicesFromAssemblies(
            //                    typeof(Program).Assembly,
            //                    typeof(Pagamentos.Domain.Events.PagamentoEventHandler).Assembly
            //                ));

            return builder;
        }
    }
}
