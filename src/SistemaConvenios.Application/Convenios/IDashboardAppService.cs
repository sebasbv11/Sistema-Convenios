namespace SistemaConvenios.Application.Convenios;

public interface IDashboardAppService
{
    Task<DashboardDto> ObtenerAsync(CancellationToken cancellationToken = default);
}
