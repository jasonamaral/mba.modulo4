FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
WORKDIR /app
EXPOSE 5002

FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

# Copiar arquivos de projeto
COPY ["src/Conteudo.API/Conteudo.API.csproj", "src/Conteudo.API/"]
COPY ["src/Conteudo.Application/Conteudo.Application.csproj", "src/Conteudo.Application/"]
COPY ["src/Conteudo.Domain/Conteudo.Domain.csproj", "src/Conteudo.Domain/"]
COPY ["src/Conteudo.Infrastructure/Conteudo.Infrastructure.csproj", "src/Conteudo.Infrastructure/"]

# Restaurar dependências
RUN dotnet restore "src/Conteudo.API/Conteudo.API.csproj"

# Copiar código fonte
COPY . .

# Build da aplicação
WORKDIR "/src/src/Conteudo.API"
RUN dotnet build "Conteudo.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Conteudo.API.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app

# Instalar curl para health checks
RUN apt-get update && apt-get install -y curl && rm -rf /var/lib/apt/lists/*

# Copiar arquivos publicados
COPY --from=publish /app/publish .

# Definir variáveis de ambiente para HTTP apenas
ENV ASPNETCORE_URLS="http://+:5002"

ENTRYPOINT ["dotnet", "Conteudo.API.dll"] 