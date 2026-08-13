using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MUTZI.GUIA.DOMAIN.Entities
{
    [Table("vw_sunat_reason_for_transfer")]
    public class MotivoTraslado
    {
        [Key]
        [Column("reason_for_transfer_id")]
        public int MotivoTrasladoId { get; set; }

        [Column("name")]
        public string Nombre { get; set; }

        [Column("code")]
        public string CodigoSunat { get; set; }

    }
}
