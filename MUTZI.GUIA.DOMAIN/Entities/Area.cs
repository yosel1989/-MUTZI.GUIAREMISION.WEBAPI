using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MUTZI.GUIA.DOMAIN.Entities
{
    [Table("area", Schema = "rrhh")]
    public partial class Area
    {
        [Key]
        [Column("Id")]
        public int Id { get; set; }

        [Column("Descripcion")]
        public string Descripcion { get; set; }

        [Column("LocalId")]
        public int LocalId { get; set; }

        [Column("Estado")]
        public int Estado { get; set; }
    }
}
