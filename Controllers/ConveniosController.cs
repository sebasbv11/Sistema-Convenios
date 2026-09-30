using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using SistemaConvenios.Application.Convenios;
using SistemaConvenios.Domain;
using SistemaConvenios.Models;

namespace SistemaConvenios.Controllers;

[Authorize]
public sealed class ConveniosController : Controller
{
    private readonly IConvenioAppService _convenios;
    private readonly UserManager<Usuario> _userManager;

    public ConveniosController(
        IConvenioAppService convenios,
        UserManager<Usuario> userManager)
    {
        _convenios = convenios;
        _userManager = userManager;
    }

    public async Task<IActionResult> Index(
        string? busqueda,
        string? estado,
        int? tipoId,
        int? facultadId,
        int? carreraId,
        string? tipoEntidad,
        string? empresa,
        string? provincia,
        string? ciudad,
        DateTime? fechaDesde,
        DateTime? fechaHasta,
        string? reporte,
        CancellationToken cancellationToken)
    {
        var filtro = new ConvenioFiltro(
            busqueda, estado, tipoId, facultadId, carreraId, tipoEntidad,
            empresa, provincia, ciudad, fechaDesde, fechaHasta, reporte);
        var resultado = await _convenios.BuscarAsync(filtro, cancellationToken);
        CargarFiltros(resultado, filtro);
        return View(resultado.Convenios);
    }

    public async Task<IActionResult> ExportarCsv(
        string? busqueda,
        string? estado,
        int? tipoId,
        int? facultadId,
        int? carreraId,
        string? tipoEntidad,
        string? empresa,
        string? provincia,
        string? ciudad,
        DateTime? fechaDesde,
        DateTime? fechaHasta,
        string? reporte,
        CancellationToken cancellationToken)
    {
        var bytes = await _convenios.ExportarCsvAsync(
            new ConvenioFiltro(
                busqueda, estado, tipoId, facultadId, carreraId, tipoEntidad,
                empresa, provincia, ciudad, fechaDesde, fechaHasta, reporte),
            cancellationToken);
        return File(bytes, "text/csv; charset=utf-8", $"convenios_{DateTime.Now:yyyyMMdd_HHmm}.csv");
    }

    public async Task<IActionResult> Detalle(int id, CancellationToken cancellationToken)
    {
        var convenio = await _convenios.ObtenerDetalleAsync(id, cancellationToken);
        return convenio is null ? NotFound() : View(convenio);
    }

    [HttpGet, Authorize(Roles = "Admin,Secretaria")]
    public async Task<IActionResult> Crear(CancellationToken cancellationToken)
    {
        await CargarCatalogosFormulario(cancellationToken: cancellationToken);
        return View(new ConvenioFormViewModel
        {
            Numero = await _convenios.GenerarNumeroAsync(cancellationToken),
            FechaInicio = DateTime.Today,
            FechaVencimiento = DateTime.Today.AddYears(2),
            Estado = ConvenioEstados.Vigente
        });
    }

    [HttpPost, Authorize(Roles = "Admin,Secretaria"), ValidateAntiForgeryToken]
    public async Task<IActionResult> Crear(
        ConvenioFormViewModel model,
        CancellationToken cancellationToken)
    {
        if (!ModelState.IsValid)
        {
            await CargarCatalogosFormulario(model, cancellationToken);
            return View(model);
        }

        var result = await _convenios.CrearAsync(
            new CrearConvenioCommand(
                model.TipoConvenioId!.Value,
                model.EntidadId!.Value,
                model.AreaPromotoraId!.Value,
                model.ConvenioPadreId,
                model.Ambitos,
                model.Objeto,
                model.FechaInicio,
                model.FechaVencimiento,
                model.Estado,
                model.NumeroResolucion,
                model.Supervisor,
                model.ContactoGestionNombre,
                model.ContactoGestionEmail,
                model.ContactoGestionTelefono,
                model.RenovacionAutomatica,
                model.Observaciones,
                model.FacultadIds,
                model.CarreraIds,
                _userManager.GetUserId(User),
                User.Identity?.Name),
            cancellationToken);

        if (!result.Succeeded)
        {
            ModelState.AddModelError(string.Empty, result.Error!);
            await CargarCatalogosFormulario(model, cancellationToken);
            return View(model);
        }

        TempData["Exito"] = $"Convenio creado correctamente.";
        return RedirectToAction(nameof(Detalle), new { id = result.Value });
    }

