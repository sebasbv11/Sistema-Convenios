using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SistemaConvenios.Models;

namespace SistemaConvenios.Controllers;

[Authorize(Roles = RolesSistema.Admin)]
public sealed class UsuariosController : Controller
{
    private readonly UserManager<Usuario> _userManager;

    public UsuariosController(UserManager<Usuario> userManager)
    {
        _userManager = userManager;
    }

    public async Task<IActionResult> Index()
    {
        var usuarios = await _userManager.Users.OrderBy(x => x.NombreCompleto).ToListAsync();
        var resultado = new List<UsuarioListaViewModel>(usuarios.Count);

        foreach (var usuario in usuarios)
        {
            var roles = await _userManager.GetRolesAsync(usuario);
            resultado.Add(new UsuarioListaViewModel(
                usuario.Id,
                usuario.Email ?? string.Empty,
                usuario.NombreCompleto,
                usuario.Cargo,
                roles.FirstOrDefault() ?? "Sin rol",
                usuario.Activo,
                usuario.LockoutEnd.HasValue && usuario.LockoutEnd > DateTimeOffset.UtcNow));
        }

        return View(resultado);
    }

    public IActionResult Crear()
    {
        CargarRoles();
        return View(new CrearUsuarioViewModel());
    }

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Crear(CrearUsuarioViewModel model)
    {
        ValidarRol(model.Rol);
        if (!ModelState.IsValid)
        {
            CargarRoles();
            return View(model);
        }

        var email = model.Email.Trim();
        var usuario = new Usuario
        {
            UserName = email,
            Email = email,
            EmailConfirmed = true,
            NombreCompleto = model.NombreCompleto.Trim(),
            Cargo = model.Cargo?.Trim(),
            Activo = true
        };

        var resultado = await _userManager.CreateAsync(usuario, model.Password);
        if (resultado.Succeeded)
        {
            var rol = await _userManager.AddToRoleAsync(usuario, model.Rol);
            if (rol.Succeeded)
            {
                TempData["Exito"] = "Usuario creado correctamente.";
                return RedirectToAction(nameof(Index));
            }

            await _userManager.DeleteAsync(usuario);
            AgregarErrores(rol);
        }
        else
        {
            AgregarErrores(resultado);
        }

        CargarRoles();
        return View(model);
    }

