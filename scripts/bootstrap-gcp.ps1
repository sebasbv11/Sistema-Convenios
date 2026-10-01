param(
    [Parameter(Mandatory = $true)]
    [string]$ProjectId,
    [string]$Region = "southamerica-west1"
)

$ErrorActionPreference = "Stop"
foreach ($command in @("gcloud", "terraform")) {
    if (-not (Get-Command $command -ErrorAction SilentlyContinue)) {
        throw "$command no está instalado o no está en PATH."
    }
}

$terraformDirectory = Join-Path $PSScriptRoot "..\infra\gcp"
$variablesFile = Join-Path $terraformDirectory "terraform.tfvars"
$stateBucket = "$ProjectId-sistema-convenios-tfstate"

gcloud.cmd config set project $ProjectId
gcloud.cmd services enable storage.googleapis.com

$previousErrorActionPreference = $ErrorActionPreference
$ErrorActionPreference = "Continue"
gcloud.cmd storage buckets describe "gs://$stateBucket" 2>$null
$bucketExists = $LASTEXITCODE -eq 0
$ErrorActionPreference = $previousErrorActionPreference
if (-not $bucketExists) {
    gcloud.cmd storage buckets create "gs://$stateBucket" `
        --project $ProjectId `
        --location $Region `
        --uniform-bucket-level-access
    gcloud.cmd storage buckets update "gs://$stateBucket" --versioning
}

if (-not (Test-Path $variablesFile)) {
    Copy-Item (Join-Path $terraformDirectory "terraform.tfvars.example") $variablesFile
    (Get-Content $variablesFile -Raw).Replace("reemplazar-proyecto-gcp", $ProjectId) |
        Set-Content $variablesFile
}

Push-Location $terraformDirectory
try {
    terraform init -reconfigure -backend-config="bucket=$stateBucket"
    terraform fmt -recursive
    terraform validate
}
finally {
    Pop-Location
}

Write-Host "Bootstrap listo. Revisa infra/gcp/terraform.tfvars y ejecuta terraform plan."
