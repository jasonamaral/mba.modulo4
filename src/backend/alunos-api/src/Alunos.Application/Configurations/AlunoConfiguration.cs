using Alunos.Domain.Interfaces;
using Alunos.Infrastructure.Data;
using Alunos.Infrastructure.Repositories;
using Core.Data.Constants;
using Core.Mediator;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System.Globalization;

namespace Alunos.Application.Configurations;
public static class AlunoConfiguration
{
    public static IServiceCollection ConfigurarAlunoApplication(this IServiceCollection services, string stringConexao, bool ehProducao)
    {
        return services.ConfigurarInjecoesDependenciasRepository()
            .ConfigurarInjecoesDependenciasApplication()
            .ConfigurarRepositorios(stringConexao, ehProducao);
    }

    private static IServiceCollection ConfigurarInjecoesDependenciasRepository(this IServiceCollection services)
    {
        services.AddScoped<IAlunoRepository, AlunoRepository>();
        return services;
    }

    private static IServiceCollection ConfigurarInjecoesDependenciasApplication(this IServiceCollection services)
    {
        services.AddScoped<IMediatorHandler, MediatorHandler>();

        //services.AddScoped<INotificationHandler<DomainNotificacaoRaiz>, DomainNotificacaoHandler>();
        //services.AddScoped<INotificationHandler<PagamentoConfirmadoEvent>, PagamentoConfirmadoEventHandler>();
        //services.AddScoped<INotificationHandler<PagamentoRecusadoEvent>, PagamentoRecusadoEventHandler>();
        //services.AddScoped<INotificationHandler<RegistrarProblemaHistoricoAprendizadoEvent>, RegistrarProblemaHistoricoAprendizadoEventHandler>();

        //// TODO :: Definir se devo ou não manter este Command
        ////services.AddScoped<IRequestHandler<AtualizarPagamentoMatriculaCommand, bool>, AtualizarPagamentoMatriculaCommandHandler>();                
        //services.AddScoped<IRequestHandler<CadastrarAlunoCommand, bool>, CadastrarAlunoCommandHandler>();
        //services.AddScoped<IRequestHandler<ConcluirCursoCommand, bool>, ConcluirCursoCommandHandler>();
        //services.AddScoped<IRequestHandler<MatricularAlunoCommand, bool>, MatricularAlunoCommandHandler>();
        //services.AddScoped<IRequestHandler<RegistrarHistoricoAprendizadoCommand, bool>, RegistrarHistoricoAprendizadoCommandHandler>();
        //services.AddScoped<IRequestHandler<SolicitarCertificadoCommand, bool>, SolicitarCertificadoCommandHandler>();

        //services.AddScoped<IAlunoQueryService, AlunoQueryService>();
        return services;
    }

    private static IServiceCollection ConfigurarRepositorios(this IServiceCollection services, string stringConexao, bool ehProducao)
    {
        services.AddDbContext<AlunoDbContext>(o =>
        {
            if (ehProducao)
            {
                o.UseSqlServer(stringConexao);
            }
            else
            {
                var connection = new SqliteConnection(stringConexao);
                connection.CreateCollation(DatabaseTypeConstant.Collate, (x, y) =>
                {
                    if (x == null && y == null) return 0;
                    if (x == null) return -1;
                    if (y == null) return 1;

                    // Comparação ignorando maiúsculas/minúsculas e acentos
                    return string.Compare(x, y, CultureInfo.CurrentCulture, CompareOptions.IgnoreCase | CompareOptions.IgnoreNonSpace);
                });

                o.UseSqlite(connection);
            }
        });

        return services;
    }
}
