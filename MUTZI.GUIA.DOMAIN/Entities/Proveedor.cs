using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MUTZI.GUIA.DOMAIN.Entities;

[Table("proveedor")]
public partial class Proveedor
{
    [Key]
    [Column("proveedor_id")]
    public int ProveedorId { get; set; }

    [Column("tipo_documento")]
    [MaxLength(20)]
    public string TipoDocumento { get; set; } = null!;

    [Column("numero_documento")]
    [MaxLength(20)]
    public string NumeroDocumento { get; set; } = null!;

    [Column("razon_social")]
    [MaxLength(250)]
    public string RazonSocial { get; set; } = null!;

    [Column("ubigeo_id")]
    [MaxLength(6)]
    public string? UbigeoId { get; set; }

    [Column("direccion")]
    [MaxLength(250)]
    public string? Direccion { get; set; }

    [Column("pais")]
    [MaxLength(3)]
    public string? Pais { get; set; }

    [Column("email")]
    [MaxLength(50)]
    public string? Email { get; set; }

    [Column("codigo_sunat")]
    [MaxLength(4)]
    public string? CodigoSunat { get; set; }

    [Column("fecha_registro")]
    public DateTime? FechaRegistro { get; set; } = DateTime.UtcNow;

    [Column("fecha_modifico")]
    public DateTime? FechaModifico { get; set; }

    [Column("usuario_registro")]
    public string UsuarioRegistro { get; set; }

    [Column("usuario_registro_nombre")]
    public string UsuarioRegistroNombre { get; set; }

    [Column("usuario_modifico")]
    public string? UsuarioModifico { get; set; }

    [Column("usuario_modifico_nombre")]
    public string? UsuarioModificoNombre { get; set; }

    [Column("estado", TypeName = "enum('activo','inactivo)")]
    public string? Estado { get; set; } = "activo";

    [Column("estado_id")]
    public sbyte? EstadoId { get; set; } = 1;

    public virtual ICollection<GuiaRemision> GuiaRemisions { get; set; } = new List<GuiaRemision>();

    [ForeignKey(nameof(UbigeoId))]
    [InverseProperty(nameof(Ubigeo.Proveedors))]
    public virtual Ubigeo? Ubigeo { get; set; }

}
