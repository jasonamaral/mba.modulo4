# Estrutura Interna dos Microsserviços

## Princípios de Design

Cada microsserviço segue os princípios de **Clean Architecture** e **DDD (Domain-Driven Design)**, mantendo completa independência e isolamento.

## Estrutura Padrão de um Microsserviço

### Exemplo: Auth API

```
auth-api/
├── src/
│   ├── Auth.API/                          # Camada de Apresentação
│   │   ├── Controllers/
│   │   │   ├── AuthController.cs
│   │   │   └── UsersController.cs
│   │   ├── Middlewares/
│   │   │   ├── JwtMiddleware.cs
│   │   │   └── ExceptionMiddleware.cs
│   │   ├── Configurations/
│   │   │   ├── DependencyInjection.cs
│   │   │   └── SwaggerConfig.cs
│   │   ├── Program.cs
│   │   ├── appsettings.json
│   │   └── Auth.API.csproj
│   │
│   ├── Auth.Domain/                       # Camada de Domínio
│   │   ├── Entities/
│   │   │   ├── User.cs
│   │   │   └── Role.cs
│   │   ├── ValueObjects/
│   │   │   ├── Email.cs
│   │   │   └── Password.cs
│   │   ├── Interfaces/
│   │   │   ├── IUserRepository.cs
│   │   │   └── IAuthService.cs
│   │   ├── Events/
│   │   │   ├── UserRegisteredEvent.cs
│   │   │   └── UserLoggedInEvent.cs
│   │   ├── Exceptions/
│   │   │   └── AuthDomainException.cs
│   │   └── Auth.Domain.csproj
│   │
│   ├── Auth.Application/                  # Camada de Aplicação
│   │   ├── Commands/
│   │   │   ├── RegisterUser/
│   │   │   │   ├── RegisterUserCommand.cs
│   │   │   │   ├── RegisterUserCommandHandler.cs
│   │   │   │   └── RegisterUserCommandValidator.cs
│   │   │   └── LoginUser/
│   │   │       ├── LoginUserCommand.cs
│   │   │       ├── LoginUserCommandHandler.cs
│   │   │       └── LoginUserCommandValidator.cs
│   │   ├── Queries/
│   │   │   └── GetUser/
│   │   │       ├── GetUserQuery.cs
│   │   │       └── GetUserQueryHandler.cs
│   │   ├── DTOs/
│   │   │   ├── LoginRequestDto.cs
│   │   │   ├── LoginResponseDto.cs
│   │   │   └── RegisterRequestDto.cs
│   │   ├── Services/
│   │   │   ├── AuthService.cs
│   │   │   └── TokenService.cs
│   │   ├── Interfaces/
│   │   │   ├── IAuthService.cs
│   │   │   └── ITokenService.cs
│   │   ├── Behaviors/
│   │   │   ├── ValidationBehavior.cs
│   │   │   └── LoggingBehavior.cs
│   │   └── Auth.Application.csproj
│   │
│   └── Auth.Infrastructure/               # Camada de Infraestrutura
│       ├── Data/
│       │   ├── AuthDbContext.cs
│       │   ├── Configurations/
│       │   │   ├── UserConfiguration.cs
│       │   │   └── RoleConfiguration.cs
│       │   └── Migrations/
│       ├── Repositories/
│       │   ├── UserRepository.cs
│       │   └── RoleRepository.cs
│       ├── Services/
│       │   ├── EmailService.cs
│       │   └── HashingService.cs
│       ├── Messaging/
│       │   ├── EventBus.cs
│       │   ├── EventPublisher.cs
│       │   └── Events/
│       │       ├── UserRegisteredEventHandler.cs
│       │       └── UserLoggedInEventHandler.cs
│       ├── Configurations/
│       │   ├── DatabaseConfiguration.cs
│       │   ├── JwtConfiguration.cs
│       │   └── RabbitMqConfiguration.cs
│       └── Auth.Infrastructure.csproj
├── tests/
│   ├── Auth.UnitTests/
│   │   ├── Domain/
│   │   │   ├── UserTests.cs
│   │   │   └── EmailTests.cs
│   │   ├── Application/
│   │   │   ├── RegisterUserCommandHandlerTests.cs
│   │   │   └── LoginUserCommandHandlerTests.cs
│   │   └── Auth.UnitTests.csproj
│   │
│   ├── Auth.IntegrationTests/
│   │   ├── Controllers/
│   │   │   └── AuthControllerTests.cs
│   │   ├── Infrastructure/
│   │   │   └── DatabaseTests.cs
│   │   └── Auth.IntegrationTests.csproj
│   │
│   └── Auth.ContractTests/
│       ├── UserRegisteredEventContractTests.cs
│       └── Auth.ContractTests.csproj
├── docker/
│   ├── Dockerfile
│   ├── docker-compose.yml
│   └── docker-compose.override.yml
├── Auth.API.sln
└── README.md
```

