using SistemaConvenios.Domain;
using SistemaConvenios.Domain.Common;
using SistemaConvenios.Domain.Exceptions;
using SistemaConvenios.Domain.ValueObjects;

namespace SistemaConvenios.Models;

public sealed class Convenio : AggregateRoot<int>
{
    private Convenio()
    {
    }

    private Convenio(
        string numero,
        int tipoConvenioId,
        int entidadId,
        int areaPromotoraId,
        int? convenioPadreId,
        string ambito,
        string objeto,
        DateTime fechaInicio,
        DateTime fechaVencimiento,
        string estado,
        string? numeroResolucion,
        string? supervisor,
        string? contactoGestionNombre,
        string? contactoGestionEmail,
        string? contactoGestionTelefono,
        bool renovacionAutomatica,
        string? observaciones,
        string? usuarioCreadorId,
        string? usuario)
    {
        Numero = NumeroConvenio.Crear(numero).Valor;
        AplicarDatos(
            tipoConvenioId, entidadId, areaPromotoraId, convenioPadreId,
            ambito, objeto, fechaInicio,
            fechaVencimiento, numeroResolucion, supervisor,
            contactoGestionNombre, contactoGestionEmail, contactoGestionTelefono,
            renovacionAutomatica, observaciones);
        Estado = ConvenioEstados.Validar(estado);
        UsuarioCreadorId = usuarioCreadorId;
        FechaCreacion = DateTime.UtcNow;
        RegistrarHistorial(string.Empty, Estado, "Convenio creado", usuario);
    }

    public string Numero { get; private set; } = string.Empty;
    public int TipoConvenioId { get; private set; }
    public TipoConvenio? TipoConvenio { get; private set; }
    public int EntidadId { get; private set; }
    public Entidad? Entidad { get; private set; }
    public int AreaPromotoraId { get; private set; }
    public AreaPromotora? AreaPromotora { get; private set; }
    public int? ConvenioPadreId { get; private set; }
    public Convenio? ConvenioPadre { get; private set; }
    public string Ambito { get; private set; } = string.Empty;
    public string Objeto { get; private set; } = string.Empty;
    public DateTime FechaInicio { get; private set; }
    public DateTime FechaVencimiento { get; private set; }
    public string Estado { get; private set; } = ConvenioEstados.Vigente;
    public string NumeroResolucion { get; private set; } = string.Empty;
    public string? Supervisor { get; private set; }
    public string? ContactoGestionNombre { get; private set; }
    public string? ContactoGestionEmail { get; private set; }
    public string? ContactoGestionTelefono { get; private set; }
    public bool RenovacionAutomatica { get; private set; }
    public string? Observaciones { get; private set; }
    public DateTime? FechaSuscripcion { get; private set; }
    public DateTime? FechaUltimaFirma { get; private set; }
    public string? DomicilioContractual { get; private set; }
    public string? MecanismoSolucionControversias { get; private set; }
    public bool ConfidencialidadIndefinida { get; private set; }
    public bool TieneErogacion { get; private set; }
    public decimal? Presupuesto { get; private set; }
    public string Moneda { get; private set; } = "USD";
    public string? FuenteFinanciamiento { get; private set; }
    public string? CondicionesRenovacion { get; private set; }
    public DateTime FechaCreacion { get; private set; } = DateTime.UtcNow;
    public DateTime? FechaModificacion { get; private set; }
    public string? UsuarioCreadorId { get; private set; }

    public ICollection<ConvenioFacultad> ConvenioFacultades { get; private set; } = new List<ConvenioFacultad>();
    public ICollection<ConvenioCarrera> ConvenioCarreras { get; private set; } = new List<ConvenioCarrera>();
    public ICollection<Convenio> ConveniosDerivados { get; private set; } = new List<Convenio>();
    public ICollection<HistorialEstado> Historial { get; private set; } = new List<HistorialEstado>();
    public ICollection<ArchivoConvenio> Archivos { get; private set; } = new List<ArchivoConvenio>();
    public ICollection<ParteConvenio> Partes { get; private set; } = new List<ParteConvenio>();
    public ICollection<FirmanteConvenio> Firmantes { get; private set; } = new List<FirmanteConvenio>();
    public ICollection<ResponsableConvenio> Responsables { get; private set; } = new List<ResponsableConvenio>();
    public ICollection<AmbitoConvenio> Ambitos { get; private set; } = new List<AmbitoConvenio>();
    public ICollection<ClausulaConvenio> Clausulas { get; private set; } = new List<ClausulaConvenio>();
    public ICollection<ObligacionConvenio> Obligaciones { get; private set; } = new List<ObligacionConvenio>();
    public ICollection<ActividadConvenio> Actividades { get; private set; } = new List<ActividadConvenio>();
    public ICollection<EstudianteConvenio> Estudiantes { get; private set; } = new List<EstudianteConvenio>();
    public ICollection<ConvenioRelacionado> ConveniosRelacionados { get; private set; } = new List<ConvenioRelacionado>();
    public ICollection<ModificacionConvenio> Modificaciones { get; private set; } = new List<ModificacionConvenio>();
    public ICollection<EvaluacionConvenio> Evaluaciones { get; private set; } = new List<EvaluacionConvenio>();
    public CierreConvenio? Cierre { get; private set; }

