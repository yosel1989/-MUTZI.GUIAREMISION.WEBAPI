using System.Text.Json.Serialization;

namespace MUTZI.GUIA.APPLICATION.DTOs.Catalogos
{
    public class TipoDocumentoDTO
    {
        [JsonPropertyName("id")]
        public int TipoDocumentoId { get; set; }

        [JsonPropertyName("descripcion")]
        public string Descripcion { get; set; }

        [JsonPropertyName("descripcion_corta")]
        public string DescripcionCorta { get; set; }

        [JsonPropertyName("codigo_sunat")]
        public string CodigoSunat { get; set; }

        [JsonPropertyName("min")]
        public int? Minimo { get; set; }

        [JsonPropertyName("max")]
        public int? Maximo { get; set; }

        [JsonPropertyName("es_natural")]
        public bool EsPersonaNatural { get; set; }

        [JsonPropertyName("es_juridica")]
        public bool EsPersonaJuridica { get; set; }
    }


    public class TipoDocumentoToSelectDTO
    {
        [JsonPropertyName("id")]
        public int TipoDocumentoId { get; set; }

        [JsonPropertyName("descripcion")]
        public string Descripcion { get; set; }

        [JsonPropertyName("descripcion_corta")]
        public string DescripcionCorta { get; set; }

        [JsonPropertyName("codigo_sunat")]
        public string CodigoSunat { get; set; }

        [JsonPropertyName("min")]
        public int? Minimo { get; set; }

        [JsonPropertyName("max")]
        public int? Maximo { get; set; }

    }
}
