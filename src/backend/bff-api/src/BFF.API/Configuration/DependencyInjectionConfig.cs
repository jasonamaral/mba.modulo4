using BFF.API.Services;
using BFF.Application.Interfaces.Services;

namespace BFF.API.Configuration
{
    public static class DependencyInjectionConfig
    {
        public static void RegisterServices(this IServiceCollection services)
        {
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

            // Serviços de API
            services.AddScoped<IConteudoService, ConteudoService>();
            
            // Serviços de infraestrutura
            services.AddScoped<ICacheService, Infrastructure.Services.CacheService>();
            services.AddScoped<IRestApiService, Infrastructure.Services.RestApiService>();
            services.AddScoped<IHttpClientService, Infrastructure.Services.HttpClientService>();
            services.AddScoped<IDashboardService, Infrastructure.Services.DashboardService>();
        }
    }
}
