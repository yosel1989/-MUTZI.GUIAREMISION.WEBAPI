using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MUTZI.GUIA.DOMAIN.Entities.Catalogo
{
    [Table("vw_rrhh_area")]
    public partial class Area
    {
        [Key]
        [Column("Id")]
        public int Id { get; set; }

        [Column("descripcion")]
        public string Descripcion { get; set; }

        [Column("LocalId")]
        public int LocalId { get; set; }

        [Column("Estado")]
        public int Estado { get; set; }
    }
}
