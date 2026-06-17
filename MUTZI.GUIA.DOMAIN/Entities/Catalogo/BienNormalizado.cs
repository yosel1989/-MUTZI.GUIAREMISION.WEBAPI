using System.ComponentModel.DataAnnotations.Schema;

namespace MUTZI.GUIA.DOMAIN.Entities.Catalogo
{
    [Table("vw_mdata_sunat_standardized_goods_catalog")]
    public class BienNormalizado
    {
        [Column("bien_normalizado_id")]
        public int BienNormalizadoId { get; set; }

        [Column("descripcion")]
        public string Descripcion { get; set; }

        [Column("segmento_codigo")]
        public string SegmentoCodigo { get; set; }

        [Column("segmento")]
        public string Segmento { get; set; }

        [Column("familia_codigo")]
        public string FamiliaCodigo { get; set; }

        [Column("familia")]
        public string Familia { get; set; }

        [Column("clase_codigo")]
        public string ClaseCodigo { get; set; }

        [Column("clase")]
        public string Clase { get; set; }

        [Column("partida_arancelaria")]
        public string PartidaArancelaria { get; set; }

        [Column("codigo_sunat")]
        public string CodigoSunat { get; set; }

        [Column("activo")]
        public bool Activo { get; set; }
    }
}
