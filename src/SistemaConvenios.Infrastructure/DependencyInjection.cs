using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
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
        services.AddDbContext<ApplicationDbContext>(options =>
            options.UseNpgsql(configuration.GetConnectionString("DefaultConnection")));

        services.AddScoped<IUnitOfWork>(provider =>
            provider.GetRequiredService<ApplicationDbContext>());
        services.AddScoped<IConvenioRepository, ConvenioRepository>();
        services.AddScoped<IEntidadRepository, EntidadRepository>();
        services.AddScoped<ICatalogoRepository, CatalogoRepository>();
        services.AddScoped<IGestionContractualRepository, GestionContractualRepository>();
        services.AddSingleton<IArchivoStorage, LocalArchivoStorage>();
        services.AddSingleton<IEmailSender, EmailSender>();
        return services;
    }
}
