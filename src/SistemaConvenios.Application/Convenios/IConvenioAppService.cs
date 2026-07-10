using SistemaConvenios.Application.Common;

namespace SistemaConvenios.Application.Convenios;

public interface IConvenioAppService
{
    Task<ConvenioIndexDto> BuscarAsync(
        ConvenioFiltro filtro,
        CancellationToken cancellationToken = default);

    Task<ConvenioDetalleDto?> ObtenerDetalleAsync(
        int id,
        CancellationToken cancellationToken = default);

    Task<ConvenioEdicionDto?> ObtenerParaEditarAsync(
        int id,
        CancellationToken cancellationToken = default);

    Task<ConvenioFormularioCatalogosDto> ObtenerCatalogosFormularioAsync(
        CancellationToken cancellationToken = default);

    Task<string> GenerarNumeroAsync(CancellationToken cancellationToken = default);

    Task<OperationResult<int>> CrearAsync(
        CrearConvenioCommand command,
        CancellationToken cancellationToken = default);

    Task<OperationResult> EditarAsync(
        EditarConvenioCommand command,
        CancellationToken cancellationToken = default);

    Task<OperationResult> CambiarEstadoAsync(
        CambiarEstadoCommand command,
        CancellationToken cancellationToken = default);

    Task<OperationResult> SubirArchivoAsync(
        SubirArchivoCommand command,
        CancellationToken cancellationToken = default);

    Task<ArchivoDescargaDto?> DescargarArchivoAsync(
        int id,
        CancellationToken cancellationToken = default);

    Task<OperationResult> EliminarArchivoAsync(
        int archivoId,
        int convenioId,
        CancellationToken cancellationToken = default);

    Task<byte[]> ExportarCsvAsync(
        ConvenioFiltro filtro,
        CancellationToken cancellationToken = default);
}
