using SistemaConvenios.Domain.Exceptions;

namespace SistemaConvenios.Domain.ValueObjects;

public readonly record struct RucIdentificacion
{
    public string Valor { get; }

    private RucIdentificacion(string valor)
    {
        Valor = valor;
    }

    public static RucIdentificacion Crear(string valor)
    {
        var normalizado = (valor ?? string.Empty).Trim();
        if ((normalizado.Length != 10 && normalizado.Length != 13) ||
            normalizado.Any(c => !char.IsDigit(c)))
        {
            throw new DomainException("El RUC o cédula debe contener 10 o 13 dígitos.");
        }

        return new RucIdentificacion(normalizado);
    }

    public override string ToString() => Valor;
}
