using SistemaConvenios.Application.Abstractions;
using SistemaConvenios.Domain;

namespace SistemaConvenios.Application.Convenios;

public sealed class DashboardAppService : IDashboardAppService
{
    private readonly IConvenioRepository _convenios;
    private readonly IUnitOfWork _unitOfWork;

    public DashboardAppService(IConvenioRepository convenios, IUnitOfWork unitOfWork)
    {
        _convenios = convenios;
        _unitOfWork = unitOfWork;
    }

    public async Task<DashboardDto> ObtenerAsync(CancellationToken cancellationToken = default)
    {
        var convenios = await _convenios.ObtenerTodosAsync(cancellationToken);
        var hoy = DateTime.UtcNow.Date;
        var huboCambios = false;

        foreach (var convenio in convenios)
            huboCambios |= convenio.ActualizarEstadoPorFecha(hoy);

        if (huboCambios)
            await _unitOfWork.SaveChangesAsync(cancellationToken);

        var vigentes = convenios.Count(x =>
            ConvenioEstados.Activos.Contains(x.Estado) &&
            x.FechaInicio.Date <= hoy &&
            x.FechaVencimiento.Date >= hoy);

        var porVencer30 = convenios.Count(x =>
            ConvenioEstados.Activos.Contains(x.Estado) &&
            x.FechaVencimiento.Date >= hoy &&
            x.FechaVencimiento.Date <= hoy.AddDays(30));

        var renovadosAutomaticos = convenios.Count(x => x.Estado == ConvenioEstados.RenovadoAutomatico);

        var internacionales = convenios.Count(x =>
            x.TipoConvenio?.Nombre == "Internacional" &&
            ConvenioEstados.Activos.Contains(x.Estado) &&
            x.FechaInicio.Date <= hoy &&
            x.FechaVencimiento.Date >= hoy);

        return new DashboardDto(
            vigentes,
            porVencer30,
            renovadosAutomaticos,
            internacionales,
            convenios
                .OrderByDescending(x => x.FechaCreacion)
                .Take(10)
                .Select(x => x.ToListaDto())
                .ToList(),
            convenios
                .GroupBy(x => x.TipoConvenio?.Nombre ?? "Sin tipo")
                .ToDictionary(x => x.Key, x => x.Count()),
            convenios
                .SelectMany(x => x.ConvenioCarreras)
                .Where(x => x.Carrera is not null)
                .GroupBy(x => x.Carrera!.Nombre)
                .OrderByDescending(x => x.Count())
                .Take(6)
                .ToDictionary(x => x.Key, x => x.Count()),
            convenios
                .GroupBy(x => x.Estado)
                .ToDictionary(x => x.Key, x => x.Count()));
    }
}
