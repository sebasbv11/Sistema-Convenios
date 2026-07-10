using SistemaConvenios.Application.Abstractions;
using SistemaConvenios.Application.Common;
using SistemaConvenios.Domain.Exceptions;
using SistemaConvenios.Models;

namespace SistemaConvenios.Application.Convenios;

public sealed class GestionContractualAppService : IGestionContractualAppService
{
    private readonly IGestionContractualRepository _repository;
    private readonly IEntidadRepository _entidades;
    private readonly ICatalogoRepository _catalogos;
    private readonly IUnitOfWork _unitOfWork;

    public GestionContractualAppService(
        IGestionContractualRepository repository,
        IEntidadRepository entidades,
        ICatalogoRepository catalogos,
        IUnitOfWork unitOfWork)
    {
        _repository = repository;
        _entidades = entidades;
        _catalogos = catalogos;
        _unitOfWork = unitOfWork;
    }

    public async Task<GestionContractualDto?> ObtenerAsync(
        int convenioId,
        CancellationToken cancellationToken = default)
    {
        var convenio = await _repository.ObtenerAsync(convenioId, cancellationToken);
        if (convenio is null)
            return null;

        var entidades = await _entidades.BuscarAsync(null, cancellationToken);
        var carreras = await _catalogos.ObtenerCarrerasAsync(cancellationToken);
        var disponibles = await _repository.ObtenerConveniosDisponiblesAsync(convenioId, cancellationToken);

        return new GestionContractualDto(
            convenio.Id,
            convenio.Numero,
            convenio.Estado,
            new DatosLegalesDto(
                convenio.FechaSuscripcion,
                convenio.FechaUltimaFirma,
                convenio.DomicilioContractual,
                convenio.MecanismoSolucionControversias,
                convenio.ConfidencialidadIndefinida,
                convenio.TieneErogacion,
                convenio.Presupuesto,
                convenio.Moneda,
                convenio.FuenteFinanciamiento,
                convenio.CondicionesRenovacion),
            convenio.Ambitos.Select(x => x.Nombre).ToList(),
            convenio.Partes.Select(x => new ParteDto(
                x.Id, x.EntidadId, x.Nombre, x.Alias, x.TipoParte, x.Principal,
                x.Direccion, x.Telefono, x.Email, x.Ruc)).ToList(),
            convenio.Firmantes.Select(x => new FirmanteDto(
                x.Id, x.ParteConvenioId, x.Nombre, x.Cargo, x.Identificacion,
                x.TipoFirma, x.FechaFirma, x.FundamentoRepresentacion)).ToList(),
            convenio.Responsables.Select(x => new ResponsableDto(
                x.Id, x.ParteConvenioId, x.Nombre, x.Rol, x.Cargo,
                x.Email, x.Telefono, x.Principal)).ToList(),
            convenio.Clausulas.OrderBy(x => x.Orden).Select(x =>
                new ClausulaDto(x.Id, x.Titulo, x.Tipo, x.Contenido, x.Orden)).ToList(),
            convenio.Obligaciones.OrderBy(x => x.Actor).ThenBy(x => x.Orden).Select(x =>
                new ObligacionDto(x.Id, x.Actor, x.Descripcion, x.Orden,
                    x.Estado, x.FechaCumplimiento, x.Evidencia)).ToList(),
            convenio.Actividades.OrderBy(x => x.FechaInicio).Select(x =>
                new ActividadDto(x.Id, x.Nombre, x.Tipo, x.Descripcion,
                    x.FechaInicio, x.FechaFin, x.HorasPlanificadas, x.Estado,
                    x.Lugar, x.Responsable, x.Participantes.Count)).ToList(),
            convenio.Estudiantes.OrderBy(x => x.Nombre).Select(x =>
                new EstudianteDto(x.Id, x.CarreraId, x.Carrera?.Nombre,
                    x.Identificacion, x.Nombre, x.Email, x.Telefono,
                    x.NumeroPoliza, x.CubiertoSeguro)).ToList(),
            convenio.Actividades.SelectMany(x => x.Participantes).Select(x =>
                new ParticipacionDto(
                    x.Id, x.ActividadConvenioId, x.ActividadConvenio?.Nombre ?? "",
                    x.EstudianteConvenioId, x.EstudianteConvenio?.Nombre ?? "",
                    x.HorasAsignadas, x.HorasCumplidas, x.Calificacion,
                    x.Evaluacion, x.CertificadoEmitido, x.FechaCertificado)).ToList(),
            convenio.ConveniosRelacionados.Select(x =>
                new RelacionDto(x.Id, x.ConvenioRelacionadoId,
                    x.Relacionado?.Numero ?? "", x.TipoRelacion)).ToList(),
            convenio.Modificaciones.OrderByDescending(x => x.Fecha).Select(x =>
                new ModificacionDto(x.Id, x.Tipo, x.Descripcion, x.Motivo, x.Fecha)).ToList(),
            convenio.Evaluaciones.OrderByDescending(x => x.Fecha).Select(x =>
                new EvaluacionDto(x.Id, x.Tipo, x.Fecha, x.Calificacion,
                    x.Conclusion, x.Responsable)).ToList(),
            convenio.Cierre is null ? null : new CierreDto(
                convenio.Cierre.Id, convenio.Cierre.Fecha, convenio.Cierre.Causal,
                convenio.Cierre.Resumen, convenio.Cierre.ObligacionesPendientes,
                convenio.Cierre.ResponsablesSeguimiento),
            entidades.Where(x => x.Activo).Select(x =>
                new CatalogoItemDto(x.Id, x.Nombre)).ToList(),
            carreras.Where(x => x.Activo).Select(x =>
                new CatalogoItemDto(x.Id, x.Nombre, x.Siglas, x.FacultadId)).ToList(),
            disponibles.Select(x =>
                new CatalogoItemDto(x.Id, $"{x.Numero} - {x.Entidad?.Nombre}")).ToList());
    }

