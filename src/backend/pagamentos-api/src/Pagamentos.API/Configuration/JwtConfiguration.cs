using Core.Identidade;

namespace Pagamentos.API.Configuration
{
    public static class JwtConfiguration
    {
        public static WebApplicationBuilder AddJwtConfiguration(this WebApplicationBuilder builder)
        {
            builder.Services.AddJwtConfiguration(builder.Configuration);
            return builder;
        }
    }
}
