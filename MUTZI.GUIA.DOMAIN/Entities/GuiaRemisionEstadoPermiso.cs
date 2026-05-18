using System.ComponentModel.DataAnnotations.Schema;

namespace MUTZI.GUIA.DOMAIN.Entities
{
    [Table("guia_remision_estado_permiso", Schema = "logistica")]
    public class GuiaRemisionEstadoPermiso
    {
        [Column("guia_remision_estado_id")]
        public int GuiaRemisionEstadoId { get; set; }

        [Column("guia_remision_estado_id_permitido")]
        public int GuiaRemisionEstadoIdPermitido { get; set; }

        [Column("usuario_registro")]
        public string UsuarioRegistro { get; set; }

        [Column("usuario_registro_nombre")]
        public string UsuarioRegistroNombre { get; set; }

        [Column("fecha_registro")]
        public DateTime FechaRegistro { get; set; } = DateTime.UtcNow;

        [ForeignKey(nameof(GuiaRemisionEstadoId))]
        public virtual GuiaRemisionEstado EstadoOrigen { get; set; }

        [ForeignKey(nameof(GuiaRemisionEstadoIdPermitido))]
        public virtual GuiaRemisionEstado EstadoDestino { get; set; }

    }
}
