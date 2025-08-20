using Core.Utils;
using MessageBus;

namespace Pagamentos.API.Configuration;

public static class MessageBusConfig
{
    public static WebApplicationBuilder AddMessageBusConfiguration(this WebApplicationBuilder builder, IConfiguration configuration)
    {
        builder.Services.AddMessageBus(configuration?.GetMessageQueueConnection("MessageBus")!);

        return builder;
    }
}
