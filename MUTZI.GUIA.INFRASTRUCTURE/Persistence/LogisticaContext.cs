using Microsoft.EntityFrameworkCore;
using MUTZI.GUIA.DOMAIN.Entities;

namespace MUTZI.GUIA.INFRASTRUCTURE.Persistence;

public partial class LogisticaContext : DbContext
{
    public LogisticaContext(DbContextOptions<LogisticaContext> options)
        : base(options)
    {
    }


    public virtual DbSet<Conductor> Conductor { get; set; }
    public virtual DbSet<ConductorGuid> ConductorGuids { get; set; }
    public virtual DbSet<GuiaRemisionDatoEnvio> DatosEnvios { get; set; }
    public virtual DbSet<GuiaRemisionDestino> GuiaRemisionDestino { get; set; }
    public virtual DbSet<DocRelacionado> DocRelacionados { get; set; }
    public virtual DbSet<EmailDestinatarioGuid> EmailDestinatarioGuids { get; set; }
    public virtual DbSet<EmailRemiteTransportistaGuid> EmailRemiteTransportistaGuids { get; set; }
    public virtual DbSet<GuiaRemisionDetalle> GuiaRemisionDetalle { get; set; }
    public virtual DbSet<GuiaRemision> GuiaRemision { get; set; }
    public virtual DbSet<GuiaRemisionOrigen> GuiaRemisionOrigen { get; set; }
    public virtual DbSet<Proveedor> Proveedor { get; set; }
    public virtual DbSet<Transportista> Transportista { get; set; }
    public virtual DbSet<UnidadTransporte> UnidadTransporte { get; set; }
    public virtual DbSet<UnidadTransporteGuid> UnidadTransporteGuids { get; set; }
    public virtual DbSet<Ubigeo> Ubigeo { get; set; }
    public virtual DbSet<UnidadMedida> UnidadMedida { get; set; }
    public virtual DbSet<VentaConfig> VentaConfig { get; set; }
    public virtual DbSet<Establecimiento> Establecimiento { get; set; }
    public virtual DbSet<Perfil> Perfil { get; set; }
    public virtual DbSet<Permiso> Permiso { get; set; }
    public virtual DbSet<PerfilPermiso> PerfilPermiso { get; set; }
    public virtual DbSet<GuiaRemisionHistorial> GuiaRemisionHistorial { get; set; }
    public virtual DbSet<GuiaRemisionEstado> GuiaRemisionEstado { get; set; }
    public virtual DbSet<GuiaRemisionEstadoPermiso> GuiaRemisionEstadoPermiso { get; set; }
    public virtual DbSet<Area> Area { get; set; }
    public virtual DbSet<MotivoTraslado> MotivoTraslado { get; set; }

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
