using Microsoft.AspNetCore.Identity;

namespace SistemaConvenios.Models;

public sealed class Usuario : IdentityUser
{
    public string NombreCompleto { get; set; } = string.Empty;
    public string? Cargo { get; set; }
    public bool Activo { get; set; } = true;
    public DateTime FechaCreacion { get; set; } = DateTime.UtcNow;
    public ICollection<Convenio> ConveniosAdministrados { get; set; } = new List<Convenio>();
}
