using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MUTZI.GUIA.DOMAIN.Entities;

[Table("destinatario")]
public partial class Destinatario
{
    [Key]
    [Column("destinatarioID")]
    public int DestinatarioId { get; set; }

    [Column("tipoDocumento")]
    [MaxLength(20)]
    public string TipoDocumento { get; set; } = null!;

    [Column("numeroDocumento")]
    [MaxLength(15)]
    public string NumeroDocumento { get; set; } = null!;

    [Column("razonSocial")]
    [MaxLength(250)]
    public string RazonSocial { get; set; } = null!;

    [Column("ubigeoID")]
    [MaxLength(6)]
    public string? UbigeoId { get; set; }

    [Column("direccion")]
    [MaxLength(250)]
    public string? Direccion { get; set; }

    [Column("pais")]
    [MaxLength(3)]
    public string? Pais { get; set; }

    [Column("codigoSunat")]
    [MaxLength(4)]
    public string? CodigoSunat { get; set; }

    [Column("fechaCreacion")]
    public DateTime? FechaCreacion { get; set; }

    [Column("fechaEdicion")]
    public DateTime? FechaEdicion { get; set; }

    [Column("empleadoIDCreacion")]
    public int? EmpleadoIdcreacion { get; set; }

    [Column("empleadoNombreCreacion")]
    [MaxLength(100)]
    public string? EmpleadoNombreCreacion { get; set; }

    [Column("empleadoIDEdicion")]
    public int? EmpleadoIdedicion { get; set; }

    [Column("empleadoNombreEdicion")]
    [MaxLength(100)]
    public string? EmpleadoNombreEdicion { get; set; }

    [Column("estado")]
    public string? Estado { get; set; } = "activo";

    [Column("idEstado")]
    public sbyte? IdEstado { get; set; } = 1;

    [ForeignKey(nameof(UbigeoId))]
    public virtual Ubigeo? Ubigeo { get; set; }

    public virtual ICollection<EmailDestinatarioGuid> EmailDestinatarioGuids { get; set; } = new List<EmailDestinatarioGuid>();

    public virtual ICollection<GuiaRemision> GuiaRemisions { get; set; } = new List<GuiaRemision>();
}
