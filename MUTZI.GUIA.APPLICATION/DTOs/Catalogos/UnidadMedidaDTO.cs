using System.Text.Json.Serialization;

namespace MUTZI.GUIA.APPLICATION.DTOs.Catalogos
{
    public class UnidadMedidaDTO
    {
        [JsonPropertyName("id")]
        public int UnidadMedidaId { get; set; }

        [JsonPropertyName("descripcion")]
        public string Descripcion { get; set; }

        [JsonPropertyName("descripcion_corta")]
        public string DescripcionCorta { get; set; }

        [JsonPropertyName("codigo_sunat")]
        public string CodigoSunat { get; set; }

        [JsonPropertyName("es_unidad_peso")]
        public bool EsUnidadPeso { get; set; } = false;

        [JsonPropertyName("es_unidad_volumen")]
        public bool EsUnidadVolumen { get; set; } = false;

        [JsonPropertyName("es_unidad_longitud")]
        public bool EsUnidadLongitud { get; set; } = false;

        [JsonPropertyName("es_unidad_conteo")]
        public bool EsUnidadConteo { get; set; } = false;
    }



    public class UnidadMedidaToSelectDTO
    {
        [JsonPropertyName("id")]
        public int UnidadMedidaId { get; set; }

        [JsonPropertyName("descripcion")]
        public string Descripcion { get; set; }

        [JsonPropertyName("descripcion_corta")]
        public string DescripcionCorta { get; set; }

        [JsonPropertyName("codigo_sunat")]
        public string CodigoSunat { get; set; }
    }
}
