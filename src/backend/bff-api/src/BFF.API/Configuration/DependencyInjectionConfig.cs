using BFF.API.Services;
using BFF.Application.Interfaces.Services;
using Core.Utils;

namespace BFF.API.Configuration
{
    public static class DependencyInjectionConfig
    {
        public static void RegisterServices(this IServiceCollection services)
        {
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

            services.AddScoped<IConteudoService, ConteudoService>();
            services.AddScoped<ICacheService, Infrastructure.Services.CacheService>();
            services.AddScoped<IHttpClientService, Infrastructure.Services.HttpClientService>();
            services.AddScoped<IDashboardService, Infrastructure.Services.DashboardService>();

            services.RegisterNotification();
        }
    }
}