    public async Task<IActionResult> Editar(string id)
    {
        var usuario = await _userManager.FindByIdAsync(id);
        if (usuario is null)
            return NotFound();

        var roles = await _userManager.GetRolesAsync(usuario);
        CargarRoles();
        return View(new EditarUsuarioViewModel
        {
            Id = usuario.Id,
            Email = usuario.Email ?? string.Empty,
            NombreCompleto = usuario.NombreCompleto,
            Cargo = usuario.Cargo,
            Rol = roles.FirstOrDefault() ?? RolesSistema.Visualizador
        });
    }

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Editar(EditarUsuarioViewModel model)
    {
        ValidarRol(model.Rol);
        var usuario = await _userManager.FindByIdAsync(model.Id);
        if (usuario is null)
            return NotFound();

        var rolesActuales = await _userManager.GetRolesAsync(usuario);
        if (rolesActuales.Contains(RolesSistema.Admin) && model.Rol != RolesSistema.Admin &&
            !await HayOtroAdministradorActivo(usuario.Id))
            ModelState.AddModelError(nameof(model.Rol), "No se puede quitar el rol al último administrador activo.");

        if (!ModelState.IsValid)
        {
            model.Email = usuario.Email ?? string.Empty;
            CargarRoles();
            return View(model);
        }

        usuario.NombreCompleto = model.NombreCompleto.Trim();
        usuario.Cargo = model.Cargo?.Trim();
        var actualizado = await _userManager.UpdateAsync(usuario);
        if (!actualizado.Succeeded)
        {
            AgregarErrores(actualizado);
            CargarRoles();
            return View(model);
        }

        if (!rolesActuales.Contains(model.Rol))
        {
            var agregado = await _userManager.AddToRoleAsync(usuario, model.Rol);
            if (!agregado.Succeeded)
            {
                AgregarErrores(agregado);
                CargarRoles();
                return View(model);
            }
        }

        var sobrantes = rolesActuales.Where(x => x != model.Rol).ToArray();
        if (sobrantes.Length > 0)
            await _userManager.RemoveFromRolesAsync(usuario, sobrantes);

        await _userManager.UpdateSecurityStampAsync(usuario);
        TempData["Exito"] = "Usuario actualizado correctamente.";
        return RedirectToAction(nameof(Index));
    }

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> CambiarActivo(string id)
    {
        var usuario = await _userManager.FindByIdAsync(id);
        if (usuario is null)
            return NotFound();

        var actualId = _userManager.GetUserId(User);
        if (usuario.Id == actualId && usuario.Activo)
        {
            TempData["Error"] = "No puedes desactivar tu propia cuenta.";
            return RedirectToAction(nameof(Index));
        }

        if (usuario.Activo && await _userManager.IsInRoleAsync(usuario, RolesSistema.Admin) &&
            !await HayOtroAdministradorActivo(usuario.Id))
        {
            TempData["Error"] = "No se puede desactivar al último administrador activo.";
            return RedirectToAction(nameof(Index));
        }

        usuario.Activo = !usuario.Activo;
        await _userManager.UpdateAsync(usuario);
        await _userManager.SetLockoutEndDateAsync(usuario, usuario.Activo ? null : DateTimeOffset.MaxValue);
        await _userManager.UpdateSecurityStampAsync(usuario);
        TempData["Exito"] = usuario.Activo ? "Usuario activado." : "Usuario desactivado.";
        return RedirectToAction(nameof(Index));
    }

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Desbloquear(string id)
    {
        var usuario = await _userManager.FindByIdAsync(id);
        if (usuario is null)
            return NotFound();

        await _userManager.SetLockoutEndDateAsync(usuario, null);
        await _userManager.ResetAccessFailedCountAsync(usuario);
        TempData["Exito"] = "Cuenta desbloqueada.";
        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> RestablecerPassword(string id)
    {
        var usuario = await _userManager.FindByIdAsync(id);
        if (usuario is null)
            return NotFound();

        return View(new RestablecerPasswordAdminViewModel
        {
            Id = usuario.Id,
            Email = usuario.Email ?? string.Empty
        });
    }

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> RestablecerPassword(RestablecerPasswordAdminViewModel model)
    {
        var usuario = await _userManager.FindByIdAsync(model.Id);
        if (usuario is null)
            return NotFound();

        model.Email = usuario.Email ?? string.Empty;
        if (!ModelState.IsValid)
            return View(model);

        var token = await _userManager.GeneratePasswordResetTokenAsync(usuario);
        var resultado = await _userManager.ResetPasswordAsync(usuario, token, model.Password);
        if (!resultado.Succeeded)
        {
            AgregarErrores(resultado);
            return View(model);
        }

        await _userManager.SetLockoutEndDateAsync(usuario, null);
        await _userManager.ResetAccessFailedCountAsync(usuario);
        await _userManager.UpdateSecurityStampAsync(usuario);
        TempData["Exito"] = "Contraseña restablecida y sesiones anteriores invalidadas.";
        return RedirectToAction(nameof(Index));
    }

    private async Task<bool> HayOtroAdministradorActivo(string usuarioId)
    {
        var administradores = await _userManager.GetUsersInRoleAsync(RolesSistema.Admin);
        return administradores.Any(x => x.Id != usuarioId && x.Activo);
    }

    private void ValidarRol(string rol)
    {
        if (!RolesSistema.Todos.Contains(rol, StringComparer.Ordinal))
            ModelState.AddModelError("Rol", "El rol seleccionado no es válido.");
    }

    private void CargarRoles() => ViewBag.Roles = RolesSistema.Todos;

    private void AgregarErrores(IdentityResult resultado)
    {
        foreach (var error in resultado.Errors)
            ModelState.AddModelError(string.Empty, error.Description);
    }
}
