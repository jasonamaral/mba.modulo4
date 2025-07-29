using Core.Notification;
using Microsoft.Extensions.DependencyInjection;

namespace Core.Utils
{
    public static class NotificationExtension
    {
        public static IServiceCollection RegisterNotification(this IServiceCollection services)
        {
            services.AddScoped<INotificador, Notificador>();
            return services;
        }
    }
}
