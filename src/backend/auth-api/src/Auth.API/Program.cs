using Auth.API.Extensions;
using Auth.Application.Interfaces;
using Auth.Application.Services;
using Auth.Infrastructure.Data;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Configurar JWT
var jwtSettings = builder.Configuration.GetSection("JwtSettings");
builder.Services.Configure<JwtSettings>(jwtSettings);

// Configurar Entity Framework
builder.Services.AddDbContext<AuthDbContext>(options =>
{
    var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
    var isDevelopment = builder.Environment.IsDevelopment();
    
    if (isDevelopment)
    {
        // SQLite para desenvolvimento
        options.UseSqlite(connectionString);
    }
    else
    {
        // SQL Server para produção
        options.UseSqlServer(connectionString);
    }
});

// Configurar Identity
builder.Services.AddIdentity<ApplicationUser, IdentityRole>(options =>
{
    options.Password.RequireDigit = true;
    options.Password.RequireLowercase = true;
    options.Password.RequireUppercase = true;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequiredLength = 8;
    
    options.User.RequireUniqueEmail = true;
    options.SignIn.RequireConfirmedEmail = false;
})
.AddEntityFrameworkStores<AuthDbContext>()
.AddDefaultTokenProviders();

// Configurar JWT Authentication
var jwtSettingsConfig = jwtSettings.Get<JwtSettings>();
var key = Encoding.ASCII.GetBytes(jwtSettingsConfig?.SecretKey ?? "default-key");

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.RequireHttpsMetadata = false;
    options.SaveToken = true;
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(key),
        ValidateIssuer = true,
        ValidIssuer = jwtSettingsConfig?.Issuer,
        ValidateAudience = true,
        ValidAudience = jwtSettingsConfig?.Audience,
        ValidateLifetime = true,
        ClockSkew = TimeSpan.Zero
    };
});

// Configurar Controllers
builder.Services.AddControllers();

// Configurar Swagger
builder.Services.AddSwaggerConfiguration();

// Configurar CORS
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

// Configurar serviços da aplicação
builder.Services.AddScoped<IAuthService, AuthService>();

// Configurar HttpContextAccessor
builder.Services.AddHttpContextAccessor();

var app = builder.Build();

// Configurar pipeline
app.UseSwaggerConfiguration();

app.UseCors();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

// Health Check
app.MapGet("/health", () => Results.Ok(new { Status = "Healthy", DateTime = DateTime.UtcNow }))
    .WithName("HealthCheck")
    .WithOpenApi();

// Seed inicial do banco de dados
using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<AuthDbContext>();
    var userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();
    var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
    
    await context.Database.MigrateAsync();
    await SeedDataAsync(userManager, roleManager);
}

app.Run();

// Método para seed inicial
static async Task SeedDataAsync(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
{
    // Criar roles se não existirem
    if (!await roleManager.RoleExistsAsync("Administrador"))
    {
        await roleManager.CreateAsync(new IdentityRole("Administrador"));
    }
    
    if (!await roleManager.RoleExistsAsync("Usuario"))
    {
        await roleManager.CreateAsync(new IdentityRole("Usuario"));
    }

    // Criar usuário administrador padrão se não existir
    var adminEmail = "admin@plataforma.com";
    var adminUser = await userManager.FindByEmailAsync(adminEmail);
    
    if (adminUser == null)
    {
        adminUser = new ApplicationUser
        {
            UserName = adminEmail,
            Email = adminEmail,
            Nome = "Administrador",
            DataNascimento = new DateTime(1990, 1, 1),
            DataCadastro = DateTime.UtcNow,
            Ativo = true,
            EmailConfirmed = true
        };

        var result = await userManager.CreateAsync(adminUser, "Teste123@");
        if (result.Succeeded)
        {
            await userManager.AddToRoleAsync(adminUser, "Administrador");
        }
    }
}
