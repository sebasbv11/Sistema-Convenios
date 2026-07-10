using SistemaConvenios.Domain;
using SistemaConvenios.Domain.Exceptions;

namespace SistemaConvenios.Models;

public sealed class ConvenioFacultad
{
    private ConvenioFacultad()
    {
    }

    private ConvenioFacultad(int convenioId, int facultadId)
    {
        ConvenioId = convenioId;
        FacultadId = facultadId;
    }

    public int ConvenioId { get; private set; }
    public Convenio? Convenio { get; private set; }
    public int FacultadId { get; private set; }
    public Facultad? Facultad { get; private set; }

    public static ConvenioFacultad Crear(int convenioId, int facultadId)
    {
        if (facultadId <= 0)
            throw new DomainException("El identificador de facultad no es válido.");
        return new ConvenioFacultad(convenioId, facultadId);
    }
}

public sealed class ConvenioCarrera
{
    private ConvenioCarrera()
    {
    }

    private ConvenioCarrera(int convenioId, int carreraId)
    {
        ConvenioId = convenioId;
        CarreraId = carreraId;
    }

    public int ConvenioId { get; private set; }
    public Convenio? Convenio { get; private set; }
    public int CarreraId { get; private set; }
    public Carrera? Carrera { get; private set; }

    public static ConvenioCarrera Crear(int convenioId, int carreraId)
    {
        if (carreraId <= 0)
            throw new DomainException("El identificador de carrera no es válido.");
        return new ConvenioCarrera(convenioId, carreraId);
    }
}

public sealed class HistorialEstado
{
    private HistorialEstado()
    {
    }

    private HistorialEstado(
        int convenioId,
        string estadoAnterior,
        string estadoNuevo,
        string? observacion,
        string? cambiadoPor)
    {
        ConvenioId = convenioId;
        EstadoAnterior = estadoAnterior;
        EstadoNuevo = ConvenioEstados.Validar(estadoNuevo);
        Observacion = string.IsNullOrWhiteSpace(observacion) ? null : observacion.Trim();
        FechaCambio = DateTime.UtcNow;
        CambiadoPor = string.IsNullOrWhiteSpace(cambiadoPor) ? null : cambiadoPor.Trim();
    }

    public int Id { get; private set; }
    public int ConvenioId { get; private set; }
    public Convenio? Convenio { get; private set; }
    public string EstadoAnterior { get; private set; } = string.Empty;
    public string EstadoNuevo { get; private set; } = string.Empty;
    public string? Observacion { get; private set; }
    public DateTime FechaCambio { get; private set; } = DateTime.UtcNow;
    public string? CambiadoPor { get; private set; }

    public static HistorialEstado Crear(
        int convenioId,
        string estadoAnterior,
        string estadoNuevo,
        string? observacion,
        string? cambiadoPor) =>
        new(convenioId, estadoAnterior, estadoNuevo, observacion, cambiadoPor);
}

public sealed class ArchivoConvenio
{
    private ArchivoConvenio()
    {
    }

    private ArchivoConvenio(
        int convenioId,
        string nombreOriginal,
        string rutaFisica,
        string tipoDocumento,
        string? descripcion,
        long tamanioBytes,
        string? subidoPor)
    {
        if (tamanioBytes <= 0)
            throw new DomainException("El archivo está vacío.");

        ConvenioId = convenioId;
        NombreOriginal = Requerido(nombreOriginal, "El nombre del archivo es obligatorio.");
        RutaFisica = Requerido(rutaFisica, "La ruta del archivo es obligatoria.");
        TipoDocumento = Requerido(tipoDocumento, "El tipo de documento es obligatorio.");
        Descripcion = string.IsNullOrWhiteSpace(descripcion) ? null : descripcion.Trim();
        TamanioBytes = tamanioBytes;
        FechaSubida = DateTime.UtcNow;
        SubidoPor = string.IsNullOrWhiteSpace(subidoPor) ? null : subidoPor.Trim();
    }

    public int Id { get; private set; }
    public int ConvenioId { get; private set; }
    public Convenio? Convenio { get; private set; }
    public string NombreOriginal { get; private set; } = string.Empty;
    public string RutaFisica { get; private set; } = string.Empty;
    public string TipoDocumento { get; private set; } = "Convenio";
    public string? Descripcion { get; private set; }
    public long TamanioBytes { get; private set; }
    public DateTime FechaSubida { get; private set; } = DateTime.UtcNow;
    public string? SubidoPor { get; private set; }

    public static ArchivoConvenio Crear(
        int convenioId,
        string nombreOriginal,
        string rutaFisica,
        string tipoDocumento,
        string? descripcion,
        long tamanioBytes,
        string? subidoPor) =>
        new(
            convenioId, nombreOriginal, rutaFisica, tipoDocumento,
            descripcion, tamanioBytes, subidoPor);

    private static string Requerido(string valor, string mensaje)
    {
        var normalizado = (valor ?? string.Empty).Trim();
        if (string.IsNullOrWhiteSpace(normalizado))
            throw new DomainException(mensaje);
        return normalizado;
    }
}
