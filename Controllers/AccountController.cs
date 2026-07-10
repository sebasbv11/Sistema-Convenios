using System.Text;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using SistemaConvenios.Application.Abstractions;
using SistemaConvenios.Models;

namespace SistemaConvenios.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<Usuario> _userManager;
        private readonly SignInManager<Usuario> _signInManager;
        private readonly IEmailSender _emailSender;
        private readonly IWebHostEnvironment _env;
        private readonly IConfiguration _config;

        public AccountController(
            UserManager<Usuario> userManager,
            SignInManager<Usuario> signInManager,
            IEmailSender emailSender,
            IWebHostEnvironment env,
            IConfiguration config)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _emailSender = emailSender;
            _env = env;
            _config = config;
        }

        // GET /Account/Login
        [AllowAnonymous]
        public IActionResult Login(string? returnUrl = null)
        {
            if (User.Identity?.IsAuthenticated == true)
                return RedirectToAction("Index", "Home");

            ViewData["ReturnUrl"] = returnUrl;
            return View();
        }

        // POST /Account/Login
        [HttpPost, AllowAnonymous, ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model, string? returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;

            if (!ModelState.IsValid)
                return View(model);

            var usuario = await _userManager.FindByEmailAsync(model.Email);
            if (usuario == null || !usuario.Activo)
            {
                ModelState.AddModelError(string.Empty, "Credenciales inválidas.");
                return View(model);
            }

            var result = await _signInManager.PasswordSignInAsync(
                model.Email, model.Password, model.RememberMe, lockoutOnFailure: true);

            if (result.Succeeded)
            {
                if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
                    return Redirect(returnUrl);
                return RedirectToAction("Index", "Home");
            }

            if (result.IsLockedOut)
            {
                ModelState.AddModelError(string.Empty, "Cuenta bloqueada por múltiples intentos fallidos. Intente en 15 minutos.");
                return View(model);
            }

            ModelState.AddModelError(string.Empty, "Correo o contraseña incorrectos.");
            return View(model);
        }

        // POST /Account/Logout
        [HttpPost, Authorize, ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Login");
        }

        // GET /Account/CambiarPassword
        [Authorize]
        public IActionResult CambiarPassword()
        {
            return View();
        }

        // POST /Account/CambiarPassword
        [HttpPost, Authorize, ValidateAntiForgeryToken]
        public async Task<IActionResult> CambiarPassword(CambiarPasswordViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var usuario = await _userManager.GetUserAsync(User);
            if (usuario == null)
                return RedirectToAction("Login");

            var result = await _userManager.ChangePasswordAsync(
                usuario, model.PasswordActual, model.NuevoPassword);

            if (result.Succeeded)
            {
                await _signInManager.RefreshSignInAsync(usuario);
                TempData["Exito"] = "Contraseña cambiada correctamente.";
                return RedirectToAction("Index", "Home");
            }

            foreach (var error in result.Errors)
                ModelState.AddModelError(string.Empty, error.Description);

            return View(model);
        }

        // GET /Account/OlvidePassword
        [AllowAnonymous]
        public IActionResult OlvidePassword() => View();

        // POST /Account/OlvidePassword
        [HttpPost, AllowAnonymous, ValidateAntiForgeryToken]
        public async Task<IActionResult> OlvidePassword(OlvidePasswordViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var usuario = await _userManager.FindByEmailAsync(model.Email);
            if (usuario != null && usuario.Activo)
            {
                var token = await _userManager.GeneratePasswordResetTokenAsync(usuario);
                var code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(token));
                var callbackUrl = Url.Action(
                    nameof(RestablecerPassword),
                    "Account",
                    new { email = model.Email, code },
                    protocol: Request.Scheme)!;

                var cuerpo = $@"
                    <p>Hola {usuario.NombreCompleto},</p>
                    <p>Recibimos una solicitud para restablecer tu contraseña en el Sistema de Convenios FCVT.</p>
                    <p><a href=""{callbackUrl}"">Haz clic aquí para crear una nueva contraseña</a></p>
                    <p>Si no solicitaste este cambio, ignora este correo.</p>
                    <p class=""text-muted"">El enlace expira en 24 horas.</p>";

                await _emailSender.SendEmailAsync(model.Email, "Restablecer contraseña — FCVT", cuerpo);

                if (_env.IsDevelopment() && string.IsNullOrWhiteSpace(_config["Smtp:Host"]))
                    TempData["EnlaceDev"] = callbackUrl;
            }

            return RedirectToAction(nameof(OlvidePasswordConfirmacion));
        }

        [AllowAnonymous]
        public IActionResult OlvidePasswordConfirmacion() => View();

        // GET /Account/RestablecerPassword
        [AllowAnonymous]
        public IActionResult RestablecerPassword(string? code, string? email)
        {
            if (string.IsNullOrEmpty(code))
                return BadRequest("Enlace inválido o incompleto.");

            return View(new RestablecerPasswordViewModel
            {
                Code = code,
                Email = email ?? string.Empty
            });
        }

        // POST /Account/RestablecerPassword
        [HttpPost, AllowAnonymous, ValidateAntiForgeryToken]
        public async Task<IActionResult> RestablecerPassword(RestablecerPasswordViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var usuario = await _userManager.FindByEmailAsync(model.Email);
            if (usuario == null || !usuario.Activo)
            {
                ModelState.AddModelError(string.Empty, "No se pudo restablecer la contraseña. Verifica el enlace o solicita uno nuevo.");
                return View(model);
            }

            string token;
            try
            {
                token = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(model.Code));
            }
            catch
            {
                ModelState.AddModelError(string.Empty, "El enlace de recuperación no es válido.");
                return View(model);
            }

            var result = await _userManager.ResetPasswordAsync(usuario, token, model.Password);
            if (result.Succeeded)
            {
                await _userManager.SetLockoutEndDateAsync(usuario, null);
                await _userManager.ResetAccessFailedCountAsync(usuario);
                TempData["Exito"] = "Tu contraseña fue restablecida. Ya puedes ingresar.";
                return RedirectToAction(nameof(Login));
            }

            foreach (var error in result.Errors)
                ModelState.AddModelError(string.Empty, error.Description);

            return View(model);
        }

        /// <summary>
        /// Solo desarrollo: desbloquea una cuenta tras intentos fallidos.
        /// Visitar: /Account/DesbloquearCuenta?email=admin@fcvt.edu.ec
        /// </summary>
        [AllowAnonymous]
        public async Task<IActionResult> DesbloquearCuenta(string email)
        {
            if (!_env.IsDevelopment())
                return NotFound();

            if (string.IsNullOrWhiteSpace(email))
                return Content("Indica el correo: /Account/DesbloquearCuenta?email=admin@fcvt.edu.ec");

            var usuario = await _userManager.FindByEmailAsync(email);
            if (usuario == null)
                return Content($"No existe el usuario {email}.");

            await _userManager.SetLockoutEndDateAsync(usuario, null);
            await _userManager.ResetAccessFailedCountAsync(usuario);

            return Content($"Cuenta {email} desbloqueada. Ya puedes ingresar con tu contraseña actual.");
        }

        [AllowAnonymous]
        public IActionResult AccesoDenegado() => View();
    }
}
