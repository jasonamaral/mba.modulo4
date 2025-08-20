using Alunos.Application.Commands.AtualizarPagamento;
using Alunos.Application.Commands.CadastrarAluno;
using Alunos.Application.Commands.ConcluirCurso;
using Alunos.Application.Commands.MatricularAluno;
using Alunos.Application.Commands.RegistrarHistoricoAprendizado;
using Alunos.Application.Commands.SolicitarCertificado;
using Alunos.Application.Events.RegistrarProblemaHistorico;
using Alunos.Application.Integration;
using Alunos.Application.Interfaces;
using Alunos.Application.Queries;
using Alunos.Domain.Interfaces;
using Alunos.Infrastructure.Repositories;
using Core.Communication;
using Core.Mediator;
using Core.Messages;
using Core.Utils;
using MediatR;

namespace Alunos.API.Configurations;

public static class DependencyInjectionConfigutarion
{
    public static void RegisterServices(this IServiceCollection services)
    {
        // Application
        services.AddScoped<IMediatorHandler, MediatorHandler>();

        // Events
        services.AddScoped<INotificationHandler<DomainNotificacaoRaiz>, DomainNotificacaoHandler>();
        services.AddScoped<INotificationHandler<RegistrarProblemaHistoricoAprendizadoEvent>, RegistrarProblemaHistoricoAprendizadoEventHandler>();

        // Commands
        services.AddScoped<IRequestHandler<CadastrarAlunoCommand, CommandResult>, CadastrarAlunoCommandHandler>();
        services.AddScoped<IRequestHandler<MatricularAlunoCommand, CommandResult>, MatricularAlunoCommandHandler>();
        services.AddScoped<IRequestHandler<RegistrarHistoricoAprendizadoCommand, CommandResult>, RegistrarHistoricoAprendizadoCommandHandler>();
        services.AddScoped<IRequestHandler<ConcluirCursoCommand, CommandResult>, ConcluirCursoCommandHandler>();
        services.AddScoped<IRequestHandler<SolicitarCertificadoCommand, CommandResult>, SolicitarCertificadoCommandHandler>();
        services.AddScoped<IRequestHandler<AtualizarPagamentoMatriculaCommand, CommandResult>, AtualizarPagamentoMatriculaCommandHandler>();

        // Services
        services.AddScoped<IAlunoQueryService, AlunoQueryService>();
        services.AddScoped<IRegistroAlunoIntegrationService, RegistroAlunoIntegrationService>();
        services.AddScoped<IRegistroPagamentoIntegrationService, RegistroPagamentoIntegrationService>();

        // Data
        services.AddScoped<IAlunoRepository, AlunoRepository>();

        // Notification
        services.RegisterNotification();
    }
}
