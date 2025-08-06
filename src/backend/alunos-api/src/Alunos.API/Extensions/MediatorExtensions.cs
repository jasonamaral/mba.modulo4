using Core.Mediator;

namespace Alunos.API.Extensions;

public static class MediatorExtensions
{
    public static void AddMediatorConfiguration(this IServiceCollection services)
    {
        services.AddScoped<IMediatorHandler, MediatorHandler>();
    }
} 