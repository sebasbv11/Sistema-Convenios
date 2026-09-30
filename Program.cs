using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpOverrides;
using System.Threading.RateLimiting;
using Microsoft.EntityFrameworkCore;
using SistemaConvenios.Application;
using SistemaConvenios.Data;
using SistemaConvenios.Infrastructure;
using SistemaConvenios.Models;

var builder = WebApplication.CreateBuilder(args);

builder.Logging.ClearProviders();
builder.Logging.AddConsole();
builder.Logging.AddDebug();

builder.Services.AddApplication();
builder.Services.AddInfrastructure(builder.Configuration);

builder.Services.AddIdentity<Usuario, IdentityRole>(options =>
{
    options.Password.RequireDigit = true;
    options.Password.RequireLowercase = true;
    options.Password.RequireUppercase = true;
    options.Password.RequireNonAlphanumeric = true;
    options.Password.RequiredLength = 8;
    options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(15);
    options.Lockout.MaxFailedAccessAttempts = 5;
    options.User.RequireUniqueEmail = true;
})
.AddEntityFrameworkStores<ApplicationDbContext>()
.AddDefaultTokenProviders();

builder.Services.ConfigureApplicationCookie(options =>
{
    options.LoginPath = "/Account/Login";
    options.LogoutPath = "/Account/Logout";
    options.AccessDeniedPath = "/Account/AccesoDenegado";
    options.Cookie.HttpOnly = true;
    options.Cookie.SameSite = SameSiteMode.Strict;
    options.Cookie.SecurePolicy = builder.Environment.IsDevelopment()
        ? CookieSecurePolicy.SameAsRequest
        : CookieSecurePolicy.Always;
    options.SlidingExpiration = true;
    options.ExpireTimeSpan = TimeSpan.FromHours(8);
});

builder.Services.AddControllersWithViews();
builder.Services.AddRateLimiter(options =>
{
    options.RejectionStatusCode = StatusCodes.Status429TooManyRequests;
    options.AddPolicy("login", context => RateLimitPartition.GetFixedWindowLimiter(
        context.Connection.RemoteIpAddress?.ToString() ?? "desconocida",
        _ => new FixedWindowRateLimiterOptions
        {
            PermitLimit = 10,
            Window = TimeSpan.FromMinutes(1),
            QueueLimit = 0
        }));
});

var configuredKeysPath = builder.Configuration["DataProtection:KeysPath"];
var dataProtectionProvider = builder.Configuration["DataProtection:Provider"] ??
    (builder.Environment.IsDevelopment() || builder.Environment.IsEnvironment("Testing")
        ? "FileSystem"
        : "Database");
if (dataProtectionProvider.Equals("Database", StringComparison.OrdinalIgnoreCase))
{
    builder.Services.AddDataProtection()
        .SetApplicationName("SistemaConvenios")
        .PersistKeysToDbContext<ApplicationDbContext>();
}
else if (dataProtectionProvider.Equals("FileSystem", StringComparison.OrdinalIgnoreCase))
{
    var keysPath = string.IsNullOrWhiteSpace(configuredKeysPath)
        ? Path.Combine(builder.Environment.ContentRootPath, "obj", "DataProtectionKeys")
        : Path.IsPathRooted(configuredKeysPath)
            ? configuredKeysPath
            : Path.Combine(builder.Environment.ContentRootPath, configuredKeysPath);
    Directory.CreateDirectory(keysPath);
    builder.Services.AddDataProtection()
        .SetApplicationName("SistemaConvenios")
        .PersistKeysToFileSystem(new DirectoryInfo(keysPath));
}
else
{
    throw new InvalidOperationException(
        $"DataProtection:Provider '{dataProtectionProvider}' no está soportado.");
}

var app = builder.Build();

if (args.Contains("--migrate", StringComparer.OrdinalIgnoreCase))
{
    using var migrationScope = app.Services.CreateScope();
    var migrationDb = migrationScope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    await migrationDb.Database.MigrateAsync();
    return;
}

