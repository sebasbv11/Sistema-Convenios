namespace SistemaConvenios.Application.Convenios;

public sealed record ConvenioFiltro(
    string? Busqueda = null,
    string? Estado = null,
    int? TipoId = null,
    int? FacultadId = null,
    int? CarreraId = null,
    string? TipoEntidad = null,
    string? Empresa = null,
    string? Provincia = null,
    string? Ciudad = null,
    DateTime? FechaDesde = null,
    DateTime? FechaHasta = null,
    string? Reporte = null);

public sealed record CrearConvenioCommand(
    int TipoConvenioId,
    int EntidadId,
    int AreaPromotoraId,
    int? ConvenioPadreId,
    IReadOnlyCollection<string> Ambitos,
    string Objeto,
    DateTime FechaInicio,
    DateTime FechaVencimiento,
    string Estado,
    string? NumeroResolucion,
    string? Supervisor,
    string? ContactoGestionNombre,
    string? ContactoGestionEmail,
    string? ContactoGestionTelefono,
    bool RenovacionAutomatica,
    string? Observaciones,
    IReadOnlyCollection<int> FacultadIds,
    IReadOnlyCollection<int> CarreraIds,
    string? UsuarioId,
    string? Usuario);

public sealed record EditarConvenioCommand(
    int Id,
    int TipoConvenioId,
    int EntidadId,
    int AreaPromotoraId,
    int? ConvenioPadreId,
    IReadOnlyCollection<string> Ambitos,
    string Objeto,
    DateTime FechaInicio,
    DateTime FechaVencimiento,
    string Estado,
    string? NumeroResolucion,
    string? Supervisor,
    string? ContactoGestionNombre,
    string? ContactoGestionEmail,
    string? ContactoGestionTelefono,
    bool RenovacionAutomatica,
    string? Observaciones,
    IReadOnlyCollection<int> FacultadIds,
    IReadOnlyCollection<int> CarreraIds,
    string? Usuario);

public sealed record CambiarEstadoCommand(
    int Id,
    string NuevoEstado,
    string? Observacion,
    string? Usuario);

public sealed record SubirArchivoCommand(
    int ConvenioId,
    string NombreOriginal,
    string TipoDocumento,
    string? Descripcion,
    long TamanioBytes,
    Stream Contenido,
    string? Usuario);

public sealed record CatalogoItemDto(int Id, string Nombre, string? Siglas = null, int? PadreId = null);

public sealed record ConvenioListaDto(
    int Id,
    string Numero,
    string Entidad,
    string TipoConvenio,
    string Ambito,
    string? AreaPromotora,
    string? ConvenioPadreNumero,
    DateTime FechaInicio,
    DateTime FechaVencimiento,
    string? Provincia,
    string? Ciudad,
    string Estado);

public sealed record ArchivoConvenioDto(
    int Id,
    string NombreOriginal,
    string TipoDocumento,
    string? Descripcion,
    long TamanioBytes,
    DateTime FechaSubida);

public sealed record HistorialEstadoDto(
    DateTime FechaCambio,
    string EstadoAnterior,
    string EstadoNuevo,
    string? Observacion,
    string? CambiadoPor);

public sealed record EntidadResumenDto(
    int Id,
    string Nombre,
    string TipoEntidad,
    string Ruc,
    string RepresentanteLegal,
    string CargoRepresentante,
    string? TelefonoRepresentante,
    string? ContactoGestionNombre,
    string? ContactoGestionEmail,
    string? ContactoGestionTelefono,
    string Provincia,
    string Ciudad,
    string Pais,
    string Email);

public sealed record ConvenioDetalleDto(
    int Id,
    string Numero,
    int TipoConvenioId,
    string TipoConvenio,
    int EntidadId,
    EntidadResumenDto? Entidad,
    int AreaPromotoraId,
    string? AreaPromotora,
    int? ConvenioPadreId,
    string? ConvenioPadreNumero,
    string? ConvenioPadreTipo,
    string Ambito,
    string Objeto,
    DateTime FechaInicio,
    DateTime FechaVencimiento,
    string Estado,
    string NumeroResolucion,
    string? Supervisor,
    string? ContactoGestionNombre,
    string? ContactoGestionEmail,
    string? ContactoGestionTelefono,
    bool RenovacionAutomatica,
    string? Observaciones,
    IReadOnlyList<CatalogoItemDto> Facultades,
    IReadOnlyList<CatalogoItemDto> Carreras,
    IReadOnlyList<ArchivoConvenioDto> Archivos,
    IReadOnlyList<HistorialEstadoDto> Historial);

public sealed record ConvenioEdicionDto(
    int Id,
    string Numero,
    int TipoConvenioId,
    int EntidadId,
    int AreaPromotoraId,
    int? ConvenioPadreId,
    IReadOnlyList<string> Ambitos,
    string Objeto,
    DateTime FechaInicio,
    DateTime FechaVencimiento,
    string Estado,
    string NumeroResolucion,
    string? Supervisor,
    string? ContactoGestionNombre,
    string? ContactoGestionEmail,
    string? ContactoGestionTelefono,
    bool RenovacionAutomatica,
    string? Observaciones,
    IReadOnlyList<int> FacultadIds,
    IReadOnlyList<int> CarreraIds);

public sealed record ConvenioIndexDto(
    IReadOnlyList<ConvenioListaDto> Convenios,
    IReadOnlyList<CatalogoItemDto> Tipos,
    IReadOnlyList<CatalogoItemDto> Facultades,
    IReadOnlyList<CatalogoItemDto> Carreras);

public sealed record ConvenioFormularioCatalogosDto(
    IReadOnlyList<CatalogoItemDto> Tipos,
    IReadOnlyList<CatalogoItemDto> Entidades,
    IReadOnlyList<CatalogoItemDto> AreasPromotoras,
    IReadOnlyList<CatalogoItemDto> ConveniosBase,
    IReadOnlyList<CatalogoItemDto> Facultades,
    IReadOnlyList<CatalogoItemDto> Carreras);

public sealed record ArchivoDescargaDto(string Nombre, Stream Contenido);

public sealed record DashboardDto(
    int Vigentes,
    int PorVencer30,
    int RenovadosAutomaticos,
    int Internacionales,
    IReadOnlyList<ConvenioListaDto> Recientes,
    IReadOnlyDictionary<string, int> PorTipo,
    IReadOnlyDictionary<string, int> PorCarrera,
    IReadOnlyDictionary<string, int> PorEstado);
