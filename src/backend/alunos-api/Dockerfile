FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
WORKDIR /app
EXPOSE 5003

FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

# Copiar arquivos de projeto
COPY ["src/Alunos.API/Alunos.API.csproj", "src/Alunos.API/"]
COPY ["src/Alunos.Application/Alunos.Application.csproj", "src/Alunos.Application/"]
COPY ["src/Alunos.Domain/Alunos.Domain.csproj", "src/Alunos.Domain/"]
COPY ["src/Alunos.Infrastructure/Alunos.Infrastructure.csproj", "src/Alunos.Infrastructure/"]

# Restaurar dependências
RUN dotnet restore "src/Alunos.API/Alunos.API.csproj"

# Copiar código fonte
COPY . .

# Build da aplicação
WORKDIR "/src/src/Alunos.API"
RUN dotnet build "Alunos.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Alunos.API.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app

# Instalar curl para health checks
RUN apt-get update && apt-get install -y curl && rm -rf /var/lib/apt/lists/*

# Copiar arquivos publicados
COPY --from=publish /app/publish .

# Definir variáveis de ambiente para HTTP apenas
ENV ASPNETCORE_URLS="http://+:5003"

ENTRYPOINT ["dotnet", "Alunos.API.dll"] 