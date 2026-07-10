using SistemaConvenios.Application.Common;

namespace SistemaConvenios.Application.Convenios;

public sealed record ParteDto(
    int Id, int? EntidadId, string Nombre, string? Alias, string TipoParte,
    bool Principal, string? Direccion, string? Telefono, string? Email, string? Ruc);

public sealed record FirmanteDto(
    int Id, int? ParteId, string Nombre, string Cargo, string? Identificacion,
    string TipoFirma, DateTime? FechaFirma, string? FundamentoRepresentacion);

public sealed record ResponsableDto(
    int Id, int? ParteId, string Nombre, string Rol, string? Cargo,
    string? Email, string? Telefono, bool Principal);

public sealed record ClausulaDto(int Id, string Titulo, string Tipo, string Contenido, int Orden);
public sealed record ObligacionDto(
    int Id, string Actor, string Descripcion, int Orden, string Estado,
    DateTime? FechaCumplimiento, string? Evidencia);
public sealed record ActividadDto(
    int Id, string Nombre, string Tipo, string? Descripcion, DateTime FechaInicio,
    DateTime FechaFin, int HorasPlanificadas, string Estado, string? Lugar,
    string? Responsable, int Participantes);
public sealed record EstudianteDto(
    int Id, int? CarreraId, string? Carrera, string Identificacion, string Nombre,
    string? Email, string? Telefono, string? NumeroPoliza, bool CubiertoSeguro);
public sealed record ParticipacionDto(
    int Id, int ActividadId, string Actividad, int EstudianteId, string Estudiante,
    int HorasAsignadas, int HorasCumplidas, decimal? Calificacion,
    string? Evaluacion, bool CertificadoEmitido, DateTime? FechaCertificado);
public sealed record RelacionDto(int Id, int ConvenioRelacionadoId, string Numero, string TipoRelacion);
public sealed record ModificacionDto(int Id, string Tipo, string Descripcion, string? Motivo, DateTime Fecha);
public sealed record EvaluacionDto(
    int Id, string Tipo, DateTime Fecha, decimal? Calificacion,
    string Conclusion, string? Responsable);
public sealed record CierreDto(
    int Id, DateTime Fecha, string Causal, string Resumen,
    string? ObligacionesPendientes, string? ResponsablesSeguimiento);

public sealed record DatosLegalesDto(
    DateTime? FechaSuscripcion,
    DateTime? FechaUltimaFirma,
    string? DomicilioContractual,
    string? MecanismoSolucionControversias,
    bool ConfidencialidadIndefinida,
    bool TieneErogacion,
    decimal? Presupuesto,
    string Moneda,
    string? FuenteFinanciamiento,
    string? CondicionesRenovacion);

public sealed record GestionContractualDto(
    int ConvenioId,
    string Numero,
    string Estado,
    DatosLegalesDto DatosLegales,
    IReadOnlyList<string> Ambitos,
    IReadOnlyList<ParteDto> Partes,
    IReadOnlyList<FirmanteDto> Firmantes,
    IReadOnlyList<ResponsableDto> Responsables,
    IReadOnlyList<ClausulaDto> Clausulas,
    IReadOnlyList<ObligacionDto> Obligaciones,
    IReadOnlyList<ActividadDto> Actividades,
    IReadOnlyList<EstudianteDto> Estudiantes,
    IReadOnlyList<ParticipacionDto> Participaciones,
    IReadOnlyList<RelacionDto> Relaciones,
    IReadOnlyList<ModificacionDto> Modificaciones,
    IReadOnlyList<EvaluacionDto> Evaluaciones,
    CierreDto? Cierre,
    IReadOnlyList<CatalogoItemDto> Entidades,
    IReadOnlyList<CatalogoItemDto> Carreras,
    IReadOnlyList<CatalogoItemDto> ConveniosDisponibles);

public sealed record ConfigurarDatosLegalesCommand(
    int ConvenioId, DateTime? FechaSuscripcion, DateTime? FechaUltimaFirma,
    string? DomicilioContractual, string? MecanismoSolucionControversias,
    bool ConfidencialidadIndefinida, bool TieneErogacion, decimal? Presupuesto,
    string? Moneda, string? FuenteFinanciamiento, string? CondicionesRenovacion,
    IReadOnlyCollection<string> Ambitos);

public sealed record AgregarParteCommand(
    int ConvenioId, int? EntidadId, string Nombre, string? Alias,
    string TipoParte, bool Principal, string? Direccion,
    string? Telefono, string? Email, string? Ruc);
