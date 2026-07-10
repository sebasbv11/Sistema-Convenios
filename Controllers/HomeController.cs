using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SistemaConvenios.Application.Convenios;

namespace SistemaConvenios.Controllers;

[Authorize]
public sealed class HomeController : Controller
{
    private readonly IDashboardAppService _dashboard;

    public HomeController(IDashboardAppService dashboard)
    {
        _dashboard = dashboard;
    }

    public async Task<IActionResult> Index(CancellationToken cancellationToken)
    {
        var dashboard = await _dashboard.ObtenerAsync(cancellationToken);

        ViewBag.TotalActivos = dashboard.Vigentes;
        ViewBag.PorVencer30 = dashboard.PorVencer30;
        ViewBag.RenovadosAutomaticos = dashboard.RenovadosAutomaticos;
        ViewBag.Internacionales = dashboard.Internacionales;
        ViewBag.ConveniosPorTipoLabels = dashboard.PorTipo.Keys.ToList();
        ViewBag.ConveniosPorTipoData = dashboard.PorTipo.Values.ToList();
        ViewBag.ConveniosPorCarreraLabels = dashboard.PorCarrera.Keys.ToList();
        ViewBag.ConveniosPorCarreraData = dashboard.PorCarrera.Values.ToList();
        ViewBag.ConveniosPorEstadoLabels = dashboard.PorEstado.Keys.ToList();
        ViewBag.ConveniosPorEstadoData = dashboard.PorEstado.Values.ToList();

        return View(dashboard.Recientes);
    }

    public IActionResult Privacy() => View();
}
