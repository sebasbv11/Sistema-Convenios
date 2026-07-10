using SistemaConvenios.Domain.Exceptions;

namespace SistemaConvenios.Domain.ValueObjects;

public readonly record struct PeriodoConvenio
{
    public DateTime Inicio { get; }
    public DateTime Fin { get; }

    private PeriodoConvenio(DateTime inicio, DateTime fin)
    {
        Inicio = EspecificarUtc(inicio);
        Fin = EspecificarUtc(fin);
    }

    public static PeriodoConvenio Crear(DateTime inicio, DateTime fin)
    {
        if (fin.Date < inicio.Date)
            throw new DomainException("La fecha de vencimiento no puede ser anterior a la fecha de inicio.");

        return new PeriodoConvenio(inicio, fin);
    }

    public bool EstaVencido(DateTime fecha) => Fin.Date < fecha.Date;

    public bool VenceDentroDe(DateTime fecha, int dias) =>
        Fin.Date >= fecha.Date && Fin.Date <= fecha.Date.AddDays(dias);

    private static DateTime EspecificarUtc(DateTime fecha) =>
        fecha.Kind == DateTimeKind.Utc
            ? fecha
            : DateTime.SpecifyKind(fecha.Date, DateTimeKind.Utc);
}
