using Microsoft.EntityFrameworkCore;
using SistemaConvenios.Application.Abstractions;
using SistemaConvenios.Models;

namespace SistemaConvenios.Data;

internal sealed class CatalogoRepository : ICatalogoRepository
{
    private readonly ApplicationDbContext _db;

    public CatalogoRepository(ApplicationDbContext db)
    {
        _db = db;
    }

    public async Task<IReadOnlyList<TipoConvenio>> ObtenerTiposConvenioAsync(
        CancellationToken cancellationToken = default) =>
        await _db.TiposConvenio.AsNoTracking().OrderBy(x => x.Nombre).ToListAsync(cancellationToken);

    public async Task<IReadOnlyList<Facultad>> ObtenerFacultadesAsync(
        CancellationToken cancellationToken = default) =>
        await _db.Facultades.AsNoTracking().OrderBy(x => x.Nombre).ToListAsync(cancellationToken);

    public async Task<IReadOnlyList<Carrera>> ObtenerCarrerasAsync(
        CancellationToken cancellationToken = default) =>
        await _db.Carreras
            .AsNoTracking()
            .Include(x => x.Facultad)
            .OrderBy(x => x.Nombre)
            .ToListAsync(cancellationToken);

    public async Task<IReadOnlyList<AreaPromotora>> ObtenerAreasPromotorasAsync(
        CancellationToken cancellationToken = default) =>
        await _db.Set<AreaPromotora>()
            .AsNoTracking()
            .OrderBy(x => x.Codigo)
            .ThenBy(x => x.Nombre)
            .ToListAsync(cancellationToken);
}
