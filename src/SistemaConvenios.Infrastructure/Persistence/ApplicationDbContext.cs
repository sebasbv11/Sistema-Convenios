using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using SistemaConvenios.Application.Abstractions;
using SistemaConvenios.Models;

namespace SistemaConvenios.Data;

public sealed class ApplicationDbContext : IdentityDbContext<Usuario>, IUnitOfWork
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    public DbSet<TipoConvenio> TiposConvenio => Set<TipoConvenio>();
    public DbSet<AreaPromotora> AreasPromotoras => Set<AreaPromotora>();
    public DbSet<Facultad> Facultades => Set<Facultad>();
    public DbSet<Carrera> Carreras => Set<Carrera>();
    public DbSet<Entidad> Entidades => Set<Entidad>();
    public DbSet<Convenio> Convenios => Set<Convenio>();
    public DbSet<ConvenioFacultad> ConvenioFacultades => Set<ConvenioFacultad>();
    public DbSet<ConvenioCarrera> ConvenioCarreras => Set<ConvenioCarrera>();
    public DbSet<HistorialEstado> HistorialEstados => Set<HistorialEstado>();
    public DbSet<ArchivoConvenio> ArchivosConvenio => Set<ArchivoConvenio>();
    public DbSet<ParteConvenio> PartesConvenio => Set<ParteConvenio>();
    public DbSet<FirmanteConvenio> FirmantesConvenio => Set<FirmanteConvenio>();
    public DbSet<ResponsableConvenio> ResponsablesConvenio => Set<ResponsableConvenio>();
    public DbSet<AmbitoConvenio> AmbitosConvenio => Set<AmbitoConvenio>();
    public DbSet<ClausulaConvenio> ClausulasConvenio => Set<ClausulaConvenio>();
    public DbSet<ObligacionConvenio> ObligacionesConvenio => Set<ObligacionConvenio>();
    public DbSet<ActividadConvenio> ActividadesConvenio => Set<ActividadConvenio>();
    public DbSet<EstudianteConvenio> EstudiantesConvenio => Set<EstudianteConvenio>();
    public DbSet<ParticipacionActividad> ParticipacionesActividad => Set<ParticipacionActividad>();
    public DbSet<ConvenioRelacionado> ConveniosRelacionados => Set<ConvenioRelacionado>();
    public DbSet<ModificacionConvenio> ModificacionesConvenio => Set<ModificacionConvenio>();
    public DbSet<EvaluacionConvenio> EvaluacionesConvenio => Set<EvaluacionConvenio>();
    public DbSet<CierreConvenio> CierresConvenio => Set<CierreConvenio>();

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        ConfigurarIdentity(builder);
        ConfigurarCatalogos(builder);
        ConfigurarEntidades(builder);
        ConfigurarConvenios(builder);
    }

    private static void ConfigurarIdentity(ModelBuilder builder)
    {
        builder.Entity<Usuario>(entity =>
        {
            entity.ToTable("Usuarios");
            entity.Property(x => x.NombreCompleto).HasMaxLength(100).IsRequired();
            entity.Property(x => x.Cargo).HasMaxLength(50);
        });
        builder.Entity<IdentityRole>().ToTable("Roles");
        builder.Entity<IdentityUserRole<string>>().ToTable("UsuarioRoles");
        builder.Entity<IdentityUserClaim<string>>().ToTable("UsuarioClaims");
        builder.Entity<IdentityUserLogin<string>>().ToTable("UsuarioLogins");
        builder.Entity<IdentityRoleClaim<string>>().ToTable("RoleClaims");
        builder.Entity<IdentityUserToken<string>>().ToTable("UsuarioTokens");
    }

    private static void ConfigurarCatalogos(ModelBuilder builder)
    {
        builder.Entity<TipoConvenio>(entity =>
        {
            entity.ToTable("TiposConvenio");
            entity.Property(x => x.Nombre).HasMaxLength(80).IsRequired();
            entity.Property(x => x.Descripcion).HasMaxLength(300);
            entity.HasData(
                new { Id = 1, Nombre = "Marco", Descripcion = "Convenio marco general" },
                new { Id = 2, Nombre = "Específico", Descripcion = "Convenio para actividades específicas" },
                new { Id = 3, Nombre = "Internacional", Descripcion = "Convenio con instituciones extranjeras" },
                new { Id = 4, Nombre = "Otro", Descripcion = "Otros tipos de convenio" });
        });

        builder.Entity<AreaPromotora>(entity =>
        {
            entity.ToTable("AreasPromotoras");
            entity.Property(x => x.Codigo).HasMaxLength(20).IsRequired();
            entity.Property(x => x.Nombre).HasMaxLength(150).IsRequired();
            entity.HasIndex(x => x.Codigo).IsUnique();
            entity.HasData(
                new { Id = 1, Codigo = "VIN-001", Nombre = "Vinculación con la sociedad", Activo = true },
                new { Id = 2, Codigo = "PRA-001", Nombre = "Prácticas preprofesionales", Activo = true },
                new { Id = 3, Codigo = "INV-001", Nombre = "Investigación", Activo = true },
                new { Id = 4, Codigo = "ACA-001", Nombre = "Académica", Activo = true });
        });

        builder.Entity<Facultad>(entity =>
        {
            entity.ToTable("Facultades");
            entity.Property(x => x.Nombre).HasMaxLength(200).IsRequired();
            entity.Property(x => x.Siglas).HasMaxLength(20);
            entity.HasData(new
            {
                Id = 1,
                Nombre = "Facultad de Ciencias de la Vida y Tecnologías",
                Siglas = "FCVT",
                Activo = true
            });
        });

        builder.Entity<Carrera>(entity =>
        {
            entity.ToTable("Carreras");
            entity.Property(x => x.Nombre).HasMaxLength(200).IsRequired();
            entity.Property(x => x.Siglas).HasMaxLength(20);
            entity.HasOne(x => x.Facultad)
                .WithMany(x => x.Carreras)
                .HasForeignKey(x => x.FacultadId);
            entity.HasData(
                new { Id = 1, Nombre = "Ingeniería de Software", Siglas = "IS", FacultadId = 1, Activo = true },
                new { Id = 2, Nombre = "Ingeniería Ambiental", Siglas = "IA", FacultadId = 1, Activo = true },
                new { Id = 3, Nombre = "Medicina Veterinaria", Siglas = "MV", FacultadId = 1, Activo = true });
        });
    }

    private static void ConfigurarEntidades(ModelBuilder builder)
    {
        builder.Entity<Entidad>(entity =>
        {
            entity.ToTable("Entidades");
            entity.Property(x => x.Nombre).HasMaxLength(200).IsRequired();
            entity.Property(x => x.TipoEntidad).HasMaxLength(50).IsRequired();
            entity.Property(x => x.Ruc).HasMaxLength(13).IsRequired(false);
            entity.Property(x => x.Direccion).HasMaxLength(300).IsRequired(false);
            entity.Property(x => x.Provincia).HasMaxLength(100).IsRequired(false);
            entity.Property(x => x.Ciudad).HasMaxLength(100).IsRequired(false);
            entity.Property(x => x.Pais).HasMaxLength(100).IsRequired();
            entity.Property(x => x.RepresentanteLegal).HasMaxLength(150).IsRequired(false);
            entity.Property(x => x.CedulaRepresentante).HasMaxLength(13).IsRequired(false);
            entity.Property(x => x.CargoRepresentante).HasMaxLength(100).IsRequired(false);
            entity.Property(x => x.TelefonoRepresentante).HasMaxLength(30).IsRequired(false);
            entity.Property(x => x.Email).HasMaxLength(150).IsRequired(false);
            entity.Property(x => x.Telefono).HasMaxLength(20).IsRequired(false);
            entity.Property(x => x.ContactoGestionNombre).HasMaxLength(150);
            entity.Property(x => x.ContactoGestionCargo).HasMaxLength(150);
            entity.Property(x => x.ContactoGestionEmail).HasMaxLength(150);
            entity.Property(x => x.ContactoGestionTelefono).HasMaxLength(30);
        });
    }

    private static void ConfigurarConvenios(ModelBuilder builder)
    {
        builder.Entity<Convenio>(entity =>
        {
            entity.ToTable("Convenios");
            entity.Property(x => x.Numero).HasMaxLength(20).IsRequired();
            entity.Property(x => x.Ambito).HasMaxLength(500).IsRequired();
            entity.Property(x => x.Objeto).IsRequired();
            entity.Property(x => x.Estado).HasMaxLength(30).IsRequired();
            entity.Property(x => x.NumeroResolucion).HasMaxLength(100).IsRequired(false);
            entity.Property(x => x.Supervisor).HasMaxLength(200);
            entity.Property(x => x.ContactoGestionNombre).HasMaxLength(150);
            entity.Property(x => x.ContactoGestionEmail).HasMaxLength(150);
            entity.Property(x => x.ContactoGestionTelefono).HasMaxLength(30);
            entity.Property(x => x.Observaciones).HasMaxLength(500);
            entity.Property(x => x.DomicilioContractual).HasMaxLength(300);
            entity.Property(x => x.MecanismoSolucionControversias).HasMaxLength(1000);
            entity.Property(x => x.Presupuesto).HasPrecision(18, 2);
            entity.Property(x => x.Moneda).HasMaxLength(10);
            entity.Property(x => x.FuenteFinanciamiento).HasMaxLength(300);
            entity.Property(x => x.CondicionesRenovacion).HasMaxLength(500);
            entity.HasOne(x => x.TipoConvenio)
                .WithMany(x => x.Convenios)
                .HasForeignKey(x => x.TipoConvenioId);
            entity.HasOne(x => x.Entidad)
                .WithMany(x => x.Convenios)
                .HasForeignKey(x => x.EntidadId);
            entity.HasOne(x => x.AreaPromotora)
                .WithMany(x => x.Convenios)
                .HasForeignKey(x => x.AreaPromotoraId)
                .OnDelete(DeleteBehavior.Restrict);
            entity.HasOne(x => x.ConvenioPadre)
                .WithMany(x => x.ConveniosDerivados)
                .HasForeignKey(x => x.ConvenioPadreId)
                .OnDelete(DeleteBehavior.Restrict);
            entity.HasOne<Usuario>()
                .WithMany(x => x.ConveniosAdministrados)
                .HasForeignKey(x => x.UsuarioCreadorId)
                .IsRequired(false);
        });

        builder.Entity<ConvenioFacultad>(entity =>
        {
            entity.ToTable("ConvenioFacultades");
            entity.HasKey(x => new { x.ConvenioId, x.FacultadId });
            entity.HasOne(x => x.Convenio)
                .WithMany(x => x.ConvenioFacultades)
                .HasForeignKey(x => x.ConvenioId);
            entity.HasOne(x => x.Facultad)
                .WithMany(x => x.ConvenioFacultades)
                .HasForeignKey(x => x.FacultadId);
        });

        builder.Entity<ConvenioCarrera>(entity =>
        {
            entity.ToTable("ConvenioCarreras");
            entity.HasKey(x => new { x.ConvenioId, x.CarreraId });
            entity.HasOne(x => x.Convenio)
                .WithMany(x => x.ConvenioCarreras)
                .HasForeignKey(x => x.ConvenioId);
            entity.HasOne(x => x.Carrera)
                .WithMany(x => x.ConvenioCarreras)
                .HasForeignKey(x => x.CarreraId);
        });

        builder.Entity<HistorialEstado>(entity =>
        {
            entity.ToTable("HistorialEstados");
            entity.Property(x => x.EstadoAnterior).HasMaxLength(30).IsRequired();
            entity.Property(x => x.EstadoNuevo).HasMaxLength(30).IsRequired();
            entity.Property(x => x.Observacion).HasMaxLength(500);
            entity.Property(x => x.CambiadoPor).HasMaxLength(200);
            entity.HasOne(x => x.Convenio)
                .WithMany(x => x.Historial)
                .HasForeignKey(x => x.ConvenioId);
        });

        builder.Entity<ArchivoConvenio>(entity =>
        {
            entity.ToTable("ArchivosConvenio");
            entity.Property(x => x.NombreOriginal).HasMaxLength(200).IsRequired();
            entity.Property(x => x.RutaFisica).HasMaxLength(300).IsRequired();
            entity.Property(x => x.TipoDocumento).HasMaxLength(100).IsRequired();
            entity.Property(x => x.Descripcion).HasMaxLength(300);
            entity.Property(x => x.SubidoPor).HasMaxLength(200);
            entity.HasOne(x => x.Convenio)
                .WithMany(x => x.Archivos)
                .HasForeignKey(x => x.ConvenioId);
        });

        builder.Entity<ParteConvenio>(entity =>
        {
            entity.ToTable("PartesConvenio");
            entity.Property(x => x.Nombre).HasMaxLength(200).IsRequired();
            entity.Property(x => x.Alias).HasMaxLength(50);
            entity.Property(x => x.TipoParte).HasMaxLength(50).IsRequired();
            entity.Property(x => x.Direccion).HasMaxLength(300);
            entity.Property(x => x.Telefono).HasMaxLength(50);
            entity.Property(x => x.Email).HasMaxLength(150);
            entity.Property(x => x.Ruc).HasMaxLength(20);
            entity.HasOne(x => x.Convenio).WithMany(x => x.Partes).HasForeignKey(x => x.ConvenioId);
            entity.HasOne(x => x.Entidad).WithMany().HasForeignKey(x => x.EntidadId).OnDelete(DeleteBehavior.SetNull);
        });

        builder.Entity<FirmanteConvenio>(entity =>
        {
            entity.ToTable("FirmantesConvenio");
            entity.Property(x => x.Nombre).HasMaxLength(200).IsRequired();
            entity.Property(x => x.Cargo).HasMaxLength(150).IsRequired();
            entity.Property(x => x.Identificacion).HasMaxLength(20);
            entity.Property(x => x.TipoFirma).HasMaxLength(30).IsRequired();
            entity.Property(x => x.FundamentoRepresentacion).HasMaxLength(500);
            entity.HasOne(x => x.Convenio).WithMany(x => x.Firmantes).HasForeignKey(x => x.ConvenioId);
            entity.HasOne(x => x.ParteConvenio).WithMany(x => x.Firmantes)
                .HasForeignKey(x => x.ParteConvenioId).OnDelete(DeleteBehavior.SetNull);
        });

        builder.Entity<ResponsableConvenio>(entity =>
        {
            entity.ToTable("ResponsablesConvenio");
            entity.Property(x => x.Nombre).HasMaxLength(200).IsRequired();
            entity.Property(x => x.Rol).HasMaxLength(100).IsRequired();
            entity.Property(x => x.Cargo).HasMaxLength(150);
            entity.Property(x => x.Email).HasMaxLength(150);
            entity.Property(x => x.Telefono).HasMaxLength(50);
            entity.HasOne(x => x.Convenio).WithMany(x => x.Responsables).HasForeignKey(x => x.ConvenioId);
            entity.HasOne(x => x.ParteConvenio).WithMany(x => x.Responsables)
                .HasForeignKey(x => x.ParteConvenioId).OnDelete(DeleteBehavior.SetNull);
        });

        builder.Entity<AmbitoConvenio>(entity =>
        {
            entity.ToTable("AmbitosConvenio");
            entity.Property(x => x.Nombre).HasMaxLength(80).IsRequired();
            entity.HasIndex(x => new { x.ConvenioId, x.Nombre }).IsUnique();
            entity.HasOne(x => x.Convenio).WithMany(x => x.Ambitos).HasForeignKey(x => x.ConvenioId);
        });

        builder.Entity<ClausulaConvenio>(entity =>
        {
            entity.ToTable("ClausulasConvenio");
            entity.Property(x => x.Titulo).HasMaxLength(200).IsRequired();
            entity.Property(x => x.Tipo).HasMaxLength(80).IsRequired();
            entity.Property(x => x.Contenido).IsRequired();
            entity.HasOne(x => x.Convenio).WithMany(x => x.Clausulas).HasForeignKey(x => x.ConvenioId);
        });

        builder.Entity<ObligacionConvenio>(entity =>
        {
            entity.ToTable("ObligacionesConvenio");
            entity.Property(x => x.Actor).HasMaxLength(200).IsRequired();
            entity.Property(x => x.Descripcion).IsRequired();
            entity.Property(x => x.Estado).HasMaxLength(30).IsRequired();
            entity.Property(x => x.Evidencia).HasMaxLength(500);
            entity.HasOne(x => x.Convenio).WithMany(x => x.Obligaciones).HasForeignKey(x => x.ConvenioId);
        });

        builder.Entity<ActividadConvenio>(entity =>
        {
            entity.ToTable("ActividadesConvenio");
            entity.Property(x => x.Nombre).HasMaxLength(200).IsRequired();
            entity.Property(x => x.Tipo).HasMaxLength(80).IsRequired();
            entity.Property(x => x.Descripcion).HasMaxLength(1000);
            entity.Property(x => x.Estado).HasMaxLength(30).IsRequired();
            entity.Property(x => x.Lugar).HasMaxLength(300);
            entity.Property(x => x.Responsable).HasMaxLength(200);
            entity.HasOne(x => x.Convenio).WithMany(x => x.Actividades).HasForeignKey(x => x.ConvenioId);
        });

        builder.Entity<EstudianteConvenio>(entity =>
        {
            entity.ToTable("EstudiantesConvenio");
            entity.Property(x => x.Identificacion).HasMaxLength(20).IsRequired();
            entity.Property(x => x.Nombre).HasMaxLength(200).IsRequired();
            entity.Property(x => x.Email).HasMaxLength(150);
            entity.Property(x => x.Telefono).HasMaxLength(50);
            entity.Property(x => x.NumeroPoliza).HasMaxLength(100);
            entity.HasIndex(x => new { x.ConvenioId, x.Identificacion }).IsUnique();
            entity.HasOne(x => x.Convenio).WithMany(x => x.Estudiantes).HasForeignKey(x => x.ConvenioId);
            entity.HasOne(x => x.Carrera).WithMany().HasForeignKey(x => x.CarreraId).OnDelete(DeleteBehavior.SetNull);
        });

        builder.Entity<ParticipacionActividad>(entity =>
        {
            entity.ToTable("ParticipacionesActividad");
            entity.Property(x => x.Calificacion).HasPrecision(5, 2);
            entity.Property(x => x.Evaluacion).HasMaxLength(1000);
            entity.HasIndex(x => new { x.ActividadConvenioId, x.EstudianteConvenioId }).IsUnique();
            entity.HasOne(x => x.ActividadConvenio).WithMany(x => x.Participantes)
                .HasForeignKey(x => x.ActividadConvenioId);
            entity.HasOne(x => x.EstudianteConvenio).WithMany(x => x.Participaciones)
                .HasForeignKey(x => x.EstudianteConvenioId);
        });

        builder.Entity<ConvenioRelacionado>(entity =>
        {
            entity.ToTable("ConveniosRelacionados");
            entity.Property(x => x.TipoRelacion).HasMaxLength(80).IsRequired();
            entity.HasIndex(x => new { x.ConvenioId, x.ConvenioRelacionadoId }).IsUnique();
            entity.HasOne(x => x.Convenio).WithMany(x => x.ConveniosRelacionados)
                .HasForeignKey(x => x.ConvenioId).OnDelete(DeleteBehavior.Cascade);
            entity.HasOne(x => x.Relacionado).WithMany()
                .HasForeignKey(x => x.ConvenioRelacionadoId).OnDelete(DeleteBehavior.Restrict);
        });

        builder.Entity<ModificacionConvenio>(entity =>
        {
            entity.ToTable("ModificacionesConvenio");
            entity.Property(x => x.Tipo).HasMaxLength(80).IsRequired();
            entity.Property(x => x.Descripcion).IsRequired();
            entity.Property(x => x.Motivo).HasMaxLength(500);
            entity.HasOne(x => x.Convenio).WithMany(x => x.Modificaciones).HasForeignKey(x => x.ConvenioId);
        });

        builder.Entity<EvaluacionConvenio>(entity =>
        {
            entity.ToTable("EvaluacionesConvenio");
            entity.Property(x => x.Tipo).HasMaxLength(80).IsRequired();
            entity.Property(x => x.Calificacion).HasPrecision(5, 2);
            entity.Property(x => x.Conclusion).IsRequired();
            entity.Property(x => x.Responsable).HasMaxLength(200);
            entity.HasOne(x => x.Convenio).WithMany(x => x.Evaluaciones).HasForeignKey(x => x.ConvenioId);
        });

        builder.Entity<CierreConvenio>(entity =>
        {
            entity.ToTable("CierresConvenio");
            entity.Property(x => x.Causal).HasMaxLength(200).IsRequired();
            entity.Property(x => x.Resumen).IsRequired();
            entity.Property(x => x.ObligacionesPendientes).HasMaxLength(1000);
            entity.Property(x => x.ResponsablesSeguimiento).HasMaxLength(500);
            entity.HasIndex(x => x.ConvenioId).IsUnique();
            entity.HasOne(x => x.Convenio).WithOne(x => x.Cierre)
                .HasForeignKey<CierreConvenio>(x => x.ConvenioId);
        });
    }
}
