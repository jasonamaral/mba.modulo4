using BFF.Domain.DTOs;
using Mapster;

namespace BFF.Application.Mappings;

public class BFFMappingConfig : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        // Mapeamento de CursoDto para CursoDto (mesmo tipo, mas pode ter configurações específicas)
        config.NewConfig<CursoDto, CursoDto>()
            .Map(dest => dest.Id, src => src.Id)
            .Map(dest => dest.Nome, src => src.Nome)
            .Map(dest => dest.Descricao, src => src.Descricao)
            .Map(dest => dest.Categoria, src => src.Categoria)
            .Map(dest => dest.Preco, src => src.Preco)
            .Map(dest => dest.CargaHoraria, src => src.CargaHoraria)
            .Map(dest => dest.TotalAulas, src => src.TotalAulas)
            .Map(dest => dest.Status, src => src.Status)
            .Map(dest => dest.ImagemCapa, src => src.ImagemCapa)
            .Map(dest => dest.Aulas, src => src.Aulas)
            .Map(dest => dest.CreatedAt, src => src.CreatedAt)
            .Map(dest => dest.UpdatedAt, src => src.UpdatedAt);

        // Mapeamento de AulaDto para AulaDto (mesmo tipo, mas pode ter configurações específicas)
        config.NewConfig<AulaDto, AulaDto>()
            .Map(dest => dest.Id, src => src.Id)
            .Map(dest => dest.CursoId, src => src.CursoId)
            .Map(dest => dest.Nome, src => src.Nome)
            .Map(dest => dest.Descricao, src => src.Descricao)
            .Map(dest => dest.Ordem, src => src.Ordem)
            .Map(dest => dest.DuracaoMinutos, src => src.DuracaoMinutos)
            .Map(dest => dest.VideoUrl, src => src.VideoUrl)
            .Map(dest => dest.Status, src => src.Status)
            .Map(dest => dest.CreatedAt, src => src.CreatedAt)
            .Map(dest => dest.UpdatedAt, src => src.UpdatedAt);

        // Mapeamento de AlunoDto para AlunoDto (mesmo tipo, mas pode ter configurações específicas)
        config.NewConfig<AlunoDto, AlunoDto>()
            .Map(dest => dest.Id, src => src.Id)
            .Map(dest => dest.Nome, src => src.Nome)
            .Map(dest => dest.Email, src => src.Email)
            .Map(dest => dest.Telefone, src => src.Telefone)
            .Map(dest => dest.Foto, src => src.Foto)
            .Map(dest => dest.DataNascimento, src => src.DataNascimento)
            .Map(dest => dest.Cpf, src => src.Cpf)
            .Map(dest => dest.CreatedAt, src => src.CreatedAt)
            .Map(dest => dest.UpdatedAt, src => src.UpdatedAt);
    }
}