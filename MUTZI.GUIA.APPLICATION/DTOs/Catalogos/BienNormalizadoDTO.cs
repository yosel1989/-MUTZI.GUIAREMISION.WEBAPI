using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace MUTZI.GUIA.APPLICATION.DTOs.Catalogos
{
    public class BienNormalizadoDTO
    {
        [JsonPropertyName("bien_normalizado_id")]
        public int BienNormalizadoId { get; set; }

        [JsonPropertyName("descripcion")]
        public string Descripcion { get; set; }

        [JsonPropertyName("segmento_codigo")]
        public string SegmentoCodigo { get; set; }

        [JsonPropertyName("segmento")]
        public string Segmento { get; set; }

        [JsonPropertyName("familia_codigo")]
        public string FamiliaCodigo { get; set; }

        [JsonPropertyName("familia_codigo")]
        public string Familia { get; set; }

        [JsonPropertyName("clase_codigo")]
        public string ClaseCodigo { get; set; }

        [JsonPropertyName("clase")]
        public string Clase { get; set; }

        [JsonPropertyName("partida_arancelaria")]
        public string PartidaArancelaria { get; set; }

        [JsonPropertyName("codigo_sunat")]
        public string CodigoSunat { get; set; }

        [JsonPropertyName("activo")]
        public bool Activo { get; set; }
    }



    public class BienNormalizadoToSelectDTO
    {
        [JsonPropertyName("id")]
        public int BienNormalizadoId { get; set; }

        [JsonPropertyName("descripcion")]
        public string Descripcion { get; set; }

        [JsonPropertyName("familia")]
        public string Familia { get; set; }

        [JsonPropertyName("partida_arancelaria")]
        public string PartidaArancelaria { get; set; }

        [JsonPropertyName("codigo_sunat")]
        public string CodigoSunat { get; set; }
    }
}
