using System.ComponentModel.DataAnnotations;

namespace SistemaConvenios.Models;

public static class RolesSistema
{
    public const string Admin = "Admin";
    public const string Secretaria = "Secretaria";
    public const string Visualizador = "Visualizador";
    public static readonly string[] Todos = [Admin, Secretaria, Visualizador];
}

public sealed record UsuarioListaViewModel(
    string Id,
    string Email,
    string NombreCompleto,
    string? Cargo,
    string Rol,
    bool Activo,
    bool Bloqueado);

public sealed class CrearUsuarioViewModel
{
    [Required, EmailAddress, Display(Name = "Correo electrónico")]
    public string Email { get; set; } = string.Empty;

    [Required, StringLength(100), Display(Name = "Nombre completo")]
    public string NombreCompleto { get; set; } = string.Empty;

    [StringLength(50)]
    public string? Cargo { get; set; }

    [Required]
    public string Rol { get; set; } = RolesSistema.Visualizador;

    [Required, DataType(DataType.Password), Display(Name = "Contraseña temporal")]
    public string Password { get; set; } = string.Empty;

    [Required, DataType(DataType.Password), Compare(nameof(Password), ErrorMessage = "Las contraseñas no coinciden")]
    [Display(Name = "Confirmar contraseña")]
    public string ConfirmarPassword { get; set; } = string.Empty;
}

public sealed class EditarUsuarioViewModel
{
    [Required]
    public string Id { get; set; } = string.Empty;

    public string Email { get; set; } = string.Empty;

    [Required, StringLength(100), Display(Name = "Nombre completo")]
    public string NombreCompleto { get; set; } = string.Empty;

    [StringLength(50)]
    public string? Cargo { get; set; }

    [Required]
    public string Rol { get; set; } = RolesSistema.Visualizador;
}

public sealed class RestablecerPasswordAdminViewModel
{
    [Required]
    public string Id { get; set; } = string.Empty;

    public string Email { get; set; } = string.Empty;

    [Required, DataType(DataType.Password), Display(Name = "Nueva contraseña temporal")]
    public string Password { get; set; } = string.Empty;

    [Required, DataType(DataType.Password), Compare(nameof(Password), ErrorMessage = "Las contraseñas no coinciden")]
    [Display(Name = "Confirmar contraseña")]
    public string ConfirmarPassword { get; set; } = string.Empty;
}
