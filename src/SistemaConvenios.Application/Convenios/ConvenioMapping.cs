using SistemaConvenios.Models;

namespace SistemaConvenios.Application.Convenios;

internal static class ConvenioMapping
{
    public static ConvenioListaDto ToListaDto(this Convenio convenio) =>
        new(
            convenio.Id,
            convenio.Numero,
            convenio.Entidad?.Nombre ?? string.Empty,
            convenio.TipoConvenio?.Nombre ?? string.Empty,
            convenio.Ambito,
            convenio.AreaPromotora is null
                ? null
                : $"{convenio.AreaPromotora.Codigo} - {convenio.AreaPromotora.Nombre}",
            convenio.ConvenioPadre?.Numero,
            convenio.FechaInicio,
            convenio.FechaVencimiento,
            convenio.Entidad?.Provincia,
            convenio.Entidad?.Ciudad,
            convenio.Estado);

    public static ConvenioDetalleDto ToDetalleDto(this Convenio convenio) =>
        new(
            convenio.Id,
            convenio.Numero,
            convenio.TipoConvenioId,
            convenio.TipoConvenio?.Nombre ?? string.Empty,
            convenio.EntidadId,
            convenio.Entidad is null
                ? null
                : new EntidadResumenDto(
                    convenio.Entidad.Id,
                    convenio.Entidad.Nombre,
                    convenio.Entidad.TipoEntidad,
                    convenio.Entidad.Ruc,
                    convenio.Entidad.RepresentanteLegal,
                    convenio.Entidad.CargoRepresentante,
                    convenio.Entidad.TelefonoRepresentante,
                    convenio.Entidad.ContactoGestionNombre,
                    convenio.Entidad.ContactoGestionEmail,
                    convenio.Entidad.ContactoGestionTelefono,
                    convenio.Entidad.Provincia,
                    convenio.Entidad.Ciudad,
                    convenio.Entidad.Pais,
                    convenio.Entidad.Email),
            convenio.AreaPromotoraId,
            convenio.AreaPromotora is null
                ? null
                : $"{convenio.AreaPromotora.Codigo} - {convenio.AreaPromotora.Nombre}",
            convenio.ConvenioPadreId,
            convenio.ConvenioPadre?.Numero,
            convenio.ConvenioPadre?.TipoConvenio?.Nombre,
            convenio.Ambito,
            convenio.Objeto,
            convenio.FechaInicio,
            convenio.FechaVencimiento,
            convenio.Estado,
            convenio.NumeroResolucion,
            convenio.Supervisor,
            convenio.ContactoGestionNombre,
            convenio.ContactoGestionEmail,
            convenio.ContactoGestionTelefono,
            convenio.RenovacionAutomatica,
            convenio.Observaciones,
            convenio.ConvenioFacultades
                .Where(x => x.Facultad is not null)
                .Select(x => new CatalogoItemDto(
                    x.FacultadId,
                    x.Facultad!.Nombre,
                    x.Facultad.Siglas))
                .ToList(),
            convenio.ConvenioCarreras
                .Where(x => x.Carrera is not null)
                .Select(x => new CatalogoItemDto(
                    x.CarreraId,
                    x.Carrera!.Nombre,
                    x.Carrera.Siglas,
                    x.Carrera.FacultadId))
                .ToList(),
            convenio.Archivos
                .OrderByDescending(x => x.FechaSubida)
                .Select(x => new ArchivoConvenioDto(
                    x.Id,
                    x.NombreOriginal,
                    x.TipoDocumento,
                    x.Descripcion,
                    x.TamanioBytes,
                    x.FechaSubida))
                .ToList(),
            convenio.Historial
                .OrderByDescending(x => x.FechaCambio)
                .Select(x => new HistorialEstadoDto(
                    x.FechaCambio,
                    x.EstadoAnterior,
                    x.EstadoNuevo,
                    x.Observacion,
                    x.CambiadoPor))
                .ToList());

    public static ConvenioEdicionDto ToEdicionDto(this Convenio convenio) =>
        new(
            convenio.Id,
            convenio.Numero,
            convenio.TipoConvenioId,
            convenio.EntidadId,
            convenio.AreaPromotoraId,
            convenio.ConvenioPadreId,
            convenio.Ambito,
            convenio.Objeto,
            convenio.FechaInicio,
            convenio.FechaVencimiento,
            convenio.Estado,
            convenio.NumeroResolucion,
            convenio.Supervisor,
            convenio.ContactoGestionNombre,
            convenio.ContactoGestionEmail,
            convenio.ContactoGestionTelefono,
            convenio.RenovacionAutomatica,
            convenio.Observaciones,
            convenio.ConvenioFacultades.Select(x => x.FacultadId).ToList(),
            convenio.ConvenioCarreras.Select(x => x.CarreraId).ToList());
}
