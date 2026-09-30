output "cloud_run_url" {
  value = google_cloud_run_v2_service.application.uri
}

output "project_id" {
  value = var.project_id
}

output "region" {
  value = var.region
}

output "cloud_run_service" {
  value = var.service_name
}

output "artifact_registry_repository" {
  value = google_artifact_registry_repository.application.repository_id
}

output "documents_bucket" {
  value = google_storage_bucket.documents.name
}

output "cloud_sql_connection_name" {
  value = google_sql_database_instance.postgres.connection_name
}

output "database_secret_id" {
  value = google_secret_manager_secret.database_connection.secret_id
}

output "admin_password_secret_id" {
  value = google_secret_manager_secret.admin_password.secret_id
}

output "runtime_service_account" {
  value = google_service_account.runtime.email
}

output "github_deploy_service_account" {
  value = google_service_account.github_deployer.email
}

output "github_workload_identity_provider" {
  value = google_iam_workload_identity_pool_provider.github.name
}

output "initial_admin_email" {
  value = var.initial_admin_email
}

output "initial_admin_password" {
  value     = random_password.admin.result
  sensitive = true
}