## Dependências por Camada

### Auth.API (Presentation Layer)
```xml
<PackageReference Include="Microsoft.AspNetCore.OpenApi" Version="8.0.0" />
<PackageReference Include="Swashbuckle.AspNetCore" Version="6.5.0" />
<PackageReference Include="MediatR" Version="12.0.1" />
<PackageReference Include="FluentValidation.AspNetCore" Version="11.3.0" />
<PackageReference Include="Serilog.AspNetCore" Version="8.0.0" />

<ProjectReference Include="..\Auth.Application\Auth.Application.csproj" />
<ProjectReference Include="..\Auth.Infrastructure\Auth.Infrastructure.csproj" />
```

### Auth.Domain (Domain Layer)
```xml
<!-- Sem dependências externas - apenas .NET base -->
<PackageReference Include="MediatR" Version="12.0.1" />
```

### Auth.Application (Application Layer)
```xml
<PackageReference Include="MediatR" Version="12.0.1" />
<PackageReference Include="FluentValidation" Version="11.9.0" />
<PackageReference Include="AutoMapper" Version="12.0.1" />
<PackageReference Include="Microsoft.Extensions.Logging.Abstractions" Version="8.0.0" />

<ProjectReference Include="..\Auth.Domain\Auth.Domain.csproj" />
```

### Auth.Infrastructure (Infrastructure Layer)
```xml
<PackageReference Include="Microsoft.EntityFrameworkCore.SqlServer" Version="8.0.0" />
<PackageReference Include="Microsoft.EntityFrameworkCore.Tools" Version="8.0.0" />
<PackageReference Include="Microsoft.AspNetCore.Identity.EntityFrameworkCore" Version="8.0.0" />
<PackageReference Include="RabbitMQ.Client" Version="6.8.1" />
<PackageReference Include="Microsoft.Extensions.Configuration" Version="8.0.0" />
<PackageReference Include="Microsoft.Extensions.DependencyInjection" Version="8.0.0" />

<ProjectReference Include="..\Auth.Domain\Auth.Domain.csproj" />
<ProjectReference Include="..\Auth.Application\Auth.Application.csproj" />
```

## Configuração do Dockerfile

```dockerfile
# auth-api/docker/Dockerfile
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["src/Auth.API/Auth.API.csproj", "src/Auth.API/"]
COPY ["src/Auth.Application/Auth.Application.csproj", "src/Auth.Application/"]
COPY ["src/Auth.Domain/Auth.Domain.csproj", "src/Auth.Domain/"]
COPY ["src/Auth.Infrastructure/Auth.Infrastructure.csproj", "src/Auth.Infrastructure/"]

RUN dotnet restore "src/Auth.API/Auth.API.csproj"
COPY . .
WORKDIR "/src/src/Auth.API"
RUN dotnet build "Auth.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Auth.API.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Auth.API.dll"]
```

## Docker Compose por Microsserviço

```yaml
# auth-api/docker/docker-compose.yml
version: '3.8'

services:
  auth-api:
    build:
      context: ..
      dockerfile: docker/Dockerfile
    environment:
      - ASPNETCORE_ENVIRONMENT=Development
      - ASPNETCORE_URLS=http://+:80
      - ConnectionStrings__DefaultConnection=Server=auth-db;Database=AuthDB;User Id=sa;Password=YourPassword123!;TrustServerCertificate=true
      - RabbitMQ__Host=rabbitmq
      - RabbitMQ__Username=guest
      - RabbitMQ__Password=guest
    ports:
      - "5001:80"
    depends_on:
      - auth-db
      - rabbitmq
    networks:
      - auth-network

  auth-db:
    image: mcr.microsoft.com/mssql/server:2022-latest
    environment:
      - ACCEPT_EULA=Y
      - SA_PASSWORD=YourPassword123!
    ports:
      - "1433:1433"
    volumes:
      - auth-db-data:/var/opt/mssql
    networks:
      - auth-network

  rabbitmq:
    image: rabbitmq:3-management-alpine
    ports:
      - "5672:5672"
      - "15672:15672"
    environment:
      - RABBITMQ_DEFAULT_USER=guest
      - RABBITMQ_DEFAULT_PASS=guest
    volumes:
      - rabbitmq-data:/var/lib/rabbitmq
    networks:
      - auth-network

volumes:
  auth-db-data:
  rabbitmq-data:

networks:
  auth-network:
    driver: bridge
```

## Configuração de Eventos (sem projetos compartilhados)

### Definição de Evento no Auth.Domain

