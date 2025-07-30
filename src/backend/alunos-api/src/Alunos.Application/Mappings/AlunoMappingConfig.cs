using Alunos.Domain.Entities;
using Alunos.Application.DTOs;
using Mapster;

namespace Alunos.Application.Mappings;

public class AlunoMappingConfig : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
                       // Mapeamento de Aluno para AlunoDto
               config.NewConfig<Aluno, AlunoDto>()
                   .Map(dest => dest.Id, src => src.Id)
                   .Map(dest => dest.Nome, src => src.Nome)
                   .Map(dest => dest.Email, src => src.Email)
                   .Map(dest => dest.CPF, src => src.CPF)
                   .Map(dest => dest.DataNascimento, src => src.DataNascimento)
                   .Map(dest => dest.CodigoUsuarioAutenticacao, src => src.CodigoUsuarioAutenticacao)
                   .Map(dest => dest.IsAtivo, src => src.IsAtivo)
                   .Map(dest => dest.CreatedAt, src => src.CreatedAt)
                   .Map(dest => dest.UpdatedAt, src => src.UpdatedAt)
                   .Map(dest => dest.Telefone, src => src.Telefone)
                   .Map(dest => dest.Genero, src => src.Genero)
                   .Map(dest => dest.Cidade, src => src.Cidade)
                   .Map(dest => dest.Estado, src => src.Estado)
                   .Map(dest => dest.CEP, src => src.CEP)
                   .Map(dest => dest.Idade, src => src.CalcularIdade())
                   .Map(dest => dest.Matriculas, src => new List<MatriculaDto>()); // Será preenchido pelo serviço

        // Mapeamento de Aluno para AlunoResumoDto
        config.NewConfig<Aluno, AlunoResumoDto>()
            .Map(dest => dest.Id, src => src.Id)
            .Map(dest => dest.Nome, src => src.Nome)
            .Map(dest => dest.Email, src => src.Email)
            .Map(dest => dest.IsAtivo, src => src.IsAtivo)
            .Map(dest => dest.Cidade, src => src.Cidade)
            .Map(dest => dest.Estado, src => src.Estado)
            .Map(dest => dest.CreatedAt, src => src.CreatedAt)
            .Map(dest => dest.Idade, src => src.CalcularIdade())
            .Map(dest => dest.QuantidadeMatriculasAtivas, src => 0) // Será calculado pelo serviço
            .Map(dest => dest.QuantidadeCursosConcluidos, src => 0); // Será calculado pelo serviço

                       // Mapeamento de AlunoCadastroDto para Aluno
               config.NewConfig<AlunoCadastroDto, Aluno>()
                   .Map(dest => dest.CodigoUsuarioAutenticacao, src => src.CodigoUsuarioAutenticacao)
                   .Map(dest => dest.Nome, src => src.Nome)
                   .Map(dest => dest.Email, src => src.Email)
                   .Map(dest => dest.CPF, src => src.CPF)
                   .Map(dest => dest.DataNascimento, src => src.DataNascimento)
                   .Map(dest => dest.Telefone, src => src.Telefone)
                   .Map(dest => dest.Genero, src => src.Genero)
                   .Map(dest => dest.Cidade, src => src.Cidade)
                   .Map(dest => dest.Estado, src => src.Estado)
                   .Map(dest => dest.CEP, src => src.CEP)
                   .Map(dest => dest.IsAtivo, src => true)
                   .Ignore(dest => dest.Id)
                   .Ignore(dest => dest.CreatedAt)
                   .Ignore(dest => dest.UpdatedAt)
                   .Ignore(dest => dest.MatriculasCursos);

                       // Mapeamento de AlunoAtualizarDto para Aluno
               config.NewConfig<AlunoAtualizarDto, Aluno>()
                   .Map(dest => dest.Nome, src => src.Nome)
                   .Map(dest => dest.Email, src => src.Email)
                   .Map(dest => dest.CPF, src => src.CPF)
                   .Map(dest => dest.DataNascimento, src => src.DataNascimento)
                   .Map(dest => dest.Telefone, src => src.Telefone)
                   .Map(dest => dest.Genero, src => src.Genero)
                   .Map(dest => dest.Cidade, src => src.Cidade)
                   .Map(dest => dest.Estado, src => src.Estado)
                   .Map(dest => dest.CEP, src => src.CEP)
                   .Ignore(dest => dest.Id)
                   .Ignore(dest => dest.CodigoUsuarioAutenticacao)
                   .Ignore(dest => dest.IsAtivo)
                   .Ignore(dest => dest.CreatedAt)
                   .Ignore(dest => dest.UpdatedAt)
                   .Ignore(dest => dest.MatriculasCursos);

        // Mapeamento de Aluno para AlunoPerfilDto
        config.NewConfig<Aluno, AlunoPerfilDto>()
            .Map(dest => dest.Aluno, src => src.Adapt<AlunoDto>())
            .Map(dest => dest.Estatisticas, src => new AlunoEstatisticasDto())
            .Map(dest => dest.HistoricoRecente, src => new List<HistoricoAlunoDto>());

        // Mapeamento de Aluno para AlunoDashboardDto
        config.NewConfig<Aluno, AlunoDashboardDto>()
            .Map(dest => dest.Aluno, src => src.Adapt<AlunoResumoDto>())
            .Map(dest => dest.Estatisticas, src => new AlunoEstatisticasDto())
            .Map(dest => dest.MatriculasEmAndamento, src => new List<MatriculaDto>())
            .Map(dest => dest.ProximasAulas, src => new List<ProximaAulaDto>())
            .Map(dest => dest.CertificadosRecentes, src => new List<CertificadoDto>())
            .Map(dest => dest.AtividadesRecentes, src => new List<HistoricoAlunoDto>());
    }
} 