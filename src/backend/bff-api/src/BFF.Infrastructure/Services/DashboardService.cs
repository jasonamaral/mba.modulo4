using BFF.Application.Interfaces.Services;
using BFF.Domain.DTOs;
using BFF.Domain.DTOs.Alunos.Response;
using BFF.Domain.Settings;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace BFF.Infrastructure.Services;

public class DashboardService : IDashboardService
{
    private readonly ICacheService _cacheService;
    private readonly CacheSettings _cacheSettings;
    private readonly ILogger<DashboardService> _logger;

    public DashboardService(
        ICacheService cacheService,
        IOptions<CacheSettings> cacheOptions,
        ILogger<DashboardService> logger)
    {
        _cacheService = cacheService;
        _cacheSettings = cacheOptions.Value;
        _logger = logger;
    }

    public async Task<DashboardAdminDto> GetDashboardAdminAsync()
    {
        var cacheKey = "dashboard:admin";
        
        // Tenta buscar do cache primeiro
        var cachedData = await _cacheService.GetAsync<DashboardAdminDto>(cacheKey);
        if (cachedData != null)
        {
            _logger.LogInformation("Dashboard do admin recuperado do cache");
            return cachedData;
        }

        _logger.LogInformation("Carregando dashboard do admin dos microsserviços");

        // TODO: Substituir por chamadas reais aos microsserviços
        // Por enquanto, dados mock para demonstração
        await Task.Delay(100); // Simula chamada assíncrona

        var dashboardData = new DashboardAdminDto
        {
            EstatisticasAlunos = new EstatisticasAlunosDto
            {
                TotalAlunos = 350,
                AlunosAtivos = 280,
                AlunosInativos = 70,
                NovasMatriculasHoje = 12,
                NovasMatriculasSemana = 85,
                NovasMatriculasMes = 320,
                TaxaRetencao = 85.5m
            },
            EstatisticasCursos = new EstatisticasCursosDto
            {
                TotalCursos = 25,
                CursosAtivos = 20,
                CursosInativos = 5,
                MediaAvaliacoes = 4.5m,
                TotalAulas = 500,
                HorasConteudo = 1200
            },
            RelatorioVendas = new RelatorioVendasDto
            {
                VendasHoje = 1250.00m,
                VendasSemana = 8750.00m,
                VendasMes = 35000.00m,
                VendasAno = 420000.00m,
                TicketMedio = 125.50m,
                TotalTransacoes = 280,
                TaxaConversao = 12.5m
            },
            EstatisticasUsuarios = new EstatisticasUsuariosDto
            {
                TotalUsuarios = 375,
                UsuariosAtivos = 320,
                UsuariosOnline = 45,
                AdminsAtivos = 15,
                AlunosAtivos = 305
            },
            CursosPopulares = new List<CursoPopularDto>
            {
                new CursoPopularDto
                {
                    Id = Guid.NewGuid(),
                    Nome = "ASP.NET Core Completo",
                    TotalMatriculas = 150,
                    Receita = 29998.50m,
                    MediaAvaliacoes = 4.8m,
                    TotalAvaliacoes = 120
                },
                new CursoPopularDto
                {
                    Id = Guid.NewGuid(),
                    Nome = "C# Avançado",
                    TotalMatriculas = 95,
                    Receita = 18905.00m,
                    MediaAvaliacoes = 4.6m,
                    TotalAvaliacoes = 78
                }
            },
            VendasRecentes = new List<VendaRecenteDto>
            {
                new VendaRecenteDto
                {
                    Id = Guid.NewGuid(),
                    AlunoNome = "Maria Silva",
                    CursoNome = "ASP.NET Core Completo",
                    Valor = 199.99m,
                    DataVenda = DateTime.UtcNow.AddMinutes(-30),
                    Status = "Aprovado",
                    FormaPagamento = "Cartão de Crédito"
                },
                new VendaRecenteDto
                {
                    Id = Guid.NewGuid(),
                    AlunoNome = "João Santos",
                    CursoNome = "C# Avançado",
                    Valor = 149.99m,
                    DataVenda = DateTime.UtcNow.AddHours(-2),
                    Status = "Processando",
                    FormaPagamento = "PIX"
                }
            }
        };

        // Salva no cache usando a configuração específica para dashboard
        await _cacheService.SetAsync(cacheKey, dashboardData, _cacheSettings.DashboardExpiration);
        
        _logger.LogInformation("Dashboard do admin salvo no cache por {Minutes} minutos", 
            _cacheSettings.DashboardExpiration.TotalMinutes);

        return dashboardData;
    }
} 