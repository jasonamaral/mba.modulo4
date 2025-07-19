# Plataforma Educacional - Script de Inicializacao (Windows PowerShell)
# Execucao: .\scripts\start-all.ps1

Write-Host "Iniciando Plataforma Educacional..." -ForegroundColor Green
Write-Host "=================================================" -ForegroundColor Cyan

# Detectar e navegar para a raiz do projeto
$currentPath = Get-Location
$scriptPath = $PSScriptRoot
$projectRoot = Split-Path -Parent $scriptPath

# Se o script foi executado da pasta scripts, voltar para a raiz
if ($currentPath.Path.EndsWith("scripts")) {
    Set-Location -Path $projectRoot
    Write-Host "Navegando para a raiz do projeto: $projectRoot" -ForegroundColor Yellow
}

# Verificar se Docker esta rodando
try {
    docker info | Out-Null
    Write-Host "Docker esta rodando" -ForegroundColor Green
}
catch {
    Write-Host "Docker nao esta rodando. Por favor, inicie o Docker Desktop primeiro." -ForegroundColor Red
    exit 1
}

# Verificar se arquivos docker-compose existem
$composeFile = ""
if (Test-Path "docker-compose-simple.yml") {
    $composeFile = "docker-compose-simple.yml"
    Write-Host "Usando docker-compose-simple.yml" -ForegroundColor Green
} elseif (Test-Path "docker-compose.yml") {
    $composeFile = "docker-compose.yml"
    Write-Host "Usando docker-compose.yml" -ForegroundColor Green
} else {
    Write-Host "Arquivo docker-compose nao encontrado." -ForegroundColor Red
    Write-Host "Caminho atual: $(Get-Location)" -ForegroundColor Yellow
    exit 1
}

Write-Host "Parando containers existentes..." -ForegroundColor Yellow
docker-compose -f $composeFile down

Write-Host "Construindo imagens..." -ForegroundColor Yellow
docker-compose -f $composeFile build --parallel

Write-Host "Iniciando infraestrutura (RabbitMQ, SQL Server, Redis)..." -ForegroundColor Blue
docker-compose -f $composeFile up -d rabbitmq sqlserver redis

Write-Host "Aguardando infraestrutura inicializar (60 segundos)..." -ForegroundColor Yellow
Start-Sleep -Seconds 60

Write-Host "Configurando RabbitMQ..." -ForegroundColor Magenta
# Configurar RabbitMQ via API REST
$rabbitmqBase = "http://localhost:15672/api"
$cred = [Convert]::ToBase64String([Text.Encoding]::ASCII.GetBytes("admin:admin123"))
$headers = @{ Authorization = "Basic $cred"; "Content-Type" = "application/json" }

