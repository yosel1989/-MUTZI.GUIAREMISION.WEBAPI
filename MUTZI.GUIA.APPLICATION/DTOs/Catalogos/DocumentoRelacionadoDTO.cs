
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace MUTZI.GUIA.APPLICATION.DTOs.Catalogos
{
    public class DocumentoRelacionadoDTO
    {
        [JsonPropertyName("id")]
        public int DocumentoRelacionadoId { get; set; }

        [JsonPropertyName("descripcion")]
        public string Descripcion { get; set; }

        [JsonPropertyName("descripcion_corta")]
        public string DescripcionCorta { get; set; }

        [JsonPropertyName("remitente")]
        public bool Remitente { get; set; }

        [JsonPropertyName("transportista")]
        public bool Transportista { get; set; }

        [JsonPropertyName("codigo_sunat")]
        public string CodigoSunat { get; set; }

        [JsonPropertyName("activo")]
        public bool Activo { get; set; }
    }


    public class DocumentoRelacionadoToSelectDTO
    {
        [JsonPropertyName("id")]
        public int DocumentoRelacionadoId { get; set; }

        [JsonPropertyName("descripcion")]
        public string Descripcion { get; set; }

        [JsonPropertyName("descripcion_corta")]
        public string DescripcionCorta { get; set; }

        [JsonPropertyName("codigo_sunat")]
        public string CodigoSunat { get; set; }
    }
}
