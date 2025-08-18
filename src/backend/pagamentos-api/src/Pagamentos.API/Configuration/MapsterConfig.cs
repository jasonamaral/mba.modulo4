using Mapster;

namespace Pagamentos.API.Configuration
{
    public static class MapsterConfig
    {
        public static WebApplicationBuilder AddMapsterConfiguration(this WebApplicationBuilder builder)
        {
            TypeAdapterConfig.GlobalSettings.Scan(typeof(Program).Assembly);
            return builder;
        }
    }
}
