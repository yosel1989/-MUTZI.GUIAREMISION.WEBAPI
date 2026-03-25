using System;
using System.Collections.Generic;
using MUTZI.GUIA.INFRASTRUCTURE;
using Microsoft.EntityFrameworkCore;
using Pomelo.EntityFrameworkCore.MySql.Scaffolding.Internal;
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

    public virtual DbSet<Transporte> Transportes { get; set; }

    public virtual DbSet<TransporteGuid> TransporteGuids { get; set; }

    public virtual DbSet<Ubigeo> Ubigeos { get; set; }

    public virtual DbSet<UnidadMedidum> UnidadMedida { get; set; }

    public virtual DbSet<VentaConfig> VentaConfigs { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseMySql("server=35.171.241.179;port=3306;database=logistica;user=usrlogistica;password=teB029#6;allowpublickeyretrieval=True;sslmode=Preferred", Microsoft.EntityFrameworkCore.ServerVersion.Parse("8.0.45-mysql"));

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder
            .UseCollation("utf8mb4_0900_ai_ci")
            .HasCharSet("utf8mb4");

        modelBuilder.Entity<Conductor>(entity =>
        {
            entity.HasKey(e => e.ConductorId).HasName("PRIMARY");

            entity.ToTable("conductor");

            entity.HasIndex(e => e.Licencia, "licencia_UNIQUE").IsUnique();

            entity.HasIndex(e => e.NumeroDocumento, "numeroDocumento_BUSQUEDA").IsUnique();

            entity.Property(e => e.ConductorId).HasColumnName("conductorID");
            entity.Property(e => e.Apellidos)
                .HasMaxLength(50)
                .HasColumnName("apellidos");
            entity.Property(e => e.Cargo)
                .HasMaxLength(100)
                .HasColumnName("cargo");
            entity.Property(e => e.EmpleadoIdcreacion).HasColumnName("empleadoIDCreacion");
            entity.Property(e => e.EmpleadoIdedicion).HasColumnName("empleadoIDEdicion");
            entity.Property(e => e.EmpleadoNombreCreacion)
                .HasMaxLength(100)
                .HasColumnName("empleadoNombreCreacion");
            entity.Property(e => e.EmpleadoNombreEdicion)
                .HasMaxLength(100)
                .HasColumnName("empleadoNombreEdicion");
            entity.Property(e => e.Estado)
                .HasDefaultValueSql("'activo'")
                .HasColumnType("enum('activo','inactivo')")
                .HasColumnName("estado");
            entity.Property(e => e.FechaCreacion)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("datetime")
                .HasColumnName("fechaCreacion");
            entity.Property(e => e.FechaEdicion)
                .HasColumnType("datetime")
                .HasColumnName("fechaEdicion");
            entity.Property(e => e.IdEstado)
                .HasDefaultValueSql("'1'")
                .HasColumnName("idEstado");
            entity.Property(e => e.Licencia)
                .HasMaxLength(20)
                .HasColumnName("licencia");
            entity.Property(e => e.Nombres)
                .HasMaxLength(50)
                .HasColumnName("nombres");
            entity.Property(e => e.NumeroDocumento)
                .HasMaxLength(15)
                .HasColumnName("numeroDocumento");
            entity.Property(e => e.TipoDocumento)
                .HasMaxLength(20)
                .HasColumnName("tipoDocumento");
        });

        modelBuilder.Entity<ConductorGuid>(entity =>
        {
            entity.HasKey(e => e.ConductorGuidId).HasName("PRIMARY");

            entity.ToTable("conductorGuid");

            entity.HasIndex(e => e.ConductorId, "conductorID");

            entity.HasIndex(e => e.DatosEnvioId, "datosEnvioID");

            entity.Property(e => e.ConductorGuidId).HasColumnName("conductorGuidID");
            entity.Property(e => e.ConductorId).HasColumnName("conductorID");
            entity.Property(e => e.DatosEnvioId).HasColumnName("datosEnvioID");

            entity.HasOne(d => d.Conductor).WithMany(p => p.ConductorGuids)
                .HasForeignKey(d => d.ConductorId)
                .HasConstraintName("conductorGuid_ibfk_2");

            entity.HasOne(d => d.DatosEnvio).WithMany(p => p.ConductorGuids)
                .HasForeignKey(d => d.DatosEnvioId)
                .HasConstraintName("conductorGuid_ibfk_1");
        });

        modelBuilder.Entity<DatosEnvio>(entity =>
        {
            entity.HasKey(e => e.DatosEnvioId).HasName("PRIMARY");

            entity.ToTable("datosEnvio");

            entity.HasIndex(e => e.CodigoUm, "codigoUM");

            entity.HasIndex(e => e.GuiaId, "guiaID");

            entity.Property(e => e.DatosEnvioId).HasColumnName("datosEnvioID");
            entity.Property(e => e.CodigoUm)
                .HasMaxLength(10)
                .HasColumnName("codigoUM");
            entity.Property(e => e.FechaEnvio).HasColumnName("fechaEnvio");
            entity.Property(e => e.GuiaId).HasColumnName("guiaID");
            entity.Property(e => e.MotivoEnvio)
                .HasMaxLength(100)
                .HasColumnName("motivoEnvio");
            entity.Property(e => e.PesoBruto)
                .HasPrecision(12, 3)
                .HasColumnName("pesoBruto");
            entity.Property(e => e.RazonSocialCurrier)
                .HasMaxLength(100)
                .HasColumnName("razonSocialCurrier");
            entity.Property(e => e.RegistroMtcCurrier)
                .HasMaxLength(20)
                .HasColumnName("registroMtcCurrier");
            entity.Property(e => e.RucEmpresaCurrier)
                .HasMaxLength(15)
                .HasColumnName("rucEmpresaCurrier");

            entity.HasOne(d => d.CodigoUmNavigation).WithMany(p => p.DatosEnvios)
                .HasForeignKey(d => d.CodigoUm)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("datosEnvio_ibfk_4");

            entity.HasOne(d => d.Guia).WithMany(p => p.DatosEnvios)
                .HasForeignKey(d => d.GuiaId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("datosEnvio_ibfk_1");
        });

        modelBuilder.Entity<Destinatario>(entity =>
        {
            entity.HasKey(e => e.DestinatarioId).HasName("PRIMARY");

            entity.ToTable("destinatario");

            entity.HasIndex(e => e.NumeroDocumento, "numeroDocumento_BUSQUEDA").IsUnique();

            entity.HasIndex(e => e.UbigeoId, "ubigeoID");

            entity.Property(e => e.DestinatarioId).HasColumnName("destinatarioID");
            entity.Property(e => e.CodigoSunat)
                .HasMaxLength(4)
                .HasColumnName("codigoSunat");
            entity.Property(e => e.Direccion)
                .HasMaxLength(250)
                .HasColumnName("direccion");
            entity.Property(e => e.EmpleadoIdcreacion).HasColumnName("empleadoIDCreacion");
            entity.Property(e => e.EmpleadoIdedicion).HasColumnName("empleadoIDEdicion");
            entity.Property(e => e.EmpleadoNombreCreacion)
                .HasMaxLength(100)
                .HasColumnName("empleadoNombreCreacion");
            entity.Property(e => e.EmpleadoNombreEdicion)
                .HasMaxLength(100)
                .HasColumnName("empleadoNombreEdicion");
            entity.Property(e => e.Estado)
                .HasDefaultValueSql("'activo'")
                .HasColumnType("enum('activo','inactivo')")
                .HasColumnName("estado");
            entity.Property(e => e.FechaCreacion)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("datetime")
                .HasColumnName("fechaCreacion");
            entity.Property(e => e.FechaEdicion)
                .HasColumnType("datetime")
                .HasColumnName("fechaEdicion");
            entity.Property(e => e.IdEstado)
                .HasDefaultValueSql("'1'")
                .HasColumnName("idEstado");
            entity.Property(e => e.NumeroDocumento)
                .HasMaxLength(15)
                .HasColumnName("numeroDocumento");
            entity.Property(e => e.Pais)
                .HasMaxLength(3)
                .HasColumnName("pais");
            entity.Property(e => e.RazonSocial)
                .HasMaxLength(250)
                .HasColumnName("razonSocial");
            entity.Property(e => e.TipoDocumento)
                .HasMaxLength(20)
                .HasColumnName("tipoDocumento");
            entity.Property(e => e.UbigeoId)
                .HasMaxLength(6)
                .HasColumnName("ubigeoID");

            entity.HasOne(d => d.Ubigeo).WithMany(p => p.Destinatarios)
                .HasForeignKey(d => d.UbigeoId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("destinatario_ibfk_1");
        });

        modelBuilder.Entity<Destino>(entity =>
        {
            entity.HasKey(e => e.DestinoId).HasName("PRIMARY");

            entity.ToTable("destino");

            entity.HasIndex(e => e.GuiaId, "guiaID");

            entity.HasIndex(e => e.UbigeoId, "ubigeoID");

            entity.Property(e => e.DestinoId).HasColumnName("destinoID");
            entity.Property(e => e.Direccion)
                .HasMaxLength(250)
                .HasColumnName("direccion");
            entity.Property(e => e.GuiaId).HasColumnName("guiaID");
            entity.Property(e => e.Pais)
                .HasMaxLength(3)
                .HasColumnName("pais");
            entity.Property(e => e.UbigeoId)
                .HasMaxLength(6)
                .HasColumnName("ubigeoID");

            entity.HasOne(d => d.Guia).WithMany(p => p.Destinos)
                .HasForeignKey(d => d.GuiaId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("destino_ibfk_1");

            entity.HasOne(d => d.Ubigeo).WithMany(p => p.Destinos)
                .HasForeignKey(d => d.UbigeoId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("destino_ibfk_2");
        });

        modelBuilder.Entity<DocRelacionado>(entity =>
        {
            entity.HasKey(e => e.DocRelacionadoId).HasName("PRIMARY");

            entity.ToTable("docRelacionado");

            entity.HasIndex(e => e.GuiaId, "guiaID");

            entity.Property(e => e.DocRelacionadoId).HasColumnName("docRelacionadoID");
            entity.Property(e => e.GuiaId).HasColumnName("guiaID");
            entity.Property(e => e.NumeroDocRef)
                .HasMaxLength(30)
                .HasColumnName("numeroDocRef");
            entity.Property(e => e.RucDocRef)
                .HasMaxLength(12)
                .HasColumnName("rucDocRef");
            entity.Property(e => e.TipoDocRef)
                .HasMaxLength(50)
                .HasColumnName("tipoDocRef");

            entity.HasOne(d => d.Guia).WithMany(p => p.DocRelacionados)
                .HasForeignKey(d => d.GuiaId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("docRelacionado_ibfk_1");
        });

        modelBuilder.Entity<EmailDestinatarioGuid>(entity =>
        {
            entity.HasKey(e => e.EmailDestinatarioGuidId).HasName("PRIMARY");

            entity.ToTable("emailDestinatarioGuid");

            entity.HasIndex(e => e.DestinatarioId, "destinatarioID");

            entity.HasIndex(e => e.GuiaId, "guiaID");

            entity.Property(e => e.EmailDestinatarioGuidId).HasColumnName("emailDestinatarioGuidID");
            entity.Property(e => e.DestinatarioId).HasColumnName("destinatarioID");
            entity.Property(e => e.Email)
                .HasMaxLength(50)
                .HasColumnName("email");
            entity.Property(e => e.GuiaId).HasColumnName("guiaID");

            entity.HasOne(d => d.Destinatario).WithMany(p => p.EmailDestinatarioGuids)
                .HasForeignKey(d => d.DestinatarioId)
                .HasConstraintName("emailDestinatarioGuid_ibfk_2");

            entity.HasOne(d => d.Guia).WithMany(p => p.EmailDestinatarioGuids)
                .HasForeignKey(d => d.GuiaId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("emailDestinatarioGuid_ibfk_1");
        });

        modelBuilder.Entity<EmailRemiteTransportistaGuid>(entity =>
        {
            entity.HasKey(e => e.EmailRemiteTransportistaGuidId).HasName("PRIMARY");

            entity.ToTable("emailRemiteTransportistaGuid");

            entity.HasIndex(e => e.GuiaId, "guiaID");

            entity.HasIndex(e => e.RemiteTransportistaId, "remiteTransportistaID");

            entity.Property(e => e.EmailRemiteTransportistaGuidId).HasColumnName("emailRemiteTransportistaGuidID");
            entity.Property(e => e.Email)
                .HasMaxLength(50)
                .HasColumnName("email");
            entity.Property(e => e.GuiaId).HasColumnName("guiaID");
            entity.Property(e => e.RemiteTransportistaId).HasColumnName("remiteTransportistaID");

            entity.HasOne(d => d.Guia).WithMany(p => p.EmailRemiteTransportistaGuids)
                .HasForeignKey(d => d.GuiaId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("emailRemiteTransportistaGuid_ibfk_1");

            entity.HasOne(d => d.RemiteTransportista).WithMany(p => p.EmailRemiteTransportistaGuids)
                .HasForeignKey(d => d.RemiteTransportistaId)
                .HasConstraintName("emailRemiteTransportistaGuid_ibfk_2");
        });

        modelBuilder.Entity<GuiaDetalle>(entity =>
        {
            entity.HasKey(e => e.GuiaDetalleId).HasName("PRIMARY");

            entity.ToTable("guiaDetalle");

            entity.HasIndex(e => e.CodigoUm, "codigoUM");

            entity.HasIndex(e => e.GuiaId, "guiaID");

            entity.Property(e => e.GuiaDetalleId).HasColumnName("guiaDetalleID");
            entity.Property(e => e.Cantidad)
                .HasMaxLength(10)
                .HasColumnName("cantidad");
            entity.Property(e => e.Codigo)
                .HasMaxLength(20)
                .HasColumnName("codigo");
            entity.Property(e => e.CodigoUm)
                .HasMaxLength(10)
                .HasColumnName("codigoUM");
            entity.Property(e => e.Descripcion)
                .HasMaxLength(100)
                .HasColumnName("descripcion");
            entity.Property(e => e.GuiaId).HasColumnName("guiaID");

            entity.HasOne(d => d.CodigoUmNavigation).WithMany(p => p.GuiaDetalles)
                .HasForeignKey(d => d.CodigoUm)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("guiaDetalle_ibfk_1");

            entity.HasOne(d => d.Guia).WithMany(p => p.GuiaDetalles)
                .HasForeignKey(d => d.GuiaId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("guiaDetalle_ibfk_2");
        });

        modelBuilder.Entity<GuiaRemision>(entity =>
        {
            entity.HasKey(e => e.GuiaId).HasName("PRIMARY");

            entity.ToTable("guiaRemision");

            entity.HasIndex(e => e.DestinatarioId, "destinatarioID");

            entity.HasIndex(e => e.Ruc, "guia_ruc_BUSQUEDA");

            entity.HasIndex(e => e.NumeroGuia, "numeroGuia").IsUnique();

            entity.HasIndex(e => e.ProveedorId, "proveedorID");

            entity.HasIndex(e => e.RemiteTransportistaId, "remiteTransportistaID");

            entity.HasIndex(e => e.RemitenteId, "remitenteID");

            entity.Property(e => e.GuiaId).HasColumnName("guiaID");
            entity.Property(e => e.Correlativo)
                .HasMaxLength(8)
                .HasColumnName("correlativo");
            entity.Property(e => e.DestinatarioId).HasColumnName("destinatarioID");
            entity.Property(e => e.EmpleadoIdcreacion).HasColumnName("empleadoIDCreacion");
            entity.Property(e => e.EmpleadoIdedicion).HasColumnName("empleadoIDEdicion");
            entity.Property(e => e.EmpleadoNombreCreacion)
                .HasMaxLength(100)
                .HasColumnName("empleadoNombreCreacion");
            entity.Property(e => e.EmpleadoNombreEdicion)
                .HasMaxLength(100)
                .HasColumnName("empleadoNombreEdicion");
            entity.Property(e => e.Estado)
                .HasMaxLength(50)
                .HasColumnName("estado");
            entity.Property(e => e.Fecha).HasColumnName("fecha");
            entity.Property(e => e.FechaCreacion)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("datetime")
                .HasColumnName("fechaCreacion");
            entity.Property(e => e.FechaEdicion)
                .HasColumnType("datetime")
                .HasColumnName("fechaEdicion");
            entity.Property(e => e.Hora)
                .HasMaxLength(10)
                .HasColumnName("hora");
            entity.Property(e => e.IdEstado).HasColumnName("idEstado");
            entity.Property(e => e.NumeroGuia)
                .HasMaxLength(15)
                .HasColumnName("numeroGuia");
            entity.Property(e => e.Observacion)
                .HasMaxLength(100)
                .HasColumnName("observacion");
            entity.Property(e => e.ProveedorId).HasColumnName("proveedorID");
            entity.Property(e => e.RemiteTransportistaId).HasColumnName("remiteTransportistaID");
            entity.Property(e => e.RemitenteId).HasColumnName("remitenteID");
            entity.Property(e => e.RespuestaTicket)
                .HasMaxLength(500)
                .HasColumnName("respuestaTicket");
            entity.Property(e => e.Ruc)
                .HasMaxLength(11)
                .HasColumnName("ruc")
                .UseCollation("utf8mb4_unicode_ci");
            entity.Property(e => e.Serie)
                .HasMaxLength(4)
                .HasColumnName("serie");
            entity.Property(e => e.TipoGuia)
                .HasMaxLength(20)
                .HasColumnName("tipoGuia");
            entity.Property(e => e.TipoTransporte)
                .HasMaxLength(10)
                .HasColumnName("tipoTransporte");
            entity.Property(e => e.TipoTraslado)
                .HasMaxLength(12)
                .HasColumnName("tipoTraslado");

            entity.HasOne(d => d.Destinatario).WithMany(p => p.GuiaRemisions)
                .HasForeignKey(d => d.DestinatarioId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("guiaRemision_ibfk_2");

            entity.HasOne(d => d.Proveedor).WithMany(p => p.GuiaRemisions)
                .HasForeignKey(d => d.ProveedorId)
                .HasConstraintName("guiaRemision_ibfk_4");

            entity.HasOne(d => d.RemiteTransportista).WithMany(p => p.GuiaRemisions)
                .HasForeignKey(d => d.RemiteTransportistaId)
                .HasConstraintName("guiaRemision_ibfk_3");

            entity.HasOne(d => d.Remitente).WithMany(p => p.GuiaRemisions)
                .HasForeignKey(d => d.RemitenteId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("guiaRemision_ibfk_1");

            entity.HasOne(d => d.RucNavigation).WithMany(p => p.GuiaRemisions)
                .HasForeignKey(d => d.Ruc)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("guiaRemision_ibfk_5");
        });

        modelBuilder.Entity<Origen>(entity =>
        {
            entity.HasKey(e => e.OrigenId).HasName("PRIMARY");

            entity.ToTable("origen");

            entity.HasIndex(e => e.GuiaId, "guiaID");

            entity.HasIndex(e => e.UbigeoId, "ubigeoID");

            entity.Property(e => e.OrigenId).HasColumnName("origenID");
            entity.Property(e => e.Direccion)
                .HasMaxLength(250)
                .HasColumnName("direccion");
            entity.Property(e => e.GuiaId).HasColumnName("guiaID");
            entity.Property(e => e.Pais)
                .HasMaxLength(3)
                .HasColumnName("pais");
            entity.Property(e => e.UbigeoId)
                .HasMaxLength(6)
                .HasColumnName("ubigeoID");

            entity.HasOne(d => d.Guia).WithMany(p => p.Origens)
                .HasForeignKey(d => d.GuiaId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("origen_ibfk_1");

            entity.HasOne(d => d.Ubigeo).WithMany(p => p.Origens)
                .HasForeignKey(d => d.UbigeoId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("origen_ibfk_2");
        });

        modelBuilder.Entity<Proveedor>(entity =>
        {
            entity.HasKey(e => e.ProveedorId).HasName("PRIMARY");

            entity.ToTable("proveedor");

            entity.HasIndex(e => e.NumeroDocumento, "numeroDocumento_BUSQUEDA").IsUnique();

            entity.HasIndex(e => e.UbigeoId, "ubigeoID");

            entity.Property(e => e.ProveedorId).HasColumnName("proveedorID");
            entity.Property(e => e.CodigoSunat)
                .HasMaxLength(4)
                .HasColumnName("codigoSunat");
            entity.Property(e => e.Direccion)
                .HasMaxLength(250)
                .HasColumnName("direccion");
            entity.Property(e => e.Email)
                .HasMaxLength(50)
                .HasColumnName("email");
            entity.Property(e => e.EmpleadoIdcreacion).HasColumnName("empleadoIDCreacion");
            entity.Property(e => e.EmpleadoIdedicion).HasColumnName("empleadoIDEdicion");
            entity.Property(e => e.EmpleadoNombreCreacion)
                .HasMaxLength(100)
                .HasColumnName("empleadoNombreCreacion");
            entity.Property(e => e.EmpleadoNombreEdicion)
                .HasMaxLength(100)
                .HasColumnName("empleadoNombreEdicion");
            entity.Property(e => e.Estado)
                .HasDefaultValueSql("'activo'")
                .HasColumnType("enum('activo','inactivo')")
                .HasColumnName("estado");
            entity.Property(e => e.FechaCreacion)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("datetime")
                .HasColumnName("fechaCreacion");
            entity.Property(e => e.FechaEdicion)
                .HasColumnType("datetime")
                .HasColumnName("fechaEdicion");
            entity.Property(e => e.IdEstado)
                .HasDefaultValueSql("'1'")
                .HasColumnName("idEstado");
            entity.Property(e => e.NumeroDocumento)
                .HasMaxLength(20)
                .HasColumnName("numeroDocumento");
            entity.Property(e => e.Pais)
                .HasMaxLength(3)
                .HasColumnName("pais");
            entity.Property(e => e.RazonSocial)
                .HasMaxLength(250)
                .HasColumnName("razonSocial");
            entity.Property(e => e.TipoDocumento)
                .HasMaxLength(20)
                .HasColumnName("tipoDocumento");
            entity.Property(e => e.UbigeoId)
                .HasMaxLength(6)
                .HasColumnName("ubigeoID");

            entity.HasOne(d => d.Ubigeo).WithMany(p => p.Proveedors)
                .HasForeignKey(d => d.UbigeoId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("proveedor_ibfk_1");
        });

        modelBuilder.Entity<RemiteTransportistum>(entity =>
        {
            entity.HasKey(e => e.RemiteTransportistaId).HasName("PRIMARY");

            entity.ToTable("remiteTransportista");

            entity.HasIndex(e => e.NumeroDocumento, "numeroDocumento_UNIQUE").IsUnique();

            entity.HasIndex(e => e.UbigeoId, "ubigeoID");

            entity.Property(e => e.RemiteTransportistaId).HasColumnName("remiteTransportistaID");
            entity.Property(e => e.CodigoSunat)
                .HasMaxLength(4)
                .HasColumnName("codigoSunat");
            entity.Property(e => e.Direccion)
                .HasMaxLength(250)
                .HasColumnName("direccion");
            entity.Property(e => e.EmpleadoIdcreacion).HasColumnName("empleadoIDCreacion");
            entity.Property(e => e.EmpleadoIdedicion).HasColumnName("empleadoIDEdicion");
            entity.Property(e => e.EmpleadoNombreCreacion)
                .HasMaxLength(100)
                .HasColumnName("empleadoNombreCreacion");
            entity.Property(e => e.EmpleadoNombreEdicion)
                .HasMaxLength(100)
                .HasColumnName("empleadoNombreEdicion");
            entity.Property(e => e.Estado)
                .HasDefaultValueSql("'activo'")
                .HasColumnType("enum('activo','inactivo')")
                .HasColumnName("estado");
            entity.Property(e => e.FechaCreacion)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("datetime")
                .HasColumnName("fechaCreacion");
            entity.Property(e => e.FechaEdicion)
                .HasColumnType("datetime")
                .HasColumnName("fechaEdicion");
            entity.Property(e => e.IdEstado)
                .HasDefaultValueSql("'1'")
                .HasColumnName("idEstado");
            entity.Property(e => e.NumeroDocumento)
                .HasMaxLength(15)
                .HasColumnName("numeroDocumento");
            entity.Property(e => e.Pais)
                .HasMaxLength(3)
                .HasColumnName("pais");
            entity.Property(e => e.RazonSocial)
                .HasMaxLength(200)
                .HasColumnName("razonSocial");
            entity.Property(e => e.TipoDocumento)
                .HasMaxLength(20)
                .HasColumnName("tipoDocumento");
            entity.Property(e => e.UbigeoId)
                .HasMaxLength(6)
                .HasColumnName("ubigeoID");

            entity.HasOne(d => d.Ubigeo).WithMany(p => p.RemiteTransportista)
                .HasForeignKey(d => d.UbigeoId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("remiteTransportista_ibfk_1");
        });

        modelBuilder.Entity<Remitente>(entity =>
        {
            entity.HasKey(e => e.RemitenteId).HasName("PRIMARY");

            entity.ToTable("remitente");

            entity.HasIndex(e => e.Descripcion, "razonSocial_UNIQUE").IsUnique();

            entity.HasIndex(e => e.Ruc, "remitente_ibfk_2_idx");

            entity.HasIndex(e => e.UbigeoId, "ubigeoID");

            entity.Property(e => e.RemitenteId).HasColumnName("remitenteID");
            entity.Property(e => e.CodigoSunat)
                .HasMaxLength(4)
                .HasColumnName("codigoSunat");
            entity.Property(e => e.Descripcion)
                .HasMaxLength(200)
                .HasColumnName("descripcion");
            entity.Property(e => e.Direccion)
                .HasMaxLength(250)
                .HasColumnName("direccion");
            entity.Property(e => e.Email)
                .HasMaxLength(100)
                .HasColumnName("email");
            entity.Property(e => e.EmpleadoIdcreacion).HasColumnName("empleadoIDCreacion");
            entity.Property(e => e.EmpleadoIdedicion).HasColumnName("empleadoIDEdicion");
            entity.Property(e => e.EmpleadoNombreCreacion)
                .HasMaxLength(100)
                .HasColumnName("empleadoNombreCreacion");
            entity.Property(e => e.EmpleadoNombreEdicion)
                .HasMaxLength(100)
                .HasColumnName("empleadoNombreEdicion");
            entity.Property(e => e.Estado)
                .HasDefaultValueSql("'activo'")
                .HasColumnType("enum('activo','inactivo')")
                .HasColumnName("estado");
            entity.Property(e => e.FechaCreacion)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("datetime")
                .HasColumnName("fechaCreacion");
            entity.Property(e => e.FechaEdicion)
                .HasColumnType("datetime")
                .HasColumnName("fechaEdicion");
            entity.Property(e => e.IdEstado)
                .HasDefaultValueSql("'1'")
                .HasColumnName("idEstado");
            entity.Property(e => e.Pais)
                .HasMaxLength(3)
                .HasColumnName("pais");
            entity.Property(e => e.Ruc)
                .HasMaxLength(11)
                .HasColumnName("ruc")
                .UseCollation("utf8mb4_unicode_ci");
            entity.Property(e => e.Serie)
                .HasMaxLength(4)
                .HasColumnName("serie");
            entity.Property(e => e.UbigeoId)
                .HasMaxLength(6)
                .HasColumnName("ubigeoID");

            entity.HasOne(d => d.RucNavigation).WithMany(p => p.Remitentes)
                .HasForeignKey(d => d.Ruc)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("remitente_ibfk_2");

            entity.HasOne(d => d.Ubigeo).WithMany(p => p.Remitentes)
                .HasForeignKey(d => d.UbigeoId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("remitente_ibfk_1");
        });

        modelBuilder.Entity<Transporte>(entity =>
        {
            entity.HasKey(e => e.TransporteId).HasName("PRIMARY");

            entity.ToTable("transporte");

            entity.HasIndex(e => e.Placa, "placa_UNIQUE").IsUnique();

            entity.HasIndex(e => e.Tarjeta, "tarjeta_UNIQUE").IsUnique();

            entity.Property(e => e.TransporteId).HasColumnName("transporteID");
            entity.Property(e => e.Descripcion)
                .HasMaxLength(50)
                .HasColumnName("descripcion");
            entity.Property(e => e.EmpleadoIdcreacion).HasColumnName("empleadoIDCreacion");
            entity.Property(e => e.EmpleadoIdedicion).HasColumnName("empleadoIDEdicion");
            entity.Property(e => e.EmpleadoNombreCreacion)
                .HasMaxLength(100)
                .HasColumnName("empleadoNombreCreacion");
            entity.Property(e => e.EmpleadoNombreEdicion)
                .HasMaxLength(100)
                .HasColumnName("empleadoNombreEdicion");
            entity.Property(e => e.Estado)
                .HasDefaultValueSql("'activo'")
                .HasColumnType("enum('activo','inactivo')")
                .HasColumnName("estado");
            entity.Property(e => e.FechaCreacion)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("datetime")
                .HasColumnName("fechaCreacion");
            entity.Property(e => e.FechaEdicion)
                .HasColumnType("datetime")
                .HasColumnName("fechaEdicion");
            entity.Property(e => e.IdEstado)
                .HasDefaultValueSql("'1'")
                .HasColumnName("idEstado");
            entity.Property(e => e.Marca)
                .HasMaxLength(20)
                .HasColumnName("marca");
            entity.Property(e => e.Modelo)
                .HasMaxLength(20)
                .HasColumnName("modelo");
            entity.Property(e => e.NumeroRegistroMtc)
                .HasMaxLength(20)
                .HasColumnName("numeroRegistroMTC");
            entity.Property(e => e.Placa)
                .HasMaxLength(20)
                .HasColumnName("placa");
            entity.Property(e => e.Tarjeta)
                .HasMaxLength(20)
                .HasColumnName("tarjeta");
        });

        modelBuilder.Entity<TransporteGuid>(entity =>
        {
            entity.HasKey(e => e.TransporteGuidId).HasName("PRIMARY");

            entity.ToTable("transporteGuid");

            entity.HasIndex(e => e.DatosEnvioId, "datosEnvioID");

            entity.HasIndex(e => e.TransporteId, "transporteID");

            entity.Property(e => e.TransporteGuidId).HasColumnName("transporteGuidID");
            entity.Property(e => e.DatosEnvioId).HasColumnName("datosEnvioID");
            entity.Property(e => e.TransporteId).HasColumnName("transporteID");

            entity.HasOne(d => d.DatosEnvio).WithMany(p => p.TransporteGuids)
                .HasForeignKey(d => d.DatosEnvioId)
                .HasConstraintName("transporteGuid_ibfk_1");

            entity.HasOne(d => d.Transporte).WithMany(p => p.TransporteGuids)
                .HasForeignKey(d => d.TransporteId)
                .HasConstraintName("transporteGuid_ibfk_2");
        });

        modelBuilder.Entity<Ubigeo>(entity =>
        {
            entity.HasKey(e => e.UbigeoId).HasName("PRIMARY");

            entity
                .ToTable("ubigeo")
                .UseCollation("utf8mb4_unicode_ci");

            entity.HasIndex(e => e.UbigeoId, "ID_BUSQUEDA");

            entity.Property(e => e.UbigeoId)
                .HasMaxLength(6)
                .HasColumnName("ubigeoID")
                .UseCollation("utf8mb4_0900_ai_ci");
            entity.Property(e => e.Departamento)
                .HasMaxLength(100)
                .HasColumnName("departamento")
                .UseCollation("utf8mb4_0900_ai_ci");
            entity.Property(e => e.Distrito)
                .HasMaxLength(100)
                .HasColumnName("distrito")
                .UseCollation("utf8mb4_0900_ai_ci");
            entity.Property(e => e.Provincia)
                .HasMaxLength(100)
                .HasColumnName("provincia")
                .UseCollation("utf8mb4_0900_ai_ci");
        });

        modelBuilder.Entity<UnidadMedidum>(entity =>
        {
            entity.HasKey(e => e.CodigoUm).HasName("PRIMARY");

            entity.ToTable("unidadMedida");

            entity.Property(e => e.CodigoUm)
                .HasMaxLength(10)
                .HasColumnName("codigoUM");
            entity.Property(e => e.Descripcion)
                .HasMaxLength(100)
                .HasColumnName("descripcion");
            entity.Property(e => e.EsUnidadConteo)
                .HasDefaultValueSql("'0'")
                .HasColumnName("es_unidad_conteo");
            entity.Property(e => e.EsUnidadLongitud)
                .HasDefaultValueSql("'0'")
                .HasColumnName("es_unidad_longitud");
            entity.Property(e => e.EsUnidadPeso)
                .HasDefaultValueSql("'0'")
                .HasColumnName("es_unidad_peso");
            entity.Property(e => e.EsUnidadVolumen)
                .HasDefaultValueSql("'0'")
                .HasColumnName("es_unidad_volumen");
            entity.Property(e => e.Estado)
                .HasMaxLength(20)
                .HasDefaultValueSql("'activo'")
                .HasColumnName("estado");
            entity.Property(e => e.IdEstado)
                .HasDefaultValueSql("'1'")
                .HasColumnName("idEstado");
        });

        modelBuilder.Entity<VentaConfig>(entity =>
        {
            entity.HasKey(e => e.Ruc).HasName("PRIMARY");

            entity
                .ToTable("VentaConfig")
                .UseCollation("utf8mb4_unicode_ci");

            entity.HasIndex(e => e.Ruc, "Ruc_UNIQUE").IsUnique();

            entity.Property(e => e.Ruc).HasMaxLength(11);
            entity.Property(e => e.CodigoSunat)
                .HasMaxLength(4)
                .HasColumnName("CodigoSUNAT");
            entity.Property(e => e.DireccionFiscal)
                .HasMaxLength(100)
                .HasColumnName("direccionFiscal");
            entity.Property(e => e.EfactApi)
                .HasMaxLength(256)
                .HasColumnName("Efact_API");
            entity.Property(e => e.EmailPrincipal)
                .HasMaxLength(50)
                .HasColumnName("emailPrincipal");
            entity.Property(e => e.Empresa).HasMaxLength(64);
            entity.Property(e => e.RetencionCuenta)
                .HasMaxLength(50)
                .HasColumnName("retencionCuenta");
            entity.Property(e => e.RetencionDescripcion)
                .HasMaxLength(100)
                .HasColumnName("retencionDescripcion");
            entity.Property(e => e.VentaConfigId)
                .HasMaxLength(36)
                .HasColumnName("ventaConfigID");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
