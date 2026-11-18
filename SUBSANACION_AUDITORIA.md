# Seguimiento de la auditoría técnica

El informe de septiembre de 2026 contiene 14 criterios y 15 no conformidades. Este seguimiento describe el estado del repositorio; no sustituye una reevaluación institucional ni acredita configuraciones de producción.

| Criterio | Estado y evidencia |
| --- | --- |
| Arquitectura | Capas separadas y reglas NetArchTest. Pendiente revisar el namespace histórico `SistemaConvenios.Models` del dominio y exclusiones de compilación. |
| Pruebas | Pruebas de dominio, autorización y PostgreSQL para creación, adenda, cierre e integridad. Pendiente una suite E2E completa mediante navegador. |
| Versiones | .NET 10; revisar periódicamente avisos de NuGet y actualizar dependencias. |
| Lógica de negocio | Validaciones y operaciones contractuales en dominio y aplicación. |
| Infraestructura | Dockerfile y Terraform GCP existentes. La aprobación institucional de infraestructura requiere validar el entorno real. |
| CI/CD | Compilación, pruebas con cobertura, migraciones, Docker y análisis CodeQL configurados; confirmar cada ejecución en GitHub Actions. |
| Entradas | Las acciones de gestión contractual verifican ModelState y el dominio aplica invariantes. |
| Base de datos | Migración de ámbitos, campos obligatorios e índices únicos; pruebas en PostgreSQL aislado. Pendiente revisar y corregir datos históricos y definir normalización de contactos sin perder historial contractual. |
| DTO | Los servicios de aplicación proporcionan DTO a la interfaz. |
| Migraciones | Migración explícita `--migrate` y script idempotente disponibles; no se aplican automáticamente en producción por defecto. Validar el script y datos existentes antes de ejecutar en cada entorno. |
| Seguridad | Roles para escritura, sin desbloqueo anónimo, limitación de login y cabeceras. La revisión de seguridad operativa requiere verificar el despliegue. |
| Observabilidad | OpenTelemetry con exportación OTLP configurable y Serilog JSON; requiere un colector y validación en el entorno de destino. |
| Secretos | User Secrets y variables de entorno, y Secret Manager en el flujo GCP. Revisar historial, acceso y rotar credenciales previamente expuestas en los sistemas donde sigan vigentes. |
| Buenas prácticas | Cookies y contraseña reforzadas; recursos CDN con versiones y hashes SRI. Pendiente reducir ViewBag. |

Las pruebas con PostgreSQL requieren `CONVENIOS_TEST_DATABASE` apuntando a una base **aislada** cuyo nombre termine en `_test`. Aplican migraciones en esa base; no se debe usar una base real. En CI se crea un servicio de PostgreSQL específico.
