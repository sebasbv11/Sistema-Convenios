using System.ComponentModel.DataAnnotations;

namespace SistemaConvenios.Models;

public sealed class ConvenioFormViewModel : IValidatableObject
{
    public int Id { get; set; }

    [Display(Name = "Número de convenio")]
    public string Numero { get; set; } = string.Empty;

    [Required(ErrorMessage = "Seleccione el tipo de convenio")]
    [Display(Name = "Tipo de convenio")]
    public int? TipoConvenioId { get; set; }

    [Required(ErrorMessage = "Seleccione la entidad contraparte")]
    [Display(Name = "Entidad contraparte")]
    public int? EntidadId { get; set; }

    [Required(ErrorMessage = "Seleccione el área promotora")]
    [Display(Name = "Área que promueve")]
    public int? AreaPromotoraId { get; set; }

    [Display(Name = "Convenio marco / internacional base")]
    public int? ConvenioPadreId { get; set; }

    [Required, StringLength(50)]
    [Display(Name = "Ámbito")]
    public string Ambito { get; set; } = string.Empty;

    [Required(ErrorMessage = "El objeto del convenio es obligatorio")]
    [Display(Name = "Objeto del convenio")]
    public string Objeto { get; set; } = string.Empty;

    [Required, DataType(DataType.Date)]
    [Display(Name = "Fecha de inicio")]
    public DateTime FechaInicio { get; set; }

    [Required, DataType(DataType.Date)]
    [Display(Name = "Fecha de vencimiento")]
    public DateTime FechaVencimiento { get; set; }

    [Required, StringLength(30)]
    [Display(Name = "Estado")]
    public string Estado { get; set; } = "Vigente";

    [StringLength(100)]
    [Display(Name = "N° Resolución de aprobación")]
    public string? NumeroResolucion { get; set; }

    [StringLength(200)]
    [Display(Name = "Supervisor / Administrador")]
    public string? Supervisor { get; set; }

    [StringLength(150)]
    [Display(Name = "Contacto de gestión")]
    public string? ContactoGestionNombre { get; set; }

    [EmailAddress, StringLength(150)]
    [Display(Name = "Correo del contacto")]
    public string? ContactoGestionEmail { get; set; }

    [StringLength(30)]
    [Display(Name = "Teléfono del contacto")]
    public string? ContactoGestionTelefono { get; set; }

    [Display(Name = "Renovación automática")]
    public bool RenovacionAutomatica { get; set; }

    [StringLength(500)]
    public string? Observaciones { get; set; }

    public List<int> FacultadIds { get; set; } = new();
    public List<int> CarreraIds { get; set; } = new();

    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        if (FechaVencimiento.Date < FechaInicio.Date)
        {
            yield return new ValidationResult(
                "La fecha de vencimiento no puede ser anterior a la fecha de inicio.",
                new[] { nameof(FechaVencimiento) });
        }
    }
}

public sealed class SubirArchivoViewModel
{
    [Required]
    public int ConvenioId { get; set; }

    [Required(ErrorMessage = "Selecciona un archivo PDF")]
    [Display(Name = "Archivo PDF")]
    public IFormFile? Archivo { get; set; }

    [Required, StringLength(100)]
    [Display(Name = "Tipo de documento")]
    public string TipoDocumento { get; set; } = "Convenio";

    [StringLength(300)]
    public string? Descripcion { get; set; }
}
