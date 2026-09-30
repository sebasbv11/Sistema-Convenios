using System.Text;
using SistemaConvenios.Application.Abstractions;
using SistemaConvenios.Application.Common;
using SistemaConvenios.Domain.Exceptions;
using SistemaConvenios.Models;

namespace SistemaConvenios.Application.Convenios;

public sealed class ConvenioAppService : IConvenioAppService
{
    private const long TamanioMaximoPdf = 10 * 1024 * 1024;

    private readonly IConvenioRepository _convenios;
    private readonly IEntidadRepository _entidades;
    private readonly ICatalogoRepository _catalogos;
    private readonly IArchivoStorage _archivos;
    private readonly IUnitOfWork _unitOfWork;

    public ConvenioAppService(
        IConvenioRepository convenios,
        IEntidadRepository entidades,
        ICatalogoRepository catalogos,
        IArchivoStorage archivos,
        IUnitOfWork unitOfWork)
    {
        _convenios = convenios;
        _entidades = entidades;
        _catalogos = catalogos;
        _archivos = archivos;
        _unitOfWork = unitOfWork;
    }

    public async Task<ConvenioIndexDto> BuscarAsync(
        ConvenioFiltro filtro,
        CancellationToken cancellationToken = default)
    {
        var convenios = await _convenios.BuscarAsync(filtro, cancellationToken);
        var tipos = await _catalogos.ObtenerTiposConvenioAsync(cancellationToken);
        var facultades = await _catalogos.ObtenerFacultadesAsync(cancellationToken);
        var carreras = await _catalogos.ObtenerCarrerasAsync(cancellationToken);

        return new ConvenioIndexDto(
            convenios.Select(x => x.ToListaDto()).ToList(),
            tipos.Select(x => new CatalogoItemDto(x.Id, x.Nombre)).ToList(),
            facultades.Select(x => new CatalogoItemDto(x.Id, x.Nombre, x.Siglas)).ToList(),
            carreras.Select(x => new CatalogoItemDto(x.Id, x.Nombre, x.Siglas, x.FacultadId)).ToList());
    }

    public async Task<ConvenioDetalleDto?> ObtenerDetalleAsync(
        int id,
        CancellationToken cancellationToken = default)
    {
        var convenio = await _convenios.ObtenerDetalleAsync(id, cancellationToken);
        return convenio?.ToDetalleDto();
    }

    public async Task<ConvenioEdicionDto?> ObtenerParaEditarAsync(
        int id,
        CancellationToken cancellationToken = default)
    {
        var convenio = await _convenios.ObtenerParaEditarAsync(id, cancellationToken);
        return convenio?.ToEdicionDto();
    }

    public async Task<ConvenioFormularioCatalogosDto> ObtenerCatalogosFormularioAsync(
        CancellationToken cancellationToken = default)
    {
        var tipos = await _catalogos.ObtenerTiposConvenioAsync(cancellationToken);
        var entidades = await _entidades.BuscarAsync(null, cancellationToken);
        var facultades = await _catalogos.ObtenerFacultadesAsync(cancellationToken);
        var carreras = await _catalogos.ObtenerCarrerasAsync(cancellationToken);
        var areas = await _catalogos.ObtenerAreasPromotorasAsync(cancellationToken);
        var conveniosBase = await _convenios.ObtenerConveniosBaseAsync(null, cancellationToken);

        return new ConvenioFormularioCatalogosDto(
            tipos.Select(x => new CatalogoItemDto(x.Id, x.Nombre)).ToList(),
            entidades
                .Where(x => x.Activo)
                .Select(x => new CatalogoItemDto(x.Id, x.Nombre))
                .ToList(),
            areas
                .Where(x => x.Activo)
                .Select(x => new CatalogoItemDto(
                    x.Id,
                    x.AreaPadre is null
                        ? $"{x.Codigo} - {x.Nombre}"
                        : $"{x.AreaPadre.Nombre} / {x.Codigo} - {x.Nombre}",
                    x.Codigo,
                    x.AreaPadreId))
                .ToList(),
            conveniosBase
                .Select(x => new CatalogoItemDto(
                    x.Id,
                    $"{x.Numero} · {x.TipoConvenio?.Nombre} · {x.Entidad?.Nombre}",
                    x.TipoConvenio?.Nombre))
                .ToList(),
            facultades
                .Where(x => x.Activo)
                .Select(x => new CatalogoItemDto(x.Id, x.Nombre, x.Siglas))
                .ToList(),
            carreras
                .Where(x => x.Activo)
                .Select(x => new CatalogoItemDto(x.Id, x.Nombre, x.Siglas, x.FacultadId))
                .ToList());
    }

