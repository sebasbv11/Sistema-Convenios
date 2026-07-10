using SistemaConvenios.Domain.Exceptions;

namespace SistemaConvenios.Models;

public sealed class ParteConvenio
{
    private ParteConvenio() { }

    private ParteConvenio(
        int convenioId, int? entidadId, string nombre, string? alias,
        string tipoParte, bool principal, string? direccion,
        string? telefono, string? email, string? ruc)
    {
        ConvenioId = convenioId;
        EntidadId = entidadId;
        Nombre = Requerido(nombre, "El nombre de la parte es obligatorio.");
        Alias = Opcional(alias);
        TipoParte = Requerido(tipoParte, "El tipo de parte es obligatorio.");
        Principal = principal;
        Direccion = Opcional(direccion);
        Telefono = Opcional(telefono);
        Email = Opcional(email);
        Ruc = Opcional(ruc);
    }

    public int Id { get; private set; }
    public int ConvenioId { get; private set; }
    public Convenio? Convenio { get; private set; }
    public int? EntidadId { get; private set; }
    public Entidad? Entidad { get; private set; }
    public string Nombre { get; private set; } = string.Empty;
    public string? Alias { get; private set; }
    public string TipoParte { get; private set; } = "Contraparte";
    public bool Principal { get; private set; }
    public string? Direccion { get; private set; }
    public string? Telefono { get; private set; }
    public string? Email { get; private set; }
    public string? Ruc { get; private set; }
    public ICollection<FirmanteConvenio> Firmantes { get; private set; } = new List<FirmanteConvenio>();
    public ICollection<ResponsableConvenio> Responsables { get; private set; } = new List<ResponsableConvenio>();

    public static ParteConvenio Crear(
        int convenioId, int? entidadId, string nombre, string? alias,
        string tipoParte, bool principal, string? direccion,
        string? telefono, string? email, string? ruc) =>
        new(convenioId, entidadId, nombre, alias, tipoParte, principal, direccion, telefono, email, ruc);

    internal static string Requerido(string? valor, string mensaje)
    {
        var normalizado = valor?.Trim();
        if (string.IsNullOrWhiteSpace(normalizado))
            throw new DomainException(mensaje);
        return normalizado;
    }

    internal static string? Opcional(string? valor) =>
        string.IsNullOrWhiteSpace(valor) ? null : valor.Trim();
}

public sealed class FirmanteConvenio
{
    private FirmanteConvenio() { }

    private FirmanteConvenio(
        int convenioId, int? parteConvenioId, string nombre, string cargo,
        string? identificacion, string tipoFirma, DateTime? fechaFirma,
        string? fundamentoRepresentacion)
    {
        ConvenioId = convenioId;
        ParteConvenioId = parteConvenioId;
        Nombre = ParteConvenio.Requerido(nombre, "El nombre del firmante es obligatorio.");
        Cargo = ParteConvenio.Requerido(cargo, "El cargo del firmante es obligatorio.");
        Identificacion = ParteConvenio.Opcional(identificacion);
        TipoFirma = ParteConvenio.Requerido(tipoFirma, "El tipo de firma es obligatorio.");
        FechaFirma = fechaFirma.HasValue ? ToUtc(fechaFirma.Value) : null;
        FundamentoRepresentacion = ParteConvenio.Opcional(fundamentoRepresentacion);
    }

    public int Id { get; private set; }
    public int ConvenioId { get; private set; }
    public Convenio? Convenio { get; private set; }
    public int? ParteConvenioId { get; private set; }
    public ParteConvenio? ParteConvenio { get; private set; }
    public string Nombre { get; private set; } = string.Empty;
    public string Cargo { get; private set; } = string.Empty;
    public string? Identificacion { get; private set; }
    public string TipoFirma { get; private set; } = "Electrónica";
    public DateTime? FechaFirma { get; private set; }
    public string? FundamentoRepresentacion { get; private set; }

