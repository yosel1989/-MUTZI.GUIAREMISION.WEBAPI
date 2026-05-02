using Microsoft.EntityFrameworkCore;
using MUTZI.GUIA.DOMAIN.Entities;

namespace MUTZI.GUIA.INFRASTRUCTURE.Persistence;

public partial class LogisticaContext : DbContext
{
    public LogisticaContext()
    {
    }

    public LogisticaContext(DbContextOptions<LogisticaContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Conductor> Conductors { get; set; }

    public virtual DbSet<ConductorGuid> ConductorGuids { get; set; }

    public virtual DbSet<DatosEnvio> DatosEnvios { get; set; }

    public virtual DbSet<Destinatario> Destinatarios { get; set; }

    public virtual DbSet<Destino> Destinos { get; set; }

    public virtual DbSet<DocRelacionado> DocRelacionados { get; set; }

    public virtual DbSet<EmailDestinatarioGuid> EmailDestinatarioGuids { get; set; }

    public virtual DbSet<EmailRemiteTransportistaGuid> EmailRemiteTransportistaGuids { get; set; }

    public virtual DbSet<GuiaDetalle> GuiaDetalles { get; set; }

    public virtual DbSet<GuiaRemision> GuiaRemisions { get; set; }

    public virtual DbSet<Origen> Origens { get; set; }

    public virtual DbSet<Proveedor> Proveedors { get; set; }

    public virtual DbSet<RemiteTransportistum> RemiteTransportista { get; set; }

    public virtual DbSet<Remitente> Remitentes { get; set; }

    public virtual DbSet<UnidadTransporte> UnidadTransportes { get; set; }

    public virtual DbSet<UnidadTransporteGuid> UnidadTransporteGuids { get; set; }

    public virtual DbSet<Ubigeo> Ubigeos { get; set; }

    public virtual DbSet<UnidadMedidum> UnidadMedida { get; set; }

    public virtual DbSet<VentaConfig> VentaConfigs { get; set; }

    public virtual DbSet<Establecimiento> Establecimiento { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder
            .UseCollation("utf8mb4_0900_ai_ci")
            .HasCharSet("utf8mb4");

        modelBuilder.Entity<GuiaRemision>(entity =>
        {
            entity.HasOne(g => g.EstablecimientoRemitente)
                  .WithMany(e => e.GuiaRemisionsRemitente)
                  .HasForeignKey(g => g.EstablecimientoRemitenteId)
                  .OnDelete(DeleteBehavior.ClientSetNull);

            entity.HasOne(g => g.EstablecimientoDestinatario)
                  .WithMany(e => e.GuiaRemisionsDestinatario)
                  .HasForeignKey(g => g.EstablecimientoDestinatarioId)
                  .OnDelete(DeleteBehavior.ClientSetNull);
        });

        modelBuilder.Entity<EmailDestinatarioGuid>()
            .HasOne(g => g.EstablecimientoDestinatario);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
