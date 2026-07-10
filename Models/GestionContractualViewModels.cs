using System.ComponentModel.DataAnnotations;

namespace SistemaConvenios.Models;

public sealed class DatosLegalesViewModel
{
    public int ConvenioId { get; set; }
    [DataType(DataType.Date)] public DateTime? FechaSuscripcion { get; set; }
    [DataType(DataType.Date)] public DateTime? FechaUltimaFirma { get; set; }
    [StringLength(300)] public string? DomicilioContractual { get; set; }
    [StringLength(1000)] public string? MecanismoSolucionControversias { get; set; }
    public bool ConfidencialidadIndefinida { get; set; }
    public bool TieneErogacion { get; set; }
    [Range(0, double.MaxValue)] public decimal? Presupuesto { get; set; }
    [StringLength(10)] public string Moneda { get; set; } = "USD";
    [StringLength(300)] public string? FuenteFinanciamiento { get; set; }
    [StringLength(500)] public string? CondicionesRenovacion { get; set; }
    public List<string> Ambitos { get; set; } = new();
}

public sealed class ParteConvenioViewModel
{
    public int ConvenioId { get; set; }
    public int? EntidadId { get; set; }
    [Required, StringLength(200)] public string Nombre { get; set; } = string.Empty;
    [StringLength(50)] public string? Alias { get; set; }
    [Required, StringLength(50)] public string TipoParte { get; set; } = "Contraparte";
    public bool Principal { get; set; }
    [StringLength(300)] public string? Direccion { get; set; }
    [StringLength(50)] public string? Telefono { get; set; }
    [EmailAddress, StringLength(150)] public string? Email { get; set; }
    [StringLength(20)] public string? Ruc { get; set; }
}

public sealed class FirmanteConvenioViewModel
{
    public int ConvenioId { get; set; }
    public int? ParteId { get; set; }
    [Required, StringLength(200)] public string Nombre { get; set; } = string.Empty;
    [Required, StringLength(150)] public string Cargo { get; set; } = string.Empty;
    [StringLength(20)] public string? Identificacion { get; set; }
    [Required] public string TipoFirma { get; set; } = "Electrónica";
    [DataType(DataType.Date)] public DateTime? FechaFirma { get; set; }
    [StringLength(500)] public string? FundamentoRepresentacion { get; set; }
}

public sealed class ResponsableConvenioViewModel
{
    public int ConvenioId { get; set; }
    public int? ParteId { get; set; }
    [Required, StringLength(200)] public string Nombre { get; set; } = string.Empty;
    [Required, StringLength(100)] public string Rol { get; set; } = "Supervisor";
    [StringLength(150)] public string? Cargo { get; set; }
    [EmailAddress, StringLength(150)] public string? Email { get; set; }
    [StringLength(50)] public string? Telefono { get; set; }
    public bool Principal { get; set; }
}

public sealed class ClausulaViewModel
{
    public int ConvenioId { get; set; }
    [Required, StringLength(200)] public string Titulo { get; set; } = string.Empty;
    [Required, StringLength(80)] public string Tipo { get; set; } = "General";
    [Required] public string Contenido { get; set; } = string.Empty;
    [Range(1, 100)] public int Orden { get; set; } = 1;
}

public sealed class ObligacionViewModel
{
    public int ConvenioId { get; set; }
    [Required, StringLength(200)] public string Actor { get; set; } = string.Empty;
    [Required] public string Descripcion { get; set; } = string.Empty;
    [Range(1, 100)] public int Orden { get; set; } = 1;
}

public sealed class ActividadViewModel
{
    public int ConvenioId { get; set; }
    [Required, StringLength(200)] public string Nombre { get; set; } = string.Empty;
    [Required, StringLength(80)] public string Tipo { get; set; } = "Vinculación";
    [StringLength(1000)] public string? Descripcion { get; set; }
    [DataType(DataType.Date)] public DateTime FechaInicio { get; set; } = DateTime.Today;
    [DataType(DataType.Date)] public DateTime FechaFin { get; set; } = DateTime.Today;
    [Range(0, 10000)] public int HorasPlanificadas { get; set; }
    [StringLength(300)] public string? Lugar { get; set; }
    [StringLength(200)] public string? Responsable { get; set; }
}

public sealed class EstudianteConvenioViewModel
{
    public int ConvenioId { get; set; }
    public int? CarreraId { get; set; }
    [Required, StringLength(20)] public string Identificacion { get; set; } = string.Empty;
    [Required, StringLength(200)] public string Nombre { get; set; } = string.Empty;
    [EmailAddress, StringLength(150)] public string? Email { get; set; }
    [StringLength(50)] public string? Telefono { get; set; }
    [StringLength(100)] public string? NumeroPoliza { get; set; }
    public bool CubiertoSeguro { get; set; }
}

public sealed class ParticipacionViewModel
{
    public int ConvenioId { get; set; }
    [Required] public int ActividadId { get; set; }
    [Required] public int EstudianteId { get; set; }
    [Range(0, 10000)] public int HorasAsignadas { get; set; }
}

public sealed class EvaluarParticipacionViewModel
{
    public int ConvenioId { get; set; }
    public int ParticipacionId { get; set; }
    [Range(0, 10000)] public int HorasCumplidas { get; set; }
    [Range(0, 100)] public decimal? Calificacion { get; set; }
    [StringLength(1000)] public string? Evaluacion { get; set; }
    public bool CertificadoEmitido { get; set; }
}

public sealed class RelacionConvenioViewModel
{
    public int ConvenioId { get; set; }
    [Required] public int ConvenioRelacionadoId { get; set; }
    [Required, StringLength(80)] public string TipoRelacion { get; set; } = "Específico";
}

public sealed class ModificacionViewModel
{
    public int ConvenioId { get; set; }
    [Required, StringLength(80)] public string Tipo { get; set; } = "Adenda";
    [Required] public string Descripcion { get; set; } = string.Empty;
    [StringLength(500)] public string? Motivo { get; set; }
    [DataType(DataType.Date)] public DateTime Fecha { get; set; } = DateTime.Today;
}

public sealed class EvaluacionConvenioViewModel
{
    public int ConvenioId { get; set; }
    [Required, StringLength(80)] public string Tipo { get; set; } = "Seguimiento";
    [DataType(DataType.Date)] public DateTime Fecha { get; set; } = DateTime.Today;
    [Range(0, 100)] public decimal? Calificacion { get; set; }
    [Required] public string Conclusion { get; set; } = string.Empty;
    [StringLength(200)] public string? Responsable { get; set; }
}

public sealed class CierreConvenioViewModel
{
    public int ConvenioId { get; set; }
    [DataType(DataType.Date)] public DateTime Fecha { get; set; } = DateTime.Today;
    [Required, StringLength(200)] public string Causal { get; set; } = string.Empty;
    [Required] public string Resumen { get; set; } = string.Empty;
    [StringLength(1000)] public string? ObligacionesPendientes { get; set; }
    [StringLength(500)] public string? ResponsablesSeguimiento { get; set; }
}