    public static Convenio Crear(
        string numero,
        int tipoConvenioId,
        int entidadId,
        int areaPromotoraId,
        int? convenioPadreId,
        string ambito,
        string objeto,
        DateTime fechaInicio,
        DateTime fechaVencimiento,
        string estado,
        string? numeroResolucion,
        string? supervisor,
        string? contactoGestionNombre,
        string? contactoGestionEmail,
        string? contactoGestionTelefono,
        bool renovacionAutomatica,
        string? observaciones,
        string? usuarioCreadorId,
        string? usuario) =>
        new(
            numero, tipoConvenioId, entidadId, areaPromotoraId, convenioPadreId,
            ambito, objeto,
            fechaInicio, fechaVencimiento, estado, numeroResolucion,
            supervisor, contactoGestionNombre, contactoGestionEmail,
            contactoGestionTelefono, renovacionAutomatica, observaciones,
            usuarioCreadorId, usuario);

    public void Actualizar(
        int tipoConvenioId,
        int entidadId,
        int areaPromotoraId,
        int? convenioPadreId,
        string ambito,
        string objeto,
        DateTime fechaInicio,
        DateTime fechaVencimiento,
        string estado,
        string? numeroResolucion,
        string? supervisor,
        string? contactoGestionNombre,
        string? contactoGestionEmail,
        string? contactoGestionTelefono,
        bool renovacionAutomatica,
        string? observaciones,
        string? usuario)
    {
        AplicarDatos(
            tipoConvenioId, entidadId, areaPromotoraId, convenioPadreId,
            ambito, objeto, fechaInicio,
            fechaVencimiento, numeroResolucion, supervisor,
            contactoGestionNombre, contactoGestionEmail, contactoGestionTelefono,
            renovacionAutomatica, observaciones);

        CambiarEstado(estado, null, usuario);
        FechaModificacion = DateTime.UtcNow;
    }

    public bool CambiarEstado(string nuevoEstado, string? observacion, string? usuario)
    {
        var estadoValidado = ConvenioEstados.Validar(nuevoEstado);
        if (Estado == estadoValidado)
            return false;

        var anterior = Estado;
        Estado = estadoValidado;
        FechaModificacion = DateTime.UtcNow;
        RegistrarHistorial(anterior, Estado, observacion, usuario);
        return true;
    }

    public bool ActualizarEstadoPorFecha(DateTime hoy)
    {
        var periodo = PeriodoConvenio.Crear(FechaInicio, FechaVencimiento);

        if (ConvenioEstados.Activos.Contains(Estado) &&
            periodo.EstaVencido(hoy) &&
            RenovacionAutomatica)
            return CambiarEstado(
                ConvenioEstados.RenovadoAutomatico,
                "Cambio automático por renovación automática",
                "Sistema");

        if (ConvenioEstados.Activos.Contains(Estado) && periodo.EstaVencido(hoy))
            return CambiarEstado(
                ConvenioEstados.Terminado,
                "Cambio automático por fecha de vencimiento",
                "Sistema");

        if (Estado == ConvenioEstados.Vigente && periodo.VenceDentroDe(hoy, 30))
            return CambiarEstado(
                ConvenioEstados.PorVencer,
                "Cambio automático: vence en 30 días o menos",
                "Sistema");

        return false;
    }

    public void ReemplazarFacultades(IEnumerable<int> facultadIds)
    {
        ConvenioFacultades.Clear();
        foreach (var id in facultadIds.Where(id => id > 0).Distinct())
            ConvenioFacultades.Add(ConvenioFacultad.Crear(Id, id));
    }

