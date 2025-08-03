using Mapster;

namespace BFF.Application.Mappings;

public class BFFMappingConfig : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        // Mapeamentos específicos do BFF podem ser adicionados aqui
        // Por exemplo, mapeamentos entre DTOs de diferentes APIs
        // ou transformações específicas para o BFF

        // Exemplo de mapeamento entre tipos diferentes (quando necessário):
        // config.NewConfig<CursoApiResponse, CursoDto>()
        //     .Map(dest => dest.Status, src => src.Status.ToString())
        //     .Map(dest => dest.Categoria, src => src.CategoriaNome);
    }
}