    public Task<OperationResult> ConfigurarDatosLegalesAsync(
        ConfigurarDatosLegalesCommand command, CancellationToken cancellationToken = default) =>
        Ejecutar(command.ConvenioId, convenio =>
        {
            convenio.ConfigurarDatosLegales(
                command.FechaSuscripcion, command.FechaUltimaFirma,
                command.DomicilioContractual, command.MecanismoSolucionControversias,
                command.ConfidencialidadIndefinida, command.TieneErogacion,
                command.Presupuesto, command.Moneda, command.FuenteFinanciamiento,
                command.CondicionesRenovacion);
            convenio.ReemplazarAmbitos(command.Ambitos);
        }, cancellationToken);

    public Task<OperationResult> AgregarParteAsync(
        AgregarParteCommand c, CancellationToken ct = default) =>
        Ejecutar(c.ConvenioId, x => x.AgregarParte(
            c.EntidadId, c.Nombre, c.Alias, c.TipoParte, c.Principal,
            c.Direccion, c.Telefono, c.Email, c.Ruc), ct);

    public Task<OperationResult> AgregarFirmanteAsync(
        AgregarFirmanteCommand c, CancellationToken ct = default) =>
        Ejecutar(c.ConvenioId, x => x.AgregarFirmante(
            c.ParteId, c.Nombre, c.Cargo, c.Identificacion,
            c.TipoFirma, c.FechaFirma, c.FundamentoRepresentacion), ct);

    public Task<OperationResult> AgregarResponsableAsync(
        AgregarResponsableCommand c, CancellationToken ct = default) =>
        Ejecutar(c.ConvenioId, x => x.AgregarResponsable(
            c.ParteId, c.Nombre, c.Rol, c.Cargo, c.Email, c.Telefono, c.Principal), ct);

    public Task<OperationResult> AgregarClausulaAsync(
        AgregarClausulaCommand c, CancellationToken ct = default) =>
        Ejecutar(c.ConvenioId, x => x.AgregarClausula(c.Titulo, c.Tipo, c.Contenido, c.Orden), ct);

    public Task<OperationResult> AgregarObligacionAsync(
        AgregarObligacionCommand c, CancellationToken ct = default) =>
        Ejecutar(c.ConvenioId, x => x.AgregarObligacion(c.Actor, c.Descripcion, c.Orden), ct);

    public Task<OperationResult> CumplirObligacionAsync(
        int convenioId, int obligacionId, string? evidencia, CancellationToken ct = default) =>
        Ejecutar(convenioId, x =>
        {
            var item = x.Obligaciones.FirstOrDefault(y => y.Id == obligacionId)
                ?? throw new DomainException("La obligación no existe.");
            item.MarcarCumplida(evidencia);
        }, ct);

