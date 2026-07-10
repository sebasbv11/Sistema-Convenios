# Arquitectura DDD

El sistema usa una arquitectura DDD pragmática con dependencias dirigidas hacia el dominio:

```text
SistemaConvenios (Web MVC)
        │
        ├──> SistemaConvenios.Application
        │           │
        │           └──> SistemaConvenios.Domain
        │
        └──> SistemaConvenios.Infrastructure
                    │
                    ├──> SistemaConvenios.Application
                    └──> SistemaConvenios.Domain
```

## Capas

- `SistemaConvenios.Domain`: agregados, entidades, value objects, invariantes y reglas de negocio.
- `SistemaConvenios.Application`: casos de uso, comandos, DTO, contratos de repositorio y puertos.
- `SistemaConvenios.Infrastructure`: Entity Framework Core, PostgreSQL, Identity, repositorios, correo y archivos.
- `SistemaConvenios`: presentación ASP.NET Core MVC, autenticación HTTP y model binding.

## Agregados

- `Convenio`: raíz de agregado. Controla periodo, cambios de estado, historial, convenio padre recursivo, área promotora, carreras, facultades, archivos y todo el ciclo contractual.
- `Entidad`: raíz de agregado. Controla identificación, información geográfica, representante legal, contacto de gestión y estado activo.

El agregado `Convenio` contiene las entidades:

- `ParteConvenio`, `FirmanteConvenio` y `ResponsableConvenio`;
- `AmbitoConvenio`, `ClausulaConvenio` y `ObligacionConvenio`;
- `ActividadConvenio`, `EstudianteConvenio` y `ParticipacionActividad`;
- `ConvenioRelacionado`, `ModificacionConvenio`, `EvaluacionConvenio` y `CierreConvenio`.

Los convenios específicos se vinculan al marco o internacional mediante `ConvenioPadreId`.
La relación libre `ConvenioRelacionado` se conserva para referencias adicionales,
adendas o vínculos no jerárquicos.

Las reglas de consistencia —periodos, ámbitos obligatorios, presupuesto, duplicados,
calificaciones, cumplimiento y cierre— se aplican dentro del dominio o del caso de uso
cuando requieren consultar el agregado completo.

## Value objects

- `NumeroConvenio`
- `RucIdentificacion`
- `PeriodoConvenio`

## Compatibilidad

Las tablas, nombres de columnas y migraciones originales se conservan. Algunos tipos del dominio mantienen el namespace histórico `SistemaConvenios.Models` para que el snapshot de Entity Framework no interprete el refactor como una reconstrucción del esquema.

Los controladores no acceden a `ApplicationDbContext`; consumen interfaces de casos de uso. Los archivos nuevos se guardan fuera de `wwwroot` y se descargan únicamente mediante una acción autenticada.

La interfaz `IGestionContractualAppService` expone los casos de uso de la gestión
contractual y `IGestionContractualRepository` encapsula la carga del agregado completo.
