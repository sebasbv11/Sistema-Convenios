using SistemaConvenios.Application.Common;

namespace SistemaConvenios.Application.Entidades;

public interface IEntidadAppService
{
    Task<IReadOnlyList<EntidadListaDto>> BuscarAsync(
        string? busqueda,
        CancellationToken cancellationToken = default);

    Task<EntidadEdicionDto?> ObtenerAsync(
        int id,
        CancellationToken cancellationToken = default);

    Task<OperationResult<int>> CrearAsync(
        GuardarEntidadCommand command,
        CancellationToken cancellationToken = default);

    Task<OperationResult> EditarAsync(
        int id,
        GuardarEntidadCommand command,
        CancellationToken cancellationToken = default);
}
