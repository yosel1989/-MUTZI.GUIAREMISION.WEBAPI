using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;

namespace MUTZI.GUIA.DOMAIN.Entities;

[Table("guia_remision_entity_carrier")]
public class GuiaRemisionEntityCarrier
{
    [Column("guia_remision_id")]
    [Description("identificador de la guía de remisión a la que pertenece")]
    public int GuiaRemisionId { get; set; }

    [Column("entity_id")]
    [Description("identificador de la empresa transportista")]
    public int EntityId { get; set; }

    [Column("created_at")]
    [Description("La fecha en la cual se registro.")]
    public DateTime CreatedAt { get; set; }

    [Column("created_at_user")]
    [Description("El usuario que realizo el registro.")]
    public string CreatedAtUser { get; set; }

    [Column("created_at_user_name")]
    [Description("El nombre del personal que realizo el registro.")]
    public string CreatedAtUserName { get; set; }

    // Relaciones

    [ForeignKey(nameof(GuiaRemisionId))]
    public virtual GuiaRemision GuiaRemision { get; set; }

    [ForeignKey(nameof(EntityId))]
    public virtual Entity Entity { get; set; }
}
