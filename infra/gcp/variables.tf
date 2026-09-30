variable "project_id" {
  description = "ID del proyecto de Google Cloud con facturación habilitada."
  type        = string
}

variable "region" {
  description = "Región común para Cloud Run, Cloud SQL, Artifact Registry y Storage."
  type        = string
  default     = "southamerica-west1"
}

variable "github_repository" {
  description = "Repositorio autorizado para desplegar mediante OIDC."
  type        = string
  default     = "sebasbv11/Sistema-Convenios"
}

variable "service_name" {
  type    = string
  default = "sistema-convenios"
}

variable "database_tier" {
  description = "Tier de Cloud SQL para staging. Ajustar antes de producción."
  type        = string
  default     = "db-f1-micro"
}

variable "initial_admin_email" {
  type    = string
  default = "admin@fcvt.edu.ec"
}

variable "bootstrap_image" {
  description = "Imagen temporal; el primer workflow la reemplaza por la aplicación."
  type        = string
  default     = "us-docker.pkg.dev/cloudrun/container/hello"
}

variable "allow_public_access" {
  description = "Permite abrir la pantalla de login desde Internet."
  type        = bool
  default     = true
}

variable "deletion_protection" {
  description = "Evita eliminar accidentalmente Cloud SQL."
  type        = bool
  default     = true
}
