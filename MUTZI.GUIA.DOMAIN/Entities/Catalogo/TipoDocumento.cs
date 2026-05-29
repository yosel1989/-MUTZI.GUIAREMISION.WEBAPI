using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MUTZI.GUIA.DOMAIN.Entities.Catalogo
{
    [Table("vw_master_data_document_type")]
    public partial class TipoDocumento
    {
        [Key]
        [Column("tipo_documento_id")]
        public int TipoDocumentoId { get; set; }

        [Column("descripcion")]
        public string Descripcion { get; set; }

        [Column("descripcion_corta")]
        public string DescripcionCorta { get; set; }

        [Column("codigo_sunat")]
        public string CodigoSunat { get; set; }

        [Column("min")]
        public int? Minimo { get; set; }

        [Column("max")]
        public int? Maximo { get; set; }

        [Column("es_persona_natural")]
        public bool EsPersonaNatural { get; set; }

        [Column("es_persona_juridica")]
        public bool EsPersonaJuridica { get; set; }
    }
}
