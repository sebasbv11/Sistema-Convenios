using Microsoft.EntityFrameworkCore;
using SistemaConvenios.Application.Abstractions;
using SistemaConvenios.Models;

namespace SistemaConvenios.Data;

internal sealed class EntidadRepository : IEntidadRepository
{
    private readonly ApplicationDbContext _db;

    public EntidadRepository(ApplicationDbContext db)
    {
        _db = db;
    }

    public async Task<IReadOnlyList<Entidad>> BuscarAsync(
        string? busqueda,
        CancellationToken cancellationToken = default)
    {
        var query = _db.Entidades.AsNoTracking();
        if (!string.IsNullOrWhiteSpace(busqueda))
        {
            var valor = busqueda.Trim();
            query = query.Where(x =>
                x.Nombre.Contains(valor) ||
                x.Provincia.Contains(valor) ||
                x.Ciudad.Contains(valor) ||
                x.Ruc.Contains(valor) ||
                x.TipoEntidad.Contains(valor) ||
                (x.ContactoGestionNombre != null && x.ContactoGestionNombre.Contains(valor)));
        }

        return await query.OrderBy(x => x.Nombre).ToListAsync(cancellationToken);
    }

    public Task<Entidad?> ObtenerAsync(int id, CancellationToken cancellationToken = default) =>
        _db.Entidades.FirstOrDefaultAsync(x => x.Id == id, cancellationToken);

    public Task<bool> ExisteRucAsync(
        string ruc,
        int? exceptoId,
        CancellationToken cancellationToken = default) =>
        _db.Entidades.AnyAsync(
            x => x.Ruc == ruc && (!exceptoId.HasValue || x.Id != exceptoId),
            cancellationToken);

    public Task AgregarAsync(Entidad entidad, CancellationToken cancellationToken = default) =>
        _db.Entidades.AddAsync(entidad, cancellationToken).AsTask();
}
