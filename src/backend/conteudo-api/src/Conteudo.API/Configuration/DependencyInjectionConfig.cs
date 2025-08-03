using Conteudo.Application.Commands.AtualizarCurso;
using Conteudo.Application.Commands.CadastrarCategoria;
using Conteudo.Application.Commands.CadastrarCurso;
using Conteudo.Application.Interfaces.Services;
using Conteudo.Application.Services;
using Conteudo.Domain.Interfaces.Repositories;
using Conteudo.Infrastructure.Data;
using Conteudo.Infrastructure.Repositories;
using Core.Communication;
using Core.Mediator;
using Core.Messages;
using Core.Utils;
using MediatR;

namespace Conteudo.API.Configuration
{
    public static class DependencyInjectionConfig
    {
        public static void RegisterServices(this IServiceCollection services)
        {
            // Application
            services.AddScoped<IMediatorHandler, MediatorHandler>();

            // Commands
            services.AddScoped<INotificationHandler<DomainNotificacaoRaiz>, DomainNotificacaoHandler>();
            services.AddScoped<IRequestHandler<CadastrarCursoCommand, CommandResult>, CadastrarCursoCommandHandler>();
            services.AddScoped<IRequestHandler<CadastrarCategoriaCommand, CommandResult>, CadastrarCategoriaCommandHandler>();
            services.AddScoped<IRequestHandler<AtualizarCursoCommand, CommandResult>, AlterarCursoCommandHandler>();

            // Services
            services.AddScoped<ICursoAppService, CursoAppService>();
            services.AddScoped<ICategoriaAppService, CategoriaAppService>();

            // Data
            services.AddScoped<ICursoRepository, CursoRepository>();
            services.AddScoped<ICategoriaRepository, CategoriaRepository>();
            services.AddScoped<ConteudoDbContext>();

            // Notification
            services.RegisterNotification();
        }
    }
}
