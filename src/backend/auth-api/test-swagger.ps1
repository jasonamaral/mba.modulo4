#!/usr/bin/env pwsh
# Script para testar o Swagger da Auth API

Write-Host "🔍 Testando Swagger da Auth API..." -ForegroundColor Green

# Função para testar uma URL
function Test-Url {
    param(
        [string]$Url,
        [string]$Description
    )
    
    Write-Host "Testando $Description..." -ForegroundColor Yellow
    
    try {
        if ($Url -like "https://*") {
            $response = Invoke-RestMethod -Uri $Url -SkipCertificateCheck -ErrorAction Stop
        } else {
            $response = Invoke-RestMethod -Uri $Url -ErrorAction Stop
        }
        
        Write-Host "✅ $Description: OK" -ForegroundColor Green
        return $true
    }
    catch {
        Write-Host "❌ $Description: ERRO - $($_.Exception.Message)" -ForegroundColor Red
        return $false
    }
}

# Aguardar alguns segundos para a aplicação inicializar
Write-Host "Aguardando 5 segundos para a aplicação inicializar..." -ForegroundColor Cyan
Start-Sleep -Seconds 5

# Testar endpoints
$results = @()

# Testar Health Check
$results += Test-Url -Url "https://localhost:5001/health" -Description "Health Check (HTTPS)"
$results += Test-Url -Url "http://localhost:5002/health" -Description "Health Check (HTTP)"

# Testar Swagger JSON
$results += Test-Url -Url "https://localhost:5001/swagger/v1/swagger.json" -Description "Swagger JSON (HTTPS)"
$results += Test-Url -Url "http://localhost:5002/swagger/v1/swagger.json" -Description "Swagger JSON (HTTP)"

# Resumo
Write-Host "`n📊 Resumo dos Testes:" -ForegroundColor Cyan
$sucessos = ($results | Where-Object { $_ -eq $true }).Count
$total = $results.Count

Write-Host "Sucessos: $sucessos/$total" -ForegroundColor $(if ($sucessos -eq $total) { "Green" } else { "Yellow" })

if ($sucessos -gt 0) {
    Write-Host "`n🎉 Swagger URLs para testar no navegador:" -ForegroundColor Green
    Write-Host "https://localhost:5001/swagger" -ForegroundColor Blue
    Write-Host "http://localhost:5002/swagger" -ForegroundColor Blue
} else {
    Write-Host "`n❌ Nenhum endpoint está respondendo. Verifique se a aplicação está rodando." -ForegroundColor Red
}

Write-Host "`n📝 Para executar a aplicação:" -ForegroundColor Cyan
Write-Host "cd src/Auth.API" -ForegroundColor Gray
Write-Host "dotnet run --launch-profile https" -ForegroundColor Gray 