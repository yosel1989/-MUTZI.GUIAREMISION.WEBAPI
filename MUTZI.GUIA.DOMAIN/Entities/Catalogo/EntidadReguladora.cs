using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MUTZI.GUIA.DOMAIN.Entities.Catalogo
{
    public class EntidadReguladora
    {
        [Key]
        [Column("entidad_reguladora_id")]
        public int EntidadReguladoraId { get; set; }

        [Column("descripcion")]
        public string Descripcion { get; set; }

        [Column("descripcion_corta")]
        public string DescripcionCorta { get; set; }

        [Column("codigo_sunat")]
        public string CodigoSunat { get; set; }
    }
}
