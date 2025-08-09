using Core.Utils;
using MessageBus;

namespace Auth.API.Extensions;

public static class MessageBusExtensions
{
    public static void AddMessageBusConfiguration(this IServiceCollection services, IConfiguration configuration)
    {
        // Preferir variáveis RabbitMQ:* em Docker; fallback para MessageQueueConnection do appsettings (localhost)
        var rabbitHost = configuration["RabbitMQ:Host"];
        string connection;
        if (!string.IsNullOrWhiteSpace(rabbitHost))
        {
            var rabbitPort = configuration["RabbitMQ:Port"] ?? "5672";
            var rabbitUser = configuration["RabbitMQ:Username"] ?? "guest";
            var rabbitPass = configuration["RabbitMQ:Password"] ?? "guest";
            connection = $"host={rabbitHost}:{rabbitPort};username={rabbitUser};password={rabbitPass};publisherConfirms=true;timeout=30";
        }
        else
        {
            connection = configuration.GetMessageQueueConnection("MessageBus");
        }

        services.AddMessageBus(connection);
    }
}