    public Task<OperationResult> AgregarActividadAsync(
        AgregarActividadCommand c, CancellationToken ct = default) =>
        Ejecutar(c.ConvenioId, x => x.AgregarActividad(
            c.Nombre, c.Tipo, c.Descripcion, c.FechaInicio, c.FechaFin,
            c.HorasPlanificadas, c.Lugar, c.Responsable), ct);

    public Task<OperationResult> AgregarEstudianteAsync(
        AgregarEstudianteCommand c, CancellationToken ct = default) =>
        Ejecutar(c.ConvenioId, x => x.AgregarEstudiante(
            c.CarreraId, c.Identificacion, c.Nombre, c.Email, c.Telefono,
            c.NumeroPoliza, c.CubiertoSeguro), ct);

    public Task<OperationResult> AgregarParticipacionAsync(
        AgregarParticipacionCommand c, CancellationToken ct = default) =>
        Ejecutar(c.ConvenioId, x =>
        {
            var actividad = x.Actividades.FirstOrDefault(y => y.Id == c.ActividadId)
                ?? throw new DomainException("La actividad no existe.");
            if (!x.Estudiantes.Any(y => y.Id == c.EstudianteId))
                throw new DomainException("El estudiante no pertenece al convenio.");
            if (actividad.Participantes.Any(y => y.EstudianteConvenioId == c.EstudianteId))
                throw new DomainException("El estudiante ya está asignado a esta actividad.");
            actividad.Participantes.Add(
                ParticipacionActividad.Crear(c.ActividadId, c.EstudianteId, c.HorasAsignadas));
        }, ct);

    public Task<OperationResult> EvaluarParticipacionAsync(
        EvaluarParticipacionCommand c, CancellationToken ct = default) =>
        Ejecutar(c.ConvenioId, x =>
        {
            var item = x.Actividades.SelectMany(y => y.Participantes)
                .FirstOrDefault(y => y.Id == c.ParticipacionId)
                ?? throw new DomainException("La participación no existe.");
            item.Evaluar(c.HorasCumplidas, c.Calificacion, c.Evaluacion, c.CertificadoEmitido);
        }, ct);

    public Task<OperationResult> AgregarRelacionAsync(
        AgregarRelacionCommand c, CancellationToken ct = default) =>
        Ejecutar(c.ConvenioId, x => x.AgregarConvenioRelacionado(c.ConvenioRelacionadoId, c.TipoRelacion), ct);

    public Task<OperationResult> AgregarModificacionAsync(
        AgregarModificacionCommand c, CancellationToken ct = default) =>
        Ejecutar(c.ConvenioId, x => x.AgregarModificacion(c.Tipo, c.Descripcion, c.Motivo, c.Fecha), ct);

    public Task<OperationResult> AgregarEvaluacionAsync(
        AgregarEvaluacionCommand c, CancellationToken ct = default) =>
        Ejecutar(c.ConvenioId, x => x.AgregarEvaluacion(
            c.Tipo, c.Fecha, c.Calificacion, c.Conclusion, c.Responsable), ct);

    public Task<OperationResult> RegistrarCierreAsync(
        RegistrarCierreCommand c, CancellationToken ct = default) =>
        Ejecutar(c.ConvenioId, x => x.Cerrar(
            c.Fecha, c.Causal, c.Resumen, c.ObligacionesPendientes,
            c.ResponsablesSeguimiento, c.Usuario), ct);

    public async Task<OperationResult> EliminarAsync(
        int convenioId, string tipo, int id, CancellationToken cancellationToken = default)
    {
        var convenio = await _repository.ObtenerAsync(convenioId, cancellationToken);
        if (convenio is null)
            return OperationResult.Failure("El convenio no existe.");
        await _repository.EliminarAsync(convenioId, tipo, id, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        return OperationResult.Success();
    }

    private async Task<OperationResult> Ejecutar(
        int convenioId, Action<Convenio> action, CancellationToken cancellationToken)
    {
        var convenio = await _repository.ObtenerAsync(convenioId, cancellationToken);
        if (convenio is null)
            return OperationResult.Failure("El convenio no existe.");
        try
        {
            action(convenio);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
            return OperationResult.Success();
        }
        catch (DomainException ex)
        {
            return OperationResult.Failure(ex.Message);
        }
    }
}
