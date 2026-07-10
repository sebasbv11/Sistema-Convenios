using SistemaConvenios.Application.Convenios;
using SistemaConvenios.Application.Entidades;
using SistemaConvenios.Models;

namespace SistemaConvenios.Application.Abstractions;

public interface IUnitOfWork
{
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}

public interface IConvenioRepository
{
    Task<IReadOnlyList<Convenio>> BuscarAsync(
        ConvenioFiltro filtro,
        CancellationToken cancellationToken = default);

    Task<IReadOnlyList<Convenio>> ObtenerTodosAsync(
        CancellationToken cancellationToken = default);

    Task<Convenio?> ObtenerDetalleAsync(
        int id,
        CancellationToken cancellationToken = default);

    Task<Convenio?> ObtenerParaEditarAsync(
        int id,
        CancellationToken cancellationToken = default);

    Task<Convenio?> ObtenerBaseConRepresentantesAsync(
        int id,
        CancellationToken cancellationToken = default);

    Task<IReadOnlyList<Convenio>> ObtenerConveniosBaseAsync(
        int? exceptoId = null,
        CancellationToken cancellationToken = default);

    Task<Convenio?> ObtenerParaArchivosAsync(
        int id,
        CancellationToken cancellationToken = default);

    Task<ArchivoConvenio?> ObtenerArchivoAsync(
        int id,
        CancellationToken cancellationToken = default);

    Task<string> GenerarNumeroAsync(
        int anio,
        CancellationToken cancellationToken = default);

    Task AgregarAsync(Convenio convenio, CancellationToken cancellationToken = default);
}

public interface IGestionContractualRepository
{
    Task<Convenio?> ObtenerAsync(int convenioId, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<Convenio>> ObtenerConveniosDisponiblesAsync(
        int exceptoId,
        CancellationToken cancellationToken = default);
    Task EliminarAsync(
        int convenioId,
        string tipo,
        int id,
        CancellationToken cancellationToken = default);
}

public interface IEntidadRepository
{
    Task<IReadOnlyList<Entidad>> BuscarAsync(
        string? busqueda,
        CancellationToken cancellationToken = default);

    Task<Entidad?> ObtenerAsync(int id, CancellationToken cancellationToken = default);

    Task<bool> ExisteRucAsync(
        string ruc,
        int? exceptoId,
        CancellationToken cancellationToken = default);

    Task AgregarAsync(Entidad entidad, CancellationToken cancellationToken = default);
}

public interface ICatalogoRepository
{
    Task<IReadOnlyList<TipoConvenio>> ObtenerTiposConvenioAsync(
        CancellationToken cancellationToken = default);

    Task<IReadOnlyList<Facultad>> ObtenerFacultadesAsync(
        CancellationToken cancellationToken = default);

    Task<IReadOnlyList<Carrera>> ObtenerCarrerasAsync(
        CancellationToken cancellationToken = default);

    Task<IReadOnlyList<AreaPromotora>> ObtenerAreasPromotorasAsync(
        CancellationToken cancellationToken = default);
}

public interface IArchivoStorage
{
    Task<string> GuardarPdfAsync(
        int convenioId,
        string nombreOriginal,
        Stream contenido,
        CancellationToken cancellationToken = default);

    Task<Stream?> AbrirLecturaAsync(
        string ruta,
        CancellationToken cancellationToken = default);

    Task EliminarAsync(string ruta, CancellationToken cancellationToken = default);
}

public interface IEmailSender
{
    Task SendEmailAsync(string to, string subject, string htmlBody);
}
