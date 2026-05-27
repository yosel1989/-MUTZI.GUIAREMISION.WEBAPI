using System.Text.Json.Serialization;

namespace MUTZI.GUIA.APPLICATION.DTOs.TipoDocumento
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
    }
}
