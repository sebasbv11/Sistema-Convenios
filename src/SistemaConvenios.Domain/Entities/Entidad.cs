using SistemaConvenios.Domain.Common;
using SistemaConvenios.Domain.Exceptions;
using SistemaConvenios.Domain.ValueObjects;

namespace SistemaConvenios.Models;

public sealed class Entidad : AggregateRoot<int>
{
    private Entidad()
    {
    }

    private Entidad(
        string nombre,
        string tipoEntidad,
        string ruc,
        string direccion,
        string provincia,
        string ciudad,
        string pais,
        string representanteLegal,
        string cedulaRepresentante,
        string cargoRepresentante,
        string telefonoRepresentante,
        string email,
        string telefono,
        string? contactoGestionNombre,
        string? contactoGestionCargo,
        string? contactoGestionEmail,
        string? contactoGestionTelefono)
    {
        AplicarDatos(
            nombre, tipoEntidad, ruc, direccion, provincia, ciudad, pais,
            representanteLegal, cedulaRepresentante, cargoRepresentante,
            telefonoRepresentante, email, telefono,
            contactoGestionNombre, contactoGestionCargo,
            contactoGestionEmail, contactoGestionTelefono);
        Activo = true;
        FechaCreacion = DateTime.UtcNow;
    }

    public string Nombre { get; private set; } = string.Empty;
    public string TipoEntidad { get; private set; } = "Privada";
    public string Ruc { get; private set; } = string.Empty;
    public string Direccion { get; private set; } = string.Empty;
    public string Provincia { get; private set; } = string.Empty;
    public string Ciudad { get; private set; } = string.Empty;
    public string Pais { get; private set; } = "Ecuador";
    public string RepresentanteLegal { get; private set; } = string.Empty;
    public string CedulaRepresentante { get; private set; } = string.Empty;
    public string CargoRepresentante { get; private set; } = string.Empty;
    public string TelefonoRepresentante { get; private set; } = string.Empty;
    public string Email { get; private set; } = string.Empty;
    public string Telefono { get; private set; } = string.Empty;
    public string? ContactoGestionNombre { get; private set; }
    public string? ContactoGestionCargo { get; private set; }
    public string? ContactoGestionEmail { get; private set; }
    public string? ContactoGestionTelefono { get; private set; }
    public bool Activo { get; private set; } = true;
    public DateTime FechaCreacion { get; private set; } = DateTime.UtcNow;
    public ICollection<Convenio> Convenios { get; private set; } = new List<Convenio>();

    public static Entidad Crear(
        string nombre,
        string tipoEntidad,
        string ruc,
        string direccion,
        string provincia,
        string ciudad,
        string pais,
        string representanteLegal,
        string cedulaRepresentante,
        string cargoRepresentante,
        string telefonoRepresentante,
        string email,
        string telefono,
        string? contactoGestionNombre = null,
        string? contactoGestionCargo = null,
        string? contactoGestionEmail = null,
        string? contactoGestionTelefono = null) =>
        new(
            nombre, tipoEntidad, ruc, direccion, provincia, ciudad, pais,
            representanteLegal, cedulaRepresentante, cargoRepresentante,
            telefonoRepresentante, email, telefono,
            contactoGestionNombre, contactoGestionCargo,
            contactoGestionEmail, contactoGestionTelefono);

    public void Actualizar(
        string nombre,
        string tipoEntidad,
        string ruc,
        string direccion,
        string provincia,
        string ciudad,
        string pais,
        string representanteLegal,
        string cedulaRepresentante,
        string cargoRepresentante,
        string telefonoRepresentante,
        string email,
        string telefono,
        string? contactoGestionNombre,
        string? contactoGestionCargo,
        string? contactoGestionEmail,
        string? contactoGestionTelefono,
        bool activo)
    {
        AplicarDatos(
            nombre, tipoEntidad, ruc, direccion, provincia, ciudad, pais,
            representanteLegal, cedulaRepresentante, cargoRepresentante,
            telefonoRepresentante, email, telefono,
            contactoGestionNombre, contactoGestionCargo,
            contactoGestionEmail, contactoGestionTelefono);
        Activo = activo;
    }

    private void AplicarDatos(
        string nombre,
        string tipoEntidad,
        string ruc,
        string direccion,
        string provincia,
        string ciudad,
        string pais,
        string representanteLegal,
        string cedulaRepresentante,
        string cargoRepresentante,
        string telefonoRepresentante,
        string email,
        string telefono,
        string? contactoGestionNombre,
        string? contactoGestionCargo,
        string? contactoGestionEmail,
        string? contactoGestionTelefono)
    {
        Nombre = Requerido(nombre, "El nombre de la entidad es obligatorio.");
        TipoEntidad = Requerido(tipoEntidad, "El tipo de entidad es obligatorio.");
        Ruc = RucIdentificacion.Crear(ruc).Valor;
        Direccion = Requerido(direccion, "La dirección es obligatoria.");
        Provincia = Requerido(provincia, "La provincia es obligatoria.");
        Ciudad = Requerido(ciudad, "La ciudad es obligatoria.");
        Pais = Requerido(pais, "El país es obligatorio.");
        RepresentanteLegal = Requerido(representanteLegal, "El representante legal es obligatorio.");
        CedulaRepresentante = ValidarCedula(cedulaRepresentante);
        CargoRepresentante = Requerido(cargoRepresentante, "El cargo del representante es obligatorio.");
        TelefonoRepresentante = Requerido(telefonoRepresentante, "El teléfono del representante es obligatorio.");
        Email = ValidarEmail(email);
        Telefono = Requerido(telefono, "El teléfono es obligatorio.");
        ContactoGestionNombre = NormalizarOpcional(contactoGestionNombre);
        ContactoGestionCargo = NormalizarOpcional(contactoGestionCargo);
        ContactoGestionEmail = ValidarEmailOpcional(contactoGestionEmail);
        ContactoGestionTelefono = NormalizarOpcional(contactoGestionTelefono);
    }

    private static string Requerido(string valor, string mensaje)
    {
        var normalizado = (valor ?? string.Empty).Trim();
        if (string.IsNullOrWhiteSpace(normalizado))
            throw new DomainException(mensaje);
        return normalizado;
    }

    private static string ValidarCedula(string valor)
    {
        var normalizado = Requerido(valor, "La cédula del representante es obligatoria.");
        if (normalizado.Length != 10 || normalizado.Any(c => !char.IsDigit(c)))
            throw new DomainException("La cédula del representante debe contener 10 dígitos.");
        return normalizado;
    }

    private static string ValidarEmail(string valor)
    {
        var normalizado = Requerido(valor, "El correo es obligatorio.");
        if (!normalizado.Contains('@') || normalizado.StartsWith('@') || normalizado.EndsWith('@'))
            throw new DomainException("El correo electrónico no es válido.");
        return normalizado;
    }

    private static string? ValidarEmailOpcional(string? valor)
    {
        var normalizado = NormalizarOpcional(valor);
        if (normalizado is null)
            return null;
        if (!normalizado.Contains('@') || normalizado.StartsWith('@') || normalizado.EndsWith('@'))
            throw new DomainException("El correo de gestión no es válido.");
        return normalizado;
    }

    private static string? NormalizarOpcional(string? valor) =>
        string.IsNullOrWhiteSpace(valor) ? null : valor.Trim();
}
