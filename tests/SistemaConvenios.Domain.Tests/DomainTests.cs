using SistemaConvenios.Domain;
using SistemaConvenios.Domain.Exceptions;
using SistemaConvenios.Domain.ValueObjects;
using SistemaConvenios.Models;

namespace SistemaConvenios.Domain.Tests;

public sealed class DomainTests
{
    [Fact]
    public void Ruc_debe_tener_diez_o_trece_digitos()
    {
        Assert.Equal("1234567890", RucIdentificacion.Crear("1234567890").Valor);
        Assert.Throws<DomainException>(() => RucIdentificacion.Crear("ABC"));
    }

    [Fact]
    public void Periodo_no_permite_vencimiento_anterior_al_inicio()
    {
        Assert.Throws<DomainException>(() =>
            PeriodoConvenio.Crear(new DateTime(2026, 6, 23), new DateTime(2026, 6, 22)));
    }

    [Fact]
    public void Convenio_registra_historial_al_crearse_y_cambiar_estado()
    {
        var convenio = CrearConvenio(ConvenioEstados.Vigente, new DateTime(2027, 1, 1));

        Assert.Single(convenio.Historial);
        Assert.True(convenio.CambiarEstado(ConvenioEstados.PorVencer, "Notificación preventiva", "tester"));
        Assert.Equal(ConvenioEstados.PorVencer, convenio.Estado);
        Assert.Equal(2, convenio.Historial.Count);
    }

    [Fact]
    public void Convenio_vigente_vencido_se_marca_terminado()
    {
        var convenio = CrearConvenio(ConvenioEstados.Vigente, new DateTime(2026, 6, 1));

        Assert.True(convenio.ActualizarEstadoPorFecha(new DateTime(2026, 6, 23)));
        Assert.Equal(ConvenioEstados.Terminado, convenio.Estado);
        Assert.Equal("Sistema", convenio.Historial.Last().CambiadoPor);
    }

    [Fact]
    public void Convenio_vencido_con_renovacion_automatica_se_marca_renovado()
    {
        var convenio = Convenio.Crear(
            "CV-2026-002",
            1,
            1,
            1,
            null,
            "Vinculación",
            "Objeto de prueba",
            new DateTime(2026, 1, 1),
            new DateTime(2026, 6, 1),
            ConvenioEstados.Vigente,
            "RES-002",
            "Supervisor",
            null,
            null,
            null,
            true,
            null,
            "usuario-id",
            "tester");

        Assert.True(convenio.ActualizarEstadoPorFecha(new DateTime(2026, 6, 23)));
        Assert.Equal(ConvenioEstados.RenovadoAutomatico, convenio.Estado);
    }

    [Fact]
    public void Entidad_normaliza_sus_datos()
    {
        var entidad = Entidad.Crear(
            "  Empresa de prueba  ",
            " Privada ",
            "1234567890001",
            "Av. Principal",
            "Manabí",
            "Manta",
            "Ecuador",
            "Representante",
            "1234567890",
            "Gerente",
            "0987654321",
            "contacto@ejemplo.com",
            "0999999999");

        Assert.Equal("Empresa de prueba", entidad.Nombre);
        Assert.Equal("Privada", entidad.TipoEntidad);
    }

    [Fact]
    public void Capas_respetan_la_direccion_de_dependencias()
    {
        var domainReferences = typeof(Convenio).Assembly.GetReferencedAssemblies().Select(x => x.Name).ToList();
        var applicationAssembly = typeof(SistemaConvenios.Application.Convenios.IConvenioAppService).Assembly;
        var applicationReferences = applicationAssembly.GetReferencedAssemblies().Select(x => x.Name).ToList();

        Assert.DoesNotContain("SistemaConvenios.Application", domainReferences);
        Assert.DoesNotContain("SistemaConvenios.Infrastructure", domainReferences);
        Assert.DoesNotContain("SistemaConvenios.Infrastructure", applicationReferences);
    }

    [Fact]
    public void Convenio_administra_ambitos_partes_y_obligaciones()
    {
        var convenio = CrearConvenio(ConvenioEstados.Vigente, new DateTime(2027, 1, 1));

        convenio.ReemplazarAmbitos(new[] { "Vinculación", "Prácticas preprofesionales", "vinculación" });
        convenio.AgregarParte(null, "Galapesca S.A.", "GALAPESCA", "Contraparte", true,
            null, null, "contacto@galapesca.ec", "0990000001001");
        var obligacion = convenio.AgregarObligacion(
            "Galapesca S.A.", "Facilitar espacios para prácticas.", 1);
        obligacion.MarcarCumplida("Informe final");

        Assert.Equal(2, convenio.Ambitos.Count);
        Assert.Single(convenio.Partes);
        Assert.Equal("Cumplida", obligacion.Estado);
        Assert.Equal("Informe final", obligacion.Evidencia);
    }

    [Fact]
    public void Datos_legales_validan_fechas_y_presupuesto()
    {
        var convenio = CrearConvenio(ConvenioEstados.Vigente, new DateTime(2027, 1, 1));

        Assert.Throws<DomainException>(() => convenio.ConfigurarDatosLegales(
            new DateTime(2026, 2, 2), new DateTime(2026, 2, 1),
            null, null, false, false, null, "USD", null, null));
        Assert.Throws<DomainException>(() => convenio.ConfigurarDatosLegales(
            null, null, null, null, false, true, null, "USD", null, null));
    }

    [Fact]
    public void Actividad_no_permite_periodo_invertido()
    {
        Assert.Throws<DomainException>(() => ActividadConvenio.Crear(
            1, "Prácticas", "Vinculación", null,
            new DateTime(2026, 8, 2), new DateTime(2026, 8, 1),
            80, "Manta", "Tutor"));
    }

    [Fact]
    public void Evaluacion_no_permite_calificacion_fuera_de_rango()
    {
        var participacion = ParticipacionActividad.Crear(1, 1, 80);

        Assert.Throws<DomainException>(() =>
            participacion.Evaluar(80, 101, "Excelente", true));
    }

    [Fact]
    public void Cierre_termina_el_convenio_y_registra_historial()
    {
        var convenio = CrearConvenio(ConvenioEstados.Vigente, new DateTime(2027, 1, 1));

        convenio.Cerrar(
            new DateTime(2026, 12, 1), "Cumplimiento del objeto",
            "Se ejecutaron todas las actividades.", null, null, "tester");

        Assert.Equal(ConvenioEstados.Terminado, convenio.Estado);
        Assert.NotNull(convenio.Cierre);
        Assert.Equal("Cumplimiento del objeto", convenio.Cierre.Causal);
        Assert.Contains(convenio.Historial, x => x.Observacion == "Cierre: Cumplimiento del objeto");
    }

    private static Convenio CrearConvenio(string estado, DateTime vencimiento) =>
        Convenio.Crear(
            "CV-2026-001",
            1,
            1,
            1,
            null,
            "Vinculación",
            "Objeto de prueba",
            new DateTime(2026, 1, 1),
            vencimiento,
            estado,
            "RES-001",
            "Supervisor",
            null,
            null,
            null,
            false,
            null,
            "usuario-id",
            "tester");
}
