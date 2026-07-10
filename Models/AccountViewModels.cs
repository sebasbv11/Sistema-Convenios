using System.ComponentModel.DataAnnotations;

namespace SistemaConvenios.Models;

public sealed class LoginViewModel
{
    [Required(ErrorMessage = "El correo es obligatorio")]
    [EmailAddress(ErrorMessage = "Correo inválido")]
    [Display(Name = "Correo electrónico")]
    public string Email { get; set; } = string.Empty;

    [Required(ErrorMessage = "La contraseña es obligatoria")]
    [DataType(DataType.Password)]
    [Display(Name = "Contraseña")]
    public string Password { get; set; } = string.Empty;

    [Display(Name = "Recordarme")]
    public bool RememberMe { get; set; }
}

public sealed class CambiarPasswordViewModel
{
    [Required(ErrorMessage = "Campo obligatorio")]
    [DataType(DataType.Password)]
    [Display(Name = "Contraseña actual")]
    public string PasswordActual { get; set; } = string.Empty;

    [Required(ErrorMessage = "Campo obligatorio")]
    [StringLength(100, MinimumLength = 8, ErrorMessage = "Mínimo 8 caracteres")]
    [DataType(DataType.Password)]
    [Display(Name = "Nueva contraseña")]
    public string NuevoPassword { get; set; } = string.Empty;

    [Required(ErrorMessage = "Campo obligatorio")]
    [DataType(DataType.Password)]
    [Display(Name = "Confirmar nueva contraseña")]
    [Compare(nameof(NuevoPassword), ErrorMessage = "Las contraseñas no coinciden")]
    public string ConfirmarPassword { get; set; } = string.Empty;
}

public sealed class OlvidePasswordViewModel
{
    [Required(ErrorMessage = "El correo es obligatorio")]
    [EmailAddress(ErrorMessage = "Correo inválido")]
    [Display(Name = "Correo electrónico")]
    public string Email { get; set; } = string.Empty;
}

public sealed class RestablecerPasswordViewModel
{
    [Required]
    public string Code { get; set; } = string.Empty;

    [Required(ErrorMessage = "El correo es obligatorio")]
    [EmailAddress]
    [Display(Name = "Correo electrónico")]
    public string Email { get; set; } = string.Empty;

    [Required(ErrorMessage = "La contraseña es obligatoria")]
    [StringLength(100, MinimumLength = 8, ErrorMessage = "Mínimo 8 caracteres")]
    [DataType(DataType.Password)]
    [Display(Name = "Nueva contraseña")]
    public string Password { get; set; } = string.Empty;

    [Required(ErrorMessage = "Confirma la contraseña")]
    [DataType(DataType.Password)]
    [Display(Name = "Confirmar contraseña")]
    [Compare(nameof(Password), ErrorMessage = "Las contraseñas no coinciden")]
    public string ConfirmarPassword { get; set; } = string.Empty;
}