    public static FirmanteConvenio Crear(
        int convenioId, int? parteConvenioId, string nombre, string cargo,
        string? identificacion, string tipoFirma, DateTime? fechaFirma,
        string? fundamentoRepresentacion) =>
        new(convenioId, parteConvenioId, nombre, cargo, identificacion, tipoFirma, fechaFirma, fundamentoRepresentacion);

    private static DateTime ToUtc(DateTime value) =>
        value.Kind == DateTimeKind.Utc ? value : DateTime.SpecifyKind(value.Date, DateTimeKind.Utc);
}

public sealed class ResponsableConvenio
{
    private ResponsableConvenio() { }

    private ResponsableConvenio(
        int convenioId, int? parteConvenioId, string nombre, string rol,
        string? cargo, string? email, string? telefono, bool principal)
    {
        ConvenioId = convenioId;
        ParteConvenioId = parteConvenioId;
        Nombre = ParteConvenio.Requerido(nombre, "El nombre del responsable es obligatorio.");
        Rol = ParteConvenio.Requerido(rol, "El rol del responsable es obligatorio.");
        Cargo = ParteConvenio.Opcional(cargo);
        Email = ParteConvenio.Opcional(email);
        Telefono = ParteConvenio.Opcional(telefono);
        Principal = principal;
    }

    public int Id { get; private set; }
    public int ConvenioId { get; private set; }
    public Convenio? Convenio { get; private set; }
    public int? ParteConvenioId { get; private set; }
    public ParteConvenio? ParteConvenio { get; private set; }
    public string Nombre { get; private set; } = string.Empty;
    public string Rol { get; private set; } = string.Empty;
    public string? Cargo { get; private set; }
    public string? Email { get; private set; }
    public string? Telefono { get; private set; }
    public bool Principal { get; private set; }

    public static ResponsableConvenio Crear(
        int convenioId, int? parteConvenioId, string nombre, string rol,
        string? cargo, string? email, string? telefono, bool principal) =>
        new(convenioId, parteConvenioId, nombre, rol, cargo, email, telefono, principal);
}

public sealed class AmbitoConvenio
{
    private AmbitoConvenio() { }
    private AmbitoConvenio(int convenioId, string nombre)
    {
        ConvenioId = convenioId;
        Nombre = ParteConvenio.Requerido(nombre, "El ámbito es obligatorio.");
    }

    public int Id { get; private set; }
    public int ConvenioId { get; private set; }
    public Convenio? Convenio { get; private set; }
    public string Nombre { get; private set; } = string.Empty;
    public static AmbitoConvenio Crear(int convenioId, string nombre) => new(convenioId, nombre);
}

public sealed class ClausulaConvenio
{
    private ClausulaConvenio() { }
    private ClausulaConvenio(int convenioId, string titulo, string tipo, string contenido, int orden)
    {
        ConvenioId = convenioId;
        Titulo = ParteConvenio.Requerido(titulo, "El título de la cláusula es obligatorio.");
        Tipo = ParteConvenio.Requerido(tipo, "El tipo de cláusula es obligatorio.");
        Contenido = ParteConvenio.Requerido(contenido, "El contenido de la cláusula es obligatorio.");
        Orden = orden < 1 ? 1 : orden;
    }

    public int Id { get; private set; }
    public int ConvenioId { get; private set; }
    public Convenio? Convenio { get; private set; }
    public string Titulo { get; private set; } = string.Empty;
    public string Tipo { get; private set; } = "General";
    public string Contenido { get; private set; } = string.Empty;
    public int Orden { get; private set; }
    public static ClausulaConvenio Crear(int convenioId, string titulo, string tipo, string contenido, int orden) =>
        new(convenioId, titulo, tipo, contenido, orden);
}

public sealed class ObligacionConvenio
{
    private ObligacionConvenio() { }
    private ObligacionConvenio(int convenioId, string actor, string descripcion, int orden)
    {
        ConvenioId = convenioId;
        Actor = ParteConvenio.Requerido(actor, "El actor responsable es obligatorio.");
        Descripcion = ParteConvenio.Requerido(descripcion, "La obligación es obligatoria.");
        Orden = orden < 1 ? 1 : orden;
        Estado = "Pendiente";
    }

