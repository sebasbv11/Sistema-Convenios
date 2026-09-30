# Manual de despliegue controlado en Google Cloud

## Arquitectura

- **Cloud Run:** aplicación ASP.NET Core .NET 10, sin estado local.
- **Cloud SQL PostgreSQL 16:** datos, usuarios, roles y claves compartidas de sesión.
- **Cloud Storage:** documentos PDF en un bucket privado y versionado.
- **Secret Manager:** conexión de base de datos y contraseña inicial del administrador.
- **Artifact Registry:** imágenes Docker inmutables identificadas por commit.
- **GitHub Actions + Workload Identity Federation:** despliegue sin llaves de servicio permanentes.

Todos los recursos se crean en la misma región. El acceso de los dispositivos ocurre únicamente por HTTPS a Cloud Run; PostgreSQL y el bucket no se exponen directamente a los usuarios.

## Requisitos

1. Proyecto de Google Cloud con facturación habilitada.
2. Permisos para habilitar APIs, administrar IAM, Cloud SQL, Cloud Run, Storage y Secret Manager.
3. Google Cloud CLI, Terraform y GitHub CLI instalados.
4. Sesiones iniciadas con `gcloud auth login`, `gcloud auth application-default login` y `gh auth login`.

## 1. Preparar el estado de Terraform

Desde la raíz del repositorio:

```powershell
.\scripts\bootstrap-gcp.ps1 -ProjectId "ID-DEL-PROYECTO"
```

El script crea un bucket privado y versionado para el estado remoto. También genera `infra/gcp/terraform.tfvars`, archivo excluido de Git.

## 2. Revisar y crear la infraestructura

Edita `infra/gcp/terraform.tfvars`, especialmente proyecto, región, repositorio y correo administrador.

```powershell
cd infra\gcp
terraform plan -out staging.tfplan
terraform apply staging.tfplan
cd ..\..
```

Terraform crea Cloud SQL con backups y recuperación a un punto en el tiempo, bucket privado, secretos, Artifact Registry, cuentas de servicio, federación OIDC y el servicio de Cloud Run con una imagen temporal.

## 3. Configurar GitHub

```powershell
.\scripts\configure-github-gcp.ps1 -Environment staging
```

En GitHub, abre **Settings > Environments > staging** y añade revisores obligatorios. Así, ningún despliegue se ejecuta sin aprobación.

El despliegue automático permanece desactivado al subir el código por primera vez. Una vez creada la infraestructura, configuradas las variables anteriores y añadidos los revisores, actívalo con una variable del repositorio:

```powershell
gh variable set GCP_STAGING_DEPLOY_ENABLED --body true
```

Para desactivarlo de nuevo: `gh variable set GCP_STAGING_DEPLOY_ENABLED --body false`. Los planes `*.tfplan`, el estado de Terraform y `terraform.tfvars` quedan excluidos de Git porque pueden contener datos sensibles.

## 4. Ejecutar el primer despliegue

Haz push a `main` después de que la revisión local sea aprobada. CI siempre se ejecuta; el despliegue automático solo se inicia si `GCP_STAGING_DEPLOY_ENABLED` vale `true`. El flujo realiza, en orden:

1. CI: restauración, compilación, pruebas, migraciones pendientes y construcción Docker.
2. Autenticación OIDC contra Google Cloud.
3. Publicación de una imagen inmutable en Artifact Registry.
4. Ejecución de un Cloud Run Job que aplica las migraciones.
5. Despliegue de la nueva revisión de Cloud Run.
6. Comprobación de `/health/ready`.

También puede iniciarse manualmente desde **Actions > Desplegar staging GCP**.

## 5. Obtener el acceso inicial

```powershell
cd infra\gcp
terraform output initial_admin_email
terraform output -raw initial_admin_password
```

Cambia la contraseña al ingresar por primera vez. No copies el valor a archivos ni mensajes.

## Operación y contingencia

- **Rollback de aplicación:** selecciona una revisión anterior en Cloud Run y dirige hacia ella el 100 % del tráfico.
- **Base de datos:** usa backups automáticos y recuperación a un punto en el tiempo de Cloud SQL.
- **PDF:** Cloud Storage mantiene versiones anteriores; el acceso público está bloqueado.
- **Secretos:** crea una nueva versión en Secret Manager y despliega una nueva revisión.
- **Migraciones:** conserva `scripts/database/SistemaConvenios-idempotent.sql` como evidencia y alternativa para ejecución por DBA.
- **Destrucción:** Cloud SQL tiene protección contra eliminación. Desactívala conscientemente antes de cualquier `terraform destroy`.

## Variables principales de ejecución

| Variable | Procedencia |
|---|---|
| `ConnectionStrings__DefaultConnection` | Secret Manager |
| `SeedAdmin__Password` | Secret Manager |
| `SeedAdmin__Email` | Terraform/GitHub Environment |
| `ArchivosConfig__Provider` | `GoogleCloudStorage` |
| `ArchivosConfig__Bucket` | Terraform |
| `DataProtection__Provider` | `Database` |
| `Database__ApplyMigrationsOnStartup` | `false` |

## Límites del entorno

Este diseño está preparado para **staging controlado**. Antes de declararlo producción institucional se deben completar observabilidad OpenTelemetry, pruebas E2E/Testcontainers, revisión institucional de costos, dominio oficial, SMTP y políticas formales de retención.
