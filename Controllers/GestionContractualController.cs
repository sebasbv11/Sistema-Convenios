using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SistemaConvenios.Application.Common;
using SistemaConvenios.Application.Convenios;
using SistemaConvenios.Models;

namespace SistemaConvenios.Controllers;

[Authorize]
[Route("Convenios/{convenioId:int}/Gestion")]
public sealed class GestionContractualController : Controller
{
    private readonly IGestionContractualAppService _gestion;
    public GestionContractualController(IGestionContractualAppService gestion) => _gestion = gestion;

    [HttpGet("")]
    public async Task<IActionResult> Index(int convenioId, CancellationToken ct)
    {
        var model = await _gestion.ObtenerAsync(convenioId, ct);
        return model is null ? NotFound() : View(model);
    }

    [HttpPost("DatosLegales"), Authorize(Roles = "Admin,Secretaria"), ValidateAntiForgeryToken]
    public async Task<IActionResult> DatosLegales(int convenioId, DatosLegalesViewModel m, CancellationToken ct)
    {
        if (!ModelState.IsValid)
            return InvalidModel(convenioId);

        return Redirect(convenioId, await _gestion.ConfigurarDatosLegalesAsync(new(
            convenioId, m.FechaSuscripcion, m.FechaUltimaFirma, m.DomicilioContractual,
            m.MecanismoSolucionControversias, m.ConfidencialidadIndefinida,
            m.TieneErogacion, m.Presupuesto, m.Moneda, m.FuenteFinanciamiento,
            m.CondicionesRenovacion, m.Ambitos), ct));
    }

    [HttpPost("Parte"), Authorize(Roles = "Admin,Secretaria"), ValidateAntiForgeryToken]
    public async Task<IActionResult> Parte(int convenioId, ParteConvenioViewModel m, CancellationToken ct)
    {
        if (!ModelState.IsValid)
            return InvalidModel(convenioId);

        return Redirect(convenioId, await _gestion.AgregarParteAsync(new(
            convenioId, m.EntidadId, m.Nombre, m.Alias, m.TipoParte, m.Principal,
            m.Direccion, m.Telefono, m.Email, m.Ruc), ct));
    }

    [HttpPost("Firmante"), Authorize(Roles = "Admin,Secretaria"), ValidateAntiForgeryToken]
    public async Task<IActionResult> Firmante(int convenioId, FirmanteConvenioViewModel m, CancellationToken ct)
    {
        if (!ModelState.IsValid)
            return InvalidModel(convenioId);

        return Redirect(convenioId, await _gestion.AgregarFirmanteAsync(new(
            convenioId, m.ParteId, m.Nombre, m.Cargo, m.Identificacion,
            m.TipoFirma, m.FechaFirma, m.FundamentoRepresentacion), ct));
    }

    [HttpPost("Responsable"), Authorize(Roles = "Admin,Secretaria"), ValidateAntiForgeryToken]
    public async Task<IActionResult> Responsable(int convenioId, ResponsableConvenioViewModel m, CancellationToken ct)
    {
        if (!ModelState.IsValid)
            return InvalidModel(convenioId);

        return Redirect(convenioId, await _gestion.AgregarResponsableAsync(new(
            convenioId, m.ParteId, m.Nombre, m.Rol, m.Cargo,
            m.Email, m.Telefono, m.Principal), ct));
    }

    [HttpPost("Clausula"), Authorize(Roles = "Admin,Secretaria"), ValidateAntiForgeryToken]
    public async Task<IActionResult> Clausula(int convenioId, ClausulaViewModel m, CancellationToken ct)
    {
        if (!ModelState.IsValid)
            return InvalidModel(convenioId);

        return Redirect(convenioId, await _gestion.AgregarClausulaAsync(new(
            convenioId, m.Titulo, m.Tipo, m.Contenido, m.Orden), ct));
    }

    [HttpPost("Obligacion"), Authorize(Roles = "Admin,Secretaria"), ValidateAntiForgeryToken]
    public async Task<IActionResult> Obligacion(int convenioId, ObligacionViewModel m, CancellationToken ct)
    {
        if (!ModelState.IsValid)
            return InvalidModel(convenioId);

        return Redirect(convenioId, await _gestion.AgregarObligacionAsync(new(
            convenioId, m.Actor, m.Descripcion, m.Orden), ct));
    }

    [HttpPost("Obligacion/{id:int}/Cumplir"), Authorize(Roles = "Admin,Secretaria"), ValidateAntiForgeryToken]
    public async Task<IActionResult> CumplirObligacion(
        int convenioId, int id, string? evidencia, CancellationToken ct) =>
        Redirect(convenioId, await _gestion.CumplirObligacionAsync(convenioId, id, evidencia, ct));

    [HttpPost("Actividad"), Authorize(Roles = "Admin,Secretaria"), ValidateAntiForgeryToken]
    public async Task<IActionResult> Actividad(int convenioId, ActividadViewModel m, CancellationToken ct)
    {
        if (!ModelState.IsValid)
            return InvalidModel(convenioId);

        return Redirect(convenioId, await _gestion.AgregarActividadAsync(new(
            convenioId, m.Nombre, m.Tipo, m.Descripcion, m.FechaInicio,
            m.FechaFin, m.HorasPlanificadas, m.Lugar, m.Responsable), ct));
    }

