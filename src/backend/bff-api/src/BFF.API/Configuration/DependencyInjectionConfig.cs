using BFF.Application.Interfaces.Services;
using Core.Utils;
using Core.Notification;
using Core.Mediator;
using Core.Messages;
using MediatR;
using BFF.API.Services.Conteudos;
using BFF.API.Services.Aluno;

namespace BFF.API.Configuration;

public static class DependencyInjectionConfig
{
    public static void RegisterServices(this IServiceCollection services)
    {
        services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

        // Serviços de Core
        services.AddScoped<INotificador, Notificador>();

        // MediatR
        services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(Program).Assembly));
        services.AddScoped<IMediatorHandler, MediatorHandler>();
        services.AddScoped<INotificationHandler<DomainNotificacaoRaiz>, DomainNotificacaoHandler>();

        // Serviços de API
        services.AddScoped<IConteudoService, ConteudoService>();
        services.AddScoped<IAlunoService, AlunoService>();
        
        // Serviços de infraestrutura
        services.AddScoped<ICacheService, Infrastructure.Services.CacheService>();
        services.AddTransient<IApiClientService, Infrastructure.Services.ApiClientService>();
        services.AddScoped<IDashboardService, Infrastructure.Services.DashboardService>();

        services.RegisterNotification();
    }
}
