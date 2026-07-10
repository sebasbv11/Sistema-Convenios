using System.ComponentModel.DataAnnotations;

namespace SistemaConvenios.Models;

public sealed class EntidadFormViewModel
{
    public int Id { get; set; }

    [Required, StringLength(200)]
    [Display(Name = "Nombre de la entidad")]
    public string Nombre { get; set; } = string.Empty;

    [Required, StringLength(50)]
    [Display(Name = "Tipo de entidad")]
    public string TipoEntidad { get; set; } = "Privada";

    [Required(ErrorMessage = "El RUC o cédula es obligatorio")]
    [RegularExpression(@"^(\d{10}|\d{13})$", ErrorMessage = "Ingrese 10 dígitos para cédula o 13 para RUC")]
    [Display(Name = "RUC / Cédula")]
    public string Ruc { get; set; } = string.Empty;

    [Required(ErrorMessage = "La dirección es obligatoria"), StringLength(300)]
    [Display(Name = "Dirección")]
    public string Direccion { get; set; } = string.Empty;

    [Required(ErrorMessage = "La provincia es obligatoria"), StringLength(100)]
    [Display(Name = "Provincia")]
    public string Provincia { get; set; } = "Manabí";

    [Required(ErrorMessage = "La ciudad es obligatoria"), StringLength(100)]
    [Display(Name = "Ciudad")]
    public string Ciudad { get; set; } = string.Empty;

    [Required, StringLength(100)]
    [Display(Name = "País")]
    public string Pais { get; set; } = "Ecuador";

    [Required(ErrorMessage = "El representante legal es obligatorio"), StringLength(150)]
    [Display(Name = "Representante legal")]
    public string RepresentanteLegal { get; set; } = string.Empty;

    [Required, RegularExpression(@"^\d{10}$", ErrorMessage = "La cédula debe tener exactamente 10 dígitos")]
    [Display(Name = "Cédula del representante")]
    public string CedulaRepresentante { get; set; } = string.Empty;

    [Required(ErrorMessage = "El cargo es obligatorio"), StringLength(100)]
    [Display(Name = "Cargo del representante")]
    public string CargoRepresentante { get; set; } = string.Empty;

    [Required(ErrorMessage = "El teléfono del representante es obligatorio"), StringLength(30)]
    [Display(Name = "Teléfono del representante")]
    public string TelefonoRepresentante { get; set; } = string.Empty;

    [Required(ErrorMessage = "El correo es obligatorio"), EmailAddress, StringLength(150)]
    [Display(Name = "Correo de contacto")]
    public string Email { get; set; } = string.Empty;

    [Required(ErrorMessage = "El teléfono es obligatorio"), StringLength(20)]
    [Display(Name = "Teléfono")]
    public string Telefono { get; set; } = string.Empty;

    [StringLength(150)]
    [Display(Name = "Contacto de gestión")]
    public string? ContactoGestionNombre { get; set; }

    [StringLength(150)]
    [Display(Name = "Cargo del contacto de gestión")]
    public string? ContactoGestionCargo { get; set; }

    [EmailAddress, StringLength(150)]
    [Display(Name = "Correo del contacto de gestión")]
    public string? ContactoGestionEmail { get; set; }

    [StringLength(30)]
    [Display(Name = "Teléfono del contacto de gestión")]
    public string? ContactoGestionTelefono { get; set; }

    public bool Activo { get; set; } = true;
}
