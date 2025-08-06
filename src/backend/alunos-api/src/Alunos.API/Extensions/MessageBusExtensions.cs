using Alunos.Infrastructure.Services;
using Core.Utils;
using MessageBus;

namespace Alunos.API.Extensions;

public static class MessageBusExtensions
{
    public static void AddMessageBusConfiguration(this IServiceCollection services, IConfiguration configuration)
    {
        var msg = configuration?.GetSection("MessageQueueConnection")?["MessageBus"];
        services.AddMessageBus(msg!);

        services.AddMessageBus(configuration?.GetMessageQueueConnection("MessageBus")!)
            .AddHostedService<RegistroUsuarioIntegrationHandler>();
    }
}