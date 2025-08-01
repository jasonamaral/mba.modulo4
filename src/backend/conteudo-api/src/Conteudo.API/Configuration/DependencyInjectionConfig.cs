using Conteudo.Application.Commands;
using Conteudo.Application.Interfaces.Services;
using Conteudo.Application.Services;
using Conteudo.Domain.Interfaces.Repositories;
using Conteudo.Infrastructure.Data;
using Conteudo.Infrastructure.Repositories;
using Core.Mediator;
using Core.Utils;
using FluentValidation.Results;
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
            services.AddScoped<IRequestHandler<CadastrarCursoCommand, ValidationResult>, CursoHandler>();
            services.AddScoped<IRequestHandler<CadastrarCategoriaCommand, ValidationResult>, CategoriaHandler>();
            services.AddScoped<IRequestHandler<AtualizarCursoCommand, ValidationResult>, CursoHandler>();

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
