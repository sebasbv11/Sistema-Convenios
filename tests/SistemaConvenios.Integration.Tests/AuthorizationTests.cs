using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SistemaConvenios.Controllers;

namespace SistemaConvenios.Integration.Tests;

public sealed class AuthorizationTests
{
    [Theory]
    [InlineData(typeof(ConveniosController))]
    [InlineData(typeof(EntidadesController))]
    [InlineData(typeof(GestionContractualController))]
    public void Acciones_post_requieren_rol_de_gestion(Type controllerType)
    {
        var actions = controllerType.GetMethods()
            .Where(method => method.GetCustomAttributes(typeof(HttpPostAttribute), true).Length > 0)
            .ToArray();

        Assert.NotEmpty(actions);

        foreach (var action in actions)
        {
            var roles = action.GetCustomAttributes(typeof(AuthorizeAttribute), true)
                .Cast<AuthorizeAttribute>()
                .SelectMany(attribute => (attribute.Roles ?? string.Empty)
                    .Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries))
                .ToHashSet(StringComparer.Ordinal);

            Assert.Contains("Admin", roles);
            Assert.Contains("Secretaria", roles);
        }
    }

    [Fact]
    public void AccountController_no_expone_desbloqueo_anonimo()
    {
        Assert.Null(typeof(AccountController).GetMethod("DesbloquearCuenta"));
    }

    [Fact]
    public void UsuariosController_es_exclusivo_de_administradores()
    {
        var autorizacion = typeof(UsuariosController)
            .GetCustomAttributes(typeof(AuthorizeAttribute), true)
            .Cast<AuthorizeAttribute>()
            .Single();

        Assert.Equal("Admin", autorizacion.Roles);
        Assert.All(
            typeof(UsuariosController).GetMethods()
                .Where(x => x.GetCustomAttributes(typeof(HttpPostAttribute), true).Length > 0),
            action => Assert.NotEmpty(action.GetCustomAttributes(typeof(ValidateAntiForgeryTokenAttribute), true)));
    }
}