    public int Id { get; private set; }
    public int ConvenioId { get; private set; }
    public Convenio? Convenio { get; private set; }
    public string Actor { get; private set; } = string.Empty;
    public string Descripcion { get; private set; } = string.Empty;
    public int Orden { get; private set; }
    public string Estado { get; private set; } = "Pendiente";
    public DateTime? FechaCumplimiento { get; private set; }
    public string? Evidencia { get; private set; }

    public static ObligacionConvenio Crear(int convenioId, string actor, string descripcion, int orden) =>
        new(convenioId, actor, descripcion, orden);

    public void MarcarCumplida(string? evidencia)
    {
        Estado = "Cumplida";
        FechaCumplimiento = DateTime.UtcNow;
        Evidencia = ParteConvenio.Opcional(evidencia);
    }
}

public sealed class ActividadConvenio
{
    private ActividadConvenio() { }
    private ActividadConvenio(
        int convenioId, string nombre, string tipo, string? descripcion,
        DateTime fechaInicio, DateTime fechaFin, int horasPlanificadas,
        string? lugar, string? responsable)
    {
        if (fechaFin.Date < fechaInicio.Date)
            throw new DomainException("La actividad no puede finalizar antes de iniciar.");
        ConvenioId = convenioId;
        Nombre = ParteConvenio.Requerido(nombre, "El nombre de la actividad es obligatorio.");
        Tipo = ParteConvenio.Requerido(tipo, "El tipo de actividad es obligatorio.");
        Descripcion = ParteConvenio.Opcional(descripcion);
        FechaInicio = ToUtc(fechaInicio);
        FechaFin = ToUtc(fechaFin);
        HorasPlanificadas = Math.Max(0, horasPlanificadas);
        Lugar = ParteConvenio.Opcional(lugar);
        Responsable = ParteConvenio.Opcional(responsable);
        Estado = "Planificada";
    }

    public int Id { get; private set; }
    public int ConvenioId { get; private set; }
    public Convenio? Convenio { get; private set; }
    public string Nombre { get; private set; } = string.Empty;
    public string Tipo { get; private set; } = string.Empty;
    public string? Descripcion { get; private set; }
    public DateTime FechaInicio { get; private set; }
    public DateTime FechaFin { get; private set; }
    public int HorasPlanificadas { get; private set; }
    public string Estado { get; private set; } = "Planificada";
    public string? Lugar { get; private set; }
    public string? Responsable { get; private set; }
    public ICollection<ParticipacionActividad> Participantes { get; private set; } = new List<ParticipacionActividad>();

    public static ActividadConvenio Crear(
        int convenioId, string nombre, string tipo, string? descripcion,
        DateTime fechaInicio, DateTime fechaFin, int horasPlanificadas,
        string? lugar, string? responsable) =>
        new(convenioId, nombre, tipo, descripcion, fechaInicio, fechaFin, horasPlanificadas, lugar, responsable);

    private static DateTime ToUtc(DateTime value) =>
        value.Kind == DateTimeKind.Utc ? value : DateTime.SpecifyKind(value.Date, DateTimeKind.Utc);
}

public sealed class EstudianteConvenio
{
    private EstudianteConvenio() { }
    private EstudianteConvenio(
        int convenioId, int? carreraId, string identificacion, string nombre,
        string? email, string? telefono, string? numeroPoliza, bool cubiertoSeguro)
    {
        ConvenioId = convenioId;
        CarreraId = carreraId;
        Identificacion = ParteConvenio.Requerido(identificacion, "La identificación es obligatoria.");
        Nombre = ParteConvenio.Requerido(nombre, "El nombre del estudiante es obligatorio.");
        Email = ParteConvenio.Opcional(email);
        Telefono = ParteConvenio.Opcional(telefono);
        NumeroPoliza = ParteConvenio.Opcional(numeroPoliza);
        CubiertoSeguro = cubiertoSeguro;
    }

