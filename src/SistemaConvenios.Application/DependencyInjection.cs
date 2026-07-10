using Microsoft.Extensions.DependencyInjection;
using SistemaConvenios.Application.Convenios;
using SistemaConvenios.Application.Entidades;

namespace SistemaConvenios.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddScoped<IConvenioAppService, ConvenioAppService>();
        services.AddScoped<IEntidadAppService, EntidadAppService>();
        services.AddScoped<IDashboardAppService, DashboardAppService>();
        services.AddScoped<IGestionContractualAppService, GestionContractualAppService>();
        return services;
    }
}