    public void ReemplazarCarreras(IEnumerable<int> carreraIds)
    {
        ConvenioCarreras.Clear();
        foreach (var id in carreraIds.Where(id => id > 0).Distinct())
            ConvenioCarreras.Add(ConvenioCarrera.Crear(Id, id));
    }

    public ArchivoConvenio AgregarArchivo(
        string nombreOriginal,
        string rutaFisica,
        string tipoDocumento,
        string? descripcion,
        long tamanioBytes,
        string? usuario)
    {
        var archivo = ArchivoConvenio.Crear(
            Id, nombreOriginal, rutaFisica, tipoDocumento,
            descripcion, tamanioBytes, usuario);
        Archivos.Add(archivo);
        return archivo;
    }

    public bool EliminarArchivo(int archivoId)
    {
        var archivo = Archivos.FirstOrDefault(a => a.Id == archivoId);
        if (archivo is null)
            return false;

        Archivos.Remove(archivo);
        return true;
    }

    public void ConfigurarDatosLegales(
        DateTime? fechaSuscripcion, DateTime? fechaUltimaFirma,
        string? domicilio, string? mecanismoControversias,
        bool confidencialidadIndefinida, bool tieneErogacion,
        decimal? presupuesto, string? moneda,
        string? fuenteFinanciamiento, string? condicionesRenovacion)
    {
        if (fechaSuscripcion.HasValue && fechaUltimaFirma.HasValue &&
            fechaUltimaFirma.Value.Date < fechaSuscripcion.Value.Date)
            throw new DomainException("La última firma no puede ser anterior a la suscripción.");
        if (tieneErogacion && (!presupuesto.HasValue || presupuesto.Value < 0))
            throw new DomainException("Debe registrar un presupuesto válido cuando existe erogación.");

        FechaSuscripcion = ToUtcNullable(fechaSuscripcion);
        FechaUltimaFirma = ToUtcNullable(fechaUltimaFirma);
        DomicilioContractual = NormalizarOpcional(domicilio);
        MecanismoSolucionControversias = NormalizarOpcional(mecanismoControversias);
        ConfidencialidadIndefinida = confidencialidadIndefinida;
        TieneErogacion = tieneErogacion;
        Presupuesto = tieneErogacion ? presupuesto : null;
        Moneda = string.IsNullOrWhiteSpace(moneda) ? "USD" : moneda.Trim().ToUpperInvariant();
        FuenteFinanciamiento = tieneErogacion ? NormalizarOpcional(fuenteFinanciamiento) : null;
        CondicionesRenovacion = NormalizarOpcional(condicionesRenovacion);
        FechaModificacion = DateTime.UtcNow;
    }

    public ParteConvenio AgregarParte(
        int? entidadId, string nombre, string? alias, string tipoParte,
        bool principal, string? direccion, string? telefono, string? email, string? ruc)
    {
        var item = ParteConvenio.Crear(
            Id, entidadId, nombre, alias, tipoParte, principal,
            direccion, telefono, email, ruc);
        Partes.Add(item);
        return item;
    }

    public FirmanteConvenio AgregarFirmante(
        int? parteId, string nombre, string cargo, string? identificacion,
        string tipoFirma, DateTime? fechaFirma, string? fundamento)
    {
        ValidarParte(parteId);
        var item = FirmanteConvenio.Crear(
            Id, parteId, nombre, cargo, identificacion, tipoFirma, fechaFirma, fundamento);
        Firmantes.Add(item);
        return item;
    }

    public ResponsableConvenio AgregarResponsable(
        int? parteId, string nombre, string rol, string? cargo,
        string? email, string? telefono, bool principal)
    {
        ValidarParte(parteId);
        var item = ResponsableConvenio.Crear(
            Id, parteId, nombre, rol, cargo, email, telefono, principal);
        Responsables.Add(item);
        return item;
    }

    public void ReemplazarAmbitos(IEnumerable<string> ambitos)
    {
        var normalizados = ambitos
            .Where(x => !string.IsNullOrWhiteSpace(x))
            .Select(x => x.Trim())
            .Distinct(StringComparer.OrdinalIgnoreCase)
            .ToList();
        if (normalizados.Count == 0)
            throw new DomainException("Debe seleccionar al menos un ámbito.");

        Ambitos.Clear();
        foreach (var ambito in normalizados)
            Ambitos.Add(AmbitoConvenio.Crear(Id, ambito));
        Ambito = string.Join(", ", Ambitos.Select(x => x.Nombre));
    }

