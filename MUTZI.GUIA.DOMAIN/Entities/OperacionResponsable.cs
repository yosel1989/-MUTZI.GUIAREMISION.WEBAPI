using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MUTZI.GUIA.DOMAIN.Entities
{
    [Table("operacion_responsable")]
    public class OperacionResponsable
    {
        [Key]
        [Column("operacion_responsable_id")]
        public int OperacionResponsableId { get; set; }

        [Column("guia_remision_estado_id")]
        public int GuiaRemisionEstadoId { get; set; }

        [Column("descripcion")]
        public string Descripcion { get; set; }

        [Column("personal_registro_id")]
        public int PersonalRegistroId { get; set; }

        [Column("usuario_registro")]
        public string UsuarioRegistro { get; set; }

        [Column("usuario_registro_nombre")]
        public string UsuarioRegistroNombre { get; set; }

        [Column("fecha_registro")]
        public DateTime FechaRegistro { get; set; } = DateTime.UtcNow;

        [Column("personal_modifico_id")]
        public int? PersonalModificoId { get; set; }

        [Column("usuario_modifico")]
        public string? UsuarioModifico { get; set; }

        [Column("usuario_modifico_nombre")]
        public string? UsuarioModificoNombre { get; set; }

        [Column("fecha_modifico")]
        public DateTime? FechaModifico { get; set; }
    }
}
