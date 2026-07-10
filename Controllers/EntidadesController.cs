using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SistemaConvenios.Application.Entidades;
using SistemaConvenios.Models;

namespace SistemaConvenios.Controllers;

[Authorize]
public sealed class EntidadesController : Controller
{
    private readonly IEntidadAppService _entidades;

    public EntidadesController(IEntidadAppService entidades)
    {
        _entidades = entidades;
    }

    public async Task<IActionResult> Index(
        string? busqueda,
        CancellationToken cancellationToken)
    {
        ViewBag.Busqueda = busqueda;
        return View(await _entidades.BuscarAsync(busqueda, cancellationToken));
    }

    [HttpGet]
    public IActionResult Crear() => View(new EntidadFormViewModel());

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Crear(
        EntidadFormViewModel model,
        CancellationToken cancellationToken)
    {
        if (!ModelState.IsValid)
            return View(model);

        var result = await _entidades.CrearAsync(ToCommand(model), cancellationToken);
        if (!result.Succeeded)
        {
            ModelState.AddModelError(string.Empty, result.Error!);
            return View(model);
        }

        TempData["Exito"] = $"Entidad '{model.Nombre}' creada correctamente.";
        return RedirectToAction(nameof(Index));
    }

    [HttpGet]
    public async Task<IActionResult> Editar(int id, CancellationToken cancellationToken)
    {
        var entidad = await _entidades.ObtenerAsync(id, cancellationToken);
        if (entidad is null)
            return NotFound();

        return View(new EntidadFormViewModel
        {
            Id = entidad.Id,
            Nombre = entidad.Nombre,
            TipoEntidad = entidad.TipoEntidad,
            Ruc = entidad.Ruc,
            Direccion = entidad.Direccion,
            Provincia = entidad.Provincia,
            Ciudad = entidad.Ciudad,
            Pais = entidad.Pais,
            RepresentanteLegal = entidad.RepresentanteLegal,
            CedulaRepresentante = entidad.CedulaRepresentante,
            CargoRepresentante = entidad.CargoRepresentante,
            TelefonoRepresentante = entidad.TelefonoRepresentante,
            Email = entidad.Email,
            Telefono = entidad.Telefono,
            ContactoGestionNombre = entidad.ContactoGestionNombre,
            ContactoGestionCargo = entidad.ContactoGestionCargo,
            ContactoGestionEmail = entidad.ContactoGestionEmail,
            ContactoGestionTelefono = entidad.ContactoGestionTelefono,
            Activo = entidad.Activo
        });
    }

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Editar(
        int id,
        EntidadFormViewModel model,
        CancellationToken cancellationToken)
    {
        if (id != model.Id)
            return BadRequest();
        if (!ModelState.IsValid)
            return View(model);

        var result = await _entidades.EditarAsync(id, ToCommand(model), cancellationToken);
        if (!result.Succeeded)
        {
            ModelState.AddModelError(string.Empty, result.Error!);
            return View(model);
        }

        TempData["Exito"] = "Entidad actualizada correctamente.";
        return RedirectToAction(nameof(Index));
    }

    private static GuardarEntidadCommand ToCommand(EntidadFormViewModel model) =>
        new(
            model.Nombre,
            model.TipoEntidad,
            model.Ruc,
            model.Direccion,
            model.Provincia,
            model.Ciudad,
            model.Pais,
            model.RepresentanteLegal,
            model.CedulaRepresentante,
            model.CargoRepresentante,
            model.TelefonoRepresentante,
            model.Email,
            model.Telefono,
            model.ContactoGestionNombre,
            model.ContactoGestionCargo,
            model.ContactoGestionEmail,
            model.ContactoGestionTelefono,
            model.Activo);
}
