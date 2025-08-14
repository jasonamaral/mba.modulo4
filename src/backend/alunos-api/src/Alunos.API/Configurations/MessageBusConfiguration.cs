using Alunos.Infrastructure.Services;
using Core.Utils;
using MessageBus;

namespace Alunos.API.Configurations;
public static class MessageBusConfiguration
{
    public static void AddMessageBusConfiguration(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddMessageBus(configuration?.GetMessageQueueConnection("MessageBus")!)
            .AddHostedService<RegistroAlunoIntegrationHandler>();
    }
}