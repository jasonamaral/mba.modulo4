using Alunos.Infrastructure.Services;
using Core.Utils;
using MessageBus;

namespace Alunos.API.Configurations;
public static class MessageBusConfiguration
{
    public static void AddMessageBusConfiguration(this IServiceCollection services, IConfiguration configuration)
    {
        // Em Docker, preferir variáveis de ambiente RabbitMQ:* (host rabbitmq).
        string? rabbitHost = configuration["RabbitMQ:Host"];
        string connection = string.Empty;

        services.AddMessageBus(configuration?.GetMessageQueueConnection("MessageBus")!)
            .AddHostedService<RegistroAlunoIntegrationHandler>();
    }
}