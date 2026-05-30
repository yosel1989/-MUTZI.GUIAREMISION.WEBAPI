using System.Text.Json.Serialization;

namespace MUTZI.GUIA.APPLICATION.DTOs.Catalogos
{
    public class EntidadReguladoraDTO
    {
        [JsonPropertyName("id")]
        public int EntidadReguladoraId { get; set; }

        [JsonPropertyName("descripcion")]
        public string Descripcion { get; set; }

        [JsonPropertyName("descripcion_corta")]
        public string DescripcionCorta { get; set; }

        [JsonPropertyName("codigo_sunat")]
        public string CodigoSunat { get; set; }
    }

    public class EntidadReguladoraToSelectDTO
    {
        [JsonPropertyName("id")]
        public int EntidadReguladoraId { get; set; }

        [JsonPropertyName("descripcion")]
        public string Descripcion { get; set; }

        [JsonPropertyName("descripcion_corta")]
        public string DescripcionCorta { get; set; }

        [JsonPropertyName("codigo_sunat")]
        public string CodigoSunat { get; set; }
    }
}
