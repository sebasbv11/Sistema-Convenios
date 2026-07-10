# Sistema de Convenios FCVT

Aplicación ASP.NET Core MVC sobre .NET 10, PostgreSQL y una arquitectura DDD por capas.

## Ejecutar

Requisitos:

- .NET SDK 10
- Docker Desktop

Desde PowerShell:

```powershell
.\scripts\start.ps1
```

La aplicación queda disponible en `http://localhost:5070`.

El script reutiliza automáticamente el contenedor
`sistema-convenios-postgres` si ya existe; si no existe, lo crea con Docker
Compose y espera a que la base esté disponible antes de iniciar la aplicación.

En desarrollo se crea el administrador:

- Usuario: `admin@fcvt.edu.ec`
- Contraseña inicial: `Admin@123456`

Cambia la contraseña después del primer ingreso. En producción, configura `SeedAdmin` mediante variables de entorno o un proveedor seguro de secretos.

## Gestión contractual

Desde el detalle de un convenio, el botón **Gestión contractual** permite registrar:

- múltiples partes, firmantes, supervisores y responsables;
- fechas de suscripción y firma, ámbitos, presupuesto, financiamiento y controversias;
- cláusulas y obligaciones con evidencia de cumplimiento;
- actividades, estudiantes, seguro, horas, evaluaciones y certificados;
- convenios relacionados, adendas, modificaciones y evaluaciones de seguimiento;
- terminación y cierre con obligaciones pendientes.

Las migraciones se aplican automáticamente al iniciar la aplicación.

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
