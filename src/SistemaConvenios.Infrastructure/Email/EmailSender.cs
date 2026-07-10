using System.Net;
using System.Net.Mail;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using SistemaConvenios.Application.Abstractions;

namespace SistemaConvenios.Infrastructure.Email;

internal sealed class EmailSender : IEmailSender
{
    private readonly IConfiguration _config;
    private readonly ILogger<EmailSender> _logger;
    private readonly IHostEnvironment _environment;

    public EmailSender(
        IConfiguration config,
        ILogger<EmailSender> logger,
        IHostEnvironment environment)
    {
        _config = config;
        _logger = logger;
        _environment = environment;
    }

    public async Task SendEmailAsync(string to, string subject, string htmlBody)
    {
        var host = _config["Smtp:Host"];
        if (string.IsNullOrWhiteSpace(host))
        {
            _logger.LogWarning(
                "SMTP no configurado. Correo simulado para {To}. Asunto: {Subject}.",
                to,
                subject);

            if (_environment.IsDevelopment())
                _logger.LogDebug("Contenido del correo simulado: {Body}", htmlBody);
            return;
        }

        var port = _config.GetValue("Smtp:Port", 587);
        var user = _config["Smtp:User"];
        var password = _config["Smtp:Password"];
        var from = _config["Smtp:From"] ?? user
            ?? throw new InvalidOperationException("Debe configurar Smtp:From o Smtp:User.");

        using var message = new MailMessage
        {
            From = new MailAddress(from, _config["Smtp:FromName"] ?? "Sistema Convenios FCVT"),
            Subject = subject,
            Body = htmlBody,
            IsBodyHtml = true
        };
        message.To.Add(to);

        using var client = new SmtpClient(host, port)
        {
            EnableSsl = _config.GetValue("Smtp:EnableSsl", true),
            Credentials = string.IsNullOrWhiteSpace(user)
                ? CredentialCache.DefaultNetworkCredentials
                : new NetworkCredential(user, password)
        };
        await client.SendMailAsync(message);
    }
}
