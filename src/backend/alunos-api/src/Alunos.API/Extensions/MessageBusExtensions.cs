using Alunos.Infrastructure.Services;
using Core.Utils;
using MessageBus;

namespace Alunos.API.Extensions;

public static class MessageBusExtensions
{
    public static void AddMessageBusConfiguration(this IServiceCollection services, IConfiguration configuration)
    {
        // Em Docker, preferir variáveis de ambiente RabbitMQ:* (host rabbitmq).
        string? rabbitHost = configuration["RabbitMQ:Host"];
        string connection = string.Empty;

        if (!string.IsNullOrWhiteSpace(rabbitHost))
        {
            string rabbitPort = configuration["RabbitMQ:Port"] ?? "5672";
            string rabbitUser = configuration["RabbitMQ:Username"] ?? "guest";
            string rabbitPass = configuration["RabbitMQ:Password"] ?? "guest";
            connection = $"host={rabbitHost}:{rabbitPort};username={rabbitUser};password={rabbitPass};publisherConfirms=true;timeout=30";
        }
        else
        {
            connection = configuration?.GetSection("MessageQueueConnection")?["MessageBus"]
                         ?? configuration.GetMessageQueueConnection("MessageBus");
        }

        services
            .AddMessageBus(connection)
            .AddHostedService<RegistroUsuarioIntegrationHandler>();
    }
}