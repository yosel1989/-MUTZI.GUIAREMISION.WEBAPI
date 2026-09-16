using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MUTZI.GUIA.DOMAIN.Entities
{
    [Table("guia_remision_estado")]
    public class GuiaRemisionEstado
    {
        [Key]
        [Column("guia_remision_estado_id")]
        public int GuiaRemisionEstadoId { get; set; }

        [Column("nombre")]
        public string Nombre { get; set; }

        [Column("descripcion")]
        public string Descripcion { get; set; }

        [Column("color_hex")]
        public string? ColorHex { get; set; }

        public virtual ICollection<GuiaRemisionEstadoPermiso> EstadosPermitidosDesde { get; set; } = new List<GuiaRemisionEstadoPermiso>();

        public virtual ICollection<GuiaRemisionEstadoPermiso> EstadosPermitidosHacia { get; set; } = new List<GuiaRemisionEstadoPermiso>();

    }
}
