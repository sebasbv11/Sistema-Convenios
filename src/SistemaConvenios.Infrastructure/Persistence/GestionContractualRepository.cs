using Microsoft.EntityFrameworkCore;
using SistemaConvenios.Application.Abstractions;
using SistemaConvenios.Models;

namespace SistemaConvenios.Data;

internal sealed class GestionContractualRepository : IGestionContractualRepository
{
    private readonly ApplicationDbContext _db;
    public GestionContractualRepository(ApplicationDbContext db) => _db = db;

    public Task<Convenio?> ObtenerAsync(int convenioId, CancellationToken cancellationToken = default) =>
        _db.Convenios
            .Include(x => x.Entidad)
            .Include(x => x.Partes)
            .Include(x => x.Firmantes)
            .Include(x => x.Responsables)
            .Include(x => x.Ambitos)
            .Include(x => x.Clausulas)
            .Include(x => x.Obligaciones)
            .Include(x => x.Actividades).ThenInclude(x => x.Participantes).ThenInclude(x => x.EstudianteConvenio)
            .Include(x => x.Estudiantes).ThenInclude(x => x.Carrera)
            .Include(x => x.ConveniosRelacionados).ThenInclude(x => x.Relacionado).ThenInclude(x => x!.Entidad)
            .Include(x => x.Modificaciones)
            .Include(x => x.Evaluaciones)
            .Include(x => x.Cierre)
            .Include(x => x.Historial)
            .FirstOrDefaultAsync(x => x.Id == convenioId, cancellationToken);

    public async Task<IReadOnlyList<Convenio>> ObtenerConveniosDisponiblesAsync(
        int exceptoId, CancellationToken cancellationToken = default) =>
        await _db.Convenios.AsNoTracking().Include(x => x.Entidad)
            .Where(x => x.Id != exceptoId).OrderByDescending(x => x.FechaCreacion)
            .ToListAsync(cancellationToken);

    public async Task EliminarAsync(
        int convenioId,
        string tipo,
        int id,
        CancellationToken cancellationToken = default)
    {
        object? entity = tipo.ToLowerInvariant() switch
        {
            "parte" => await _db.PartesConvenio.FirstOrDefaultAsync(
                x => x.Id == id && x.ConvenioId == convenioId, cancellationToken),
            "firmante" => await _db.FirmantesConvenio.FirstOrDefaultAsync(
                x => x.Id == id && x.ConvenioId == convenioId, cancellationToken),
            "responsable" => await _db.ResponsablesConvenio.FirstOrDefaultAsync(
                x => x.Id == id && x.ConvenioId == convenioId, cancellationToken),
            "clausula" => await _db.ClausulasConvenio.FirstOrDefaultAsync(
                x => x.Id == id && x.ConvenioId == convenioId, cancellationToken),
            "obligacion" => await _db.ObligacionesConvenio.FirstOrDefaultAsync(
                x => x.Id == id && x.ConvenioId == convenioId, cancellationToken),
            "actividad" => await _db.ActividadesConvenio.FirstOrDefaultAsync(
                x => x.Id == id && x.ConvenioId == convenioId, cancellationToken),
            "estudiante" => await _db.EstudiantesConvenio.FirstOrDefaultAsync(
                x => x.Id == id && x.ConvenioId == convenioId, cancellationToken),
            "participacion" => await _db.ParticipacionesActividad
                .Where(x => x.Id == id)
                .Where(x => x.ActividadConvenio != null &&
                    x.ActividadConvenio.ConvenioId == convenioId)
                .FirstOrDefaultAsync(cancellationToken),
            "relacion" => await _db.ConveniosRelacionados.FirstOrDefaultAsync(
                x => x.Id == id && x.ConvenioId == convenioId, cancellationToken),
            "modificacion" => await _db.ModificacionesConvenio.FirstOrDefaultAsync(
                x => x.Id == id && x.ConvenioId == convenioId, cancellationToken),
            "evaluacion" => await _db.EvaluacionesConvenio.FirstOrDefaultAsync(
                x => x.Id == id && x.ConvenioId == convenioId, cancellationToken),
            _ => null
        };
        if (entity is not null)
            _db.Remove(entity);
    }
}