    public ClausulaConvenio AgregarClausula(string titulo, string tipo, string contenido, int orden)
    {
        var item = ClausulaConvenio.Crear(Id, titulo, tipo, contenido, orden);
        Clausulas.Add(item);
        return item;
    }

    public ObligacionConvenio AgregarObligacion(string actor, string descripcion, int orden)
    {
        var item = ObligacionConvenio.Crear(Id, actor, descripcion, orden);
        Obligaciones.Add(item);
        return item;
    }

    public ActividadConvenio AgregarActividad(
        string nombre, string tipo, string? descripcion, DateTime inicio,
        DateTime fin, int horas, string? lugar, string? responsable)
    {
        var item = ActividadConvenio.Crear(
            Id, nombre, tipo, descripcion, inicio, fin, horas, lugar, responsable);
        Actividades.Add(item);
        return item;
    }

    public EstudianteConvenio AgregarEstudiante(
        int? carreraId, string identificacion, string nombre, string? email,
        string? telefono, string? poliza, bool seguro)
    {
        var identificacionNormalizada = Requerido(
            identificacion, "La identificación es obligatoria.");
        if (Estudiantes.Any(x =>
            string.Equals(x.Identificacion, identificacionNormalizada, StringComparison.OrdinalIgnoreCase)))
            throw new DomainException("El estudiante ya está registrado en el convenio.");

        var item = EstudianteConvenio.Crear(
            Id, carreraId, identificacionNormalizada, nombre, email, telefono, poliza, seguro);
        Estudiantes.Add(item);
        return item;
    }

    public ConvenioRelacionado AgregarConvenioRelacionado(int relacionadoId, string tipo)
    {
        if (ConveniosRelacionados.Any(x => x.ConvenioRelacionadoId == relacionadoId))
            throw new DomainException("El convenio relacionado ya está registrado.");
        var item = ConvenioRelacionado.Crear(Id, relacionadoId, tipo);
        ConveniosRelacionados.Add(item);
        return item;
    }

    public void CopiarRepresentantesDesde(Convenio convenioPadre)
    {
        if (convenioPadre.Id == Id)
            throw new DomainException("Un convenio no puede heredar representantes de sí mismo.");

        if (!Partes.Any())
        {
            foreach (var parte in convenioPadre.Partes.OrderBy(x => x.Principal ? 0 : 1).ThenBy(x => x.Nombre))
            {
                Partes.Add(ParteConvenio.Crear(
                    Id,
                    parte.EntidadId,
                    parte.Nombre,
                    parte.Alias,
                    parte.TipoParte,
                    parte.Principal,
                    parte.Direccion,
                    parte.Telefono,
                    parte.Email,
                    parte.Ruc));
            }
        }

        if (!Firmantes.Any())
        {
            foreach (var firmante in convenioPadre.Firmantes.OrderBy(x => x.FechaFirma ?? DateTime.MaxValue).ThenBy(x => x.Nombre))
            {
                Firmantes.Add(FirmanteConvenio.Crear(
                    Id,
                    null,
                    firmante.Nombre,
                    firmante.Cargo,
                    firmante.Identificacion,
                    firmante.TipoFirma,
                    firmante.FechaFirma,
                    firmante.FundamentoRepresentacion));
            }
        }

        if (!Responsables.Any())
        {
            foreach (var responsable in convenioPadre.Responsables.OrderBy(x => x.Principal ? 0 : 1).ThenBy(x => x.Nombre))
            {
                Responsables.Add(ResponsableConvenio.Crear(
                    Id,
                    null,
                    responsable.Nombre,
                    responsable.Rol,
                    responsable.Cargo,
                    responsable.Email,
                    responsable.Telefono,
                    responsable.Principal));
            }
        }

        FechaModificacion = DateTime.UtcNow;
    }

    public ModificacionConvenio AgregarModificacion(
        string tipo, string descripcion, string? motivo, DateTime fecha)
    {
        var item = ModificacionConvenio.Crear(Id, tipo, descripcion, motivo, fecha);
        Modificaciones.Add(item);
        return item;
    }

