using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MUTZI.GUIA.DOMAIN.Entities
{
    [Table("catalogo_motivo_traslado")]
    public class MotivoTraslado
    {
        [Key]
        [Column("catalogo_motivo_traslado_id")]
        public int MotivoTrasladoId { get; set; }

        [Column("nombre")]
        public string Nombre { get; set; }

        [Column("codigo")]
        public string Codigo { get; set; }

        [Column("activo")]
        public bool Activo { get; set; }
    }
}
