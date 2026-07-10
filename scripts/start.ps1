$ErrorActionPreference = "Stop"

$root = Split-Path -Parent $PSScriptRoot
$containerName = "sistema-convenios-postgres"

function Invoke-Checked {
    param(
        [Parameter(Mandatory)]
        [scriptblock] $Command,

        [Parameter(Mandatory)]
        [string] $ErrorMessage
    )

    & $Command
    if ($LASTEXITCODE -ne 0) {
        throw $ErrorMessage
    }
}

function Test-DockerEngine {
    $previousPreference = $ErrorActionPreference
    try {
        $ErrorActionPreference = "SilentlyContinue"
        docker version --format "{{.Server.Version}}" 2>$null | Out-Null
        return $LASTEXITCODE -eq 0
    }
    finally {
        $ErrorActionPreference = $previousPreference
    }
}

Push-Location $root
try {
    if (-not (Test-DockerEngine)) {
        throw "Docker Desktop no está iniciado. Ábrelo y vuelve a ejecutar el script."
    }

    $existingContainer = docker ps -a `
        --filter "name=^/$containerName$" `
        --format "{{.Names}}"

    if ($existingContainer -eq $containerName) {
        $running = docker inspect `
            --format "{{.State.Running}}" `
            $containerName

        if ($running -ne "true") {
            Write-Host "Iniciando el contenedor PostgreSQL existente..."
            Invoke-Checked { docker start $containerName | Out-Null } `
                "No se pudo iniciar el contenedor PostgreSQL existente."
        }
        else {
            Write-Host "El contenedor PostgreSQL ya está iniciado."
        }
    }
    else {
        Write-Host "Creando el contenedor PostgreSQL con Docker Compose..."
        Invoke-Checked { docker compose up -d postgres } `
            "No se pudo crear o iniciar PostgreSQL con Docker Compose."
    }

    Write-Host "Esperando a que PostgreSQL esté disponible..."
    $databaseReady = $false

    for ($attempt = 1; $attempt -le 30; $attempt++) {
        docker exec $containerName `
            pg_isready -U postgres -d convenios_fcvt *> $null

        if ($LASTEXITCODE -eq 0) {
            $databaseReady = $true
            break
        }

        Start-Sleep -Seconds 2
    }

    if (-not $databaseReady) {
        docker logs --tail 40 $containerName
        throw "PostgreSQL no quedó disponible después de 60 segundos."
    }

    Write-Host "PostgreSQL listo."

    $applicationUrl = "http://localhost:5070"
    $listener = Get-NetTCPConnection `
        -LocalPort 5070 `
        -State Listen `
        -ErrorAction SilentlyContinue |
        Select-Object -First 1

    if ($null -ne $listener) {
        try {
            $response = Invoke-WebRequest `
                -Uri $applicationUrl `
                -UseBasicParsing `
                -TimeoutSec 5

            if ($response.StatusCode -ge 200 -and $response.StatusCode -lt 400) {
                Write-Host "La aplicación ya está activa en $applicationUrl"
                Write-Host "No es necesario iniciar otra instancia."
                return
            }
        }
        catch {
            $process = Get-Process `
                -Id $listener.OwningProcess `
                -ErrorAction SilentlyContinue
            $processName = if ($null -ne $process) {
                $process.ProcessName
            }
            else {
                "desconocido"
            }

            throw "El puerto 5070 está ocupado por '$processName' " +
                "(PID $($listener.OwningProcess)), pero no responde como la aplicación."
        }
    }

    Invoke-Checked { dotnet restore SistemaConvenios.csproj } `
        "No se pudieron restaurar las dependencias."

    Write-Host "Iniciando Sistema de Convenios en $applicationUrl ..."
    Invoke-Checked {
        dotnet run `
            --project SistemaConvenios.csproj `
            --launch-profile http `
            --no-restore
    } "La aplicación terminó con un error."
}
finally {
    Pop-Location
}