    public EvaluacionConvenio AgregarEvaluacion(
        string tipo, DateTime fecha, decimal? calificacion,
        string conclusion, string? responsable)
    {
        var item = EvaluacionConvenio.Crear(
            Id, tipo, fecha, calificacion, conclusion, responsable);
        Evaluaciones.Add(item);
        return item;
    }

    public void Cerrar(
        DateTime fecha, string causal, string resumen,
        string? pendientes, string? responsables, string? usuario)
    {
        if (Cierre is not null)
            throw new DomainException("El convenio ya tiene un cierre registrado.");
        Cierre = CierreConvenio.Crear(Id, fecha, causal, resumen, pendientes, responsables);
        CambiarEstado(ConvenioEstados.Terminado, $"Cierre: {causal}", usuario);
    }

    private void AplicarDatos(
        int tipoConvenioId,
        int entidadId,
        int areaPromotoraId,
        int? convenioPadreId,
        string ambito,
        string objeto,
        DateTime fechaInicio,
        DateTime fechaVencimiento,
        string? numeroResolucion,
        string? supervisor,
        string? contactoGestionNombre,
        string? contactoGestionEmail,
        string? contactoGestionTelefono,
        bool renovacionAutomatica,
        string? observaciones)
    {
        if (tipoConvenioId <= 0)
            throw new DomainException("Debe seleccionar un tipo de convenio.");
        if (entidadId <= 0)
            throw new DomainException("Debe seleccionar una entidad contraparte.");
        if (areaPromotoraId <= 0)
            throw new DomainException("Debe seleccionar el área promotora del convenio.");
        if (convenioPadreId.HasValue && convenioPadreId.Value <= 0)
            throw new DomainException("El convenio padre seleccionado no es válido.");
        if (Id > 0 && convenioPadreId == Id)
            throw new DomainException("Un convenio no puede suscribirse a sí mismo.");

        var periodo = PeriodoConvenio.Crear(fechaInicio, fechaVencimiento);
        TipoConvenioId = tipoConvenioId;
        EntidadId = entidadId;
        AreaPromotoraId = areaPromotoraId;
        ConvenioPadreId = convenioPadreId;
        Ambito = Requerido(ambito, "El ámbito es obligatorio.");
        Objeto = Requerido(objeto, "El objeto del convenio es obligatorio.");
        FechaInicio = periodo.Inicio;
        FechaVencimiento = periodo.Fin;
        NumeroResolucion = NormalizarOpcional(numeroResolucion) ?? string.Empty;
        Supervisor = NormalizarOpcional(supervisor);
        ContactoGestionNombre = NormalizarOpcional(contactoGestionNombre);
        ContactoGestionEmail = ValidarEmailOpcional(contactoGestionEmail);
        ContactoGestionTelefono = NormalizarOpcional(contactoGestionTelefono);
        RenovacionAutomatica = renovacionAutomatica;
        Observaciones = NormalizarOpcional(observaciones);
    }

    private void RegistrarHistorial(
        string anterior,
        string nuevo,
        string? observacion,
        string? usuario) =>
        Historial.Add(HistorialEstado.Crear(Id, anterior, nuevo, observacion, usuario));

    private static string Requerido(string valor, string mensaje)
    {
        var normalizado = (valor ?? string.Empty).Trim();
        if (string.IsNullOrWhiteSpace(normalizado))
            throw new DomainException(mensaje);
        return normalizado;
    }

    private static string? NormalizarOpcional(string? valor) =>
        string.IsNullOrWhiteSpace(valor) ? null : valor.Trim();

    private static string? ValidarEmailOpcional(string? valor)
    {
        var normalizado = NormalizarOpcional(valor);
        if (normalizado is null)
            return null;
        if (!normalizado.Contains('@') || normalizado.StartsWith('@') || normalizado.EndsWith('@'))
            throw new DomainException("El correo del contacto de gestión no es válido.");
        return normalizado;
    }

    private void ValidarParte(int? parteId)
    {
        if (parteId.HasValue && Partes.All(x => x.Id != parteId.Value))
            throw new DomainException("La parte seleccionada no pertenece al convenio.");
    }

    private static DateTime? ToUtcNullable(DateTime? value) =>
        value.HasValue
            ? value.Value.Kind == DateTimeKind.Utc
                ? value.Value
                : DateTime.SpecifyKind(value.Value.Date, DateTimeKind.Utc)
            : null;
}
