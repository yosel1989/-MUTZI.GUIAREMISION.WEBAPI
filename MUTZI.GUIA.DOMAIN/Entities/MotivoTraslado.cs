using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MUTZI.GUIA.DOMAIN.Entities
{
    [Table("vw_mdata_transfer_reason")]
    public class MotivoTraslado
    {
        [Key]
        [Column("motivo_traslado_id")]
        public int MotivoTrasladoId { get; set; }

        [Column("descripcion")]
        public string Nombre { get; set; }

        [Column("codigo_sunat")]
        public string CodigoSunat { get; set; }

    }
}