```csharp
// Auth.Domain/Events/UserRegisteredEvent.cs
public class UserRegisteredEvent : INotification
{
    public string EventId { get; set; } = Guid.NewGuid().ToString();
    public DateTime Timestamp { get; set; } = DateTime.UtcNow;
    public string EventType { get; set; } = "UserRegistered";
    public UserRegisteredData Data { get; set; }
}

public class UserRegisteredData
{
    public string UserId { get; set; }
    public string Email { get; set; }
    public string UserType { get; set; }
    public DateTime RegistrationDate { get; set; }
}
```

### Publisher no Auth.Infrastructure

```csharp
// Auth.Infrastructure/Messaging/EventPublisher.cs
public class EventPublisher : IEventPublisher
{
    private readonly IConnection _connection;
    private readonly IModel _channel;

    public async Task PublishAsync<T>(T eventData) where T : INotification
    {
        var eventType = typeof(T).Name;
        var message = JsonSerializer.Serialize(eventData);
        var body = Encoding.UTF8.GetBytes(message);

        _channel.BasicPublish(
            exchange: "educational-platform",
            routingKey: $"user.{eventType.ToLower()}",
            basicProperties: null,
            body: body);
    }
}
```

### Consumer em Outro Microsserviço (ex: Alunos.API)

```csharp
// Alunos.Domain/Events/UserRegisteredEvent.cs
// Cada serviço define sua própria versão do evento
public class UserRegisteredEvent
{
    public string EventId { get; set; }
    public DateTime Timestamp { get; set; }
    public string EventType { get; set; }
    public UserRegisteredData Data { get; set; }
}

// Alunos.Infrastructure/Messaging/UserRegisteredEventConsumer.cs
public class UserRegisteredEventConsumer : IConsumer<UserRegisteredEvent>
{
    private readonly IMediator _mediator;

    public async Task Consume(ConsumeContext<UserRegisteredEvent> context)
    {
        var command = new CreateAlunoProfileCommand
        {
            UserId = context.Message.Data.UserId,
            Email = context.Message.Data.Email
        };

        await _mediator.Send(command);
    }
}
```

## Vantagens da Estrutura Independente

### ✅ Benefícios

1. **Deployment Independente**: Cada serviço pode ser implantado sem afetar outros
2. **Desenvolvimento Paralelo**: Equipes podem trabalhar independentemente
3. **Tecnologia Flexível**: Cada serviço pode usar tecnologias diferentes
4. **Escalabilidade Individual**: Escalar apenas os serviços necessários
5. **Falhas Isoladas**: Falha em um serviço não afeta os outros
6. **Testes Independentes**: Cada serviço tem sua própria suíte de testes

### ⚠️ Desafios e Soluções

1. **Duplicação de Código**: 
   - **Problema**: Código similar entre serviços
   - **Solução**: Duplicação intencional, evitar premature optimization

2. **Sincronização de Dados**:
   - **Problema**: Consistência entre serviços
   - **Solução**: Eventual consistency via eventos

3. **Complexidade de Deploy**:
   - **Problema**: Múltiplos serviços para gerenciar
   - **Solução**: Automação via scripts e CI/CD

4. **Transações Distribuídas**:
   - **Problema**: Não há transações ACID entre serviços
   - **Solução**: Saga pattern para transações distribuídas

## Comandos para Criar Estrutura

```bash
# Criar estrutura do Auth API
mkdir -p auth-api/src/Auth.{API,Domain,Application,Infrastructure}
mkdir -p auth-api/tests/Auth.{UnitTests,IntegrationTests,ContractTests}
mkdir -p auth-api/docker

# Criar solution
cd auth-api
dotnet new sln --name Auth.API

# Criar projetos
dotnet new webapi -n Auth.API -o src/Auth.API
dotnet new classlib -n Auth.Domain -o src/Auth.Domain
dotnet new classlib -n Auth.Application -o src/Auth.Application
dotnet new classlib -n Auth.Infrastructure -o src/Auth.Infrastructure

# Adicionar projetos à solution
dotnet sln add src/Auth.API/Auth.API.csproj
dotnet sln add src/Auth.Domain/Auth.Domain.csproj
dotnet sln add src/Auth.Application/Auth.Application.csproj
dotnet sln add src/Auth.Infrastructure/Auth.Infrastructure.csproj

# Configurar referências
cd src/Auth.API
dotnet add reference ../Auth.Application/Auth.Application.csproj
dotnet add reference ../Auth.Infrastructure/Auth.Infrastructure.csproj

cd ../Auth.Application
dotnet add reference ../Auth.Domain/Auth.Domain.csproj

cd ../Auth.Infrastructure
dotnet add reference ../Auth.Domain/Auth.Domain.csproj
dotnet add reference ../Auth.Application/Auth.Application.csproj
```

Esta estrutura garante que cada microsserviço seja verdadeiramente independente, seguindo as melhores práticas de Clean Architecture e DDD, sem comprometer a autonomia dos serviços. 