    [HttpGet, Authorize(Roles = "Admin,Secretaria")]
    public async Task<IActionResult> Editar(int id, CancellationToken cancellationToken)
    {
        var convenio = await _convenios.ObtenerParaEditarAsync(id, cancellationToken);
        if (convenio is null)
            return NotFound();

        var model = new ConvenioFormViewModel
        {
            Id = convenio.Id,
            Numero = convenio.Numero,
            TipoConvenioId = convenio.TipoConvenioId,
            EntidadId = convenio.EntidadId,
            AreaPromotoraId = convenio.AreaPromotoraId,
            ConvenioPadreId = convenio.ConvenioPadreId,
            Ambitos = convenio.Ambitos.ToList(),
            Objeto = convenio.Objeto,
            FechaInicio = convenio.FechaInicio,
            FechaVencimiento = convenio.FechaVencimiento,
            Estado = convenio.Estado,
            NumeroResolucion = convenio.NumeroResolucion,
            Supervisor = convenio.Supervisor,
            ContactoGestionNombre = convenio.ContactoGestionNombre,
            ContactoGestionEmail = convenio.ContactoGestionEmail,
            ContactoGestionTelefono = convenio.ContactoGestionTelefono,
            RenovacionAutomatica = convenio.RenovacionAutomatica,
            Observaciones = convenio.Observaciones,
            FacultadIds = convenio.FacultadIds.ToList(),
            CarreraIds = convenio.CarreraIds.ToList()
        };
        await CargarCatalogosFormulario(model, cancellationToken);
        return View(model);
    }

    [HttpPost, Authorize(Roles = "Admin,Secretaria"), ValidateAntiForgeryToken]
    public async Task<IActionResult> Editar(
        int id,
        ConvenioFormViewModel model,
        CancellationToken cancellationToken)
    {
        if (id != model.Id)
            return BadRequest();
        if (!ModelState.IsValid)
        {
            await CargarCatalogosFormulario(model, cancellationToken);
            return View(model);
        }

        var result = await _convenios.EditarAsync(
            new EditarConvenioCommand(
                id,
                model.TipoConvenioId!.Value,
                model.EntidadId!.Value,
                model.AreaPromotoraId!.Value,
                model.ConvenioPadreId,
                model.Ambitos,
                model.Objeto,
                model.FechaInicio,
                model.FechaVencimiento,
                model.Estado,
                model.NumeroResolucion,
                model.Supervisor,
                model.ContactoGestionNombre,
                model.ContactoGestionEmail,
                model.ContactoGestionTelefono,
                model.RenovacionAutomatica,
                model.Observaciones,
                model.FacultadIds,
                model.CarreraIds,
                User.Identity?.Name),
            cancellationToken);

        if (!result.Succeeded)
        {
            ModelState.AddModelError(string.Empty, result.Error!);
            await CargarCatalogosFormulario(model, cancellationToken);
            return View(model);
        }

        TempData["Exito"] = "Convenio actualizado correctamente.";
        return RedirectToAction(nameof(Detalle), new { id });
    }

    [HttpPost, Authorize(Roles = "Admin,Secretaria"), ValidateAntiForgeryToken]
    public async Task<IActionResult> CambiarEstado(
        int id,
        string nuevoEstado,
        string? observacion,
        CancellationToken cancellationToken)
    {
        var result = await _convenios.CambiarEstadoAsync(
            new CambiarEstadoCommand(id, nuevoEstado, observacion, User.Identity?.Name),
            cancellationToken);
        TempData[result.Succeeded ? "Exito" : "Error"] =
            result.Succeeded ? $"Estado cambiado a: {nuevoEstado}" : result.Error;
        return RedirectToAction(nameof(Detalle), new { id });
    }

