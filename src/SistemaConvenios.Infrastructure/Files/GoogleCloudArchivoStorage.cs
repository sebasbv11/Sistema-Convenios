using System.Net;
using Google;
using Google.Cloud.Storage.V1;
using SistemaConvenios.Application.Abstractions;

namespace SistemaConvenios.Infrastructure.Files;

internal sealed class GoogleCloudArchivoStorage : IArchivoStorage
{
    private readonly StorageClient _client;
    private readonly string _bucket;

    public GoogleCloudArchivoStorage(StorageClient client, string bucket)
    {
        _client = client;
        _bucket = string.IsNullOrWhiteSpace(bucket)
            ? throw new InvalidOperationException("ArchivosConfig:Bucket es obligatorio para Google Cloud Storage.")
            : bucket;
    }

    public async Task<string> GuardarPdfAsync(
        int convenioId,
        string nombreOriginal,
        Stream contenido,
        CancellationToken cancellationToken = default)
    {
        await ValidarFirmaPdfAsync(contenido, cancellationToken);
        var nombre = $"convenios/{convenioId}/{DateTime.UtcNow:yyyyMMdd_HHmmss}_{Guid.NewGuid():N}.pdf";
        await _client.UploadObjectAsync(
            _bucket,
            nombre,
            "application/pdf",
            contenido,
            cancellationToken: cancellationToken);
        return $"gcs://{_bucket}/{nombre}";
    }

    public async Task<Stream?> AbrirLecturaAsync(
        string ruta,
        CancellationToken cancellationToken = default)
    {
        var objeto = ObtenerNombreObjeto(ruta);
        var contenido = new MemoryStream();
        try
        {
            await _client.DownloadObjectAsync(
                _bucket,
                objeto,
                contenido,
                cancellationToken: cancellationToken);
            contenido.Position = 0;
            return contenido;
        }
        catch (GoogleApiException ex) when (ex.HttpStatusCode == HttpStatusCode.NotFound)
        {
            await contenido.DisposeAsync();
            return null;
        }
    }

    public async Task EliminarAsync(string ruta, CancellationToken cancellationToken = default)
    {
        try
        {
            await _client.DeleteObjectAsync(
                _bucket,
                ObtenerNombreObjeto(ruta),
                cancellationToken: cancellationToken);
        }
        catch (GoogleApiException ex) when (ex.HttpStatusCode == HttpStatusCode.NotFound)
        {
            // La eliminación es idempotente.
        }
    }

    private string ObtenerNombreObjeto(string ruta)
    {
        var prefijo = $"gcs://{_bucket}/";
        var objeto = ruta.StartsWith(prefijo, StringComparison.Ordinal)
            ? ruta[prefijo.Length..]
            : ruta.Replace('\\', '/').TrimStart('/');

        if (string.IsNullOrWhiteSpace(objeto) ||
            objeto.Split('/').Any(segmento => segmento is "" or "." or ".."))
            throw new InvalidOperationException("La ruta del archivo no es válida.");

        return objeto;
    }

    private static async Task ValidarFirmaPdfAsync(Stream contenido, CancellationToken cancellationToken)
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