    [HttpPost("Estudiante"), Authorize(Roles = "Admin,Secretaria"), ValidateAntiForgeryToken]
    public async Task<IActionResult> Estudiante(int convenioId, EstudianteConvenioViewModel m, CancellationToken ct)
    {
        if (!ModelState.IsValid)
            return InvalidModel(convenioId);

        return Redirect(convenioId, await _gestion.AgregarEstudianteAsync(new(
            convenioId, m.CarreraId, m.Identificacion, m.Nombre, m.Email,
            m.Telefono, m.NumeroPoliza, m.CubiertoSeguro), ct));
    }

    [HttpPost("Participacion"), Authorize(Roles = "Admin,Secretaria"), ValidateAntiForgeryToken]
    public async Task<IActionResult> Participacion(int convenioId, ParticipacionViewModel m, CancellationToken ct)
    {
        if (!ModelState.IsValid)
            return InvalidModel(convenioId);

        return Redirect(convenioId, await _gestion.AgregarParticipacionAsync(new(
            convenioId, m.ActividadId, m.EstudianteId, m.HorasAsignadas), ct));
    }

    [HttpPost("Participacion/Evaluar"), Authorize(Roles = "Admin,Secretaria"), ValidateAntiForgeryToken]
    public async Task<IActionResult> EvaluarParticipacion(
        int convenioId, EvaluarParticipacionViewModel m, CancellationToken ct)
    {
        if (!ModelState.IsValid)
            return InvalidModel(convenioId);

        return Redirect(convenioId, await _gestion.EvaluarParticipacionAsync(new(
            convenioId, m.ParticipacionId, m.HorasCumplidas, m.Calificacion,
            m.Evaluacion, m.CertificadoEmitido), ct));
    }

    [HttpPost("Relacion"), Authorize(Roles = "Admin,Secretaria"), ValidateAntiForgeryToken]
    public async Task<IActionResult> Relacion(int convenioId, RelacionConvenioViewModel m, CancellationToken ct)
    {
        if (!ModelState.IsValid)
            return InvalidModel(convenioId);

        return Redirect(convenioId, await _gestion.AgregarRelacionAsync(new(
            convenioId, m.ConvenioRelacionadoId, m.TipoRelacion), ct));
    }

    [HttpPost("Modificacion"), Authorize(Roles = "Admin,Secretaria"), ValidateAntiForgeryToken]
    public async Task<IActionResult> Modificacion(int convenioId, ModificacionViewModel m, CancellationToken ct)
    {
        if (!ModelState.IsValid)
            return InvalidModel(convenioId);

        return Redirect(convenioId, await _gestion.AgregarModificacionAsync(new(
            convenioId, m.Tipo, m.Descripcion, m.Motivo, m.Fecha), ct));
    }

    [HttpPost("Evaluacion"), Authorize(Roles = "Admin,Secretaria"), ValidateAntiForgeryToken]
    public async Task<IActionResult> Evaluacion(int convenioId, EvaluacionConvenioViewModel m, CancellationToken ct)
    {
        if (!ModelState.IsValid)
            return InvalidModel(convenioId);

        return Redirect(convenioId, await _gestion.AgregarEvaluacionAsync(new(
            convenioId, m.Tipo, m.Fecha, m.Calificacion, m.Conclusion, m.Responsable), ct));
    }

    [HttpPost("Cierre"), Authorize(Roles = "Admin,Secretaria"), ValidateAntiForgeryToken]
    public async Task<IActionResult> Cierre(int convenioId, CierreConvenioViewModel m, CancellationToken ct)
    {
        if (!ModelState.IsValid)
            return InvalidModel(convenioId);

        return Redirect(convenioId, await _gestion.RegistrarCierreAsync(new(
            convenioId, m.Fecha, m.Causal, m.Resumen, m.ObligacionesPendientes,
            m.ResponsablesSeguimiento, User.Identity?.Name), ct));
    }

    [HttpPost("Eliminar"), Authorize(Roles = "Admin,Secretaria"), ValidateAntiForgeryToken]
    public async Task<IActionResult> Eliminar(
        int convenioId, string tipo, int id, CancellationToken ct) =>
        Redirect(convenioId, await _gestion.EliminarAsync(convenioId, tipo, id, ct));

    private IActionResult Redirect(int convenioId, OperationResult result)
    {
        TempData[result.Succeeded ? "Exito" : "Error"] =
            result.Succeeded ? "Información contractual guardada." : result.Error;
        return RedirectToAction(nameof(Index), new { convenioId });
    }

    private IActionResult InvalidModel(int convenioId)
    {
        var errors = ModelState.Values
            .SelectMany(value => value.Errors)
            .Select(error => error.ErrorMessage)
            .Where(message => !string.IsNullOrWhiteSpace(message))
            .Distinct()
            .ToArray();

        TempData["Error"] = errors.Length == 0
            ? "Revisa los datos ingresados."
            : string.Join(" ", errors);

        return RedirectToAction(nameof(Index), new { convenioId });
    }
}
