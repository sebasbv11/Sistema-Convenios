using Microsoft.EntityFrameworkCore;
using SistemaConvenios.Application.Abstractions;
using SistemaConvenios.Application.Convenios;
using SistemaConvenios.Domain;
using SistemaConvenios.Domain.ValueObjects;
using SistemaConvenios.Models;

namespace SistemaConvenios.Data;

internal sealed class ConvenioRepository : IConvenioRepository
{
    private readonly ApplicationDbContext _db;

    public ConvenioRepository(ApplicationDbContext db)
    {
        _db = db;
    }

    public async Task<IReadOnlyList<Convenio>> BuscarAsync(
        ConvenioFiltro filtro,
        CancellationToken cancellationToken = default)
    {
        var query = ConsultaCompleta().AsNoTracking();

        if (!string.IsNullOrWhiteSpace(filtro.Busqueda))
        {
            var busqueda = filtro.Busqueda.Trim();
            query = query.Where(x =>
                x.Numero.Contains(busqueda) ||
                x.Entidad!.Nombre.Contains(busqueda) ||
                x.Objeto.Contains(busqueda));
        }

        if (!string.IsNullOrWhiteSpace(filtro.Estado))
            query = query.Where(x => x.Estado == filtro.Estado);
        if (filtro.TipoId.HasValue)
            query = query.Where(x => x.TipoConvenioId == filtro.TipoId);
        if (filtro.FacultadId.HasValue)
            query = query.Where(x => x.ConvenioFacultades.Any(y => y.FacultadId == filtro.FacultadId));
        if (filtro.CarreraId.HasValue)
            query = query.Where(x => x.ConvenioCarreras.Any(y => y.CarreraId == filtro.CarreraId));
        if (!string.IsNullOrWhiteSpace(filtro.TipoEntidad))
            query = query.Where(x => x.Entidad!.TipoEntidad == filtro.TipoEntidad);
        if (!string.IsNullOrWhiteSpace(filtro.Empresa))
        {
            var empresa = filtro.Empresa.Trim();
            query = query.Where(x => x.Entidad!.Nombre.Contains(empresa));
        }
        if (!string.IsNullOrWhiteSpace(filtro.Provincia))
        {
            var provincia = filtro.Provincia.Trim();
            query = query.Where(x => x.Entidad!.Provincia.Contains(provincia));
        }
        if (!string.IsNullOrWhiteSpace(filtro.Ciudad))
        {
            var ciudad = filtro.Ciudad.Trim();
            query = query.Where(x => x.Entidad!.Ciudad.Contains(ciudad));
        }
        if (filtro.FechaDesde.HasValue)
            query = query.Where(x => x.FechaVencimiento.Date >= filtro.FechaDesde.Value.Date);
        if (filtro.FechaHasta.HasValue)
            query = query.Where(x => x.FechaInicio.Date <= filtro.FechaHasta.Value.Date);
        if (!string.IsNullOrWhiteSpace(filtro.Reporte))
        {
            var hoy = DateTime.UtcNow.Date;
            query = filtro.Reporte.Trim().ToLowerInvariant() switch
            {
                "por-vencer" => query.Where(x =>
                    x.Estado == ConvenioEstados.PorVencer ||
                    (ConvenioEstados.Activos.Contains(x.Estado) &&
                     x.FechaVencimiento.Date >= hoy &&
                     x.FechaVencimiento.Date <= hoy.AddDays(30))),
                "vigentes" => query.Where(x =>
                    ConvenioEstados.Activos.Contains(x.Estado) &&
                    x.FechaInicio.Date <= hoy &&
                    x.FechaVencimiento.Date >= hoy),
                "especificos" => query.Where(x => x.TipoConvenio!.Nombre == "Específico"),
                _ => query
            };
        }

        return await query
            .OrderByDescending(x => x.FechaCreacion)
            .ToListAsync(cancellationToken);
    }

