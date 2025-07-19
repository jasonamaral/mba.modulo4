FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
WORKDIR /app
EXPOSE 5000

FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

# Copiar arquivos de projeto
COPY ["src/BFF.API/BFF.API.csproj", "src/BFF.API/"]
COPY ["src/BFF.Application/BFF.Application.csproj", "src/BFF.Application/"]
COPY ["src/BFF.Domain/BFF.Domain.csproj", "src/BFF.Domain/"]
COPY ["src/BFF.Infrastructure/BFF.Infrastructure.csproj", "src/BFF.Infrastructure/"]

# Restaurar dependências
RUN dotnet restore "src/BFF.API/BFF.API.csproj"

# Copiar código fonte
COPY . .

# Build da aplicação
WORKDIR "/src/src/BFF.API"
RUN dotnet build "BFF.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "BFF.API.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app

# Instalar curl para health checks
RUN apt-get update && apt-get install -y curl && rm -rf /var/lib/apt/lists/*

# Copiar arquivos publicados
COPY --from=publish /app/publish .

# Definir variáveis de ambiente
ENV ASPNETCORE_URLS="http://+:5000"

ENTRYPOINT ["dotnet", "BFF.API.dll"] 