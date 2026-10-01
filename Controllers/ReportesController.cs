using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SistemaConvenios.Application.Convenios;
using SistemaConvenios.Domain;

namespace SistemaConvenios.Controllers;

[Authorize]
public sealed class ReportesController : Controller
{
    private readonly IConvenioAppService _convenios;

    public ReportesController(IConvenioAppService convenios)
    {
        _convenios = convenios;
    }

    public async Task<IActionResult> Index(
        string? tipoReporte,
        string? empresa,
        string? provincia,
        string? ciudad,
        string? estado,
        DateTime? fechaDesde,
        DateTime? fechaHasta,
        CancellationToken cancellationToken)
    {
        tipoReporte = string.IsNullOrWhiteSpace(tipoReporte) ? "por-vencer" : tipoReporte.Trim();

        var filtro = new ConvenioFiltro(
            Estado: estado,
            Empresa: empresa,
            Provincia: provincia,
            Ciudad: ciudad,
            FechaDesde: fechaDesde,
            FechaHasta: fechaHasta,
            Reporte: tipoReporte);

        var resultado = await _convenios.BuscarAsync(filtro, cancellationToken);

        ViewBag.TipoReporte = tipoReporte;
        ViewBag.EmpresaFiltro = empresa;
        ViewBag.ProvinciaFiltro = provincia;
        ViewBag.CiudadFiltro = ciudad;
        ViewBag.EstadoFiltro = estado;
        ViewBag.FechaDesdeFiltro = fechaDesde?.ToString("yyyy-MM-dd");
        ViewBag.FechaHastaFiltro = fechaHasta?.ToString("yyyy-MM-dd");
        ViewBag.Estados = ConvenioEstados.Todos;

        return View(resultado.Convenios);
    }

    public async Task<IActionResult> ExportarExcel(
        string? tipoReporte,
        string? empresa,
        string? provincia,
        string? ciudad,
        string? estado,
        DateTime? fechaDesde,
        DateTime? fechaHasta,
        CancellationToken cancellationToken)
    {
        var bytes = await _convenios.ExportarCsvAsync(new ConvenioFiltro(
            Estado: estado,
            Empresa: empresa,
            Provincia: provincia,
            Ciudad: ciudad,
            FechaDesde: fechaDesde,
            FechaHasta: fechaHasta,
            Reporte: tipoReporte), cancellationToken);
        return File(bytes, "text/csv; charset=utf-8", $"reporte_convenios_{DateTime.Now:yyyyMMdd_HHmm}.csv");
    }
}
