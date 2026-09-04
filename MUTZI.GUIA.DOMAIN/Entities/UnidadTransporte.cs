using MUTZI.GUIA.DOMAIN.Entities.Catalogo;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MUTZI.GUIA.DOMAIN.Entities;

[Table("unidad_transporte")]
public class UnidadTransporte
{
    [Key]
    [Column("unidad_transporte_id")]
    public int UnidadTransporteId { get; set; }

    [Column("descripcion")]
    [MaxLength(50)]
    public string? Descripcion { get; set; }

    [Column("modelo")]
    [MaxLength(20)]
    public string? Modelo { get; set; }

    [Column("marca")]
    [MaxLength(20)]
    public string? Marca { get; set; }

    [Column("placa")]
    [MaxLength(20)]
    public string Placa { get; set; } = null!;

    [Column("tarjeta")]
    [MaxLength(20)]
    public string? Tarjeta { get; set; }

    [Column("entidad_reguladora_vehicular_id")]
    public int? EntidadReguladoraVehicularId { get; set; }

    [Column("emisor_vehicular")]
    [MaxLength(100)]
    public string? EmisorVehicular { get; set; }

    [Column("nro_autorizacion")]
    [MaxLength(50)]
    public string? NroAutorizacion { get; set; }

    [Column("tipo", TypeName = "enum('interno','externo')")]
    [MaxLength(20)]
    public string Tipo { get; set; } = "interno";

    [Column("fecha_registro")]
    public DateTime FechaRegistro { get; set; } = DateTime.UtcNow;

    [Column("fecha_modifico")]
    public DateTime? FechaModifico { get; set; }

    [Column("usuario_registro")]
    public string UsuarioRegistro { get; set; }

    [Column("usuario_registro_nombre")]
    [MaxLength(150)]
    public string UsuarioRegistroNombre { get; set; }

    [Column("usuario_modifico")]
    public string? UsuarioModifico { get; set; }

    [Column("usuario_modifico_nombre")]
    [MaxLength(150)]
    public string? UsuarioModificoNombre { get; set; }

    [Column("active")]
    public bool Active { get; set; } = true;

    // Relaciones

    [ForeignKey(nameof(EntidadReguladoraVehicularId))]
    public virtual EntidadReguladora? EntidadReguladoraVehicular { get; set; }
    public virtual ICollection<GuiaRemisionUnidadTransporte> TransporteGuids { get; set; } = new List<GuiaRemisionUnidadTransporte>();
}
