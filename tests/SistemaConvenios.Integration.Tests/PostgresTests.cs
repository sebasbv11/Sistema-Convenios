using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Npgsql;
using SistemaConvenios.Application.Convenios;
using SistemaConvenios.Data;
using SistemaConvenios.Models;

namespace SistemaConvenios.Integration.Tests;

public sealed class PostgresFactAttribute : FactAttribute
{
    public PostgresFactAttribute()
    {
        var connection = Environment.GetEnvironmentVariable("CONVENIOS_TEST_DATABASE");
        if (string.IsNullOrWhiteSpace(connection))
            Skip = "Configure CONVENIOS_TEST_DATABASE with an isolated PostgreSQL database.";
        else if (!new NpgsqlConnectionStringBuilder(connection).Database!.EndsWith("_test", StringComparison.Ordinal))
            throw new InvalidOperationException("La base de datos de pruebas debe terminar en _test.");
    }
}

public sealed class PostgresTests
{
    [PostgresFact]
    public async Task Crear_modificar_y_cerrar_convenio_con_postgres()
    {
        await using var factory = new WebApplicationFactory<Program>().WithWebHostBuilder(host =>
        {
            host.UseEnvironment("Testing");
            host.ConfigureAppConfiguration((_, config) => config.AddInMemoryCollection(
                new Dictionary<string, string?> { ["ConnectionStrings:DefaultConnection"] = Environment.GetEnvironmentVariable("CONVENIOS_TEST_DATABASE") }));
            host.ConfigureTestServices(services =>
            {
                services.RemoveAll<ApplicationDbContext>();
                services.RemoveAll<DbContextOptions<ApplicationDbContext>>();
                services.AddDbContext<ApplicationDbContext>(options =>
                    options.UseNpgsql(Environment.GetEnvironmentVariable("CONVENIOS_TEST_DATABASE")));
            });
        });
        using var scope = factory.Services.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
        await db.Database.MigrateAsync();
        await using var transaction = await db.Database.BeginTransactionAsync();
        var entidad = Entidad.Crear("Entidad", "Privada", "1234567890002", "Dirección", "Manabí", "Manta",
            "Ecuador", "Representante", "1234567890", "Gerente", "0999999999", "prueba@example.com", "0999999999");
        db.Entidades.Add(entidad);
        await db.SaveChangesAsync();
        var area = await db.AreasPromotoras.Select(x => x.Id).FirstAsync();
        var convenios = scope.ServiceProvider.GetRequiredService<IConvenioAppService>();
        var gestion = scope.ServiceProvider.GetRequiredService<IGestionContractualAppService>();
        var creado = await convenios.CrearAsync(new CrearConvenioCommand(1, entidad.Id, area, null,
            ["Vinculación"], "Objeto de prueba", new DateTime(2026, 1, 1), new DateTime(2027, 1, 1),
            "Vigente", null, null, null, null, null, false, null, [], [], null, "prueba"));
        Assert.True(creado.Succeeded, creado.Error);
        var modificacion = await gestion.AgregarModificacionAsync(new AgregarModificacionCommand(
            creado.Value, "Adenda", "Modificación de prueba", null, new DateTime(2026, 6, 1)));
        Assert.True(modificacion.Succeeded, modificacion.Error);
        var cierre = await gestion.RegistrarCierreAsync(new RegistrarCierreCommand(
            creado.Value, new DateTime(2026, 12, 1), "Cumplimiento", "Objeto cumplido", null, null, "prueba"));
        Assert.True(cierre.Succeeded, cierre.Error);
        db.ChangeTracker.Clear();
        var guardado = await gestion.ObtenerAsync(creado.Value);
        Assert.NotNull(guardado?.Cierre);
        Assert.Contains(guardado.Modificaciones, x => x.Tipo == "Adenda");
        var numero = await db.Convenios.Where(x => x.Id == creado.Value).Select(x => x.Numero).SingleAsync();
        db.Convenios.Add(Convenio.Crear(numero, 1, entidad.Id, area, null, ["Vinculación"], "Duplicado",
            new DateTime(2026, 1, 1), new DateTime(2027, 1, 1), "Vigente", null, null, null, null,
            null, false, null, null, "prueba"));
        var error = await Assert.ThrowsAsync<DbUpdateException>(() => db.SaveChangesAsync());
        Assert.Equal(PostgresErrorCodes.UniqueViolation, Assert.IsType<PostgresException>(error.InnerException).SqlState);
    }

    [PostgresFact]
    public async Task Migraciones_imponen_unicidad_y_nulabilidad()
    {
        var connection = Environment.GetEnvironmentVariable("CONVENIOS_TEST_DATABASE")!;
        var options = new DbContextOptionsBuilder<ApplicationDbContext>().UseNpgsql(connection).Options;
        await using var db = new ApplicationDbContext(options);
        await db.Database.MigrateAsync();
        await using var transaction = await db.Database.BeginTransactionAsync();

        static Entidad NuevaEntidad() => Entidad.Crear(
            "Entidad de prueba", "Privada", "1234567890001", "Dirección", "Manabí", "Manta",
            "Ecuador", "Representante", "1234567890", "Gerente", "0999999999",
            "prueba@example.com", "0999999999");

        db.Entidades.Add(NuevaEntidad());
        await db.SaveChangesAsync();
        db.Entidades.Add(NuevaEntidad());
        var error = await Assert.ThrowsAsync<DbUpdateException>(() => db.SaveChangesAsync());
        Assert.Equal(PostgresErrorCodes.UniqueViolation, Assert.IsType<PostgresException>(error.InnerException).SqlState);
        var nullError = await Assert.ThrowsAsync<PostgresException>(() =>
            db.Database.ExecuteSqlRawAsync("UPDATE \"Entidades\" SET \"Ruc\" = NULL WHERE \"Ruc\" = '1234567890001'"));
        Assert.Equal(PostgresErrorCodes.NotNullViolation, nullError.SqlState);
    }
}
