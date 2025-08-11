using Core.Notification;
using MediatR;
using Microsoft.AspNetCore.Cors.Infrastructure;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Pagamentos.API.Authentication;
using Pagamentos.Application.Interfaces;
using Pagamentos.Application.Services;
using Pagamentos.Core.Bus;
using Pagamentos.Core.Messages.CommonMessages.Notifications;
using Pagamentos.Domain.Interfaces;
using Pagamentos.Domain.Services;
using Pagamentos.Infrastructure.Repositories;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Pagamentos.API.Configuration
{
    public static class DependencyInjectionConfig
    {
        public static WebApplicationBuilder AddDependencyInjectionConfig(this WebApplicationBuilder builder)
        {
            builder.Services.ResolveDependencies();
            return builder;
        }

        public static IServiceCollection ResolveDependencies(this IServiceCollection services)
        {
            services.AddTransient<IConfigureOptions<SwaggerGenOptions>, ConfigureSwaggerOptions>();

            services.AddScoped<IMediatrHandler, MediatorHandler>();

            services.AddScoped<DomainNotificationHandler>();
            services.AddScoped<NotificationContext>();
            services.AddScoped<INotificationHandler<DomainNotification>>(provider => provider.GetService<DomainNotificationHandler>());

            services.AddScoped<IAppIdentityUser, AppIdentityUser>();
            services.AddScoped<INotificador, Notificador>();

            services.AddScoped<IAlunoRepository, AlunoRepository>();
            services.AddScoped<IAlunoService, AlunoService>();
            services.AddScoped<IAlunoComandoAppService, AlunoAppService>();
            services.AddScoped<IAlunoConsultaAppService, AlunoAppService>();

            services.AddScoped<ICursoRepository, CursoRepository>();
            services.AddScoped<ICursoService, CursoService>();
            services.AddScoped<ICursoConsultaAppService, CursoAppService>();
            services.AddScoped<ICursoComandoAppService, CursoAppService>();

            services.AddScoped<IMatriculaRepository, MatriculaRepository>();
            services.AddScoped<IMatriculaConsultaAppService, MatriculaAppService>();
            services.AddScoped<IMatriculaComandoAppService, MatriculaAppService>();

            services.AddScoped<IPagamentoConsultaAppService, PagamentoAppService>();
            services.AddScoped<IPagamentoComandoAppService, PagamentoAppService>();
            services.AddScoped<IPagamentoRepository, PagamentoRepository>();
            services.AddScoped<IPagamentoService, PagamentoService>();
            services.AddScoped<IPagamentoCartaoCreditoFacade, PagamentoCartaoCreditoFacade>();
            services.AddScoped<IPayPalGateway, PayPalGateway>();
            services.AddScoped<Pagamentos.AntiCorruption.Interfaces.IConfigurationManager, Pagamentos.AntiCorruption.Services.ConfigurationManager>();

            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

            return services;
        }
    }
}
