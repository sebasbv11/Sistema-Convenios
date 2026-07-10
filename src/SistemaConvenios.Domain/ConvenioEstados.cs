using SistemaConvenios.Domain.Exceptions;

namespace SistemaConvenios.Domain;

public static class ConvenioEstados
{
    public const string Vigente = "Vigente";
    public const string PorVencer = "Por vencer";
    public const string Terminado = "Terminado";
    public const string RenovadoAutomatico = "Renovado automático";

    public static readonly IReadOnlyCollection<string> Todos =
        new[] { Vigente, PorVencer, Terminado, RenovadoAutomatico };

    public static readonly IReadOnlyCollection<string> Activos =
        new[] { Vigente, PorVencer, RenovadoAutomatico };

    public static string Validar(string estado)
    {
        var normalizado = (estado ?? string.Empty).Trim();
        if (!Todos.Contains(normalizado))
            throw new DomainException($"El estado '{estado}' no es válido.");

        return normalizado;
    }
}
