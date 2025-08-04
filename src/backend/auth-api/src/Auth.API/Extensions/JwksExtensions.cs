using Auth.Infrastructure.Data;

namespace Auth.API.Extensions;

public static class JwksExtensions
{
    public static IServiceCollection AddJwksConfiguration(this IServiceCollection services)
    {
        services.AddJwksManager()
                .UseJwtValidation()
                .PersistKeysToDatabaseStore<AuthDbContext>();

        return services;
    }
}