public sealed record AgregarFirmanteCommand(
    int ConvenioId, int? ParteId, string Nombre, string Cargo,
    string? Identificacion, string TipoFirma, DateTime? FechaFirma,
    string? FundamentoRepresentacion);
public sealed record AgregarResponsableCommand(
    int ConvenioId, int? ParteId, string Nombre, string Rol, string? Cargo,
    string? Email, string? Telefono, bool Principal);
public sealed record AgregarClausulaCommand(
    int ConvenioId, string Titulo, string Tipo, string Contenido, int Orden);
public sealed record AgregarObligacionCommand(
    int ConvenioId, string Actor, string Descripcion, int Orden);
public sealed record AgregarActividadCommand(
    int ConvenioId, string Nombre, string Tipo, string? Descripcion,
    DateTime FechaInicio, DateTime FechaFin, int HorasPlanificadas,
    string? Lugar, string? Responsable);
public sealed record AgregarEstudianteCommand(
    int ConvenioId, int? CarreraId, string Identificacion, string Nombre,
    string? Email, string? Telefono, string? NumeroPoliza, bool CubiertoSeguro);
public sealed record AgregarParticipacionCommand(
    int ConvenioId, int ActividadId, int EstudianteId, int HorasAsignadas);
public sealed record EvaluarParticipacionCommand(
    int ConvenioId, int ParticipacionId, int HorasCumplidas,
    decimal? Calificacion, string? Evaluacion, bool CertificadoEmitido);
public sealed record AgregarRelacionCommand(
    int ConvenioId, int ConvenioRelacionadoId, string TipoRelacion);
public sealed record AgregarModificacionCommand(
    int ConvenioId, string Tipo, string Descripcion, string? Motivo, DateTime Fecha);
public sealed record AgregarEvaluacionCommand(
    int ConvenioId, string Tipo, DateTime Fecha, decimal? Calificacion,
    string Conclusion, string? Responsable);
public sealed record RegistrarCierreCommand(
    int ConvenioId, DateTime Fecha, string Causal, string Resumen,
    string? ObligacionesPendientes, string? ResponsablesSeguimiento, string? Usuario);

public interface IGestionContractualAppService
{
    Task<GestionContractualDto?> ObtenerAsync(int convenioId, CancellationToken cancellationToken = default);
    Task<OperationResult> ConfigurarDatosLegalesAsync(ConfigurarDatosLegalesCommand command, CancellationToken cancellationToken = default);
    Task<OperationResult> AgregarParteAsync(AgregarParteCommand command, CancellationToken cancellationToken = default);
    Task<OperationResult> AgregarFirmanteAsync(AgregarFirmanteCommand command, CancellationToken cancellationToken = default);
    Task<OperationResult> AgregarResponsableAsync(AgregarResponsableCommand command, CancellationToken cancellationToken = default);
    Task<OperationResult> AgregarClausulaAsync(AgregarClausulaCommand command, CancellationToken cancellationToken = default);
    Task<OperationResult> AgregarObligacionAsync(AgregarObligacionCommand command, CancellationToken cancellationToken = default);
    Task<OperationResult> CumplirObligacionAsync(int convenioId, int obligacionId, string? evidencia, CancellationToken cancellationToken = default);
    Task<OperationResult> AgregarActividadAsync(AgregarActividadCommand command, CancellationToken cancellationToken = default);
    Task<OperationResult> AgregarEstudianteAsync(AgregarEstudianteCommand command, CancellationToken cancellationToken = default);
    Task<OperationResult> AgregarParticipacionAsync(AgregarParticipacionCommand command, CancellationToken cancellationToken = default);
    Task<OperationResult> EvaluarParticipacionAsync(EvaluarParticipacionCommand command, CancellationToken cancellationToken = default);
    Task<OperationResult> AgregarRelacionAsync(AgregarRelacionCommand command, CancellationToken cancellationToken = default);
    Task<OperationResult> AgregarModificacionAsync(AgregarModificacionCommand command, CancellationToken cancellationToken = default);
    Task<OperationResult> AgregarEvaluacionAsync(AgregarEvaluacionCommand command, CancellationToken cancellationToken = default);
    Task<OperationResult> RegistrarCierreAsync(RegistrarCierreCommand command, CancellationToken cancellationToken = default);
    Task<OperationResult> EliminarAsync(int convenioId, string tipo, int id, CancellationToken cancellationToken = default);
}
