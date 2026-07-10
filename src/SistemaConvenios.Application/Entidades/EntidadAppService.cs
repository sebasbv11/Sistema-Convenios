using SistemaConvenios.Application.Abstractions;
using SistemaConvenios.Application.Common;
using SistemaConvenios.Domain.Exceptions;
using SistemaConvenios.Models;

namespace SistemaConvenios.Application.Entidades;

public sealed class EntidadAppService : IEntidadAppService
{
    private readonly IEntidadRepository _entidades;
    private readonly IUnitOfWork _unitOfWork;

    public EntidadAppService(IEntidadRepository entidades, IUnitOfWork unitOfWork)
    {
        _entidades = entidades;
        _unitOfWork = unitOfWork;
    }

    public async Task<IReadOnlyList<EntidadListaDto>> BuscarAsync(
        string? busqueda,
        CancellationToken cancellationToken = default)
    {
        var entidades = await _entidades.BuscarAsync(busqueda, cancellationToken);
        return entidades
            .Select(x => new EntidadListaDto(
                x.Id, x.Nombre, x.TipoEntidad, x.Ruc, x.RepresentanteLegal,
                x.Provincia, x.Ciudad, x.Pais, x.Email,
                x.ContactoGestionNombre, x.ContactoGestionTelefono, x.Activo))
            .ToList();
    }

    public async Task<EntidadEdicionDto?> ObtenerAsync(
        int id,
        CancellationToken cancellationToken = default)
    {
        var entidad = await _entidades.ObtenerAsync(id, cancellationToken);
        return entidad is null
            ? null
            : new EntidadEdicionDto(
                entidad.Id,
                entidad.Nombre,
                entidad.TipoEntidad,
                entidad.Ruc,
                entidad.Direccion,
                entidad.Provincia,
                entidad.Ciudad,
                entidad.Pais,
                entidad.RepresentanteLegal,
                entidad.CedulaRepresentante,
                entidad.CargoRepresentante,
                entidad.TelefonoRepresentante,
                entidad.Email,
                entidad.Telefono,
                entidad.ContactoGestionNombre,
                entidad.ContactoGestionCargo,
                entidad.ContactoGestionEmail,
                entidad.ContactoGestionTelefono,
                entidad.Activo);
    }

    public async Task<OperationResult<int>> CrearAsync(
        GuardarEntidadCommand command,
        CancellationToken cancellationToken = default)
    {
        if (await _entidades.ExisteRucAsync(command.Ruc.Trim(), null, cancellationToken))
            return OperationResult<int>.Failure("Ya existe una entidad registrada con este RUC o cédula.");

        try
        {
            var entidad = Entidad.Crear(
                command.Nombre,
                command.TipoEntidad,
                command.Ruc,
                command.Direccion,
                command.Provincia,
                command.Ciudad,
                command.Pais,
                command.RepresentanteLegal,
                command.CedulaRepresentante,
                command.CargoRepresentante,
                command.TelefonoRepresentante,
                command.Email,
                command.Telefono,
                command.ContactoGestionNombre,
                command.ContactoGestionCargo,
                command.ContactoGestionEmail,
                command.ContactoGestionTelefono);

            await _entidades.AgregarAsync(entidad, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
            return OperationResult<int>.Success(entidad.Id);
        }
        catch (DomainException ex)
        {
            return OperationResult<int>.Failure(ex.Message);
        }
    }

    public async Task<OperationResult> EditarAsync(
        int id,
        GuardarEntidadCommand command,
        CancellationToken cancellationToken = default)
    {
        var entidad = await _entidades.ObtenerAsync(id, cancellationToken);
        if (entidad is null)
            return OperationResult.Failure("La entidad no existe.");

        if (await _entidades.ExisteRucAsync(command.Ruc.Trim(), id, cancellationToken))
            return OperationResult.Failure("Ya existe una entidad registrada con este RUC o cédula.");

        try
        {
            entidad.Actualizar(
                command.Nombre,
                command.TipoEntidad,
                command.Ruc,
                command.Direccion,
                command.Provincia,
                command.Ciudad,
                command.Pais,
                command.RepresentanteLegal,
                command.CedulaRepresentante,
                command.CargoRepresentante,
                command.TelefonoRepresentante,
                command.Email,
                command.Telefono,
                command.ContactoGestionNombre,
                command.ContactoGestionCargo,
                command.ContactoGestionEmail,
                command.ContactoGestionTelefono,
                command.Activo);

            await _unitOfWork.SaveChangesAsync(cancellationToken);
            return OperationResult.Success();
        }
        catch (DomainException ex)
        {
            return OperationResult.Failure(ex.Message);
        }
    }
}