    public async Task<IReadOnlyList<Convenio>> ObtenerTodosAsync(
        CancellationToken cancellationToken = default) =>
        await ConsultaCompleta().ToListAsync(cancellationToken);

    public Task<Convenio?> ObtenerDetalleAsync(
        int id,
        CancellationToken cancellationToken = default) =>
        ConsultaCompleta()
            .AsNoTracking()
            .Include(x => x.Archivos)
            .Include(x => x.Historial)
            .Include(x => x.AreaPromotora)
            .Include(x => x.ConvenioPadre).ThenInclude(x => x!.TipoConvenio)
            .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);

    public Task<Convenio?> ObtenerParaEditarAsync(
        int id,
        CancellationToken cancellationToken = default) =>
        _db.Convenios
            .Include(x => x.ConvenioFacultades)
            .Include(x => x.ConvenioCarreras)
            .Include(x => x.Historial)
            .Include(x => x.Partes)
            .Include(x => x.Firmantes)
            .Include(x => x.Responsables)
            .Include(x => x.Ambitos)
            .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);

    public Task<Convenio?> ObtenerBaseConRepresentantesAsync(
        int id,
        CancellationToken cancellationToken = default) =>
        _db.Convenios
            .AsNoTracking()
            .Include(x => x.TipoConvenio)
            .Include(x => x.Entidad)
            .Include(x => x.Partes)
            .Include(x => x.Firmantes)
            .Include(x => x.Responsables)
            .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);

    public async Task<IReadOnlyList<Convenio>> ObtenerConveniosBaseAsync(
        int? exceptoId = null,
        CancellationToken cancellationToken = default)
    {
        var query = _db.Convenios
            .AsNoTracking()
            .Include(x => x.TipoConvenio)
            .Include(x => x.Entidad)
            .Where(x => x.TipoConvenio!.Nombre == "Marco" || x.TipoConvenio!.Nombre == "Internacional");

        if (exceptoId.HasValue)
            query = query.Where(x => x.Id != exceptoId.Value);

        return await query
            .OrderByDescending(x => x.FechaInicio)
            .ThenBy(x => x.Numero)
            .ToListAsync(cancellationToken);
    }

    public Task<Convenio?> ObtenerParaArchivosAsync(
        int id,
        CancellationToken cancellationToken = default) =>
        _db.Convenios
            .Include(x => x.Archivos)
            .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);

    public Task<ArchivoConvenio?> ObtenerArchivoAsync(
        int id,
        CancellationToken cancellationToken = default) =>
        _db.ArchivosConvenio
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);

    public async Task<string> GenerarNumeroAsync(
        int anio,
        CancellationToken cancellationToken = default)
    {
        var prefijo = $"CV-{anio}-";
        var numeros = await _db.Convenios
            .AsNoTracking()
            .Where(x => x.Numero.StartsWith(prefijo))
            .Select(x => x.Numero)
            .ToListAsync(cancellationToken);

        var siguiente = numeros
            .Select(x => int.TryParse(x.Split('-').LastOrDefault(), out var numero) ? numero : 0)
            .DefaultIfEmpty(0)
            .Max() + 1;

        return NumeroConvenio.Generar(anio, siguiente).Valor;
    }

    public Task AgregarAsync(Convenio convenio, CancellationToken cancellationToken = default) =>
        _db.Convenios.AddAsync(convenio, cancellationToken).AsTask();

    private IQueryable<Convenio> ConsultaCompleta() =>
        _db.Convenios
            .Include(x => x.TipoConvenio)
            .Include(x => x.Entidad)
            .Include(x => x.AreaPromotora)
            .Include(x => x.ConvenioPadre).ThenInclude(x => x!.TipoConvenio)
            .Include(x => x.ConvenioFacultades).ThenInclude(x => x.Facultad)
            .Include(x => x.ConvenioCarreras).ThenInclude(x => x.Carrera)
            .Include(x => x.Ambitos);
}
