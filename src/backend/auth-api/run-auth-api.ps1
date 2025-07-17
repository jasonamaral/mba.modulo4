#!/usr/bin/env pwsh
# Script para executar a Auth API

Write-Host "🚀 Iniciando Auth API..." -ForegroundColor Green

# Verificar se estamos no diretório correto
if (-not (Test-Path "src/Auth.API/Auth.API.csproj")) {
    Write-Host "❌ Erro: Execute este script no diretório raiz do projeto auth-api" -ForegroundColor Red
    Write-Host "Diretório atual: $(Get-Location)" -ForegroundColor Yellow
    exit 1
}

# Ir para o diretório do projeto
Set-Location "src/Auth.API"

Write-Host "📁 Diretório atual: $(Get-Location)" -ForegroundColor Cyan

# Fazer build do projeto
Write-Host "🔨 Fazendo build do projeto..." -ForegroundColor Yellow
dotnet build

if ($LASTEXITCODE -ne 0) {
    Write-Host "❌ Erro no build. Verifique os erros acima." -ForegroundColor Red
    exit 1
}

Write-Host "✅ Build concluído com sucesso!" -ForegroundColor Green

# Executar aplicação
Write-Host "🌐 Executando aplicação..." -ForegroundColor Cyan
Write-Host "URLs disponíveis:" -ForegroundColor Yellow
Write-Host "  - HTTPS: https://localhost:5001/swagger" -ForegroundColor Blue
Write-Host "  - HTTP:  http://localhost:5002/swagger" -ForegroundColor Blue
Write-Host "  - Health: https://localhost:5001/health" -ForegroundColor Blue

Write-Host "`n🔄 Pressione Ctrl+C para parar a aplicação" -ForegroundColor Magenta

# Executar com perfil HTTPS
dotnet run --launch-profile https 