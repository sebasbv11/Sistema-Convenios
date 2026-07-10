using Microsoft.Extensions.Hosting;
using SistemaConvenios.Application.Abstractions;

namespace SistemaConvenios.Infrastructure.Files;

internal sealed class LocalArchivoStorage : IArchivoStorage
{
    private const string RutaPrivada = "Storage/archivos/convenios";
    private readonly string _contentRoot;

    public LocalArchivoStorage(IHostEnvironment environment)
    {
        _contentRoot = environment.ContentRootPath;
    }

    public async Task<string> GuardarPdfAsync(
        int convenioId,
        string nombreOriginal,
        Stream contenido,
        CancellationToken cancellationToken = default)
    {
        await ValidarFirmaPdfAsync(contenido, cancellationToken);

        var carpetaRelativa = $"{RutaPrivada}/{convenioId}";
        var carpetaAbsoluta = ResolverRutaSegura(carpetaRelativa);
        Directory.CreateDirectory(carpetaAbsoluta);

        var nombre = $"{DateTime.UtcNow:yyyyMMdd_HHmmss}_{Guid.NewGuid():N}.pdf";
        var rutaRelativa = $"{carpetaRelativa}/{nombre}";
        var rutaAbsoluta = ResolverRutaSegura(rutaRelativa);

        await using var destino = new FileStream(
            rutaAbsoluta,
            FileMode.CreateNew,
            FileAccess.Write,
            FileShare.None,
            81920,
            useAsync: true);
        await contenido.CopyToAsync(destino, cancellationToken);
        return rutaRelativa;
    }

    public Task<Stream?> AbrirLecturaAsync(
        string ruta,
        CancellationToken cancellationToken = default)
    {
        var rutaAbsoluta = ResolverRutaExistente(ruta);
        Stream? stream = rutaAbsoluta is null
            ? null
            : new FileStream(rutaAbsoluta, FileMode.Open, FileAccess.Read, FileShare.Read);
        return Task.FromResult(stream);
    }

    public Task EliminarAsync(string ruta, CancellationToken cancellationToken = default)
    {
        var rutaAbsoluta = ResolverRutaExistente(ruta);
        if (rutaAbsoluta is not null)
            File.Delete(rutaAbsoluta);
        return Task.CompletedTask;
    }

    private string? ResolverRutaExistente(string ruta)
    {
        var principal = ResolverRutaSegura(ruta);
        if (File.Exists(principal))
            return principal;

        if (ruta.Replace('\\', '/').StartsWith("archivos/", StringComparison.OrdinalIgnoreCase))
        {
            var privada = ResolverRutaSegura($"Storage/{ruta.Replace('\\', '/')}");
            if (File.Exists(privada))
                return privada;

            var heredada = ResolverRutaSegura($"wwwroot/{ruta.Replace('\\', '/')}");
            if (File.Exists(heredada))
                return heredada;
        }

        return null;
    }

    private string ResolverRutaSegura(string rutaRelativa)
    {
        var raiz = Path.GetFullPath(_contentRoot);
        var ruta = Path.GetFullPath(Path.Combine(
            raiz,
            rutaRelativa.Replace('/', Path.DirectorySeparatorChar)));

        if (!ruta.StartsWith(raiz + Path.DirectorySeparatorChar, StringComparison.OrdinalIgnoreCase))
            throw new InvalidOperationException("La ruta del archivo no es válida.");

        return ruta;
    }

    private static async Task ValidarFirmaPdfAsync(
        Stream contenido,
        CancellationToken cancellationToken)
    {
        if (!contenido.CanSeek)
            throw new InvalidDataException("No se puede validar el archivo recibido.");

        var posicion = contenido.Position;
        var firma = new byte[5];
        var leidos = await contenido.ReadAsync(firma, cancellationToken);
        contenido.Position = posicion;

        if (leidos != firma.Length || System.Text.Encoding.ASCII.GetString(firma) != "%PDF-")
            throw new InvalidDataException("El contenido del archivo no corresponde a un PDF válido.");
    }
}
