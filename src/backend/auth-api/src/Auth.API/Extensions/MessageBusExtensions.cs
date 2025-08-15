using Core.Utils;
using MessageBus;

namespace Auth.API.Extensions;

public static class MessageBusExtensions
{
    public static void AddMessageBusConfiguration(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddMessageBus(configuration?.GetMessageQueueConnection("MessageBus")!);
    }
}