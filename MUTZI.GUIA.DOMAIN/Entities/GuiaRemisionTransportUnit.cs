using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;

namespace MUTZI.GUIA.DOMAIN.Entities;

[Table("guia_remision_transport_unit")]
public class GuiaRemisionTransportUnit
{
    [Column("guia_remision_id")]
    [Description("identificador de la guía de remisión a la que pertenece")]
    public int GuiaRemisionId { get; set; }

    [Column("transport_unit_id")]
    [Description("identificador de la unidad de transporte a la que pertenece")]
    public int TransportUnitId { get; set; }

    [Column("type")]
    [Description("El tipo de vehiculo en la guía, si es 'principal' o 'secundario'")]
    public string Type { get; set; }

    [Column("created_at")]
    [Description("La fecha en la cual se registro.")]
    public DateTime CreatedAt { get; set; }

    [Column("created_at_user")]
    [Description("El usuario que realizo el registro.")]
    public string CreatedAtUser { get; set; }

    [Column("created_at_user_name")]
    [Description("El nombre del personal que realizo el registro.")]
    public string CreatedAtUserName { get; set; }

    [Column("updated_at")]
    [Description("La fecha en la cual se modificó.")]
    public DateTime? UpdatedAt { get; set; }

    [Column("updated_at_user")]
    [Description("El usuario que realizo el modificó.")]
    public string? UpdatedAtUser { get; set; }

    [Column("updated_at_user_name")]
    [Description("El nombre del personal que realizo la modificación.")]
    public string? UpdatedAtUserName { get; set; }


    // Relaciones

    [ForeignKey(nameof(GuiaRemisionId))]
    public virtual GuiaRemision GuiaRemision { get; set; }

    [ForeignKey(nameof(TransportUnitId))]
    public virtual TransportUnit TransportUnit { get; set; }
}
