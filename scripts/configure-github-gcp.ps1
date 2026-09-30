param(
    [string]$Environment = "staging"
)

$ErrorActionPreference = "Stop"
$terraformDirectory = Join-Path $PSScriptRoot "..\infra\gcp"

if (-not (Get-Command terraform -ErrorAction SilentlyContinue)) {
    throw "Terraform no está instalado o no está en PATH."
}
if (-not (Get-Command gh -ErrorAction SilentlyContinue)) {
    throw "GitHub CLI (gh) no está instalado o no está en PATH."
}

Push-Location $terraformDirectory
try {
    $outputs = terraform output -json | ConvertFrom-Json
}
finally {
    Pop-Location
}

$variables = @{
    GCP_PROJECT_ID                    = $outputs.project_id.value
    GCP_REGION                        = $outputs.region.value
    GCP_CLOUD_RUN_SERVICE             = $outputs.cloud_run_service.value
    GCP_ARTIFACT_REPOSITORY           = $outputs.artifact_registry_repository.value
    GCP_RUNTIME_SERVICE_ACCOUNT       = $outputs.runtime_service_account.value
    GCP_CLOUD_SQL_CONNECTION_NAME     = $outputs.cloud_sql_connection_name.value
    GCP_DATABASE_SECRET_ID            = $outputs.database_secret_id.value
    GCP_ADMIN_PASSWORD_SECRET_ID      = $outputs.admin_password_secret_id.value
    GCP_DOCUMENTS_BUCKET              = $outputs.documents_bucket.value
    GCP_WORKLOAD_IDENTITY_PROVIDER    = $outputs.github_workload_identity_provider.value
    GCP_DEPLOY_SERVICE_ACCOUNT        = $outputs.github_deploy_service_account.value
    GCP_INITIAL_ADMIN_EMAIL           = $outputs.initial_admin_email.value
}

gh api --method PUT "repos/{owner}/{repo}/environments/$Environment" | Out-Null
foreach ($entry in $variables.GetEnumerator()) {
    gh variable set $entry.Key --env $Environment --body ([string]$entry.Value)
}

Write-Host "Variables de GitHub configuradas para el entorno '$Environment'."
