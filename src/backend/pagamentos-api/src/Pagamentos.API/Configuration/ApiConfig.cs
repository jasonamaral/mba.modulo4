using Microsoft.AspNetCore.Mvc;

namespace Pagamentos.API.Configuration
{
    public static class ApiConfig
    {
        public static WebApplicationBuilder AddApiConfig(this WebApplicationBuilder builder)
        {
            builder.Services.AddControllers();

            builder.Services.AddApiVersioning(options =>
            {
                options.DefaultApiVersion = new ApiVersion(1, 0); 
                options.AssumeDefaultVersionWhenUnspecified = true;
                options.ReportApiVersions = true; 
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
