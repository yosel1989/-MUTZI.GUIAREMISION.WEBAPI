using System.ComponentModel.DataAnnotations.Schema;

namespace MUTZI.GUIA.DOMAIN.Entities.Catalogo
{
    [Table("vw_mdata_sunat_related_document_gre")]
    public class DocumentoRelacionado
    {
        [Column("documento_relacionado_id")]
        public int DocumentoRelacionadoId { get; set; }

        [Column("descripcion")]
        public string Descripcion { get; set; }

        [Column("descripcion_corta")]
        public string DescripcionCorta { get; set; }

        [Column("remitente")]
        public bool Remitente { get; set; }

        [Column("transportista")]
        public bool Transportista { get; set; }

        [Column("codigo_sunat")]
        public string CodigoSunat { get; set; }

        [Column("activo")]
        public bool Activo { get; set; }
    }
}
