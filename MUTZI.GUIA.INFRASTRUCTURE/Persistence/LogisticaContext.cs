using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using MUTZI.GUIA.DOMAIN.Entities;
using MUTZI.GUIA.DOMAIN.Entities.Catalogo;

namespace MUTZI.GUIA.INFRASTRUCTURE.Persistence;

public partial class LogisticaContext : DbContext
{
    public LogisticaContext(DbContextOptions<LogisticaContext> options)
        : base(options)
    {
    }


    public virtual DbSet<Conductor> Conductor { get; set; }
    public virtual DbSet<GuiaRemisionConductor> ConductorGuids { get; set; }
    public virtual DbSet<GuiaRemisionDatoEnvio> DatosEnvios { get; set; }
    public virtual DbSet<GuiaRemisionDestino> GuiaRemisionDestino { get; set; }
    public virtual DbSet<GuiaRemisionDocumentoRelacionado> DocRelacionados { get; set; }
    public virtual DbSet<GuiaRemisionEmailDestinatario> EmailDestinatarioGuids { get; set; }
    public virtual DbSet<EmailRemiteTransportistaGuid> EmailRemiteTransportistaGuids { get; set; }
    public virtual DbSet<GuiaRemisionDetalle> GuiaRemisionDetalle { get; set; }
    public virtual DbSet<GuiaRemision> GuiaRemision { get; set; }
    public virtual DbSet<GuiaRemisionOrigen> GuiaRemisionOrigen { get; set; }
    public virtual DbSet<Proveedor> Proveedor { get; set; }
    public virtual DbSet<Transportista> Transportista { get; set; }
    public virtual DbSet<TransportUnit> UnidadTransporte { get; set; }
    public virtual DbSet<GuiaRemisionUnidadTransporte> UnidadTransporteGuids { get; set; }
    public virtual DbSet<Ubigeo> Ubigeo { get; set; }
    public virtual DbSet<UnitMeasurement> UnidadMedida { get; set; }
    public virtual DbSet<VentaConfig> VentaConfig { get; set; }
    public virtual DbSet<EntityBranch> Establecimiento { get; set; }
    public virtual DbSet<EntityBranchSerie> EntityBranchSeries { get; set; }
    public virtual DbSet<Perfil> Perfil { get; set; }
    public virtual DbSet<Permiso> Permiso { get; set; }
    public virtual DbSet<PerfilPermiso> PerfilPermiso { get; set; }
    public virtual DbSet<GuiaRemisionHistorial> GuiaRemisionHistorial { get; set; }
    public virtual DbSet<GuiaRemisionEstado> GuiaRemisionEstado { get; set; }
    public virtual DbSet<GuiaRemisionEstadoPermiso> GuiaRemisionEstadoPermiso { get; set; }
    public virtual DbSet<MotivoTraslado> MotivoTraslado { get; set; }
    public virtual DbSet<Persona> Persona { get; set; }
    public virtual DbSet<OperacionResponsable> OperacionResponsable { get; set; }
    public virtual DbSet<IntegrationCredential> IntegrationCredentials { get; set; }
    public virtual DbSet<GuiaRemisionTransportUnit> GuiaRemisionTransportUnits { get; set; }
    public virtual DbSet<GuiaRemisionEntityCarrier> GuiaRemisionEntityCarriers { get; set; }
    public virtual DbSet<SecurityPerson> SecurityPersons { get; set; }

    // Catalogos
    public virtual DbSet<Area> Area { get; set; }
    public virtual DbSet<TipoDocumento> TipoDocumento { get; set; }
    public virtual DbSet<EntidadReguladora> EntidadReguladora { get; set; }
    public virtual DbSet<BienNormalizado> BienNormalizado { get; set; }
    public virtual DbSet<DocumentoRelacionado> DocumentoRelacionado { get; set; }
    public virtual DbSet<GuiaRemisionIndicadorTraslado> GuiaRemisionIndicadorTraslado { get; set; }
    public virtual DbSet<Entity> Entities { get; set; }
    public virtual DbSet<EntityRole> EntityRoles { get; set; }
    public virtual DbSet<Country> Countries { get; set; }
    public virtual DbSet<InvoiceType> InvoiceTypes { get; set; }
    public virtual DbSet<SecurityPersonEntityBranch> SecurityPersonEntityBranchs { get; set; }
    public virtual DbSet<SecurityPersonEntityBranchSerie> SecurityPersonEntityBranchSeries { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder
            .EnableSensitiveDataLogging()
            .LogTo(Console.WriteLine, LogLevel.Information);

    }


    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder
            .UseCollation("utf8mb4_0900_ai_ci")
            .HasCharSet("utf8mb4");

        modelBuilder.Entity<Permiso>()
        .HasMany(p => p.Perfiles)
        .WithMany(p => p.Permisos)
        .UsingEntity<PerfilPermiso>(
            j => j
                .HasOne(pp => pp.Perfil)
                .WithMany(p => p.PerfilPermisos)
                .HasForeignKey(pp => pp.PerfilId),
            j => j
                .HasOne(pp => pp.Permiso)
                .WithMany(p => p.PerfilPermisos)
                .HasForeignKey(pp => pp.PermisoId),
            j =>
            {
                j.HasKey(pp => new { pp.PermisoId, pp.PerfilId });
                j.ToTable("perfil_permiso");
            }
        );

        modelBuilder.Entity<EntityRole>()
        .HasKey(pp => new { pp.EntityId, pp.Role });

        modelBuilder.Entity<GuiaRemisionTransportUnit>()
        .HasKey(pp => new { pp.GuiaRemisionId, pp.TransportUnitId });

        modelBuilder.Entity<GuiaRemisionEntityCarrier>()
        .HasKey(pp => new { pp.GuiaRemisionId, pp.EntityId });

        modelBuilder.Entity<SecurityPersonEntityBranch>()
        .HasKey(pp => new { pp.SecurityPersonId, pp.EntityBranchId });

        modelBuilder.Entity<SecurityPersonEntityBranchSerie>()
        .HasKey(pp => new { pp.SecurityPersonId, pp.EntityBranchSerieId });

        modelBuilder.Entity<GuiaRemisionEstadoPermiso>()
        .HasKey(pp => new { pp.GuiaRemisionEstadoId, pp.GuiaRemisionEstadoIdPermitido });

        modelBuilder.Entity<GuiaRemisionEstadoPermiso>()
            .HasOne(pp => pp.EstadoOrigen)
            .WithMany(e => e.EstadosPermitidosDesde)
            .HasForeignKey(pp => pp.GuiaRemisionEstadoId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<GuiaRemisionEstadoPermiso>()
            .HasOne(pp => pp.EstadoDestino)
            .WithMany(e => e.EstadosPermitidosHacia)
            .HasForeignKey(pp => pp.GuiaRemisionEstadoIdPermitido)
            .OnDelete(DeleteBehavior.Restrict);

    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