var forwardedHeadersOptions = new ForwardedHeadersOptions
{
    ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto,
    ForwardLimit = 1
};
if (!app.Environment.IsDevelopment() && !app.Environment.IsEnvironment("Testing"))
{
    forwardedHeadersOptions.KnownIPNetworks.Clear();
    forwardedHeadersOptions.KnownProxies.Clear();
}
app.UseForwardedHeaders(forwardedHeadersOptions);

if (!app.Environment.IsDevelopment() &&
    !app.Environment.IsEnvironment("Testing"))
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

if (!app.Environment.IsDevelopment())
    app.UseHttpsRedirection();
app.Use(async (context, next) =>
{
    context.Response.Headers["X-Content-Type-Options"] = "nosniff";
    context.Response.Headers["X-Frame-Options"] = "DENY";
    context.Response.Headers["Referrer-Policy"] = "strict-origin-when-cross-origin";
    context.Response.Headers["Content-Security-Policy"] =
        "default-src 'self'; img-src 'self' data:; " +
        "style-src 'self' 'unsafe-inline' https://cdn.jsdelivr.net; " +
        "script-src 'self' 'unsafe-inline' https://cdn.jsdelivr.net; " +
        "font-src 'self' https://cdn.jsdelivr.net; frame-ancestors 'none'; base-uri 'self'; form-action 'self'";
    await next();
});
app.UseStaticFiles();
app.UseRouting();
app.UseRateLimiter();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
app.MapGet("/health", () => Results.Ok(new { status = "ok" }))
    .AllowAnonymous();
app.MapGet("/health/ready", async (ApplicationDbContext db, CancellationToken ct) =>
        await db.Database.CanConnectAsync(ct)
            ? Results.Ok(new { status = "ready" })
            : Results.StatusCode(StatusCodes.Status503ServiceUnavailable))
    .AllowAnonymous();

if (!app.Environment.IsEnvironment("Testing"))
{
    using var scope = app.Services.CreateScope();
    var services = scope.ServiceProvider;
    var db = services.GetRequiredService<ApplicationDbContext>();
    if (builder.Configuration.GetValue(
            "Database:ApplyMigrationsOnStartup",
            builder.Environment.IsDevelopment()))
    {
        await db.Database.MigrateAsync();
    }
    await SeedAdminUsuario(services, builder.Configuration);
}

app.Run();

static async Task SeedAdminUsuario(IServiceProvider services, IConfiguration configuration)
{
    var userManager = services.GetRequiredService<UserManager<Usuario>>();
    var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();

    foreach (var role in new[] { "Admin", "Secretaria", "Visualizador" })
    {
        if (!await roleManager.RoleExistsAsync(role))
            await roleManager.CreateAsync(new IdentityRole(role));
    }

    if (!configuration.GetValue("SeedAdmin:Enabled", false))
        return;

    var adminEmail = configuration["SeedAdmin:Email"];
    var adminPassword = configuration["SeedAdmin:Password"];
    if (string.IsNullOrWhiteSpace(adminEmail) || string.IsNullOrWhiteSpace(adminPassword))
        throw new InvalidOperationException("SeedAdmin requiere Email y Password.");

    if (await userManager.FindByEmailAsync(adminEmail) == null)
    {
        var admin = new Usuario
        {
            UserName = adminEmail,
            Email = adminEmail,
            NombreCompleto = "Administrador FCVT",
            Cargo = "Administrador del sistema",
            Activo = true,
            EmailConfirmed = true
        };

        var result = await userManager.CreateAsync(admin, adminPassword);
        if (!result.Succeeded)
            throw new InvalidOperationException(string.Join("; ", result.Errors.Select(x => x.Description)));

        await userManager.AddToRoleAsync(admin, "Admin");
    }
}

public partial class Program;
