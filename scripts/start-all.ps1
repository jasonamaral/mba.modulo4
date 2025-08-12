param(
    [switch]$ForceBuild,
    [switch]$CleanBuild
)

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

# Escolher compose file
$composeFile = ""
if (Test-Path "docker-compose-simple.yml") {
    $composeFile = "docker-compose-simple.yml"
    Write-Host "Usando docker-compose-simple.yml (DESENVOLVIMENTO)" -ForegroundColor Green
} elseif (Test-Path "docker-compose.yml") {
    $composeFile = "docker-compose.yml"
    Write-Host "Usando docker-compose.yml (PRODUCAO)" -ForegroundColor Green
} else {
    Write-Host "Arquivo docker-compose nao encontrado." -ForegroundColor Red
    exit 1
}

# Limpeza total se CleanBuild estiver ativo
if ($CleanBuild) {
    Write-Host "Limpando containers e imagens antigas..." -ForegroundColor Red
    docker-compose -f $composeFile down --volumes --remove-orphans
    $oldImages = docker images -q "mbamodulo4-*"
    if ($oldImages) {
        docker rmi -f $oldImages
    }
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

# Verificar se precisa fazer build
$needsBuild = $false
if ($ForceBuild -or $CleanBuild) {
    $needsBuild = $true
    Write-Host "Reconstruindo todas as imagens..." -ForegroundColor Yellow
} elseif ($missingImages.Count -gt 0) {
    $needsBuild = $true
    Write-Host "Construindo imagens faltantes: $($missingImages -join ', ')" -ForegroundColor Yellow
} else {
    Write-Host "Todas as imagens ja existem!" -ForegroundColor Green
}

# Fazer build se necessário
if ($needsBuild) {
    if ($ForceBuild -or $CleanBuild) {
        docker-compose -f $composeFile build --no-cache --parallel
    } elseif ($missingImages.Count -gt 0) {
        $buildServices = $missingImages -join " "
        docker-compose -f $composeFile build --parallel $buildServices
    }
}

Write-Host "Verificando infraestrutura (RabbitMQ, Redis)..." -ForegroundColor Blue
$infraServices = @("rabbitmq", "redis")
$missingInfra = @()

# Verificar containers usando docker-compose ps
$runningContainers = docker-compose -f $composeFile ps -q

foreach ($service in $infraServices) {
    $containerName = "plataforma-$service"
    $containerExists = $false
    
    if ($runningContainers) {
        foreach ($containerId in $runningContainers) {
            try {
                $containerInfo = docker inspect $containerId --format='{{.Name}}' 2>$null
                if ($containerInfo -and $containerInfo.Contains($containerName)) {
                    $containerExists = $true
                    break
                }
            } catch {
                # Ignorar erros de containers que foram removidos
            }
        }
    }
    
    if (-not $containerExists) {
        $missingInfra += $service
        Write-Host "Container $containerName nao encontrado" -ForegroundColor Yellow
    } else {
        Write-Host "Container $containerName ja esta rodando" -ForegroundColor Green
    }
}

function Wait-ContainerHealthy {
    param([string]$containerName, [int]$timeoutSec = 90)
    $elapsed = 0
    Write-Host "Aguardando '$containerName' ficar HEALTHY (timeout ${timeoutSec}s)..." -ForegroundColor Yellow
    while ($elapsed -lt $timeoutSec) {
        $status = docker inspect --format='{{.State.Health.Status}}' $containerName 2>$null
        if ($status -eq 'healthy') {
            Write-Host "Container '$containerName' HEALTHY" -ForegroundColor Green
            return $true
        }
        Start-Sleep -Seconds 3
        $elapsed += 3
    }
    Write-Host "Timeout aguardando '$containerName' HEALTHY. Prosseguindo assim mesmo." -ForegroundColor Yellow
    return $false
}

if ($missingInfra.Count -gt 0) {
    Write-Host "Iniciando infraestrutura faltante: $($missingInfra -join ', ')" -ForegroundColor Blue
    docker-compose -f $composeFile up -d $missingInfra
} else {
    Write-Host "Toda infraestrutura ja esta rodando!" -ForegroundColor Green
}

# Aguarda RabbitMQ ficar 'healthy' para evitar timeout nas APIs ao registrar filas
$rabbitContainer = "plataforma-rabbitmq"
try { Wait-ContainerHealthy -containerName $rabbitContainer -timeoutSec 120 | Out-Null } catch { }

Write-Host "Verificando microservicos..." -ForegroundColor Blue
$apiServices = @("auth-api", "conteudo-api", "alunos-api", "pagamentos-api")
$missingApis = @()

# Atualizar lista de containers rodando
$runningContainers = docker-compose -f $composeFile ps -q

foreach ($service in $apiServices) {
    $containerName = "plataforma-$service"
    $containerExists = $false
    
    if ($runningContainers) {
        foreach ($containerId in $runningContainers) {
            try {
                $containerInfo = docker inspect $containerId --format='{{.Name}}' 2>$null
                if ($containerInfo -and $containerInfo.Contains($containerName)) {
                    $containerExists = $true
                    break
                }
            } catch {
                # Ignorar erros de containers que foram removidos
            }
        }
    }
    
    if (-not $containerExists) {
        $missingApis += $service
        Write-Host "Container $containerName nao encontrado" -ForegroundColor Yellow
    } else {
        Write-Host "Container $containerName ja esta rodando" -ForegroundColor Green
    }
}

if ($missingApis.Count -gt 0) {
    Write-Host "Iniciando APIs faltantes: $($missingApis -join ', ')" -ForegroundColor Blue
    docker-compose -f $composeFile up -d $missingApis
    
    Write-Host "Aguardando APIs inicializarem (20 segundos)..." -ForegroundColor Yellow
    Start-Sleep -Seconds 20
} else {
    Write-Host "Todas as APIs ja estao rodando!" -ForegroundColor Green
}

Write-Host "Verificando BFF e Frontend..." -ForegroundColor Blue
$frontendServices = @("bff-api", "frontend")
$missingFrontend = @()

# Atualizar lista de containers rodando
$runningContainers = docker-compose -f $composeFile ps -q

foreach ($service in $frontendServices) {
    $containerName = "plataforma-$service"
    $containerExists = $false
    
    if ($runningContainers) {
        foreach ($containerId in $runningContainers) {
            try {
                $containerInfo = docker inspect $containerId --format='{{.Name}}' 2>$null
                if ($containerInfo -and $containerInfo.Contains($containerName)) {
                    $containerExists = $true
                    break
                }
            } catch {
                # Ignorar erros de containers que foram removidos
            }
        }
    }
    
    if (-not $containerExists) {
        $missingFrontend += $service
        Write-Host "Container $containerName nao encontrado" -ForegroundColor Yellow
    } else {
        Write-Host "Container $containerName ja esta rodando" -ForegroundColor Green
    }
}

if ($missingFrontend.Count -gt 0) {
    Write-Host "Iniciando BFF e Frontend faltantes: $($missingFrontend -join ', ')" -ForegroundColor Blue
    docker-compose -f $composeFile up -d $missingFrontend
    
    Write-Host "Aguardando inicializacao completa (10 segundos)..." -ForegroundColor Yellow
    Start-Sleep -Seconds 10
} else {
    Write-Host "BFF e Frontend ja estao rodando!" -ForegroundColor Green
}

# Recriação orquestrada (evita subir APIs antes do RabbitMQ)
$totalMissing = $missingInfra.Count + $missingApis.Count + $missingFrontend.Count
if ($ForceBuild -or $CleanBuild) {
    Write-Host ""; Write-Host "Recriando infraestrutura..." -ForegroundColor Yellow
    docker-compose -f $composeFile up -d --force-recreate rabbitmq redis
    try { Wait-ContainerHealthy -containerName $rabbitContainer -timeoutSec 120 | Out-Null } catch { }

    Write-Host "Recriando APIs..." -ForegroundColor Yellow
    docker-compose -f $composeFile up -d --force-recreate auth-api conteudo-api alunos-api pagamentos-api
    Start-Sleep -Seconds 10

    Write-Host "Recriando BFF e Frontend..." -ForegroundColor Yellow
    docker-compose -f $composeFile up -d --force-recreate bff-api frontend
} elseif ($totalMissing -gt 0) {
    Write-Host ""; Write-Host "Recriando apenas servicos faltantes, em ordem..." -ForegroundColor Yellow
    if ($missingInfra.Count -gt 0) {
        docker-compose -f $composeFile up -d --force-recreate $missingInfra
        try { Wait-ContainerHealthy -containerName $rabbitContainer -timeoutSec 120 | Out-Null } catch { }
    }
    if ($missingApis.Count -gt 0) {
        docker-compose -f $composeFile up -d --force-recreate $missingApis
        Start-Sleep -Seconds 10
    }
    if ($missingFrontend.Count -gt 0) {
        docker-compose -f $composeFile up -d --force-recreate $missingFrontend
    }
} else {
    Write-Host ""; Write-Host "Todos os containers ja estao rodando!" -ForegroundColor Green
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
