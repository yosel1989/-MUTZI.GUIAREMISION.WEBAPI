using MUTZI.GUIA.DOMAIN.Entities.Catalogo;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MUTZI.GUIA.DOMAIN.Entities;

[Table("transport_unit")]
public class TransportUnit
{
    [Key]
    [Column("transport_unit_id")]
    public int UnidadTransporteId { get; set; }

    [Column("description")]
    [MaxLength(50)]
    public string? Descripcion { get; set; }

    [Column("model")]
    [MaxLength(20)]
    public string? Modelo { get; set; }

    [Column("brand")]
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

    [Column("created_at")]
    public DateTime FechaRegistro { get; set; } = DateTime.UtcNow;

    [Column("updated_at")]
    public DateTime? FechaModifico { get; set; }

    [Column("created_at_user")]
    public string UsuarioRegistro { get; set; }

    [Column("created_at_user_name")]
    [MaxLength(150)]
    public string UsuarioRegistroNombre { get; set; }

    [Column("updated_at_user")]
    public string? UsuarioModifico { get; set; }

    [Column("updated_at_user_name")]
    [MaxLength(150)]
    public string? UsuarioModificoNombre { get; set; }

    [Column("active")]
    public bool Active { get; set; } = true;

    // Relaciones

    [ForeignKey(nameof(EntidadReguladoraVehicularId))]
    public virtual EntidadReguladora? EntidadReguladoraVehicular { get; set; }
    public virtual ICollection<GuiaRemisionUnidadTransporte> TransporteGuids { get; set; } = new List<GuiaRemisionUnidadTransporte>();
}
