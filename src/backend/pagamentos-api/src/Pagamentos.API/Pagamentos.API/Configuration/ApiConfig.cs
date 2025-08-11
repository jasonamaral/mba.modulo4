namespace Pagamentos.API.Configuration
{
    public static class ApiConfig
    {
        public static WebApplicationBuilder AddApiConfig(this WebApplicationBuilder builder)
        {
            builder.Services.AddControllers();

            builder.Services.AddApiVersioning(options =>
            {
                options.DefaultApiVersion = new ApiVersion(1, 0); // Versão padrão: 1.0
                options.AssumeDefaultVersionWhenUnspecified = true; // Usar versão padrão se não for especificada
                options.ReportApiVersions = true; // Expor informações de versões disponíveis nos cabeçalhos de resposta
            });

            builder.Services.AddVersionedApiExplorer(options =>
            {
                options.GroupNameFormat = "'v'VVV";
                options.SubstituteApiVersionInUrl = true;
            });

            return builder;
        }
    }
}
