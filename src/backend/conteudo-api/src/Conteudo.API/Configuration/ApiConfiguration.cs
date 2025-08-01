using Conteudo.API.Extensions;
using Conteudo.Application.Commands;
using Conteudo.Application.Mappings;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace Conteudo.API.Configuration
{
    public static class ApiConfiguration
    {
        public static WebApplicationBuilder AddApiConfiguration(this WebApplicationBuilder builder)
        {
            return builder
                .AddConfiguration()
                .AddDbContextConfiguration()
                .AddControllersConfiguration()
                .AddHttpContextAccessorConfiguration()
                .AddCorsConfiguration()
                .AddMediatRConfiguration()
                .AddServicesConfiguration()
                .AddAutoMapperConfiguration()
                .AddJwtConfiguration()
                .AddSwaggerConfigurationExtension();
        }

        private static WebApplicationBuilder AddConfiguration(this WebApplicationBuilder builder)
        {
            builder.Configuration.SetBasePath(builder.Environment.ContentRootPath)
                .AddJsonFile("appsettings.json", true, true)
                .AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.json", true, true)
                .AddEnvironmentVariables();
            return builder;
        }

        private static WebApplicationBuilder AddControllersConfiguration(this WebApplicationBuilder builder)
        {
            builder.Services.AddControllers()
                .ConfigureApiBehaviorOptions(opt => opt.SuppressModelStateInvalidFilter = true);
            return builder;
        }

        private static WebApplicationBuilder AddHttpContextAccessorConfiguration(this WebApplicationBuilder builder)
        {
            builder.Services.AddHttpContextAccessor();
            return builder;
        }

        private static WebApplicationBuilder AddCorsConfiguration(this WebApplicationBuilder builder)
        {
            builder.Services.AddCors(options =>
            {
                options.AddDefaultPolicy(policy =>
                {
                    policy.AllowAnyOrigin()
                          .AllowAnyHeader()
                          .AllowAnyMethod();
                });
            });
            return builder;
        }

        private static WebApplicationBuilder AddMediatRConfiguration(this WebApplicationBuilder builder)
        {
            builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblies(typeof(CadastrarCursoCommand).Assembly));
            return builder;
        }

        private static WebApplicationBuilder AddServicesConfiguration(this WebApplicationBuilder builder)
        {
            builder.Services.RegisterServices();
            return builder;
        }

        private static WebApplicationBuilder AddAutoMapperConfiguration(this WebApplicationBuilder builder)
        {
            builder.Services.AddAutoMapper(typeof(CursoMap));
            return builder;
        }

        private static WebApplicationBuilder AddSwaggerConfigurationExtension(this WebApplicationBuilder builder)
        {
            builder.Services.AddSwaggerConfiguration();
            return builder;
        }

        private static WebApplicationBuilder AddJwtConfiguration(this WebApplicationBuilder builder)
        {
            var jwtSettings = builder.Configuration.GetSection("JwtSettings");

            // Configura autenticação JWT
            builder.Services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,

                    ValidIssuer = jwtSettings["Issuer"],     
                    ValidAudience = jwtSettings["Audience"],
                    IssuerSigningKey = new SymmetricSecurityKey(
                        Encoding.ASCII.GetBytes(jwtSettings["SecretKey"]!)
                    ),
                    ClockSkew = TimeSpan.Zero
                };
            });
            builder.Services.AddAuthorization();
            return builder;
        }
    }
}