    public int Id { get; private set; }
    public int ConvenioId { get; private set; }
    public Convenio? Convenio { get; private set; }
    public int? CarreraId { get; private set; }
    public Carrera? Carrera { get; private set; }
    public string Identificacion { get; private set; } = string.Empty;
    public string Nombre { get; private set; } = string.Empty;
    public string? Email { get; private set; }
    public string? Telefono { get; private set; }
    public string? NumeroPoliza { get; private set; }
    public bool CubiertoSeguro { get; private set; }
    public ICollection<ParticipacionActividad> Participaciones { get; private set; } = new List<ParticipacionActividad>();

    public static EstudianteConvenio Crear(
        int convenioId, int? carreraId, string identificacion, string nombre,
        string? email, string? telefono, string? numeroPoliza, bool cubiertoSeguro) =>
        new(convenioId, carreraId, identificacion, nombre, email, telefono, numeroPoliza, cubiertoSeguro);
}

public sealed class ParticipacionActividad
{
    private ParticipacionActividad() { }
    private ParticipacionActividad(int actividadConvenioId, int estudianteConvenioId, int horasAsignadas)
    {
        ActividadConvenioId = actividadConvenioId;
        EstudianteConvenioId = estudianteConvenioId;
        HorasAsignadas = Math.Max(0, horasAsignadas);
    }

    public int Id { get; private set; }
    public int ActividadConvenioId { get; private set; }
    public ActividadConvenio? ActividadConvenio { get; private set; }
    public int EstudianteConvenioId { get; private set; }
    public EstudianteConvenio? EstudianteConvenio { get; private set; }
    public int HorasAsignadas { get; private set; }
    public int HorasCumplidas { get; private set; }
    public decimal? Calificacion { get; private set; }
    public string? Evaluacion { get; private set; }
    public bool CertificadoEmitido { get; private set; }
    public DateTime? FechaCertificado { get; private set; }

    public static ParticipacionActividad Crear(int actividadId, int estudianteId, int horas) =>
        new(actividadId, estudianteId, horas);

    public void Evaluar(int horasCumplidas, decimal? calificacion, string? evaluacion, bool certificado)
    {
        if (calificacion is < 0 or > 100)
            throw new DomainException("La calificación debe estar entre 0 y 100.");
        HorasCumplidas = Math.Max(0, horasCumplidas);
        Calificacion = calificacion;
        Evaluacion = ParteConvenio.Opcional(evaluacion);
        CertificadoEmitido = certificado;
        FechaCertificado = certificado ? DateTime.UtcNow : null;
    }
}

public sealed class ConvenioRelacionado
{
    private ConvenioRelacionado() { }
    private ConvenioRelacionado(int convenioId, int convenioRelacionadoId, string tipoRelacion)
    {
        if (convenioId == convenioRelacionadoId)
            throw new DomainException("Un convenio no puede relacionarse consigo mismo.");
        ConvenioId = convenioId;
        ConvenioRelacionadoId = convenioRelacionadoId;
        TipoRelacion = ParteConvenio.Requerido(tipoRelacion, "El tipo de relación es obligatorio.");
    }

    public int Id { get; private set; }
    public int ConvenioId { get; private set; }
    public Convenio? Convenio { get; private set; }
    public int ConvenioRelacionadoId { get; private set; }
    public Convenio? Relacionado { get; private set; }
    public string TipoRelacion { get; private set; } = "Específico";
    public static ConvenioRelacionado Crear(int convenioId, int relacionadoId, string tipo) =>
        new(convenioId, relacionadoId, tipo);
}