try {
    # Criar exchange
    $exchangeBody = @{ type = "topic"; durable = $true } | ConvertTo-Json
    Invoke-RestMethod -Uri "$rabbitmqBase/exchanges/%2F/plataforma.events" -Method Put -Headers $headers -Body $exchangeBody

    # Criar filas
    $queueBody = @{ durable = $true; auto_delete = $false } | ConvertTo-Json
    
    Invoke-RestMethod -Uri "$rabbitmqBase/queues/%2F/pagamento-confirmado" -Method Put -Headers $headers -Body $queueBody
    Invoke-RestMethod -Uri "$rabbitmqBase/queues/%2F/matricula-realizada" -Method Put -Headers $headers -Body $queueBody
    Invoke-RestMethod -Uri "$rabbitmqBase/queues/%2F/certificado-gerado" -Method Put -Headers $headers -Body $queueBody
    Invoke-RestMethod -Uri "$rabbitmqBase/queues/%2F/usuario-registrado" -Method Put -Headers $headers -Body $queueBody
    Invoke-RestMethod -Uri "$rabbitmqBase/queues/%2F/curso-finalizado" -Method Put -Headers $headers -Body $queueBody

    # Criar bindings
    $bindingBody = @{ routing_key = "pagamento.confirmado" } | ConvertTo-Json
    Invoke-RestMethod -Uri "$rabbitmqBase/bindings/%2F/e/plataforma.events/q/pagamento-confirmado" -Method Post -Headers $headers -Body $bindingBody
    
    $bindingBody = @{ routing_key = "matricula.realizada" } | ConvertTo-Json
    Invoke-RestMethod -Uri "$rabbitmqBase/bindings/%2F/e/plataforma.events/q/matricula-realizada" -Method Post -Headers $headers -Body $bindingBody
    
    $bindingBody = @{ routing_key = "certificado.gerado" } | ConvertTo-Json
    Invoke-RestMethod -Uri "$rabbitmqBase/bindings/%2F/e/plataforma.events/q/certificado-gerado" -Method Post -Headers $headers -Body $bindingBody
    
    $bindingBody = @{ routing_key = "usuario.registrado" } | ConvertTo-Json
    Invoke-RestMethod -Uri "$rabbitmqBase/bindings/%2F/e/plataforma.events/q/usuario-registrado" -Method Post -Headers $headers -Body $bindingBody
    
    $bindingBody = @{ routing_key = "curso.finalizado" } | ConvertTo-Json
    Invoke-RestMethod -Uri "$rabbitmqBase/bindings/%2F/e/plataforma.events/q/curso-finalizado" -Method Post -Headers $headers -Body $bindingBody

    Write-Host "RabbitMQ configurado com sucesso!" -ForegroundColor Green
}
catch {
    Write-Host "Erro ao configurar RabbitMQ: $($_.Exception.Message)" -ForegroundColor Yellow
    Write-Host "Continuando com a inicializacao..." -ForegroundColor Yellow
}

Write-Host "Iniciando microservicos..." -ForegroundColor Blue
docker-compose -f $composeFile up -d auth-api conteudo-api alunos-api pagamentos-api

Write-Host "Aguardando APIs inicializarem (45 segundos)..." -ForegroundColor Yellow
Start-Sleep -Seconds 45

Write-Host "Iniciando BFF e Frontend..." -ForegroundColor Blue
docker-compose -f $composeFile up -d bff-api frontend

Write-Host "Aguardando inicializacao completa (30 segundos)..." -ForegroundColor Yellow
Start-Sleep -Seconds 30

Write-Host ""
Write-Host "Sistema iniciado com sucesso!" -ForegroundColor Green
Write-Host "=================================================" -ForegroundColor Cyan
Write-Host "URLs de Acesso:" -ForegroundColor White
Write-Host ""
Write-Host "   Frontend:           http://localhost:4200" -ForegroundColor Cyan
Write-Host "   BFF API:           http://localhost:5000" -ForegroundColor Cyan
Write-Host "   Auth API:          http://localhost:5001" -ForegroundColor Cyan
Write-Host "   Conteudo API:      http://localhost:5002" -ForegroundColor Cyan
Write-Host "   Alunos API:        http://localhost:5003" -ForegroundColor Cyan
Write-Host "   Pagamentos API:    http://localhost:5004" -ForegroundColor Cyan
Write-Host ""
Write-Host "Infraestrutura:" -ForegroundColor White
Write-Host ""
Write-Host "   RabbitMQ:          http://localhost:15672 (admin/admin123)" -ForegroundColor Magenta
Write-Host "   SQL Server:        localhost:1433 (sa/PlataformaEducacional123!)" -ForegroundColor Blue
Write-Host "   Redis:             localhost:6379" -ForegroundColor Red
Write-Host ""
Write-Host "Status dos containers:" -ForegroundColor White
docker-compose -f $composeFile ps
Write-Host ""
Write-Host "Para ver logs: docker-compose logs -f [service_name]" -ForegroundColor Yellow
Write-Host "Para parar tudo: .\scripts\stop-all.ps1" -ForegroundColor Red 