    [HttpPost, Authorize(Roles = "Admin,Secretaria"), ValidateAntiForgeryToken]
    public async Task<IActionResult> SubirArchivo(
        SubirArchivoViewModel model,
        CancellationToken cancellationToken)
    {
        if (!ModelState.IsValid || model.Archivo is null)
        {
            TempData["Error"] = "Selecciona un archivo PDF válido.";
            return RedirectToAction(nameof(Detalle), new { id = model.ConvenioId });
        }

        await using var stream = model.Archivo.OpenReadStream();
        var result = await _convenios.SubirArchivoAsync(
            new SubirArchivoCommand(
                model.ConvenioId,
                model.Archivo.FileName,
                model.TipoDocumento,
                model.Descripcion,
                model.Archivo.Length,
                stream,
                User.Identity?.Name),
            cancellationToken);

        TempData[result.Succeeded ? "Exito" : "Error"] =
            result.Succeeded ? $"Archivo '{model.Archivo.FileName}' subido correctamente." : result.Error;
        return RedirectToAction(nameof(Detalle), new { id = model.ConvenioId });
    }

    public async Task<IActionResult> DescargarArchivo(int id, CancellationToken cancellationToken)
    {
        var archivo = await _convenios.DescargarArchivoAsync(id, cancellationToken);
        return archivo is null
            ? NotFound("Archivo no encontrado en el servidor.")
            : File(archivo.Contenido, "application/pdf", archivo.Nombre);
    }

    [HttpPost, Authorize(Roles = "Admin,Secretaria"), ValidateAntiForgeryToken]
    public async Task<IActionResult> EliminarArchivo(
        int id,
        int convenioId,
        CancellationToken cancellationToken)
    {
        var result = await _convenios.EliminarArchivoAsync(id, convenioId, cancellationToken);
        TempData[result.Succeeded ? "Exito" : "Error"] =
            result.Succeeded ? "Archivo eliminado." : result.Error;
        return RedirectToAction(nameof(Detalle), new { id = convenioId });
    }

    private async Task CargarCatalogosFormulario(
        ConvenioFormViewModel? model = null,
        CancellationToken cancellationToken = default)
    {
        var catalogos = await _convenios.ObtenerCatalogosFormularioAsync(cancellationToken);
        ViewBag.TiposConvenio = new SelectList(
            catalogos.Tipos, "Id", "Nombre", model?.TipoConvenioId);
        ViewBag.Entidades = new SelectList(
            catalogos.Entidades, "Id", "Nombre", model?.EntidadId);
        ViewBag.AreasPromotoras = new SelectList(
            catalogos.AreasPromotoras, "Id", "Nombre", model?.AreaPromotoraId);
        ViewBag.ConveniosBase = new SelectList(
            catalogos.ConveniosBase, "Id", "Nombre", model?.ConvenioPadreId);
        ViewBag.Facultades = catalogos.Facultades;
        ViewBag.Carreras = catalogos.Carreras;
        ViewBag.Ambitos = new MultiSelectList(
            new[] { "Vinculación", "Prácticas", "Académico", "Investigativo", "Ayuda/Posgrado", "Congresos", "Otro" },
            model?.Ambitos);
        ViewBag.Estados = new SelectList(ConvenioEstados.Todos, model?.Estado);
        ViewBag.TipoEspecificoId = catalogos.Tipos
            .FirstOrDefault(x => x.Nombre.Equals("Específico", StringComparison.OrdinalIgnoreCase))
            ?.Id;
    }

    private void CargarFiltros(ConvenioIndexDto resultado, ConvenioFiltro filtro)
    {
        ViewBag.Busqueda = filtro.Busqueda;
        ViewBag.EstadoFiltro = filtro.Estado;
        ViewBag.TipoFiltro = filtro.TipoId;
        ViewBag.FacultadFiltro = filtro.FacultadId;
        ViewBag.CarreraFiltro = filtro.CarreraId;
        ViewBag.TipoEntidadFiltro = filtro.TipoEntidad;
        ViewBag.EmpresaFiltro = filtro.Empresa;
        ViewBag.ProvinciaFiltro = filtro.Provincia;
        ViewBag.CiudadFiltro = filtro.Ciudad;
        ViewBag.FechaDesdeFiltro = filtro.FechaDesde?.ToString("yyyy-MM-dd");
        ViewBag.FechaHastaFiltro = filtro.FechaHasta?.ToString("yyyy-MM-dd");
        ViewBag.ReporteFiltro = filtro.Reporte;
        ViewBag.TiposConvenio = new SelectList(resultado.Tipos, "Id", "Nombre", filtro.TipoId);
        ViewBag.Facultades = new SelectList(resultado.Facultades, "Id", "Nombre", filtro.FacultadId);
        ViewBag.Carreras = new SelectList(resultado.Carreras, "Id", "Nombre", filtro.CarreraId);
    }
}
