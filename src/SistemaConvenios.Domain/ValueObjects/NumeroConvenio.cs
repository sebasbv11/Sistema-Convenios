using System.Text.RegularExpressions;
using SistemaConvenios.Domain.Exceptions;

namespace SistemaConvenios.Domain.ValueObjects;

public readonly record struct NumeroConvenio
{
    private static readonly Regex Patron = new(
        @"^CV-(?<anio>\d{4})-(?<secuencia>\d{3,})$",
        RegexOptions.Compiled | RegexOptions.CultureInvariant);

    public string Valor { get; }

    private NumeroConvenio(string valor)
    {
        Valor = valor;
    }

    public static NumeroConvenio Crear(string valor)
    {
        var normalizado = (valor ?? string.Empty).Trim().ToUpperInvariant();
        if (!Patron.IsMatch(normalizado))
            throw new DomainException("El número de convenio debe usar el formato CV-AAAA-NNN.");

        return new NumeroConvenio(normalizado);
    }

    public static NumeroConvenio Generar(int anio, int secuencia)
    {
        if (anio < 2000 || secuencia < 1)
            throw new DomainException("No se puede generar un número de convenio con esos valores.");

        return new NumeroConvenio($"CV-{anio}-{secuencia:D3}");
    }

    public override string ToString() => Valor;
}
