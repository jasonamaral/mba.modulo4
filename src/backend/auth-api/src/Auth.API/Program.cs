using Auth.API.Extensions;
using Auth.Application.Interfaces;
using Auth.Application.Services;
using Auth.Application.Settings;
using Auth.Domain.Entities;
using Auth.Infrastructure.Data;
using Core.Utils;
using Mapster;
using Microsoft.AspNetCore.Identity;

var builder = WebApplication.CreateBuilder(args);

builder.WebHost.ConfigureKestrel(options =>
{
    // Ignora HTTPS, escuta apenas porta 5001 em qualquer IP
    options.ListenAnyIP(5001);
});

// Configurações
builder.Services.Configure<JwtSettings>(builder.Configuration.GetSection("JwtSettings"));

// Configurar Mapster
TypeAdapterConfig.GlobalSettings.Scan(typeof(Program).Assembly);

// Configurações separadas por responsabilidade (SRP)
builder.Services.AddDatabaseConfiguration(builder.Configuration, builder.Environment);
builder.Services.AddIdentityConfiguration();
builder.Services.AddJwtConfiguration(builder.Configuration);
builder.Services.AddJwksConfiguration();

builder.Services.AddAuthorization();
builder.Services.AddMemoryCache();

// Application Services (DIP - dependendo de abstrações)
builder.Services.AddScoped<AuthService, AuthService>();
builder.Services.AddScoped<IAuthDbContext>(provider => provider.GetRequiredService<AuthDbContext>());

// MediatR e Mediator
builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(Program).Assembly));
builder.Services.AddScoped<Core.Mediator.IMediatorHandler, Core.Mediator.MediatorHandler>();
builder.Services.AddScoped<MediatR.INotificationHandler<Core.Messages.DomainNotificacaoRaiz>, Core.Messages.DomainNotificacaoHandler>();

// Notification
builder.Services.RegisterNotification();

// Controllers
builder.Services.AddControllers();

// Swagger
builder.Services.AddSwaggerConfiguration();

builder.Services.AddMessageBusConfiguration(builder.Configuration);

// CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

// Health Checks
builder.Services.AddHealthChecks();

var app = builder.Build();

// Configure pipeline
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Auth API v1");
        c.RoutePrefix = "swagger";
    });
}

// Removido UseHttpsRedirection para desenvolvimento
// app.UseHttpsRedirection();

app.UseCors("AllowAll");
app.UseJwksDiscovery();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.MapHealthChecks("/health");

// Inicializar banco de dados
using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<AuthDbContext>();
    var userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();
    var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();

    await InitializeDatabaseAsync(context, userManager, roleManager);
}

app.Run();

static async Task InitializeDatabaseAsync(AuthDbContext context, UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
{
    // Criar banco se não existir
    await context.Database.EnsureCreatedAsync();

    // Criar roles se não existirem
    string[] roles = { "Administrador", "Usuario" };
    foreach (var role in roles)
    {
        if (!await roleManager.RoleExistsAsync(role))
        {
            await roleManager.CreateAsync(new IdentityRole(role));
        }
    }

    // Criar usuário admin padrão se não existir
    const string adminEmail = "admin@auth.api";
    var adminUser = await userManager.FindByEmailAsync(adminEmail);

    if (adminUser == null)
    {
        #region Crio o ADMIN

        adminUser = new ApplicationUser
        {
            UserName = adminEmail,
            Email = adminEmail,
            Nome = "Administrador do Sistema",
            DataNascimento = new DateTime(1990, 1, 1),
            EmailConfirmed = true
        };

        var result = await userManager.CreateAsync(adminUser, "Teste@123");
        if (result.Succeeded)
        {
            await userManager.AddToRoleAsync(adminUser, "Administrador");
        }

        #endregion Crio o ADMIN

        #region Crio o aluno 1

        var aluno1 = new ApplicationUser
        {
            Id = "06b1b8f1-f079-4048-9c8d-190c8056ea60",
            UserName = "aluno1@auth.api",
            Email = "aluno1@auth.api",
            Nome = "Aluno UM do Sistema",
            DataNascimento = new DateTime(1973, 1, 1),
            EmailConfirmed = true
        };

        var resultAlunoUm = await userManager.CreateAsync(aluno1, "Teste@123");
        if (result.Succeeded)
        {
            await userManager.AddToRoleAsync(aluno1, "Usuario");
        }

        #endregion Crio o aluno 1

        #region Crio o aluno 1

        var aluno2 = new ApplicationUser
        {
            Id = "ca39e314-c960-42bc-9c9d-3cad9b589a8d",
            UserName = "aluno2@auth.api",
            Email = "aluno2@auth.api",
            Nome = "Aluno DOIS do Sistema",
            DataNascimento = new DateTime(2016, 1, 1),
            EmailConfirmed = true
        };

        var resultAlunoDois = await userManager.CreateAsync(aluno2, "Teste@123");
        if (result.Succeeded)
        {
            await userManager.AddToRoleAsync(aluno2, "Usuario");
        }

        #endregion Crio o aluno 1
    }
}
