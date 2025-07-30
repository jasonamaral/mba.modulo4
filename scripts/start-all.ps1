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
    Write-Host "Usando docker-compose-simple.yml (DESENVOLVIMENTO)" -ForegroundColor Green
} elseif (Test-Path "docker-compose.yml") {
    $composeFile = "docker-compose.yml"
    Write-Host "Usando docker-compose.yml (PRODUCAO)" -ForegroundColor Green
} else {
    Write-Host "Arquivo docker-compose nao encontrado." -ForegroundColor Red
    Write-Host "Caminho atual: $(Get-Location)" -ForegroundColor Yellow
    exit 1
}

# Criar pasta data se não existir (para SQLite)
if (-not (Test-Path "data")) {
    New-Item -ItemType Directory -Path "data" | Out-Null
    Write-Host "Pasta data criada para arquivos SQLite" -ForegroundColor Green
}

Write-Host "Verificando containers existentes..." -ForegroundColor Yellow
$existingContainers = docker-compose -f $composeFile ps -q

if ($existingContainers) {
    Write-Host "Containers existentes encontrados. Verificando quais estao faltando..." -ForegroundColor Green
} else {
    Write-Host "Nenhum container encontrado. Iniciando todos os servicos..." -ForegroundColor Green
}

Write-Host "Verificando imagens..." -ForegroundColor Yellow
$services = @("auth-api", "conteudo-api", "alunos-api", "pagamentos-api", "bff-api", "frontend")
$missingImages = @()

foreach ($service in $services) {
    $imageName = "mbamodulo4-$service"
    $imageExists = docker images -q $imageName
    
    if (-not $imageExists) {
        $missingImages += $service
        Write-Host "Imagem $imageName nao encontrada" -ForegroundColor Yellow
    } else {
        Write-Host "Imagem $imageName encontrada" -ForegroundColor Green
    }
}

if ($missingImages.Count -gt 0) {
    Write-Host "Construindo imagens faltantes: $($missingImages -join ', ')" -ForegroundColor Yellow
    $buildServices = $missingImages -join " "
    docker-compose -f $composeFile build --parallel $buildServices
} else {
    Write-Host "Todas as imagens ja existem!" -ForegroundColor Green
}

Write-Host "Verificando infraestrutura (RabbitMQ, Redis)..." -ForegroundColor Blue
$infraServices = @("rabbitmq", "redis")
$missingInfra = @()

foreach ($service in $infraServices) {
    $containerName = "plataforma-$service"
    $containerExists = docker ps -q -f "name=$containerName"
    
    if (-not $containerExists) {
        $missingInfra += $service
        Write-Host "Container $containerName nao encontrado" -ForegroundColor Yellow
    } else {
        Write-Host "Container $containerName ja esta rodando" -ForegroundColor Green
    }
}

if ($missingInfra.Count -gt 0) {
    Write-Host "Iniciando infraestrutura faltante: $($missingInfra -join ', ')" -ForegroundColor Blue
    $infraServicesToStart = $missingInfra -join " "
    docker-compose -f $composeFile up -d $infraServicesToStart
    
    if ($missingInfra.Count -gt 0) {
        Write-Host "Aguardando infraestrutura inicializar (15 segundos)..." -ForegroundColor Yellow
        Start-Sleep -Seconds 15
    }
} else {
    Write-Host "Toda infraestrutura ja esta rodando!" -ForegroundColor Green
}

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

Write-Host "Verificando microservicos..." -ForegroundColor Blue
$apiServices = @("auth-api", "conteudo-api", "alunos-api", "pagamentos-api")
$missingApis = @()

foreach ($service in $apiServices) {
    $containerName = "plataforma-$service"
    $containerExists = docker ps -q -f "name=$containerName"
    
    if (-not $containerExists) {
        $missingApis += $service
        Write-Host "Container $containerName nao encontrado" -ForegroundColor Yellow
    } else {
        Write-Host "Container $containerName ja esta rodando" -ForegroundColor Green
    }
}

if ($missingApis.Count -gt 0) {
    Write-Host "Iniciando APIs faltantes: $($missingApis -join ', ')" -ForegroundColor Blue
    $apisToStart = $missingApis -join " "
    docker-compose -f $composeFile up -d $apisToStart
    
    Write-Host "Aguardando APIs inicializarem (20 segundos)..." -ForegroundColor Yellow
    Start-Sleep -Seconds 20
} else {
    Write-Host "Todas as APIs ja estao rodando!" -ForegroundColor Green
}

Write-Host "Verificando BFF e Frontend..." -ForegroundColor Blue
$frontendServices = @("bff-api", "frontend")
$missingFrontend = @()

foreach ($service in $frontendServices) {
    $containerName = "plataforma-$service"
    $containerExists = docker ps -q -f "name=$containerName"
    
    if (-not $containerExists) {
        $missingFrontend += $service
        Write-Host "Container $containerName nao encontrado" -ForegroundColor Yellow
    } else {
        Write-Host "Container $containerName ja esta rodando" -ForegroundColor Green
    }
}

if ($missingFrontend.Count -gt 0) {
    Write-Host "Iniciando BFF e Frontend faltantes: $($missingFrontend -join ', ')" -ForegroundColor Blue
    $frontendToStart = $missingFrontend -join " "
    docker-compose -f $composeFile up -d $frontendToStart
    
    Write-Host "Aguardando inicializacao completa (10 segundos)..." -ForegroundColor Yellow
    Start-Sleep -Seconds 10
} else {
    Write-Host "BFF e Frontend ja estao rodando!" -ForegroundColor Green
}

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
Write-Host "   Redis:             localhost:6379" -ForegroundColor Red
Write-Host ""
Write-Host "Banco de Dados (SQLite):" -ForegroundColor White
Write-Host ""
Write-Host "   Auth DB:           ./data/auth-dev.db" -ForegroundColor Green
Write-Host "   Alunos DB:         ./data/alunos-dev.db" -ForegroundColor Green
Write-Host "   Conteudo DB:       ./data/conteudo-dev.db" -ForegroundColor Green
Write-Host "   Pagamentos DB:     ./data/pagamentos-dev.db" -ForegroundColor Green
Write-Host ""
Write-Host "Status dos containers:" -ForegroundColor White
docker-compose -f $composeFile ps
Write-Host ""
Write-Host "Para ver logs: docker-compose -f $composeFile logs -f [service_name]" -ForegroundColor Yellow
Write-Host "Para parar tudo: docker-compose -f $composeFile down" -ForegroundColor Red
Write-Host ""
Write-Host "NOTA: Sistema rodando em modo DESENVOLVIMENTO com SQLite" -ForegroundColor Yellow 