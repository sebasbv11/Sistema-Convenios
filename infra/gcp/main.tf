locals {
  database_name = "convenios_fcvt"
  database_user = "convenios_app"
  bucket_name   = "${var.project_id}-convenios-documentos"
  required_apis = toset([
    "artifactregistry.googleapis.com",
    "iamcredentials.googleapis.com",
    "run.googleapis.com",
    "secretmanager.googleapis.com",
    "sqladmin.googleapis.com",
    "sts.googleapis.com",
    "storage.googleapis.com"
  ])
}

resource "google_project_service" "apis" {
  for_each           = local.required_apis
  service            = each.value
  disable_on_destroy = false
}

resource "random_password" "database" {
  length  = 32
  special = false
}

resource "random_password" "admin" {
  length           = 24
  special          = true
  override_special = "!@#%_-"
  min_upper        = 2
  min_lower        = 2
  min_numeric      = 2
}

resource "google_artifact_registry_repository" "application" {
  location      = var.region
  repository_id = var.service_name
  description   = "Imágenes del Sistema de Convenios"
  format        = "DOCKER"
  depends_on    = [google_project_service.apis]
}

resource "google_storage_bucket" "documents" {
  name                        = local.bucket_name
  location                    = var.region
  uniform_bucket_level_access = true
  public_access_prevention    = "enforced"
  force_destroy               = false

  versioning {
    enabled = true
  }

  lifecycle_rule {
    condition {
      num_newer_versions = 5
    }
    action {
      type = "Delete"
    }
  }

  depends_on = [google_project_service.apis]
}

resource "google_sql_database_instance" "postgres" {
  name                = "${var.service_name}-postgres"
  region              = var.region
  database_version    = "POSTGRES_16"
  deletion_protection = var.deletion_protection

  settings {
    tier              = var.database_tier
    edition           = "ENTERPRISE"
    availability_type = "ZONAL"
    disk_type         = "PD_SSD"
    disk_size         = 10
    disk_autoresize   = true

    backup_configuration {
      enabled                        = true
      point_in_time_recovery_enabled = true
      start_time                     = "03:00"
      transaction_log_retention_days = 7
      backup_retention_settings {
        retained_backups = 7
        retention_unit   = "COUNT"
      }
    }

    ip_configuration {
      ipv4_enabled = true
    }

    maintenance_window {
      day          = 7
      hour         = 4
      update_track = "stable"
    }
  }

  depends_on = [google_project_service.apis]
}

resource "google_sql_database" "application" {
  name     = local.database_name
  instance = google_sql_database_instance.postgres.name
}

resource "google_sql_user" "application" {
  name     = local.database_user
  instance = google_sql_database_instance.postgres.name
  password = random_password.database.result
}

resource "google_secret_manager_secret" "database_connection" {
  secret_id = "${var.service_name}-database-connection"
  replication {
    auto {}
  }
  depends_on = [google_project_service.apis]
}

resource "google_secret_manager_secret_version" "database_connection" {
  secret = google_secret_manager_secret.database_connection.id
  secret_data = join(";", [
    "Host=/cloudsql/${google_sql_database_instance.postgres.connection_name}",
    "Database=${google_sql_database.application.name}",
    "Username=${google_sql_user.application.name}",
    "Password=${random_password.database.result}",
    "SSL Mode=Disable",
    "Pooling=true",
    "Maximum Pool Size=20"
  ])
}

resource "google_secret_manager_secret" "admin_password" {
  secret_id = "${var.service_name}-initial-admin-password"
  replication {
    auto {}
  }
  depends_on = [google_project_service.apis]
}

resource "google_secret_manager_secret_version" "admin_password" {
  secret      = google_secret_manager_secret.admin_password.id
  secret_data = random_password.admin.result
}

resource "google_service_account" "runtime" {
  account_id   = "${var.service_name}-runtime"
  display_name = "Sistema Convenios - Cloud Run"
}

resource "google_project_iam_member" "runtime_cloudsql" {
  project = var.project_id
  role    = "roles/cloudsql.client"
  member  = "serviceAccount:${google_service_account.runtime.email}"
}

resource "google_secret_manager_secret_iam_member" "runtime_database_secret" {
  secret_id = google_secret_manager_secret.database_connection.id
  role      = "roles/secretmanager.secretAccessor"
  member    = "serviceAccount:${google_service_account.runtime.email}"
}

resource "google_secret_manager_secret_iam_member" "runtime_admin_secret" {
  secret_id = google_secret_manager_secret.admin_password.id
  role      = "roles/secretmanager.secretAccessor"
  member    = "serviceAccount:${google_service_account.runtime.email}"
}

resource "google_storage_bucket_iam_member" "runtime_documents" {
  bucket = google_storage_bucket.documents.name
  role   = "roles/storage.objectAdmin"
  member = "serviceAccount:${google_service_account.runtime.email}"
}

