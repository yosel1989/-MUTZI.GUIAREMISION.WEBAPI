using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MUTZI.GUIA.DOMAIN.Entities
{
    [Table("guia_remision_historial", Schema = "logistica")]
    public class GuiaRemisionHistorial
    {
        [Key]
        [Column("guia_remision_historial_id")]
        public int GuiaRemisionHistorialId { get; set; }

        [Column("guia_remision_id")]
        public int GuiaRemisionId { get; set; }

        [Column("motivo")]
        [MaxLength(45)]
        [Required(ErrorMessage = "El motivo es obligatorio.")]
        public string Motivo { get; set; }

        [Column("valor_anterior")]
        [MaxLength(150)]
        public string? ValorAnterior { get; set; }

        [Column("valor_nuevo")]
        [MaxLength(150)]
        public string? ValorNuevo { get; set; }

        [Column("descripcion")]
        [MaxLength(250)]
        public string? Descripcion { get; set; }

        [Column("fecha_registro")]
        [Required(ErrorMessage = "La fecha de registro es obligatoria.")]
        public DateTime FechaRegistro { get; set; }

        [Column("usuario_registro")]
        [MaxLength(75)]
        public string UsuarioRegistro { get; set; }

        [Column("usuario_registro_nombre")]
        [MaxLength(150)]
        public string UsuarioRegistroNombre { get; set; }

        [Column("area_id")]
        public int AreaId { get; set; }

        [ForeignKey(nameof(GuiaRemisionId))]
        public virtual GuiaRemision GuiaRemision { get; set; }

        [ForeignKey(nameof(AreaId))]
        public virtual Area Area { get; set; }
    }
}
