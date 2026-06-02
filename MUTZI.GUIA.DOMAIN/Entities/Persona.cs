using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MUTZI.GUIA.DOMAIN.Entities
{
    [Table("vw_seguridad_persona")]
    public class Persona
    {
        [Key]
        [Column("persona_id")]
        public int PersonaId { get; set; }

        [Column("nombre")]
        public string Nombre { get; set; }

        [Column("apellido_paterno")]
        public string? ApellidoPaterno { get; set; }

        [Column("apellido_materno")]
        public string? ApellidoMaterno { get; set; }

        [Column("cargo")]
        public string? Cargo { get; set; }
    }
}