resource "google_cloud_run_v2_service" "application" {
  name                = var.service_name
  location            = var.region
  deletion_protection = false
  ingress             = "INGRESS_TRAFFIC_ALL"

  template {
    service_account = google_service_account.runtime.email
    timeout         = "300s"

    scaling {
      min_instance_count = 0
      max_instance_count = 3
    }

    containers {
      image = var.bootstrap_image

      ports {
        container_port = 8080
      }

      resources {
        limits = {
          cpu    = "1"
          memory = "512Mi"
        }
        cpu_idle = true
      }

      env {
        name  = "ASPNETCORE_ENVIRONMENT"
        value = "Production"
      }
      env {
        name  = "Database__ApplyMigrationsOnStartup"
        value = "false"
      }
      env {
        name  = "DataProtection__Provider"
        value = "Database"
      }
      env {
        name  = "ArchivosConfig__Provider"
        value = "GoogleCloudStorage"
      }
      env {
        name  = "ArchivosConfig__Bucket"
        value = google_storage_bucket.documents.name
      }
      env {
        name  = "SeedAdmin__Enabled"
        value = "true"
      }
      env {
        name  = "SeedAdmin__Email"
        value = var.initial_admin_email
      }
      env {
        name = "ConnectionStrings__DefaultConnection"
        value_source {
          secret_key_ref {
            secret  = google_secret_manager_secret.database_connection.secret_id
            version = google_secret_manager_secret_version.database_connection.version
          }
        }
      }
      env {
        name = "SeedAdmin__Password"
        value_source {
          secret_key_ref {
            secret  = google_secret_manager_secret.admin_password.secret_id
            version = google_secret_manager_secret_version.admin_password.version
          }
        }
      }

      volume_mounts {
        name       = "cloudsql"
        mount_path = "/cloudsql"
      }
    }

    volumes {
      name = "cloudsql"
      cloud_sql_instance {
        instances = [google_sql_database_instance.postgres.connection_name]
      }
    }
  }

  lifecycle {
    ignore_changes = [template[0].containers[0].image]
  }

  depends_on = [
    google_project_service.apis,
    google_project_iam_member.runtime_cloudsql,
    google_secret_manager_secret_iam_member.runtime_database_secret,
    google_secret_manager_secret_iam_member.runtime_admin_secret,
    google_storage_bucket_iam_member.runtime_documents
  ]
}

resource "google_cloud_run_v2_service_iam_member" "public" {
  count    = var.allow_public_access ? 1 : 0
  name     = google_cloud_run_v2_service.application.name
  location = google_cloud_run_v2_service.application.location
  role     = "roles/run.invoker"
  member   = "allUsers"
}

resource "google_service_account" "github_deployer" {
  account_id   = "${var.service_name}-github"
  display_name = "GitHub Actions - Sistema Convenios"
}

resource "google_project_iam_member" "github_run_admin" {
  project = var.project_id
  role    = "roles/run.admin"
  member  = "serviceAccount:${google_service_account.github_deployer.email}"
}

resource "google_artifact_registry_repository_iam_member" "github_writer" {
  location   = google_artifact_registry_repository.application.location
  repository = google_artifact_registry_repository.application.name
  role       = "roles/artifactregistry.writer"
  member     = "serviceAccount:${google_service_account.github_deployer.email}"
}

resource "google_service_account_iam_member" "github_use_runtime" {
  service_account_id = google_service_account.runtime.name
  role               = "roles/iam.serviceAccountUser"
  member             = "serviceAccount:${google_service_account.github_deployer.email}"
}

resource "google_iam_workload_identity_pool" "github" {
  workload_identity_pool_id = "${var.service_name}-github"
  display_name              = "GitHub Sistema Convenios"
  depends_on                = [google_project_service.apis]
}

resource "google_iam_workload_identity_pool_provider" "github" {
  workload_identity_pool_id          = google_iam_workload_identity_pool.github.workload_identity_pool_id
  workload_identity_pool_provider_id = "github"
  display_name                       = "GitHub Actions"
  attribute_mapping = {
    "google.subject"       = "assertion.sub"
    "attribute.repository" = "assertion.repository"
    "attribute.ref"        = "assertion.ref"
  }
  attribute_condition = "assertion.repository == '${var.github_repository}' && assertion.ref == 'refs/heads/main'"
  oidc {
    issuer_uri = "https://token.actions.githubusercontent.com/"
  }
}

resource "google_service_account_iam_member" "github_workload_identity" {
  service_account_id = google_service_account.github_deployer.name
  role               = "roles/iam.workloadIdentityUser"
  member             = "principalSet://iam.googleapis.com/${google_iam_workload_identity_pool.github.name}/attribute.repository/${var.github_repository}"
}
