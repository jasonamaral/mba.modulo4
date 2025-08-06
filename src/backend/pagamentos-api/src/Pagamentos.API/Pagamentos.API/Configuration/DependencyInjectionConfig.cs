using Core.Mediator;
using Core.Utils;

namespace Pagamentos.API.Configuration;

public static class DependencyInjectionConfig
{
    public static void RegisterServices(this IServiceCollection services)
    {
        // Application
        services.AddScoped<IMediatorHandler, MediatorHandler>();

        // Commands
        //services.AddScoped<IRequestHandler<CancelarPagamentoCommand, CommandResult>, CancelarPagamentoHandler>();
        //services.AddScoped<IRequestHandler<ProcessarPagamentoCommand, CommandResult>, ProcessarPagamentoHandler>();
        //services.AddScoped<IRequestHandler<SolicitarReembolsoCommand, CommandResult>, SolicitarReembolsoHandler>();

        // Services
        //services.AddScoped<ICursoAppService, CursoAppService>();
        //services.AddScoped<ICategoriaAppService, CategoriaAppService>();

        // Data
        //services.AddScoped<ICursoRepository, CursoRepository>();
        //services.AddScoped<ICategoriaRepository, CategoriaRepository>();
        //services.AddScoped<ConteudoDbContext>();

        // Notification
        services.RegisterNotification();
    }
}