    public Task<string> GenerarNumeroAsync(CancellationToken cancellationToken = default) =>
        _convenios.GenerarNumeroAsync(DateTime.UtcNow.Year, cancellationToken);

    public async Task<OperationResult<int>> CrearAsync(
        CrearConvenioCommand command,
        CancellationToken cancellationToken = default)
    {
        try
        {
            var convenioPadre = await ValidarConvenioPadreAsync(
                command.TipoConvenioId,
                command.ConvenioPadreId,
                command.FechaInicio,
                null,
                cancellationToken);
            if (!convenioPadre.Succeeded)
                return OperationResult<int>.Failure(convenioPadre.Error!);

            var numero = await GenerarNumeroAsync(cancellationToken);
            var convenio = Convenio.Crear(
                numero,
                command.TipoConvenioId,
                command.EntidadId,
                command.AreaPromotoraId,
                command.ConvenioPadreId,
                command.Ambitos,
                command.Objeto,
                command.FechaInicio,
                command.FechaVencimiento,
                command.Estado,
                command.NumeroResolucion,
                command.Supervisor,
                command.ContactoGestionNombre,
                command.ContactoGestionEmail,
                command.ContactoGestionTelefono,
                command.RenovacionAutomatica,
                command.Observaciones,
                command.UsuarioId,
                command.Usuario);

            convenio.ReemplazarFacultades(command.FacultadIds);
            convenio.ReemplazarCarreras(command.CarreraIds);

            await _convenios.AgregarAsync(convenio, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
            if (convenioPadre.Value is not null)
            {
                convenio.CopiarRepresentantesDesde(convenioPadre.Value);
                await _unitOfWork.SaveChangesAsync(cancellationToken);
            }

            return OperationResult<int>.Success(convenio.Id);
        }
        catch (DomainException ex)
        {
            return OperationResult<int>.Failure(ex.Message);
        }
    }

    public async Task<OperationResult> EditarAsync(
        EditarConvenioCommand command,
        CancellationToken cancellationToken = default)
    {
        var convenio = await _convenios.ObtenerParaEditarAsync(command.Id, cancellationToken);
        if (convenio is null)
            return OperationResult.Failure("El convenio no existe.");

        try
        {
            var convenioPadre = await ValidarConvenioPadreAsync(
                command.TipoConvenioId,
                command.ConvenioPadreId,
                command.FechaInicio,
                command.Id,
                cancellationToken);
            if (!convenioPadre.Succeeded)
                return OperationResult.Failure(convenioPadre.Error!);

            convenio.Actualizar(
                command.TipoConvenioId,
                command.EntidadId,
                command.AreaPromotoraId,
                command.ConvenioPadreId,
                command.Ambitos,
                command.Objeto,
                command.FechaInicio,
                command.FechaVencimiento,
                command.Estado,
                command.NumeroResolucion,
                command.Supervisor,
                command.ContactoGestionNombre,
                command.ContactoGestionEmail,
                command.ContactoGestionTelefono,
                command.RenovacionAutomatica,
                command.Observaciones,
                command.Usuario);
            convenio.ReemplazarFacultades(command.FacultadIds);
            convenio.ReemplazarCarreras(command.CarreraIds);
            if (convenioPadre.Value is not null && (!convenio.Firmantes.Any() || !convenio.Responsables.Any()))
                convenio.CopiarRepresentantesDesde(convenioPadre.Value);

            await _unitOfWork.SaveChangesAsync(cancellationToken);
            return OperationResult.Success();
        }
        catch (DomainException ex)
        {
            return OperationResult.Failure(ex.Message);
        }
    }

    public async Task<OperationResult> CambiarEstadoAsync(
        CambiarEstadoCommand command,
        CancellationToken cancellationToken = default)
    {
        var convenio = await _convenios.ObtenerParaEditarAsync(command.Id, cancellationToken);
        if (convenio is null)
            return OperationResult.Failure("El convenio no existe.");

        try
        {
            convenio.CambiarEstado(command.NuevoEstado, command.Observacion, command.Usuario);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
            return OperationResult.Success();
        }
        catch (DomainException ex)
        {
            return OperationResult.Failure(ex.Message);
        }
    }

    public async Task<OperationResult> SubirArchivoAsync(
        SubirArchivoCommand command,
        CancellationToken cancellationToken = default)
    {
        if (!Path.GetExtension(command.NombreOriginal).Equals(".pdf", StringComparison.OrdinalIgnoreCase))
            return OperationResult.Failure("Solo se permiten archivos PDF.");

        if (command.TamanioBytes <= 0)
            return OperationResult.Failure("El archivo está vacío.");

        if (command.TamanioBytes > TamanioMaximoPdf)
            return OperationResult.Failure("El archivo no puede superar 10 MB.");

        var convenio = await _convenios.ObtenerParaArchivosAsync(command.ConvenioId, cancellationToken);
        if (convenio is null)
            return OperationResult.Failure("El convenio no existe.");

        string? ruta = null;
        try
        {
            ruta = await _archivos.GuardarPdfAsync(
                command.ConvenioId,
                command.NombreOriginal,
                command.Contenido,
                cancellationToken);

            convenio.AgregarArchivo(
                command.NombreOriginal,
                ruta,
                command.TipoDocumento,
                command.Descripcion,
                command.TamanioBytes,
                command.Usuario);

            await _unitOfWork.SaveChangesAsync(cancellationToken);
            return OperationResult.Success();
        }
        catch (DomainException ex)
        {
            if (ruta is not null)
                await _archivos.EliminarAsync(ruta, cancellationToken);
            return OperationResult.Failure(ex.Message);
        }
        catch (InvalidDataException ex)
        {
            if (ruta is not null)
                await _archivos.EliminarAsync(ruta, cancellationToken);
            return OperationResult.Failure(ex.Message);
        }
        catch
        {
            if (ruta is not null)
                await _archivos.EliminarAsync(ruta, cancellationToken);
            throw;
        }
    }

    public async Task<ArchivoDescargaDto?> DescargarArchivoAsync(
        int id,
        CancellationToken cancellationToken = default)
    {
        var archivo = await _convenios.ObtenerArchivoAsync(id, cancellationToken);
        if (archivo is null)
            return null;

        var contenido = await _archivos.AbrirLecturaAsync(archivo.RutaFisica, cancellationToken);
        return contenido is null ? null : new ArchivoDescargaDto(archivo.NombreOriginal, contenido);
    }

    public async Task<OperationResult> EliminarArchivoAsync(
        int archivoId,
        int convenioId,
        CancellationToken cancellationToken = default)
    {
        var convenio = await _convenios.ObtenerParaArchivosAsync(convenioId, cancellationToken);
        var archivo = convenio?.Archivos.FirstOrDefault(x => x.Id == archivoId);
        if (convenio is null || archivo is null)
            return OperationResult.Failure("El archivo no existe.");

        var ruta = archivo.RutaFisica;
        convenio.EliminarArchivo(archivoId);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        await _archivos.EliminarAsync(ruta, cancellationToken);
        return OperationResult.Success();
    }

    public async Task<byte[]> ExportarCsvAsync(
        ConvenioFiltro filtro,
        CancellationToken cancellationToken = default)
    {
        var convenios = await _convenios.BuscarAsync(filtro, cancellationToken);
        var sb = new StringBuilder();
        sb.AppendLine("Numero,Entidad,RUC,TipoEntidad,Provincia,Ciudad,TipoConvenio,ConvenioBase,AreaPromotora,Ambito,Facultades,Carreras,FechaInicio,FechaVencimiento,Estado,NumeroResolucion,Supervisor,ContactoGestion,TelefonoGestion");

        foreach (var c in convenios.OrderBy(x => x.FechaVencimiento))
        {
            var facultades = string.Join(" | ", c.ConvenioFacultades.Select(x => x.Facultad?.Siglas ?? x.Facultad?.Nombre ?? ""));
            var carreras = string.Join(" | ", c.ConvenioCarreras.Select(x => x.Carrera?.Nombre ?? ""));
            sb.AppendLine(string.Join(',', new[]
            {
                Csv(c.Numero),
                Csv(c.Entidad?.Nombre),
                Csv(c.Entidad?.Ruc),
                Csv(c.Entidad?.TipoEntidad),
                Csv(c.Entidad?.Provincia),
                Csv(c.Entidad?.Ciudad),
                Csv(c.TipoConvenio?.Nombre),
                Csv(c.ConvenioPadre?.Numero),
                Csv(c.AreaPromotora is null ? null : $"{c.AreaPromotora.Codigo} - {c.AreaPromotora.Nombre}"),
                Csv(c.Ambito),
                Csv(facultades),
                Csv(carreras),
                Csv(c.FechaInicio.ToString("yyyy-MM-dd")),
                Csv(c.FechaVencimiento.ToString("yyyy-MM-dd")),
                Csv(c.Estado),
                Csv(c.NumeroResolucion),
                Csv(c.Supervisor),
                Csv(c.ContactoGestionNombre),
                Csv(c.ContactoGestionTelefono)
            }));
        }

        return Encoding.UTF8.GetPreamble().Concat(Encoding.UTF8.GetBytes(sb.ToString())).ToArray();
    }

    private static string Csv(string? value) => $"\"{(value ?? string.Empty).Replace("\"", "\"\"")}\"";

    private async Task<OperationResult<Convenio?>> ValidarConvenioPadreAsync(
        int tipoConvenioId,
        int? convenioPadreId,
        DateTime fechaInicio,
        int? convenioActualId,
        CancellationToken cancellationToken)
    {
        var tipos = await _catalogos.ObtenerTiposConvenioAsync(cancellationToken);
        var tipo = tipos.FirstOrDefault(x => x.Id == tipoConvenioId);
        if (tipo is null)
            return OperationResult<Convenio?>.Failure("El tipo de convenio seleccionado no existe.");

        var esEspecifico = string.Equals(tipo.Nombre, "Específico", StringComparison.OrdinalIgnoreCase);
        if (!esEspecifico)
            return OperationResult<Convenio?>.Success(null);

        if (!convenioPadreId.HasValue)
            return OperationResult<Convenio?>.Failure(
                "Un convenio específico debe suscribirse a un convenio Marco o Internacional existente.");

        if (convenioActualId.HasValue && convenioPadreId.Value == convenioActualId.Value)
            return OperationResult<Convenio?>.Failure("Un convenio específico no puede suscribirse a sí mismo.");

        var convenioPadre = await _convenios.ObtenerBaseConRepresentantesAsync(
            convenioPadreId.Value,
            cancellationToken);

        if (convenioPadre is null)
            return OperationResult<Convenio?>.Failure(
                "El convenio base seleccionado no existe.");

        var tipoPadre = convenioPadre.TipoConvenio?.Nombre ?? string.Empty;
        if (!string.Equals(tipoPadre, "Marco", StringComparison.OrdinalIgnoreCase) &&
            !string.Equals(tipoPadre, "Internacional", StringComparison.OrdinalIgnoreCase))
            return OperationResult<Convenio?>.Failure(
                "El convenio base de un específico debe ser Marco o Internacional.");

        if (convenioPadre.FechaInicio.Date > fechaInicio.Date)
            return OperationResult<Convenio?>.Failure(
                "El convenio Marco o Internacional debe existir antes de iniciar el convenio específico.");

        return OperationResult<Convenio?>.Success(convenioPadre);
    }
}
