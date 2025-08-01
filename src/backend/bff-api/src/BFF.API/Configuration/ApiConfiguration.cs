using BFF.API.Extensions;
using BFF.API.Settings;
using BFF.Domain.Settings;
using Mapster;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace BFF.API.Configuration
{
    public static class ApiConfiguration
    {
        public static WebApplicationBuilder AddApiConfiguration(this WebApplicationBuilder builder)
        {
            return builder
                .AddConfiguration()
                .AddRedisConfiguration()
                .AddControllersConfiguration()
                .AddJwtConfiguration()
                .AddHttpContextAccessorConfiguration()
                .AddCorsConfiguration()
                .AddServicesConfiguration()
                .AddMapsterConfiguration()
                .AddSwaggerConfigurationExtension();
        }

        private static WebApplicationBuilder AddConfiguration(this WebApplicationBuilder builder)
        {
            builder.Configuration.SetBasePath(builder.Environment.ContentRootPath)
                .AddJsonFile("appsettings.json", true, true)
                .AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.json", true, true)
                .AddEnvironmentVariables();

            builder.Services.Configure<JwtSettings>(builder.Configuration.GetSection("JwtSettings"));
            builder.Services.Configure<ApiSettings>(builder.Configuration.GetSection("ApiSettings"));
            builder.Services.Configure<CacheSettings>(builder.Configuration.GetSection("CacheSettings"));
            builder.Services.Configure<RedisSettings>(builder.Configuration.GetSection("RedisSettings"));
            builder.Services.Configure<ResilienceSettings>(builder.Configuration.GetSection("ResilienceSettings"));

            return builder;
        }

        private static WebApplicationBuilder AddRedisConfiguration(this WebApplicationBuilder builder)
        {
            // Redis Cache
            var redisSettings = builder.Configuration.GetSection("RedisSettings").Get<RedisSettings>();
            if (redisSettings != null && !string.IsNullOrEmpty(redisSettings.ConnectionString))
            {
                builder.Services.AddStackExchangeRedisCache(options =>
                {
                    options.Configuration = redisSettings.ConnectionString;
                    options.InstanceName = redisSettings.KeyPrefix;
                });
            }
            else
            {
                // Fallback para cache em memória se Redis não estiver disponível
                builder.Services.AddMemoryCache();
            }
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
            // CORS
            builder.Services.AddCors(options =>
            {
                options.AddPolicy("AllowedOrigins", policy =>
                {
                    var corsSettings = builder.Configuration.GetSection("CORS");
                    var allowedOrigins = corsSettings.GetSection("AllowedOrigins").Get<string[]>();
                    var allowedMethods = corsSettings.GetSection("AllowedMethods").Get<string[]>();
                    var allowedHeaders = corsSettings.GetSection("AllowedHeaders").Get<string[]>();

                    policy.WithOrigins(allowedOrigins ?? new[] { "http://localhost:4200" })
                          .WithMethods(allowedMethods ?? new[] { "GET", "POST", "PUT", "DELETE", "OPTIONS" })
                          .WithHeaders(allowedHeaders ?? new[] { "Content-Type", "Authorization" })
                          .AllowCredentials();
                });
            });
            return builder;
        }

        private static WebApplicationBuilder AddJwtConfiguration(this WebApplicationBuilder builder)
        {
            // JWT Authentication
            var jwtSettings = builder.Configuration.GetSection("JwtSettings").Get<JwtSettings>();
            if (jwtSettings != null)
            {
                builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                    .AddJwtBearer(options =>
                    {
                        options.TokenValidationParameters = new TokenValidationParameters
                        {
                            ValidateIssuer = true,
                            ValidateAudience = true,
                            ValidateLifetime = true,
                            ValidateIssuerSigningKey = true,
                            ValidIssuer = jwtSettings.Issuer,
                            ValidAudience = jwtSettings.Audience,
                            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings.SecretKey))
                        };

                        // Adicionar eventos para debug
                        options.Events = new JwtBearerEvents
                        {
                            OnAuthenticationFailed = context =>
                            {
                                Console.WriteLine($"BFF - Falha na autenticação: {context.Exception.Message}");
                                return Task.CompletedTask;
                            },
                            OnTokenValidated = context =>
                            {
                                Console.WriteLine("BFF - Token validado com sucesso");
                                return Task.CompletedTask;
                            },
                            OnChallenge = context =>
                            {
                                Console.WriteLine($"BFF - Challenge: {context.Error}, {context.ErrorDescription}");
                                return Task.CompletedTask;
                            }
                        };
                    });
            }
            return builder;
        }

        private static WebApplicationBuilder AddServicesConfiguration(this WebApplicationBuilder builder)
        {
            builder.Services.RegisterServices();
            return builder;
        }

        private static WebApplicationBuilder AddMapsterConfiguration(this WebApplicationBuilder builder)
        {
            // Configurar Mapster
            TypeAdapterConfig.GlobalSettings.Scan(typeof(Program).Assembly);
            return builder;
        }

        private static WebApplicationBuilder AddSwaggerConfigurationExtension(this WebApplicationBuilder builder)
        {
            builder.Services.AddSwaggerConfiguration();
            return builder;
        }
    }
}
