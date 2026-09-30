using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Google.Cloud.Storage.V1;
using SistemaConvenios.Application.Abstractions;
using SistemaConvenios.Data;
using SistemaConvenios.Infrastructure.Email;
using SistemaConvenios.Infrastructure.Files;

namespace SistemaConvenios.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("DefaultConnection");
        if (string.IsNullOrWhiteSpace(connectionString))
        {
            throw new InvalidOperationException(
                "No se configuró ConnectionStrings:DefaultConnection. " +
                "Usa User Secrets en desarrollo o variables de entorno en despliegue.");
        }

        services.AddDbContext<ApplicationDbContext>(options =>
            options.UseNpgsql(connectionString));

        services.AddScoped<IUnitOfWork>(provider =>
            provider.GetRequiredService<ApplicationDbContext>());
        services.AddScoped<IConvenioRepository, ConvenioRepository>();
        services.AddScoped<IEntidadRepository, EntidadRepository>();
        services.AddScoped<ICatalogoRepository, CatalogoRepository>();
        services.AddScoped<IGestionContractualRepository, GestionContractualRepository>();
        var proveedorArchivos = configuration["ArchivosConfig:Provider"] ?? "Local";
        if (proveedorArchivos.Equals("GoogleCloudStorage", StringComparison.OrdinalIgnoreCase))
        {
            var bucket = configuration["ArchivosConfig:Bucket"];
            if (string.IsNullOrWhiteSpace(bucket))
                throw new InvalidOperationException(
                    "ArchivosConfig:Bucket es obligatorio cuando Provider=GoogleCloudStorage.");

            services.AddSingleton(_ => StorageClient.Create());
            services.AddSingleton<IArchivoStorage>(provider =>
                new GoogleCloudArchivoStorage(
                    provider.GetRequiredService<StorageClient>(),
                    bucket));
        }
        else if (proveedorArchivos.Equals("Local", StringComparison.OrdinalIgnoreCase))
        {
            services.AddSingleton<IArchivoStorage, LocalArchivoStorage>();
        }
        else
        {
            throw new InvalidOperationException(
                $"ArchivosConfig:Provider '{proveedorArchivos}' no está soportado.");
        }
        services.AddSingleton<IEmailSender, EmailSender>();
        return services;
    }
}
