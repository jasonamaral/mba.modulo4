using Conteudo.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Conteudo.API.Configuration
{
    public static class DbContextConfiguration
    {
        public static WebApplicationBuilder AddDbContextConfiguration(this WebApplicationBuilder builder)
        {   
            if (builder.Environment.IsDevelopment())
            {
                builder.Services.AddDbContext<ConteudoDbContext>(opt =>
                {
                    opt.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection"));
                });
            } 
            else
            {
                builder.Services.AddDbContext<ConteudoDbContext>(opt =>
                {
                    opt.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
                });
            }
            return builder;
        }
    }
}