public sealed class ModificacionConvenio
{
    private ModificacionConvenio() { }
    private ModificacionConvenio(int convenioId, string tipo, string descripcion, string? motivo, DateTime fecha)
    {
        ConvenioId = convenioId;
        Tipo = ParteConvenio.Requerido(tipo, "El tipo de modificación es obligatorio.");
        Descripcion = ParteConvenio.Requerido(descripcion, "La descripción es obligatoria.");
        Motivo = ParteConvenio.Opcional(motivo);
        Fecha = fecha.Kind == DateTimeKind.Utc ? fecha : DateTime.SpecifyKind(fecha.Date, DateTimeKind.Utc);
    }

    public int Id { get; private set; }
    public int ConvenioId { get; private set; }
    public Convenio? Convenio { get; private set; }
    public string Tipo { get; private set; } = string.Empty;
    public string Descripcion { get; private set; } = string.Empty;
    public string? Motivo { get; private set; }
    public DateTime Fecha { get; private set; }
    public static ModificacionConvenio Crear(int convenioId, string tipo, string descripcion, string? motivo, DateTime fecha) =>
        new(convenioId, tipo, descripcion, motivo, fecha);
}

public sealed class EvaluacionConvenio
{
    private EvaluacionConvenio() { }
    private EvaluacionConvenio(
        int convenioId, string tipo, DateTime fecha, decimal? calificacion,
        string conclusion, string? responsable)
    {
        if (calificacion is < 0 or > 100)
            throw new DomainException("La calificación debe estar entre 0 y 100.");
        ConvenioId = convenioId;
        Tipo = ParteConvenio.Requerido(tipo, "El tipo de evaluación es obligatorio.");
        Fecha = fecha.Kind == DateTimeKind.Utc ? fecha : DateTime.SpecifyKind(fecha.Date, DateTimeKind.Utc);
        Calificacion = calificacion;
        Conclusion = ParteConvenio.Requerido(conclusion, "La conclusión es obligatoria.");
        Responsable = ParteConvenio.Opcional(responsable);
    }

    public int Id { get; private set; }
    public int ConvenioId { get; private set; }
    public Convenio? Convenio { get; private set; }
    public string Tipo { get; private set; } = string.Empty;
    public DateTime Fecha { get; private set; }
    public decimal? Calificacion { get; private set; }
    public string Conclusion { get; private set; } = string.Empty;
    public string? Responsable { get; private set; }
    public static EvaluacionConvenio Crear(
        int convenioId, string tipo, DateTime fecha, decimal? calificacion,
        string conclusion, string? responsable) =>
        new(convenioId, tipo, fecha, calificacion, conclusion, responsable);
}

public sealed class CierreConvenio
{
    private CierreConvenio() { }
    private CierreConvenio(
        int convenioId, DateTime fecha, string causal, string resumen,
        string? obligacionesPendientes, string? responsablesSeguimiento)
    {
        ConvenioId = convenioId;
        Fecha = fecha.Kind == DateTimeKind.Utc ? fecha : DateTime.SpecifyKind(fecha.Date, DateTimeKind.Utc);
        Causal = ParteConvenio.Requerido(causal, "La causal de cierre es obligatoria.");
        Resumen = ParteConvenio.Requerido(resumen, "El resumen de cierre es obligatorio.");
        ObligacionesPendientes = ParteConvenio.Opcional(obligacionesPendientes);
        ResponsablesSeguimiento = ParteConvenio.Opcional(responsablesSeguimiento);
    }

    public int Id { get; private set; }
    public int ConvenioId { get; private set; }
    public Convenio? Convenio { get; private set; }
    public DateTime Fecha { get; private set; }
    public string Causal { get; private set; } = string.Empty;
    public string Resumen { get; private set; } = string.Empty;
    public string? ObligacionesPendientes { get; private set; }
    public string? ResponsablesSeguimiento { get; private set; }
    public static CierreConvenio Crear(
        int convenioId, DateTime fecha, string causal, string resumen,
        string? obligacionesPendientes, string? responsablesSeguimiento) =>
        new(convenioId, fecha, causal, resumen, obligacionesPendientes, responsablesSeguimiento);
}
