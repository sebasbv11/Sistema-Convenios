# Sistema de Convenios FCVT

Aplicación ASP.NET Core MVC sobre .NET 10, PostgreSQL y una arquitectura DDD por capas.

## Ejecutar

Requisitos:

- .NET SDK 10
- Docker Desktop

Desde PowerShell:

```powershell
Copy-Item .env.example .env
dotnet user-secrets set "ConnectionStrings:DefaultConnection" "Host=localhost;Port=5433;Database=convenios_fcvt;Username=postgres;Password=CAMBIAR"
dotnet user-secrets set "SeedAdmin:Enabled" "true"
dotnet user-secrets set "SeedAdmin:Email" "administrador@ejemplo.edu.ec"
dotnet user-secrets set "SeedAdmin:Password" "CAMBIAR-POR-UNA-CONTRASENA-SEGURA"
.\scripts\start.ps1
```

Edita `.env` y usa allí la misma contraseña local de PostgreSQL. Los archivos
`.env` y `appsettings.Development.json` están excluidos de Git. En despliegue,
inyecta estos valores mediante un gestor de secretos y variables de entorno.

La aplicación queda disponible en `http://localhost:5070`.

El script reutiliza automáticamente el contenedor
`sistema-convenios-postgres` si ya existe; si no existe, lo crea con Docker
Compose y espera a que la base esté disponible antes de iniciar la aplicación.

En desarrollo, el administrador inicial se crea únicamente cuando `SeedAdmin`
está habilitado mediante User Secrets. Cambia esa contraseña en el primer ingreso.

El rol `Admin` dispone del menú **Usuarios y roles** para crear cuentas, asignar
los roles `Admin`, `Secretaria` o `Visualizador`, activar, desactivar, desbloquear
y restablecer contraseñas. El sistema impide desactivar la propia cuenta o dejar
la aplicación sin un administrador activo.

## Gestión contractual

Desde el detalle de un convenio, el botón **Gestión contractual** permite registrar:

- múltiples partes, firmantes, supervisores y responsables;
- fechas de suscripción y firma, ámbitos, presupuesto, financiamiento y controversias;
- cláusulas y obligaciones con evidencia de cumplimiento;
- actividades, estudiantes, seguro, horas, evaluaciones y certificados;
- convenios relacionados, adendas, modificaciones y evaluaciones de seguimiento;
- terminación y cierre con obligaciones pendientes.

En desarrollo, las migraciones se aplican automáticamente. En despliegue deben
ejecutarse de forma controlada mediante un script SQL idempotente antes de iniciar
la nueva versión de la aplicación. El script generado se encuentra en
`scripts/database/SistemaConvenios-idempotent.sql`.

En producción configura `DataProtection__KeysPath` con una carpeta persistente y
compartida por todas las instancias de la aplicación; en desarrollo las claves se
guardan automáticamente bajo `obj/DataProtectionKeys`.

## Reglas nuevas de convenios

- Estados válidos: `Vigente`, `Por vencer`, `Terminado`, `Renovado automático`.
- Los convenios `Específico` deben suscribirse a un convenio `Marco` o `Internacional` existente.
- El convenio base se guarda con relación recursiva (`ConvenioPadreId`) y sus representantes se copian al específico.
- El área promotora se selecciona desde catálogo con código (`VIN-001`, `PRA-001`, `INV-001`, `ACA-001`).
- Entidades registran provincia, ciudad, teléfono de representante y contacto de gestión.
- El menú **Reportes** permite consultar por vencer, vigentes y específicos, filtrando por empresa, provincia, ciudad, estado y rango de fechas.

Si Docker Desktop está cerrado, primero ábrelo y luego ejecuta:

```powershell
.\scripts\start.ps1
```

También puedes aplicar migraciones manualmente:

```powershell
.\.tools\dotnet-ef.exe database update --no-build --project src\SistemaConvenios.Infrastructure\SistemaConvenios.Infrastructure.csproj --startup-project SistemaConvenios.csproj --context ApplicationDbContext
```

## Verificación

```powershell
dotnet build SistemaConvenios.sln
dotnet test SistemaConvenios.sln
```

La solución incluye pruebas de dominio e integración web.

La arquitectura se describe en [ARCHITECTURE.md](ARCHITECTURE.md).
El despliegue controlado en Google Cloud se documenta en
[MANUAL_DESPLIEGUE_GCP.md](MANUAL_DESPLIEGUE_GCP.md).

## Imagen y entorno controlado

El repositorio incluye un `Dockerfile`, validación automática en GitHub Actions y
publicación de imágenes en GitHub Container Registry. Para levantar el entorno de
despliegue local por primera vez:

```powershell
docker compose -f docker-compose.deploy.yml up -d postgres
Get-Content scripts/database/SistemaConvenios-idempotent.sql -Raw | docker compose -f docker-compose.deploy.yml exec -T postgres psql -U postgres -d convenios_fcvt
docker compose -f docker-compose.deploy.yml --profile app up -d web
```

La aplicación queda en `http://localhost:8080` y su comprobación de vida en
`http://localhost:8080/health`. Este entorno sirve para pruebas controladas; las
credenciales reales deben inyectarse mediante secretos y no guardarse en Git.
